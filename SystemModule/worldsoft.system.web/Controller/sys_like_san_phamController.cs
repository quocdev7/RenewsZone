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
    public partial class sys_like_san_phamController : BaseAuthenticationController
    {
        public sys_like_san_pham_repo repo; 

        public sys_like_san_phamController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_like_san_pham_repo(context);
        }

        //public IActionResult getListUse()
        //{
        //    var result = repo._context.sys_like_san_phams
        //        .Where(d => d.status_del == 1).
        //         Select(d => new
        //         {
        //             id = d.id,
        //             name = d.name
        //         }).ToList();
        //    return Json(result);
        //}
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_like_san_pham_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);


            if (!check)
            {
                return generateError();
            }
            var db =  repo._context.sys_like_san_phams.Where(d => d.email == model.db.email).FirstOrDefault();
            if (db == null)
            {
                model.db.create_date = DateTime.Now;
                await repo.insert(model);

            }
            else
            {
                db.email = model.db.email;
                db.create_date = DateTime.Now;
                repo._context.SaveChanges();
            }
           

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> theo_doi([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_like_san_pham_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!String.IsNullOrEmpty(model.db.email))
            {
                model.db.email = model.db.email.Trim();
            }
      
            if (!check)
            {
                return generateError();
            }
            var db = repo._context.sys_like_san_phams.Where(d => d.email == model.db.email).FirstOrDefault();
            if (db == null)
            {
                model.db.create_date = DateTime.Now;
                await repo.insert(model);

            }
            else
            {
                db.email = model.db.email;
                db.create_date = DateTime.Now;
                repo._context.SaveChanges();
            }


            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_like_san_pham_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
          
            await repo.update(model);
            return Json(model);
        }


        //public async Task<IActionResult> delete([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    repo.delete(id, getUserId());
        //    return Json("");
        //}


        public async Task<IActionResult> getElementById(long id)
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
                    //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.email.Contains(search))
                     ;
              

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.create_date).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_like_san_pham_model> result = new DTResult<sys_like_san_pham_model>
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
