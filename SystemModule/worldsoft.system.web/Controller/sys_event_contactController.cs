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
    public partial class sys_event_contactController : BaseAuthenticationController
    {
        public sys_event_contact_repo repo;

        public sys_event_contactController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_event_contact_repo(context);
        }


      
     

       
        public async Task<IActionResult> getListItem([FromBody] JObject json)
        {
            var model = repo.FindAllItem(json.GetValue("id").ToString()).ToList();
            return Json(model);
        }

    


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_event_contact_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            var user_create= getUserId();
            model.db.check_in_date = DateTime.Now;
            model.db.date_add = DateTime.Now;
            model.db.check_in_status = 1;
            model.db.company_id = repo._context.users.Where(q=>q.Id == model.db.user_id).Select(q=>q.id_company).SingleOrDefault();
            model.db.id = DateTime.Now.Ticks + "";

          

            await repo.insert(model, user_create);
            
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> danh_gia([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var review_rate =  JsonConvert.DeserializeObject<int>(json.GetValue("review_rate").ToString());
            var review_note = json.GetValue("review_note").ToString();
            var userid = getUserId();
            var db = await repo._context.sys_event_participates.Where(d => d.event_id == event_id && d.role ==1 && d.user_id == userid).FirstOrDefaultAsync();
            if(db != null)
            {
                db.review_note = review_note;
                db.review_rate = review_rate;
                repo._context.SaveChanges();
                return generateSuscess();
            }
            else
            {
                return generateError();
            }
           

          
        }
        [HttpPost]
        public async Task<IActionResult> get_list_danh_gia([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();

            var list_danh_gia = repo._context.sys_event_participates.Where(d => d.event_id == event_id && d.role == 1 && (d.review_rate??0)==0)
                .Select(d => new sys_event_contact_model()
                {
                    db = d,
                    user_name = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.FirstName + " " + q.LastName).SingleOrDefault(),
                    position = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_job_title).SingleOrDefault(),
                    avatar_link = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.avatar_path).SingleOrDefault(),
                    ten_cong_ty =
                    repo._context.sys_companys.Where(q => q.id == repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_company).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                    school_year = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.school_year).SingleOrDefault(),
                    faculty = repo._context.sys_khoas.Where(q => q.id == repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_khoa).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                    
                    role_view = d.role,
                    // ten_quoc_gia = repo._context.sys_countrys.Where(q => q.id == repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_country).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                    email = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.email).SingleOrDefault(),
                    dienthoai = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.phone).SingleOrDefault(),
                }).ToList();


            return Json(list_danh_gia);
        }


        [HttpPost]
        public async Task<IActionResult> get_list_tham_du([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var role = int.Parse(json.GetValue("role").ToString());


            var list_nguoi_tham_du = repo._context.sys_event_participates.Where(d => d.event_id == event_id  && d.role ==role)
                .Select(d => new sys_event_contact_model()
                {
                    db = d,
                    user_name = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.FirstName + " " + q.LastName).SingleOrDefault(),
                    position = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_job_title).SingleOrDefault(),
                    avatar_link = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.avatar_path).SingleOrDefault(),
                    ten_cong_ty =
                    repo._context.sys_companys.Where(q => q.id == repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_company).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                    school_year = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.school_year ).SingleOrDefault(),
                    faculty = repo._context.sys_khoas.Where(q => q.id == repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_khoa).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                    role_view = d.role,
                    // ten_quoc_gia = repo._context.sys_countrys.Where(q => q.id == repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.id_country).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                    email = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.email).SingleOrDefault(),
                    dienthoai = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.phone).SingleOrDefault(),
                }).ToList();

            //var list_ban_to_chuc = repo._context.sys_event_contact_participates.Where(d =>d.event_id == event_id && d.role ==2)
            //  .Select(d => new sys_event_contact_participate_model()
            //  {
            //      db =d,
            //      user_name = repo._context.users.Where(q => q.Id == d.user_id).Select(q => q.FirstName + "" + q.LastName).SingleOrDefault()
            //  }).ToList();

            return Json(list_nguoi_tham_du);
        }
       

       

        
       
      
      

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_contact_model>(json.GetValue("data").ToString());
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

            
            var itemToRemove = repo._context.sys_event_participates.Where(x => x.id == id && x.check_in_status == 1).SingleOrDefault();
            if(itemToRemove != null)
            {
                repo._context.RemoveRange(itemToRemove);
                repo._context.SaveChanges();
                return Json("");
            }
            else
            {
                //0 đã confirm ko dc xoá
                return Json("0");
            };
          
            //repo.delete(id, user_id);
           
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
                var hinh_thuc = int.Parse(dictionary["hinh_thuc"]);
                if (trang_thai == -1)
                {
                    trang_thai = 0;
                }
                if (hinh_thuc == -1)
                {
                    hinh_thuc = 0;
                }
                var search = dictionary["search"];
                var event_id = dictionary["event_id"];
                var query = repo.FindAll()
                    .Where(d => d.db.check_in_status == trang_thai || trang_thai == 0)
                    .Where(d => d.db.role == hinh_thuc || hinh_thuc == 0)
                     .Where(d => d.db.event_id == event_id)
                     .Where(d => string.IsNullOrEmpty(search) || d.user_name.Contains(search) || d.ten_su_kien.Contains(search) ||
                     d.ten_quoc_gia.Contains(search) || d.ten_cong_ty.Contains(search) || d.faculty.Contains(search) || d.position.Contains(search))
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderByDescending(d => d.db.date_add).ToList());
                //var dataList = query.ToList();
                DTResult<sys_event_contact_model> result = new DTResult<sys_event_contact_model>
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
        public async Task<IActionResult> update_status_event([FromBody] JObject json)
        {
            try
            {
                //Mời mới hiện sự kiện lên

                var user_id = getUserId();
                var event_id = json.GetValue("event_id").ToString();
                var status = String.IsNullOrEmpty(json.GetValue("status").ToString())? "1": json.GetValue("status").ToString();
                var db = await repo._context.sys_event_participates.Where(d => d.event_id == event_id
                && d.user_id == user_id && d.role == 1).FirstOrDefaultAsync();
                db.check_in_status = int.Parse(status);
                repo._context.SaveChanges();
                //saveDetail(model);

                return generateSuscess();
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    

     
        [HttpPost]
        public async Task<IActionResult> getEventParticipate()
        {
            try
            {
                //Mời mới hiện sự kiện lên
                var user_id = getUserId();
                var a = Request;
                var query = repo.FindAll()
                    //.Where(d =>d.db.status_del == 1)
                          //.Where(d => d.db.time_start <= DateTime.Now.Date.AddDays(1))
                    .Where(d => repo._context.sys_event_participates.Where(t=>t.user_id == user_id && d.db.id == t.event_id).Count()>0)
                     ;
               

                var dataList = query.ToList();
                dataList.ForEach(
                    q => 
                    {
                        var check_in_status = repo._context.sys_event_participates.Where(
                            t => t.user_id == user_id && t.event_id == q.db.id && t.role ==1).Select(k => k.check_in_status).SingleOrDefault();
                        //q.db.check_in_status = check_in_status;
                   
                    }
                    ); 
                return Json(dataList);
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
