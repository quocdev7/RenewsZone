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
using worldsoft.DataBase.Helper;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Threading;
using worldsoft.common.Common;
using worldsoft.common.Models;

namespace worldsoft.system.web.Controller
{
    public partial class approved_eventController : BaseAuthenticationController
    {
        public approved_event_repo repo;
        private IMailService _mailService;
        private IUserService _userService;
        public approved_eventController(IUserService userService, IMailService mailService, worldsoftDefautContext context) : base(userService)
        {
            repo = new approved_event_repo(context);
            _userService = userService;
            _mailService = mailService;
        }



        public IActionResult count_notification_event()
        {

            var user_id = getUserId();
            var result = repo._context.sys_event_participates
                .Where(d => d.user_id == user_id && d.role == 1)
                 .Where(d => repo._context.sys_events.Where(q => q.id == d.event_id).Select(q => q.status_del == 1).SingleOrDefault())
                ;
            var su_kien_tham_gia = result.Where(q => q.check_in_status == 3).Count();
            var su_kien_duoc_moi = result.Where(q => q.check_in_status == 1).Count();
            var tong_su_kien = result.Where(q => q.check_in_status != 2).Count();

            var data = new
            {
                su_kien_tham_gia = su_kien_tham_gia,
                su_kien_duoc_moi = su_kien_duoc_moi,
                tong_su_kien = tong_su_kien

            };
            return Json(data);

        }
        public IActionResult countSuKien()
        {

            var user_id = getUserId();




            var result = repo._context.sys_event_participates
                .Where(d => d.user_id == user_id && d.role == 1)
                 .Where(d => repo._context.sys_events.Where(q => q.id == d.event_id).Select(q => q.status_del == 1).SingleOrDefault())
                ;
            var su_kien_tu_choi = result.Where(q => q.check_in_status == 2).Count();
            var su_kien_dong_y = result.Where(q => q.check_in_status == 3).Count();
            var tong_su_kien = su_kien_tu_choi + su_kien_dong_y;

            var data = new
            {
                su_kien_tu_choi = su_kien_tu_choi,
                su_kien_dong_y = su_kien_dong_y,
                tong_su_kien = tong_su_kien

            };
            return Json(data);

        }


        public IActionResult getListUse()
        {
            var now = DateTime.Now.Date;
            var result = repo._context.sys_events
                .Where(d => d.status_del == 1)
                .Where(d => (now >= d.time_start.Value.Date && d.time_end.Value.Date >= now) || (d.time_start.Value.Date >= now))
                .Select(d => new
                {
                    id = d.id,
                    name = d.title
                }).ToList();



            ;
            return Json(result);
        }

