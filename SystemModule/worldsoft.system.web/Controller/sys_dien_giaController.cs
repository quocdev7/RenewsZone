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
    public partial class sys_dien_giaController : BaseAuthenticationController
    {
        public sys_dien_gia_repo repo;

        public sys_dien_giaController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_dien_gia_repo(context);
        }

        
        public IActionResult getListUseNew([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var result = repo._context.sys_dien_gias
                         .Where(d => d.id_su_kien == id)
                .Where(d => d.status_del == 1)
                 .
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     chuc_danh = d.chuc_danh,
                     image = d.image,

                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.sys_dien_gias
                .Where(d => d.status_del == 1)
                 .
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     chuc_danh = d.chuc_danh,
                     image = d.image,

                 }).ToList();
            return Json(result);
        }
     
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_dien_gia_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_dien_gia_model>(json.GetValue("data").ToString());
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
        public async Task<IActionResult> edit_en([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_dien_gia_en_model>(json.GetValue("data").ToString());
            var check = checkModelStateEditEn(model);
            if (!check)
            {
                return generateError();
            }
            var data = repo._context.sys_dien_gia_ens.Where(q => q.id_dien_gia == model.db.id_dien_gia).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_dien_gia_en_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.id_dien_gia = model.db.id_dien_gia;
                model_language.db.name = model.db.name;
                model_language.db.chuc_danh = model.db.chuc_danh;
                model_language.db.cong_ty = model.db.cong_ty;
                await repo.insert_en(model_language);
            }
            else
            {


                data.name = model.db.name;
                data.chuc_danh = model.db.chuc_danh;
                data.cong_ty = model.db.cong_ty;
                repo._context.SaveChanges();
            }
            return Json(model);
        }

        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_dien_gia = json.GetValue("id_dien_gia").ToString();

            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_dien_gia_en_model();
            model = repo._context.sys_dien_gia_ens.Where(d => d.id_dien_gia == id_dien_gia).Select(d => new sys_dien_gia_en_model()
            {

                db = d

            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_dien_gia_en_model();
                model.db.id_dien_gia = id_dien_gia;
            }
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

                var id_su_kien = dictionary["id_su_kien"];

                var search = dictionary["search"];
                var query = repo.FindAll()
                     //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.Contains(search))
                     .Where(d => d.db.id_su_kien.Equals(id_su_kien) || id_su_kien == "-1")
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderBy(d => d.db.stt).Skip(param.Start).Take(param.Length)
        .ToList());
                DTResult<sys_dien_gia_model> result = new DTResult<sys_dien_gia_model>
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
