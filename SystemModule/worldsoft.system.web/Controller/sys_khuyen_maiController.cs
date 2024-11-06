using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using worldsoft.common.BaseClass;
using worldsoft.common.common;
using worldsoft.common.Services;
using worldsoft.DataBase.Provider;
using worldsoft.system.data.DataAccess;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    public partial class sys_khuyen_maiController : BaseAuthenticationController
    {
        public sys_khuyen_mai_repo repo;

        public sys_khuyen_maiController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_khuyen_mai_repo(context);
        }
        public IActionResult getListUse()
        {
            var lstDoiTac = repo._context.sys_doi_tacs.ToList();
            var result = repo._context.sys_khuyen_mais
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                 }).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_khuyen_mai_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_khuyen_mai_model>(json.GetValue("data").ToString());
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

                var search = dictionary["search"];
                var query = repo.FindAll()
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                   .ToList());
                DTResult<sys_khuyen_mai_model> result = new DTResult<sys_khuyen_mai_model>
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
