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
    public partial class sys_cot_moc_su_kienController : BaseAuthenticationController
    {
        private sys_cot_moc_su_kien_repo repo;

        public sys_cot_moc_su_kienController(IUserService userService,worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_cot_moc_su_kien_repo(context);
        }

        public IActionResult getListUse()
        {

            var result = repo._context.sys_cot_moc_su_kiens
               
                .Where(d=>d.status_del==1)
                .OrderBy(d=>d.stt)
                 .Select(d => new sys_cot_moc_su_kien_model()
                 {
                     id = d.id,
                     note=d.note,
                     note_mobile = d.note_mobile,
                     time=d.time,
                     name = d.name,
                 }).ToList();

            result.ForEach(q =>
            {
                var language = repo._context.sys_cot_moc_su_kien_ens.Where(d => d.id_cot_moc == q.id).FirstOrDefault();

                if (language != null)
                {
                    q.name_language = language.name ?? "";
                    q.time_language = language.time ?? "";
                    q.note_language = language.note ?? "";
                    q.note_mobile_language = language.note_mobile ?? "";
                }
            });
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
          
            var model = JsonConvert.DeserializeObject<sys_cot_moc_su_kien_model>(json.GetValue("data").ToString());
            var check= checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;
            model.db.update_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cot_moc_su_kien_model>(json.GetValue("data").ToString());
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

        public async Task<IActionResult> edit_language([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cot_moc_su_kien_language_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit_language(model);
            if (!check)
            {
                return generateError();
            }
            var data = repo._context.sys_cot_moc_su_kien_ens.Where(q => q.id_cot_moc == model.db.id_cot_moc).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_cot_moc_su_kien_language_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.id_cot_moc = model.db.id_cot_moc;
                model_language.db.name = model.db.name;
                model_language.db.time = model.db.time;
                model_language.db.note = model.db.note;
                model_language.db.note_mobile = model.db.note_mobile;
                await repo.insert_language(model_language);
            }
            else
            {


                data.name = model.db.name;
                data.time = model.db.time;
                data.note = model.db.note;
                data.note_mobile = model.db.note_mobile;
                repo._context.SaveChanges();
            }
            return Json(model);
        }
        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_cot_moc = json.GetValue("id_cot_moc").ToString();

            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_cot_moc_su_kien_language_model();
            model = repo._context.sys_cot_moc_su_kien_ens.Where(d => d.id_cot_moc == id_cot_moc).Select(d => new sys_cot_moc_su_kien_language_model()
            {

                db = d

            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_cot_moc_su_kien_language_model();
                model.db.id_cot_moc = id_cot_moc;
            }
            return Json(model);
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
            return Json("");
        }
        //public async Task<IActionResult> revert([FromBody] JObject json)
        //{
        //    var id = json.GetValue("id").ToString();
        //    repo.revert(id, getUserId());
        //    return Json("");
        //}
        

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
                     .Where(d => d.db.name.Contains(search))
                      // .Where(d => d.db.id_su_kien.Equals(id_su_kien) || id_su_kien == "-1")
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderBy(d => d.db.stt).Skip(param.Start).Take(param.Length)
          .ToList());
                DTResult<sys_cot_moc_su_kien_model> result = new DTResult<sys_cot_moc_su_kien_model>
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
