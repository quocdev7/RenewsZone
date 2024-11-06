﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.common.BaseClass;
using worldsoft.common.common;
using worldsoft.common.Services;
using worldsoft.system.data.DataAccess;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;

namespace worldsoft.system.web.Controller
{
    public partial class sys_group_userController : BaseAuthenticationController
    {
        public sys_group_user_repo repo;

        public sys_group_userController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_group_user_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_group_users
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name
                 }).ToList();
            return Json(result);
        }
        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }
        public async Task<IActionResult> getListRole([FromBody] JObject json)
        {
            var model = repo.FindAllRole(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_group_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.update_date = DateTime.Now;
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_group_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);
            return Json(model);
        }


        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> revert([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.revert(id, getUserId());
            return Json("");
        }
        


        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }

        public IActionResult getListUser()
        {
            var result = repo._context.users
                .Where(d => d.status_del == 1)
                 .Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " " + d.LastName
                 }).ToList();
            return Json(result);
        }

      
        [HttpPost]

        public async Task<IActionResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = dictionary["search"];
                var query = repo.FindAll()
                    //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
        .OrderByDescending(d => d.db.update_date).ToList());
                DTResult<sys_group_user_model> result = new DTResult<sys_group_user_model>
                {
                    start = param.Start,
                    draw = param.Draw,
                    data = dataList,
                    recordsFiltered = count,
                    recordsTotal = count
                };
                return Json(result);
            }

            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

        }

    }
}