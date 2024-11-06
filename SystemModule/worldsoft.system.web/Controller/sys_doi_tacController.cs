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
    public partial class sys_doi_tacController : BaseAuthenticationController
    {
        public sys_doi_tac_repo repo;

        public sys_doi_tacController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_doi_tac_repo(context);
        }

        [HttpPost]
        public async Task<IActionResult> edit_language([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_doi_tac_language_model>(json.GetValue("data").ToString());
            var check = checkModelStateEditLanguage(model);
            if (!check)
            {
                return generateError();
            }
            var data = repo._context.sys_doi_tac_languages.Where(q => q.id_doi_tac == model.db.id_doi_tac).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_doi_tac_language_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.name = model.db.name;
                model_language.db.id_doi_tac = model.db.id_doi_tac;
                model_language.db.note = model.db.note;
                await repo.insert_language(model_language);
            }
            else
            {
                data.name = model.db.name;
                data.note = model.db.note;
                repo._context.SaveChanges();
            }


            return Json(model);
        }
        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_loai_doi_tac = json.GetValue("id_loai_doi_tac").ToString();
            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_doi_tac_language_model();

            model = repo._context.sys_doi_tac_languages.Where(d => d.id_doi_tac == id_loai_doi_tac).Select(d => new sys_doi_tac_language_model()
            {
                db = d
            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_doi_tac_language_model();
                model.db.id_doi_tac = id_loai_doi_tac;
            }
            return Json(model);
        }
        public async Task<IActionResult> get_list_doi_tac()
        {
            var list_doi_tac = repo.FindAll().ToList();
            var count = list_doi_tac.Count();
            var result = repo._context.sys_loai_doi_tacs
                .Where(q => q.status_del == 1)
                .Select(q => new sys_loai_doi_tac_ref_model
                {
                    db = q,
                 
                    lst_doi_tac = repo._context.sys_doi_tacs.Where(d => d.id_loai_doi_tac == q.id).Select(t => new sys_doi_tac_model
                    {
                        db = t,
                        name_en = repo._context.sys_doi_tac_languages.Where(d =>d.id_doi_tac ==t.id).Select(q=>q.name).FirstOrDefault(),
                        note_en = repo._context.sys_doi_tac_languages.Where(d => d.id_doi_tac == t.id).Select(q => q.note).FirstOrDefault(),
                    }).OrderBy(t=>t.db.stt).ToList()
                }).OrderBy(q=>q.db.stt).ToList();

         
            return Json(result);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.sys_doi_tacs
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
            var model = JsonConvert.DeserializeObject<sys_doi_tac_model>(json.GetValue("data").ToString());
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

            handler_after_save(model);
            return Json(model);
        }


        public sys_doi_tac_model handler_after_save(sys_doi_tac_model model)
        {
            model.loai_doi_tac = repo._context.sys_loai_doi_tacs.Where(t => t.id == model.db.id_loai_doi_tac).Select(d => d.name).SingleOrDefault();
            return model;
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_doi_tac_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);
            handler_after_save(model);
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
                     //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);
                var id_loai_doi_tac = dictionary["id_loai_doi_tac"];
                
                query = query.Where(d => d.db.id_loai_doi_tac == id_loai_doi_tac || id_loai_doi_tac =="-1");
                

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderBy(q=>q.db.stt).Skip(param.Start).Take(param.Length)
                   .ToList());
                DTResult<sys_doi_tac_model> result = new DTResult<sys_doi_tac_model>
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
