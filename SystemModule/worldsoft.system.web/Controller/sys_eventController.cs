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
using worldsoft.fireBase.API;

namespace worldsoft.system.web.Controller
{
    public partial class sys_eventController : BaseAuthenticationController
    {
        public sys_event_repo repo;
        private IMailService _mailService;
        private IUserService _userService;
        public SendNotificationApi firebaseAPI;
        public sys_eventController(IUserService userService, IMailService mailService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_event_repo(context);
            _userService = userService;
            _mailService = mailService;
            //firebaseAPI = new SendNotificationApi(context);
        }

        public IActionResult get_title_event([FromBody] JObject json)
        {
            var id_event = json.GetValue("id_event").ToString();
            var title_event = repo._context.sys_events.Where(d => d.id == id_event).Select(d => d.title).SingleOrDefault()
                .Replace("<", "").Replace(">", "").Replace("&", "").Replace("\'", "").Replace("\"", "")
                .Replace("- ".ToString(), string.Empty).Replace("-".ToString(), " ");
            var tieu_de = title_event.Trim();
            tieu_de = StringFunctions.NonUnicode(tieu_de).Replace(' ', '-');
            if (tieu_de.Length > 150)
                tieu_de = tieu_de.Substring(0, 150);
            return Json(tieu_de);
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
            //08 
            //09 - 16
            var now = DateTime.Now.Date;
            var result = repo._context.sys_events
                //.Where(d => d.status_del == 1)
                .Where(d => (now >= d.time_start.Value.Date && d.time_end.Value.Date >= now) || (d.time_start.Value.Date >= now))
                .Select(d => new
                {
                    id = d.id,
                    name = d.title
                }).ToList();



            ;
            return Json(result);
        }
        public IActionResult getListUseKhachMoi()
        {
            //08 
            //09 - 16
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
        public IActionResult getListEventUser()
        {
            //08 
            //09 - 16

            var user_id = getUserId();
            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
         
            var now = DateTime.Now.Date;
            var result = repo._context.sys_events
                .Where(d => d.create_by == user_id 
                || repo._context.sys_event_khach_mois.Where(q => q.email == email && d.id ==q.id_su_kien).Count() >0)
            
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
                    id = t.id,
                    ly_do = t.ly_do,
                    id_su_kien = t.id_su_kien,
                    anh_su_kien = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.logo).SingleOrDefault(),
                    ten_su_kien = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.title).SingleOrDefault(),
                    ngay_bat_dau = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.time_start).SingleOrDefault(),
                    ngay_ket_thuc = repo._context.sys_events.Where(d => d.id == t.id_su_kien).Select(d => d.time_end).SingleOrDefault(),
                    check_in_status = t.check_in_status,
                    ngay_thuc_hien = t.update_date ?? t.create_date

                }).OrderBy(d => d.ngay_bat_dau).ToList();

            return Json(list_history);


        }

        public int checkRole(sys_event_model model)
        {
            var role = 3;
            var cau_hinh = repo._context.sys_cau_hinh_duyet_su_kiens.Where(a => a.user_id == getUserId()).FirstOrDefault();
            var user = repo._context.users.Where(q => q.Id == getUserId()).FirstOrDefault();
            if (user.Username == "administrator")
            {
                role = 1;
            }
            else
            {

                //A B

                //A
                //B
                //A B
                //A B C

                if (cau_hinh != null)
                {

                    if (cau_hinh.id_khoa.Contains("-1"))
                    {
                        role = 1;
                    }
                    else
                    {

                        var khoas = cau_hinh.id_khoa.Split(",").ToList();
                        for (var i = 0; i < model.khoa.Count; i++)
                        {
                            var id_khoa = model.khoa[i];

                            if (khoas.Contains(id_khoa)) { role = 1; }
                            else {
                                role = 3;
                            }

                        }

                    }
                }
                


            }

            return role;
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;

            var status_del = checkRole(model);

            if (status_del == 1)
            {
                model.db.update_by = getUserId();
                model.db.update_date = DateTime.Now;
            }
            model.db.status_del = status_del;

            var email_cam_on = repo._context.sys_type_mails.Where(q => q.code == "11").Select(q => q.id).FirstOrDefault();
            var email_moi = repo._context.sys_type_mails.Where(q => q.code == "9").Select(q => q.id).FirstOrDefault();
            model.db.id_template_invite = repo._context.sys_template_mails.Where(q => q.id_type == email_moi).Select(q => q.id).FirstOrDefault();
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
            var list = repo._context.sys_anh_noi_bat_su_kiens.Where(d => d.id_su_kien == event_id && d.status_del==1)
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
            var list_chuong_trinh = repo._context.sys_event_programs.Where(d => d.event_id == event_id && d.status_del==1)
                .OrderBy(d => d.start_time)
                .Select(d => new sys_event_program_model()
                {
                    db=d,
                    ten_dien_gia = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.name).FirstOrDefault(),
                    chuc_danh = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.chuc_danh).FirstOrDefault(),
                    ten_dien_gia_en = repo._context.sys_dien_gia_ens.Where(q => q.id_dien_gia == d.id_dien_gia).Select(q => q.name).FirstOrDefault(),
                    chuc_danh_en = repo._context.sys_dien_gia_ens.Where(q => q.id_dien_gia == d.id_dien_gia).Select(q => q.chuc_danh).FirstOrDefault(),
                    anh_dai_dien = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.image).FirstOrDefault(),
                }).Distinct().ToList();
            list_chuong_trinh.ForEach(p =>
            {
                var language = repo._context.sys_event_program_ens.Where(d => d.id_event_program == p.db.id).FirstOrDefault();


                if (language != null)
                {
                    p.name_en = language.name ?? "";
                    p.description_en = language.description ?? "";
                }
            });
            return Json(list_chuong_trinh);

        }

        [HttpPost]
        public async Task<IActionResult> get_list_group_event_program([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();

            // ct1 22/06/2022 10h  - 22/06/2022 12h
            // ct2 22/06/2022 13h  - 22/06/2022 14h
            var group_data = new List<sys_event_program_ref_model>();

            var data = repo._context.sys_event_programs.Where(d => d.event_id == event_id && d.status_del ==1).OrderBy(d => d.start_time).Select(
                d => new sys_event_program_ref_model
                {
                    start_time_day = d.start_time.Value.Day,
                    start_time_month = d.start_time.Value.Month,
                    start_time_year = d.start_time.Value.Year,
                    start_time = d.start_time
                }).ToList();

            try
            {
                group_data = data.GroupBy(q => new { q.start_time_day, q.start_time_month, q.start_time_year })
                .Select(g => new sys_event_program_ref_model
                {
                    start_time_day = g.Key.start_time_day,
                    start_time_month = g.Key.start_time_month,
                    start_time_year = g.Key.start_time_year,
                    start_time = g.First().start_time,
                    list_chuong_trinh = repo._context.sys_event_programs.Where(d => d.event_id == event_id)
                        .Where(d => d.start_time.Value.Day == g.First().start_time_day
                            && d.start_time.Value.Month == g.First().start_time_month
                            && d.start_time.Value.Year == g.First().start_time_year
                        ).Select(d => new sys_event_program_model()
                        {
                            db = d,
                            ten_dien_gia = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.name).FirstOrDefault(),
                            chuc_danh = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.chuc_danh).FirstOrDefault(),
                            ten_dien_gia_en = repo._context.sys_dien_gia_ens.Where(q => q.id_dien_gia == d.id_dien_gia).Select(q => q.name).FirstOrDefault(),
                            chuc_danh_en = repo._context.sys_dien_gia_ens.Where(q => q.id_dien_gia == d.id_dien_gia).Select(q => q.chuc_danh).FirstOrDefault(),
                            anh_dai_dien = repo._context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.image).FirstOrDefault(),

                        }).ToList()
                  
                }).ToList();

            }
            catch (Exception e)
            {

            }






            return Json(group_data);

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

        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_event = json.GetValue("id_event").ToString();

            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_event_en_model();
            model = repo._context.sys_event_ens.Where(d => d.id_event == id_event).Select(d => new sys_event_en_model()
            {

                db = d

            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_event_en_model();
                model.db.id_event = id_event;
            }
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_model>(json.GetValue("data").ToString());
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

            var status_del = checkRole(model);

            if (status_del == 1)
            {
                model.db.update_by = getUserId();
                model.db.update_date = DateTime.Now;
            }
            model.db.status_del = status_del;

            await repo.update(model);
            model = await repo.getElementById(model.db.id);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> edit_en([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_en_model>(json.GetValue("data").ToString());
            var check = checkModelStateEditEn(model);
            if (!check)
            {
                return generateError();
            }

            var data = repo._context.sys_event_ens.Where(q => q.id_event == model.db.id_event).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_event_en_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.id_event = model.db.id_event;
                model_language.db.title = model.db.title;
                model_language.db.mo_ta = model.db.mo_ta;
                model_language.db.location = model.db.location;
                model_language.db.dieu_kien_tham_gia = model.db.dieu_kien_tham_gia;
                model_language.db.ban_to_chuc = model.db.ban_to_chuc;
                model_language.db.mo_ta_mobile = model.db.mo_ta_mobile;
                model_language.db.luu_y_tham_gia_mobile = model.db.luu_y_tham_gia_mobile;
                await repo.insert_en(model_language);
            }
            else
            {


                data.title = model.db.title;
                data.mo_ta = model.db.mo_ta;
                data.location = model.db.location;
                data.luu_y_tham_gia = model.db.luu_y_tham_gia;
                data.dieu_kien_tham_gia = model.db.dieu_kien_tham_gia;
                data.ban_to_chuc = model.db.ban_to_chuc;
                data.mo_ta_mobile = model.db.mo_ta_mobile;
                data.luu_y_tham_gia_mobile = model.db.luu_y_tham_gia_mobile;
                repo._context.SaveChanges();
            }


            return Json(model);
        }


        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            //Quản lý sự kiện_Sự kiện đã đăng ký bị hủy bỏ
            var type = "19";
            //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");

            var id = json.GetValue("id").ToString();
            var user_id = getUserId();

            var sk = repo._context.sys_events.Where(q => q.id == id).FirstOrDefault();
            var user_recive = new List<String>();
            var lst_user_dk = repo._context.sys_event_khach_mois.Where(q => q.id_su_kien == id && q.check_in_status == 1).Select(q => q.email).Distinct().ToList();
            for (int i= 0; i < lst_user_dk.Count; i++){
                var email = lst_user_dk[i];
                var user = repo._context.users.Where(q => q.email == email).Select(q=>q.Id).FirstOrDefault();
                if (user == null) continue;
                user_recive.Add(user);
            }

            var model = new sys_notification_ref_model();
            model.ten_su_kien = sk.title ?? "";
            model.ly_do_huy_bo_su_kien = "";

            //await firebaseAPI.send_notificatin_web(getUserId(), type, model, user_recive);
            //user_recive.Add();

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
                        d => d.email == email && d.id_su_kien == t.db.id).OrderByDescending(t=>t.create_date).Select(k => k.check_in_status).FirstOrDefault()) ?? 0;

                }
                else
                {
                    t.check_in_status = 0;
                }
                if (t.db.ngay_den_han_dang_ky >= now && t.trang_thai == 4 && t.check_in_status == 0 && t.db.is_register_event == 1) t.cho_phep_dang_ky = 1;

                query.ForEach(q =>
                {
                    var language = repo._context.sys_event_ens.Where(d => d.id_event == q.db.id).FirstOrDefault();

                    if (language != null)
                    {
                        q.title_en = language.title ?? "";
                        q.mo_ta_en = language.mo_ta ?? "";
                        q.ban_to_chuc_en = language.ban_to_chuc ?? "";
                        q.dieu_kien_tham_gia_en = language.dieu_kien_tham_gia ?? "";
                        q.location_en = language.location ?? "";
                        q.luu_y_tham_gia_en = language.luu_y_tham_gia ?? "";
                    }
                });
            });
            return Json(query);
        }

        public async Task<IActionResult> getEvents([FromBody] JObject json)
        {
            var now = DateTime.Now.Date;
            var user_id = getUserId();
            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
            var query = repo.FindAllNewPortal(user_id).Where(q => q.db.status_del != 2)
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

                if (user_id != null)
                {
                    t.check_in_status = (repo._context.sys_event_khach_mois.Where(
                           d => d.email == email && d.id_su_kien == t.db.id).OrderByDescending(t => t.create_date).Select(k => k.check_in_status).FirstOrDefault()) ?? 0;


                }
                else
                {
                    t.check_in_status = 0;
                }

                if (t.db.ngay_den_han_dang_ky >= now && t.trang_thai == 4 && t.check_in_status == 0 && t.db.is_register_event == 1) t.cho_phep_dang_ky = 1;
                query.ForEach(p =>
                {
                    var language = repo._context.sys_event_ens.Where(d => d.id_event == p.db.id).FirstOrDefault();

                    if (language != null)
                    {
                        p.title_en = language.title ?? "";
                        p.mo_ta_en = language.mo_ta ?? "";
                        p.ban_to_chuc_en = language.ban_to_chuc ?? "";
                        p.dieu_kien_tham_gia_en = language.dieu_kien_tham_gia ?? "";
                        p.location_en = language.location ?? "";
                        p.luu_y_tham_gia_en = language.luu_y_tham_gia ?? "";
                    }
                });

            });
            return Json(query);
        }
        public async Task<IActionResult> get_hot_events([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var now = DateTime.Now.Date;
            var user_id = getUserId();
            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
            var query = repo.FindAllNewPortal(user_id).Where(q => q.db.status_del != 2 && q.db.id != id)
                     .OrderByDescending(t => t.db.time_start).Skip(0).Take(4)
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

                if (user_id != null)
                {
                    t.check_in_status = (repo._context.sys_event_khach_mois.Where(
                        d => d.email == email && d.id_su_kien == t.db.id).OrderByDescending(t => t.create_date).Select(k => k.check_in_status).FirstOrDefault()) ?? 0;


                }
                else
                {
                    t.check_in_status = 0;
                }

                if (t.db.ngay_den_han_dang_ky >= now && t.trang_thai == 4 && t.check_in_status == 0 && t.db.is_register_event == 1) t.cho_phep_dang_ky = 1;


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
                    d => d.email == email && d.id_su_kien == model.db.id).OrderByDescending(d=>d.create_date).Select(k => k.check_in_status).FirstOrDefault()) ?? 0;

            }
            else
            {
                model.check_in_status = 0;
            }
            if (model.db.ngay_den_han_dang_ky >= now && model.trang_thai == 4 && model.check_in_status == 0 && model.db.is_register_event == 1) model.cho_phep_dang_ky = 1 ;


            var language = repo._context.sys_event_ens.Where(d => d.id_event == id).FirstOrDefault();

            if (language != null)
            {
                model.title_en = language.title ?? "";
                model.mo_ta_en = language.mo_ta ?? "";
                model.mo_ta_mobile_en = language.mo_ta_mobile ?? "";
                model.ban_to_chuc_en = language.ban_to_chuc ?? "";
                model.dieu_kien_tham_gia_en = language.dieu_kien_tham_gia ?? "";
                model.location_en = language.location ?? "";
                model.luu_y_tham_gia_en = language.luu_y_tham_gia ?? "";
                model.luu_y_tham_gia_mobile_en = language.luu_y_tham_gia_mobile ?? "";

            }
            return Json(model);
        }
        public async Task<IActionResult> approval([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var user_id = getUserId();
            repo.approval(id, user_id);

            var su_kien_duoc_phe_duyet = "6";
            send_mail(id, su_kien_duoc_phe_duyet);

            return Json("");
        }
        public async Task<IActionResult> cancel([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var reason = json.GetValue("reason").ToString();
            repo.cancel(id, getUserId(), reason);

            var su_kien_khong_duoc_phe_duyet = "7";
            send_mail(id, su_kien_khong_duoc_phe_duyet);
            return Json("");
        }
        [HttpPost]
        public async Task<IActionResult> send_mail(string id, string type)
        {


            try
            {


                var sk = repo._context.sys_events.Where(q => q.id == id).FirstOrDefault();

                var user = repo._context.users.Where(q => q.Id == sk.create_by).FirstOrDefault();
                var user_duyet = repo._context.users.Where(q => q.Id == sk.id_user_approval).FirstOrDefault();
                var email = user.email;
                var msg = "";
                var body = "";
                var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();

                //maumail.noidung ?? "";
                body = maumail.template;
                body = body.Replace("@@link@@", "https://" + Request.Host.Value);
                body = body.Replace("@@user_name@@", user.full_name);
                body = body.Replace("@@tieu_de_su_kien@@", sk.title);
                body = body.Replace("@@sdt_nguoi_duyet_su_kien@@", user_duyet.phone);
                body = body.Replace("@@Email_nguoi_duyet_su_kien@@", user_duyet.email);
                body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
                //body = body.Replace("@@url_reset@@", _appsetting.domain + "/systemverify.ctr/confirmResetPass?q=" + HttpUtility.UrlEncode(encryptconfirmparam));

                var dblogmail = new sys_log_mail_db();
                dblogmail.id = Guid.NewGuid().ToString();
                dblogmail.tieu_de = type_mail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = user.email;

                dblogmail.send_date = DateTime.Now;
                dblogmail.user_id = id;
                dblogmail.ket_qua = 0;
                //dblogmail.db.ngay_gui = DateTime.Now;
                //dblogmail.db.nguoi_gui = getUserId();
                repo._context.sys_log_mails.Add(dblogmail);
                repo._context.SaveChanges();
                var id_log_mail = dblogmail.id;
                try
                {
                    _mailService.SendEmailAsync(new MailRequest
                    {
                        Body = body,
                        Subject = dblogmail.tieu_de,
                        ToEmail = email, //CMAESCrypto.DecryptText(),
                        CCEmail = "",
                    });

                }
                catch (Exception e)
                {

                    return Json("error:" + e.ToString());

                }




            }
            catch
            {
                return Json("error");

            }
            return Json("");


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


                var query = Enumerable.Empty<sys_event_model>().AsQueryable();

                query = repo.FindAll()
                    .Where(d => id_hinh_thuc == "-1" || d.db.type.Contains(id_hinh_thuc))
                    .Where(d => is_register_event == -1 || d.db.is_register_event == is_register_event)
                     .Where(d => d.db.time_start >= tuNgay && d.db.time_start <= denNgay)

                    .Where(d => string.IsNullOrEmpty(search) || d.db.title.Contains(search) || d.db.intro.Contains(search) || d.db.location.Contains(search))
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





                //rule Sk
                // administrator  full
                // user quản trị  xem sk mình tạo || sự kiện thuộc ban tổ chức
                // user thường
                var cau_hinh = repo._context.sys_cau_hinh_duyet_su_kiens.Where(a => a.user_id == getUserId()).FirstOrDefault();
                var user = repo._context.users.Where(q => q.Id == getUserId()).FirstOrDefault();

                if (cau_hinh != null)
                {
                    if (user.Username == "administrator")
                    {

                    }
                    else
                    {
                        if (user.type == 1)
                        {
                            query = query.Where(q => q.db.create_by == getUserId()
                            || repo._context.sys_event_participates.Where(d => d.event_id == q.db.id && d.role == 1 && d.user_id == getUserId()).Count() > 0);

                        }


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


                DTResult<sys_event_model> result = new DTResult<sys_event_model>
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

                var db = repo._context.sys_event_khach_mois.Where(d => d.id_su_kien == event_id
                && d.email == user.email).FirstOrDefault();
                if (db == null)
                {
                    db = new sys_event_khach_moi_db
                    {
                        id_su_kien = event_id,
                        email = user.email,
                        check_in_status = status,
                        dien_thoai = user.phone,
                        create_by = user_id,
                        company = user.company,
                        position = user.position,
                        avatar_path = user.avatar_path,
                        create_date = DateTime.Now,
                        name = user.full_name,
                        status_del = 1,
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
        public async Task<IActionResult> getNguoiNhanHocBongEvent([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
            
            var list_nguoi_nhan_hoc_bong = repo._context.sys_event_nguoi_nhan_hoc_bongs.Where(d => d.id_su_kien == event_id && d.status_del == 1)
                .Select(d => new sys_event_nguoi_nhan_hoc_bong_model()
                {
                    db = d,
                    ten_su_kien = repo._context.sys_events.Where(t => t.id == d.id_su_kien).Select(d => d.title).SingleOrDefault(),
                    ten_tien_te = repo._context.sys_tien_tes.Where(t => t.id == d.id_tien_te).Select(d => d.name).SingleOrDefault(),

                }).ToList();

            return Json(list_nguoi_nhan_hoc_bong);

        }
        [HttpPost]
        public async Task<IActionResult> getNhaTaiTroEvent([FromBody] JObject json)
        {
            var event_id = json.GetValue("event_id").ToString();
       
            var list_nha_tai_tro = repo._context.sys_event_nha_tai_tros.Where(d => d.id_su_kien == event_id && d.status_del == 1).Where(d=>d.is_tai_tro == true)
                .Select(d => new sys_event_nha_tai_tro_model()
                {
                    db = d,
                    ten_su_kien = repo._context.sys_events.Where(t => t.id == d.id_su_kien).Select(d => d.title).SingleOrDefault(),
                    ten_tien_te = repo._context.sys_tien_tes.Where(t => t.id == d.id_tien_te).Select(d => d.name).SingleOrDefault(),

                }).ToList();

            return Json(list_nha_tai_tro);

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

            list_qa.ForEach(p =>
            {
                var language = repo._context.sys_event_qa_ens.Where(d => d.id_event_q_a == p.db.id).FirstOrDefault();

                if (language != null)
                {
                    p.question_en = language.question ?? "";
                    p.answer_en = language.answer ?? "";                  
                }
            });
            return Json(list_qa);


        }
        [HttpPost]
        public async Task<IActionResult> getQAEventnew()
        {
  
            var list_qa = repo._context.sys_event_qas
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
