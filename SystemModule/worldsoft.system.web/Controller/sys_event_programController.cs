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
using worldsoft.DataBase.Helper;
using System.IO;
using System.Web;

namespace worldsoft.system.web.Controller
{
    public partial class sys_event_programController : BaseAuthenticationController
    {
        public sys_event_program_repo repo;

        public sys_event_programController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_event_program_repo(context);
        }






        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> getListEventProgram()
        {
            var user_id = getUserId();
            var lst_id_su_kien = repo.FindAllSuKienThamGia()
                 .Where(d => d.status_del == 1)
                       .Where(d => d.time_start <= DateTime.Now.Date.AddDays(1))
                       .Where(d => repo._context.sys_event_participates.Where(t => t.user_id == user_id && d.id_su_kien == t.event_id
                     && t.check_in_status == 3).Count() > 0).Select(q => q.id_su_kien).ToList();


            var model = repo.FindAllEventProgram().Where(q => lst_id_su_kien.Contains(q.db.event_id)).ToList();

            return Json(model);
        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_event_program_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = DateTime.Now.Ticks + "";
            model.db.create_date = DateTime.Now;
            model.db.status_del =1;
            await repo.insert(model);
            return Json(model);
        }












        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_program_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            //model.db.update_by = getUserId();
            //model.db.update_date = DateTime.Now;

            await repo.update(model);
            return Json(model);
        }

        public async Task<IActionResult> edit_language([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_program_en_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit_language(model);
            if (!check)
            {
                return generateError();
            }
            var data = repo._context.sys_event_program_ens.Where(q => q.id_event_program == model.db.id_event_program).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_event_program_en_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.id_event_program = model.db.id_event_program;
                model_language.db.name = model.db.name;
                model_language.db.description = model.db.description;
                await repo.insert_language(model_language);
            }
            else
            {


                data.name = model.db.name;
                data.description = model.db.description;
                repo._context.SaveChanges();
            }
            return Json(model);
        }
        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_event_program = json.GetValue("id_event_program").ToString();

            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_event_program_en_model();
            model = repo._context.sys_event_program_ens.Where(d => d.id_event_program == id_event_program).Select(d => new sys_event_program_en_model()
            {

                db = d

            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_event_program_en_model();
                model.db.id_event_program = id_event_program;
            }
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
                var event_id = dictionary["event_id"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                      .Where(d => d.db.event_id == event_id)
                            .Where(d => d.db.event_id.Equals(event_id) || event_id == "-1")
                    .Where(d => string.IsNullOrEmpty(search) || d.db.name.Contains(search) || d.ten_su_kien.Contains(search) || d.db.presenter.Contains(search)
                     || d.db.description.Contains(search) || d.db.location.Contains(search)
                    )
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderBy(d => d.db.create_date).ToList());
                //var dataList = query.ToList();
                DTResult<sys_event_program_model> result = new DTResult<sys_event_program_model>
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







        [HttpPost]
        public async Task<IActionResult> upload_file_event()
        {

            var request = Request;
            foreach (var formFile in Request.Form.Files)
            {
                var tick = Guid.NewGuid();
                var filename = formFile.FileName.Trim('"') + "";
                filename = StringFunctions.NonUnicode(filename);
                var currentpath = Directory.GetCurrentDirectory();
                var path = Path.Combine(currentpath, "file_upload", "logo_event");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var pathsave = Path.Combine(path, tick + "." + filename.Split(".").Last());
                using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload"), ""));
                //+ "&name=" + HttpUtility.UrlEncode(filename);
                return Json(new { path = file_path });
            }
            return generateError();
        }


    }
}
