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
    public partial class sys_user_companyController : BaseAuthenticationController
    {
        private sys_user_company_repo repo;

        public sys_user_companyController(IUserService userService,worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_user_company_repo(context);
        }
        [HttpPost]
        public async Task<IActionResult> add_nhan_vien([FromBody] JObject json)
        {

            var user_id = json.GetValue("user_id").ToString();
            var id_company = json.GetValue("id_company").ToString();
            var model = new sys_user_company_model();
            var check = checkModelStateCreate(model, user_id);
            if (!check)
            {
                return generateError();
            }


            model.db.id = DateTime.Now.Ticks + "";
            model.db.user_id = user_id;
            model.db.company_id = id_company;
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo._context.sys_user_companys.AddAsync(model.db);
            repo._context.SaveChanges();

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
                //var id_user = dictionary["id_user"];
                var id_company = dictionary["id_company"];
                var query = repo.FindAll()
                     .Where(d => String.IsNullOrEmpty(id_company)|| d.db.company_id== id_company)
                     .Where(d => d.full_name.Contains(search)|| d.company_name.Contains(search))
                     ;
         
             

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
       .ToList());
                DTResult<sys_user_company_model> result = new DTResult<sys_user_company_model>
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
