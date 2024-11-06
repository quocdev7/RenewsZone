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
    public partial class sys_loai_cau_hinhController : BaseAuthenticationController
    {
        private sys_loai_cau_hinh_repo repo;

        public sys_loai_cau_hinhController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_loai_cau_hinh_repo(context);
        }


        /* public IActionResult getListUse()
          {
              var result = repo._context.sys_loai_cau_hinh
                  .Where(d=>d.id=="")
                   .Select(d => new
                   {
                       id = d.id,
                       //name = d.name
                   }).ToList();
              return Json(result);
          }*/
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_loai_cau_hinh_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.id = Guid.NewGuid().ToString();

            // model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_loai_cau_hinh_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
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
                           //.Where(d => d.db.status_del == 1)
                           .Where(d => d.db.name.Contains(search) || d.db.code.Contains(search));

                //query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db).Skip(param.Start).Take(param.Length).ToList());
                DTResult<sys_loai_cau_hinh_model> result = new DTResult<sys_loai_cau_hinh_model>
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
