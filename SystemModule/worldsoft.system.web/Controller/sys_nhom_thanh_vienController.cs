using MassTransit;
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
    public partial class sys_nhom_thanh_vienController : BaseAuthenticationController
    {
        public sys_nhom_thanh_vien_repo repo;

        public sys_nhom_thanh_vienController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_nhom_thanh_vien_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_nhom_thanh_viens
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name
                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUseAll()
        {
            var result = repo._context.sys_nhom_thanh_viens
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name
                 }).ToList();
            var itemAll = new { id = "-1", name = "Tất cả" };
            result.Insert(0, itemAll);
            //result.Add(new { id = "-1", name = "Tất cả" });
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_nhom_thanh_vien_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = DateTime.Now.Ticks + "";
            model.db.create_date = DateTime.Now;
          
            await repo.insert(model);
             return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_nhom_thanh_vien_model>(json.GetValue("data").ToString());
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
            var user_id = getUserId();
            repo.delete(id, user_id);
            return Json("");
        }

        public async Task<IActionResult> sudung([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var user_id = getUserId();
            repo.revert(id, user_id);
            return Json("");
        }

        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }

        public IActionResult getListUser()
        {
            var result = repo._context.users.
                 Select(d => new
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
                var status_del = int.Parse(dictionary["status_del"]);
                var search = dictionary["search"];
                var query = repo.FindAll()
                      .Where(d => d.db.status_del == status_del)
                    .Where(d => string.IsNullOrEmpty(search) || d.db.name.Contains(search) || d.db.note.Contains(search))
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_nhom_thanh_vien_model> result = new DTResult<sys_nhom_thanh_vien_model>
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