        public IActionResult getListUseAll()
        {
            var result = repo._context.sys_events
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.title
                 }).ToList();
            var itemAll = new { id = "-1", name = "Tất cả" };
            result.Insert(0, itemAll);
            //result.Add(new { id = "-1", name = "Tất cả" });
            return Json(result);
        }
       


      
        [HttpGet]
        public async Task<IActionResult> getMyEvent(string token)
        {
            var user_id = getUserId();
            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
            var list_history = 
                
                repo._context.sys_event_khach_mois
                .Where(d => d.email == email)
                .Select(t => new sys_my_event_model()
                {
                    id =t.id,
                    ly_do=t.ly_do,
                    id_su_kien = t.id_su_kien,
                    anh_su_kien =  repo._context.sys_events.Where(d=>d.id == t.id_su_kien ).Select(d=>d.logo).SingleOrDefault(),
                    ten_su_kien = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.title).SingleOrDefault(),
                    ngay_bat_dau = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.time_start).SingleOrDefault(),
                    ngay_ket_thuc = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.time_end).SingleOrDefault(),
                    check_in_status =  t.check_in_status,
                    ngay_thuc_hien = t.update_date??t.create_date

                }).OrderBy(d=>d.ngay_bat_dau).ToList();
            
            return Json(list_history);


        }



        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<approved_event_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;

   
            var email_cam_on = repo._context.sys_type_mails.Where(q=>q.code == "11").Select(q=>q.id).FirstOrDefault();
            var email_moi = repo._context.sys_type_mails.Where(q => q.code == "9").Select(q => q.id).FirstOrDefault();
            model.db.id_template_invite = repo._context.sys_template_mails.Where(q=>q.id_type == email_moi).Select(q=>q.id).FirstOrDefault();
            model.db.id_template_thanks = repo._context.sys_template_mails.Where(q => q.id_type == email_cam_on).Select(q => q.id).FirstOrDefault();
            model.trangthai_dangky = model.db.is_register_event == 1 ? "Công khai" : "Riêng tư";

            if (model.db.logo == null)
            {
                model.db.logo = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 3).Select(q => q.image).FirstOrDefault(); //_appsetting.avatar;
            }

            await repo.insert(model);
            model = await repo.getElementById(model.db.id);  

            return Json(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> get_list_hinh_anh([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var list = repo._context.sys_anh_noi_bat_su_kiens.Where(d => d.id_su_kien == event_id)
                .Select(d => new sys_anh_noi_bat_su_kien_model()
                {
                    db = d,

                }).ToList();
          
            return Json(list);

        }

        [HttpPost]
        public async Task<IActionResult> get_list_event_program([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var list_chuong_trinh = repo._context.sys_event_programs.Where(d => d.event_id == event_id)
                .Select(d => new sys_event_program_model()
                {
                    db = d,
                    ten_dien_gia = repo._context.sys_dien_gias.Where(q=>q.id== d.id_dien_gia).Select(q=>q.name).FirstOrDefault(),
                    chuc_danh = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.chuc_danh).FirstOrDefault(),

                    anh_dai_dien = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.image).FirstOrDefault(),


                }).ToList();
            //return Json(new {
            //    list_chuong_trinh = list_chuong_trinh,
            //});
            list_chuong_trinh = list_chuong_trinh.OrderBy(q => q.db.start_time).ToList();
            return Json(list_chuong_trinh);

        }

        [HttpPost]
        public async Task<IActionResult> get_list_file([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var list_file = repo._context.sys_event_files.Where(d => d.event_id == event_id)
                .Select(d => new sys_event_file_model()
                {
                    db = d

                }).ToList();
            //return Json(new {
            //    list_chuong_trinh = list_chuong_trinh,
            //});
            return Json(list_file);

        }
        [HttpPost]
        public async Task<IActionResult> get_list_image([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var list_file = repo._context.sys_event_images.Where(d => d.event_id == event_id)
                .Select(d => new sys_event_image_model()
                {
                    db = d,

                }).ToList();
            //return Json(new {
            //    list_chuong_trinh = list_chuong_trinh,
            //});
            return Json(list_file);

        }
      

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<
                approved_event_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            if (model.db.logo == null)
            {
                model.db.logo = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 3).Select(q => q.image).FirstOrDefault(); //_appsetting.avatar;
            }
            await repo.update(model);
            model = await repo.getElementById(model.db.id);
            return Json(model);
        }

      
        public async Task<IActionResult> approval([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var user_id = getUserId();
            repo.approval(id, user_id);
            return Json("");
        }
        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var reason = json.GetValue("reason").ToString();
            repo.delete(id, getUserId(), reason);
            return Json("");
        }

        public async Task<IActionResult> getElementById([FromBody] JObject json)
        {
            string id = json.GetValue("id").ToString();
            var model = await repo.getElementById(id);
            return Json(model);
        }
        
         public async Task<IActionResult> getEventCurrents([FromBody] JObject json)
        {
            var now = DateTime.Now.Date;
            var user_id = getUserId();
            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
            var query = repo.FindAllNewPortal(user_id)
                    .OrderByDescending(t => t.db.time_start)
                    .Where(d => d.db.status_del == 1).Take(4).ToList();
            query.ForEach(t =>
            {
                if (t.db.status_del == 2) t.trang_thai = 2;
                if (t.db.status_del == 1)
                {
                    if (now >= t.db.time_start.Value.Date && t.db.time_end.Value.Date >= now)
                    {
                        t.trang_thai = 1;
                    }
                    if (now > t.db.time_end.Value.Date)
                    {
                        t.trang_thai = 3;
                    }
                    if (t.db.time_start.Value.Date >= now)
                    {
                        t.trang_thai = 4;
                    }
                }
                if (user_id != null)
                {
                    t.check_in_status = (repo._context.sys_event_khach_mois.Where(
                        d => d.email == email && d.id_su_kien == t.db.id).Select(k => k.check_in_status).SingleOrDefault()) ?? 0;

                }
                else
                {
                    t.check_in_status = 0;
                }
                if (t.db.ngay_den_han_dang_ky >= now &&  t.trang_thai ==4   &&  t.check_in_status == 0 && t.db.is_register_event == 1) t.cho_phep_dang_ky = 1;
            });
            return Json(query);
        }
        public async Task<IActionResult> getEvents([FromBody] JObject json)
        {
            var now = DateTime.Now.Date;
            var user_id = getUserId();
            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
            var query = repo.FindAllNewPortal(user_id).Where(q=>q.db.status_del!=2)
                     .OrderByDescending(t => t.db.time_start)
                    .ToList();
            query.ForEach(t =>
            {
                if (t.db.status_del == 2) t.trang_thai = 2;
                if (t.db.status_del == 1)
                {
                    if (now >= t.db.time_start.Value.Date && t.db.time_end.Value.Date >= now)
                    {
                        t.trang_thai = 1;
                    }
                    if (now > t.db.time_end.Value.Date)
                    {
                        t.trang_thai = 3;
                    }
                    if (t.db.time_start.Value.Date >= now)
                    {
                        t.trang_thai = 4;
                    }
                }
               
                if (user_id != null) {
                    t.check_in_status = (repo._context.sys_event_khach_mois.Where(
                        d => d.email == email && d.id_su_kien == t.db.id).Select(k => k.check_in_status).SingleOrDefault()) ?? 0;

                }
                else
                {
                    t.check_in_status = 0;
                }

                if (t.db.ngay_den_han_dang_ky >= now && t.trang_thai == 4  && t.check_in_status == 0 && t.db.is_register_event == 1) t.cho_phep_dang_ky = 1;


            });
            return Json(query);
        }
     
        public async Task<IActionResult> getDetailEvent([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var user_id = getUserId();
            var model = await repo.getElementById(id);
            if (user_id != null)
            {
                var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
              
            }
            var now = DateTime.Now.Date;
            if (model.db.status_del == 2) model.trang_thai = 2;
                if (model.db.status_del == 1)
                {
                    if (now >= model.db.time_start.Value.Date && model.db.time_end.Value.Date >= now)
                    {
                    model.trang_thai = 1;
                    }
                    if (now > model.db.time_end.Value.Date)
                    {
                    model.trang_thai = 3;
                    }
                    if (model.db.time_start.Value.Date >= now)
                    {
                    model.trang_thai = 4;
                    }
                }

            if (user_id != null)
            {
                var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
                model.check_in_status = (repo._context.sys_event_khach_mois.Where(
                    d => d.email == email && d.id_su_kien == model.db.id).Select(k => k.check_in_status).SingleOrDefault()) ?? 0;

            }
            else
            {
                model.check_in_status = 0;
            }
            if (model.db.ngay_den_han_dang_ky >= now && model.trang_thai == 4  && model.check_in_status == 0 && model.db.is_register_event == 1) model.cho_phep_dang_ky = 1;

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

                var search = dictionary["search"];

                var id_hinh_thuc = dictionary["id_hinh_thuc"];
                var is_register_event = int.Parse(dictionary["is_register_event"]);

                var tuNgay = new DateTime(1900, 1, 1);
                var denNgay = new DateTime(1900, 1, 1);
                if (!string.IsNullOrEmpty(dictionary["tu_ngay"]))
                {
                    tuNgay = DateTime.Parse(dictionary["tu_ngay"]);
                }
                if (!string.IsNullOrEmpty(dictionary["tu_ngay"]))
                {
                    denNgay = DateTime.Parse(dictionary["den_ngay"]);
                }

                var query = repo.FindAll()
                    .Where(d=> id_hinh_thuc == "-1" ||d.db.type.Contains(id_hinh_thuc))
                    .Where(d => is_register_event == -1 || d.db.is_register_event == is_register_event)
                     .Where(d => d.db.time_start >= tuNgay && d.db.time_end <= denNgay)

                    .Where(d => string.IsNullOrEmpty(search) || d.db.title.Contains(search) || d.db.intro.Contains(search)|| d.db.location.Contains(search))
                     ;
                var now = DateTime.Now.Date;
                if (trang_thai == -1)
                {

                }

             

                if (trang_thai == 2)
                {
                    query = query.Where(d => d.db.status_del == trang_thai);
                }
                if (trang_thai != -1 && trang_thai != 2)
                {
                    query = query.Where(d => d.db.status_del == 1);
                    if (trang_thai == 1)
                    {

                        query = query.Where(d => now >= d.db.time_start.Value.Date && d.db.time_end.Value.Date >= now);
                    }
                    if (trang_thai == 3)
                    {

                        query = query.Where(d => now > d.db.time_end.Value.Date);
                    }
                    if (trang_thai == 4)
                    {

                        query = query.Where(d => d.db.time_start.Value.Date >= now);
                    }


                }

                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderByDescending(d => d.db.time_start).ToList());
                //var dataList = query.ToList();
                //1 .dang_dien_ra 2 .cancel  3 .ket_thuc 4 .sap_toi
                dataList.ForEach(t =>
                {
                    t.types = (t.db.type ?? "").Split(",").ToList();
                    t.khoa = (t.db.id_khoa ?? "").Split(",").ToList();
                    t.hinhthuc = (t.db.hinh_thuc_user ?? "").Split(",").ToList();
                    if (t.db.status_del == 2) t.trang_thai = 2;
                    if (t.db.status_del == 1)
                    {
                        if (now >= t.db.time_start.Value.Date && t.db.time_end.Value.Date >= now)
                        {
                            t.trang_thai = 1;
                        }
                        if (now > t.db.time_end.Value.Date)
                        {
                            t.trang_thai = 3;
                        }
                        if (t.db.time_start.Value.Date >= now)
                        {
                            t.trang_thai = 4;
                        }
                    }

                });


                DTResult<approved_event_model> result = new DTResult<approved_event_model>
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
        public async Task<IActionResult> register_event([FromBody] JObject json)
        {
            try
            {
                //Mời mới hiện sự kiện lên

                var user_id = getUserId();
                var user = repo._context.users.Where(d => d.Id == user_id).SingleOrDefault();
                var event_id = json.GetValue("event_id").ToString();
                var status = 6;

                var db =  repo._context.sys_event_khach_mois.Where(d => d.id_su_kien == event_id
                && d.email == user_id ).FirstOrDefault();
                if (db == null)
                {
                    db = new sys_event_khach_moi_db
                    {
                        id_su_kien = event_id,
                        email = user.email,
                        check_in_status = status,
                        dien_thoai =user.phone,
                        create_by = user_id,
                        create_date =DateTime.Now,
                        name = user.full_name,
                        status_del=1,
                        id = Guid.NewGuid().ToString(),
                    };
                    repo._context.sys_event_khach_mois.Add(db);
                    repo._context.SaveChanges();
                }
                else
                {
                    db.check_in_status = status;
                    repo._context.SaveChanges();
                }
                return generateSuscess();
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

       

        [HttpPost]
        public async Task<IActionResult> getQAEvent([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            var list_qa = repo._context.sys_event_qas.Where(d => d.event_id == event_id)
                .Select(d => new sys_event_qa_model()
                {
                    db = d,
                    user_answer = repo._context.users.Where(q => q.Id == d.user_id_answer).Select(q => q.FirstName + " " + q.LastName).SingleOrDefault(),
                    user_question = repo._context.users.Where(q => q.Id == d.user_id_question).Select(q => q.FirstName + " " + q.LastName).SingleOrDefault(),

                }).ToList();
            //return Json(new {
            //    list_chuong_trinh = list_chuong_trinh,
            //});
            return Json(list_qa);

        }


   

     
    }
}
