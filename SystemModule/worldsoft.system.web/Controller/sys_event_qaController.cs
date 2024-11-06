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
using worldsoft.DataBase.System;
using worldsoft.common.Helpers;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using worldsoft.DataBase.Helper;

namespace worldsoft.system.web.Controller
{
    public partial class sys_event_qaController : BaseAuthenticationController
    {
        public sys_event_qa_repo repo;

        public sys_event_qaController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_event_qa_repo(context);
        }


      
     

       
        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

    


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_event_qa_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
           

            if (!String.IsNullOrEmpty(model.db.answer))
            {
                model.db.time_answer = DateTime.Now;
                model.db.user_id_answer = getUserId();
            }
          
            model.db.time_question = DateTime.Now;
            model.db.user_id_question = getUserId();
            model.db.id = DateTime.Now.Ticks + "";
            model.db.create_date = DateTime.Now;
            model.db.create_by= getUserId();
            await repo.insert(model);
             return Json(model);
        }
    
      

       

        
       
      
      

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_qa_model>(json.GetValue("data").ToString());
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
            var model = JsonConvert.DeserializeObject<sys_event_qa_en_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit_language(model);
            if (!check)
            {
                return generateError();
            }
            var data = repo._context.sys_event_qa_ens.Where(q => q.id_event_q_a == model.db.id_event_q_a).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_event_qa_en_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.id_event_q_a = model.db.id_event_q_a;
                model_language.db.question = model.db.question;
                model_language.db.answer = model.db.answer;
                await repo.insert_language(model_language);
            }
            else
            {


                data.question = model.db.question;
                data.answer = model.db.answer;
                repo._context.SaveChanges();
            }
            return Json(model);
        }
        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_event_q_a = json.GetValue("id_event_q_a").ToString();

            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_event_qa_en_model();
            model = repo._context.sys_event_qa_ens.Where(d => d.id_event_q_a == id_event_q_a).Select(d => new sys_event_qa_en_model()
            {

                db = d

            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_event_qa_en_model();
                model.db.id_event_q_a = id_event_q_a;
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
            repo.sudung(id, user_id);
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
                //var trang_thai = int.Parse(dictionary["trang_thai"]);
                var search = dictionary["search"];
                var event_id = dictionary["event_id"];
                var query = repo.FindAll()
                     .Where(d => d.db.event_id == event_id)
                    .Where(d => string.IsNullOrEmpty(search) || d.ten_su_kien.Contains(search) || d.db.question.Contains(search)
                    | d.db.answer.Contains(search) || d.user_answer.Contains(search) || d.user_question.Contains(search)
                    )
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderByDescending(d => d.db.create_date).ToList());
               
                DTResult<sys_event_qa_model> result = new DTResult<sys_event_qa_model>
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
