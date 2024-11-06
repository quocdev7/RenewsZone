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
    public partial class sys_event_emailController : BaseAuthenticationController
    {
        public sys_event_email_repo repo;

        public sys_event_emailController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_event_email_repo(context);
        }


      
     

       
        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

    


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_event_email_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            //model.db.create_by = getUserId();
           //model.db.status_del = 1;
            //model.db.id = DateTime.Now.Ticks + "";
           // model.db.create_date = DateTime.Now;
          
            await repo.insert(model);
             return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> create_khachmoi([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var list_khach_moi = JsonConvert.DeserializeObject<List<sys_event_email_model>>(json.GetValue("list_khach_moi").ToString());
            var list_ban_to_chuc = JsonConvert.DeserializeObject<List<sys_event_email_model>>(json.GetValue("list_ban_to_chuc").ToString());

            var lstmodel = list_khach_moi.Concat(list_ban_to_chuc);

            await repo.saveKhachMoiEvent(list_khach_moi, event_id);
            await repo.saveBanToChucEvent(list_ban_to_chuc, event_id);
            return Json(lstmodel);
        }


      

       

        
       
      
      

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_email_model>(json.GetValue("data").ToString());
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
                var trang_thai = int.Parse(dictionary["trang_thai"]);
                var event_id = dictionary["event_id"];
                var search = dictionary["search"];
                var send_status = dictionary["send_status"];
                var query = repo.FindAll()
                    .Where(q=>q.db.send_status == trang_thai || trang_thai == -1)
                      .Where(d => d.db.event_id == event_id)
                    .Where(d => string.IsNullOrEmpty(search) || d.db.title.Contains(search) || d.db.mailto.Contains(search))
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderByDescending(d => d.db.create_date).ToList());
                //var dataList = query.ToList();
                dataList.ForEach(t =>
                {
                    //t.db.mailto = CMAESCrypto.DecryptText(t.db.mailto);
                });
                DTResult<sys_event_email_model> result = new DTResult<sys_event_email_model>
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
