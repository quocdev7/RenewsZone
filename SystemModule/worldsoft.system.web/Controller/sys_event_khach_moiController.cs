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
using worldsoft.common.Common;
using worldsoft.common.Models;
using System.Net;
using worldsoft.fireBase.API;

namespace worldsoft.system.web.Controller
{
    public partial class sys_event_khach_moiController : BaseAuthenticationController
    {

        public sys_event_khach_moi_repo repo;
        private IMailService _mailService;
        public SendNotificationApi firebaseAPI;
        public sys_event_khach_moiController(IUserService userService, worldsoftDefautContext context, IMailService mailService) : base(userService)
        {
            repo = new sys_event_khach_moi_repo(context);

            _mailService = mailService;
            //firebaseAPI = new SendNotificationApi(context);
        }
      
        

        public IActionResult getListUse()
        {
            var result = repo._context.sys_event_khach_mois
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     avatar_path = d.avatar_path,
                     count = repo._context.sys_event_khach_mois.Where(t => t.id == d.id && d.status_del == 1).Count(),

                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUseDuocMoi([FromBody] JObject json)

        {
            var id = json.GetValue("id").ToString();
            var result = repo._context.sys_event_khach_mois
                .Where(d => d.status_del == 1 && d.check_in_status == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     avatar_path = d.avatar_path,


                 }).ToList();
            return Json(result);
        }
  
        public IActionResult getListUseThamGia([FromBody] JObject json)

        {

            var id = json.GetValue("id").ToString();

            var result = repo._context.sys_event_khach_mois
                .Where(d => d.id_su_kien == d.id &&  d.check_in_status == 2  ).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     avatar_path = d.avatar_path,
 

                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUseDangKyVaDuocMoi([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var result = repo._context.sys_event_khach_mois
                .Where(d => d.status_del == 1 && (d.check_in_status == 1 || d.check_in_status == 6) ).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     avatar_path = d.avatar_path,


                 }).ToList();
            return Json(result);
        }
        public IActionResult getListUseDangKy([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var result = repo._context.sys_event_khach_mois
                .Where(d => d.status_del == 1 &&  d.check_in_status == 6).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     avatar_path = d.avatar_path,


                 }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {


            var model = JsonConvert.DeserializeObject<sys_event_khach_moi_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.check_in_status = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;

            await repo.insert(model);
            send_mail(model, "9");

            //Quản lý sự kiện_Lời mời tham gia sự kiện từ người khác
            var type = "13";
            //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");
            var sk = repo._context.sys_events.Where(q => q.id == model.db.id_su_kien).FirstOrDefault();
            var user_recive = new List<String>();
            var lst_user_dk = repo._context.sys_event_khach_mois.Where(q => q.id_su_kien == model.db.id_su_kien && q.email ==model.db.email).Select(q => q.email).Distinct().ToList();
            for (int i = 0; i < lst_user_dk.Count; i++)
            {
                var email = lst_user_dk[i];
                var user = repo._context.users.Where(q => q.email == email).Select(q => q.Id).FirstOrDefault();
                if (user == null) continue;
                user_recive.Add(user);
            }

            var modelN = new sys_notification_ref_model();
            modelN.ho_va_ten_nguoi_gui_loi_moi_tham_gia_su_kien = repo._context.users.Where(q => q.Id == model.db.create_by).Select(q => q.full_name).FirstOrDefault();
            //await firebaseAPI.send_notificatin_web(getUserId(), type, modelN, user_recive);


            return Json(model);
        }


        [HttpPost]
        public async Task<IActionResult> gui_thu_cam_on([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var now = DateTime.Now.Date;
            var error = "";
            var sk_ketthuc = repo._context.sys_events.Where(q => q.id == id && now > q.time_end.Value.Date).FirstOrDefault();

            if(sk_ketthuc != null)
            {
                var lst_mail = repo.FindAll().Where(q => q.db.id_su_kien == id && q.db.check_in_status == 1).ToList();
                if (lst_mail.Count() > 0)
                {
                    send_list_mail(id, lst_mail, "11");

                }
                else
                {
                    //Chưa có khách mời trong sự kiện này
                    error = "1";
                }



            }
            else
            {
                //Sự kiện chưa kết thúc.Bạn chưa thể gửi thư cảm ơn.
                error = "2";
            }

            return Json(error);
        }
            [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_khach_moi_model>(json.GetValue("data").ToString());
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


        

        public async Task<IActionResult> update_status([FromBody] JObject json)
        {


           
            var id = json.GetValue("id").ToString();
            var status = json.GetValue("status").ToString();
            var ly_do = json.GetValue("ly_do").ToString();
            repo.update_status(id, status, ly_do, getUserId());
            if(status == "5")
            {
                //Sự kiện_Không đủ điều kiện tham gia sự kiện
                var model = await repo.getElementById(id);
                send_mail(model, "26");

                //Quản lý sự kiện_Không đủ điều kiện tham gia sự kiện
                var type = "15";

             
                var dk = repo._context.sys_event_khach_mois.Where(q => q.id == id).SingleOrDefault();
                var user_id = repo._context.users.Where(q => q.email == dk.email).Select(q => q.Id).FirstOrDefault();
                var su_kien = repo._context.sys_events.Where(q => q.id == dk.id_su_kien).FirstOrDefault();
                
                var user_recive = new List<String>();
                user_recive.Add(user_id);
               

                var modelN = new sys_notification_ref_model();
                modelN.ten_su_kien = su_kien.title ?? "";
                modelN.ly_do_khong_duyet_dang_ky_tham_gia = ly_do ?? "";

                //await firebaseAPI.send_notificatin_web(getUserId(), type, modelN, user_recive);

                //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");
            }

            if (status == "3")
            {
                //Sự kiện _Vé tham dự sự kiện
                var model = await repo.getElementById(id);
                send_mail(model, "24");

                //Quản lý sự kiện_Yêu cầu đăng ký tham gia sự kiện được chấp thuận
                var type2 = "14";

                var dk = repo._context.sys_event_khach_mois.Where(q => q.id == id).SingleOrDefault();
                var user_id = repo._context.users.Where(q => q.email == dk.email).Select(q => q.Id).FirstOrDefault();
                var su_kien = repo._context.sys_events.Where(q => q.id == dk.id_su_kien).FirstOrDefault();

                var user_recive = new List<String>();
                user_recive.Add(user_id);


                var modelN = new sys_notification_ref_model();
                modelN.ten_su_kien = su_kien.title ?? "";

                //await firebaseAPI.send_notificatin_web(getUserId(), type2, modelN, user_recive);



                //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");

            }

            return Json("");
        }
        [HttpPost]
        public async Task<IActionResult> tuchoi([FromBody] JObject json)
        {
           

            var id = json.GetValue("id").ToString();
                var ly_do = json.GetValue("ly_do").ToString();
                var db = repo._context.sys_event_khach_mois.Where(d => d.id == id).FirstOrDefault();
                db.update_date = DateTime.Now;
                db.check_in_status = 2;
                 db.ly_do = ly_do;
                repo._context.SaveChanges();

            //Quản lý sự kiện_Lời mời tham gia sự kiện không được chấp nhận
            var model = await repo.getElementById(db.id);
            var type = "17";
            var loi_moi = repo._context.sys_event_khach_mois.Where(q => q.id == id).SingleOrDefault();
            var user_id = repo._context.users.Where(q => q.email == loi_moi.email).Select(q => q.Id).FirstOrDefault();
            var su_kien = repo._context.sys_events.Where(q => q.id == loi_moi.id_su_kien).FirstOrDefault();

            var user_recive = new List<String>();
            user_recive.Add(user_id);


            var modelN = new sys_notification_ref_model();
            modelN.ho_va_ten_nguoi_nhan_loi_moi_tham_gia_su_kien = repo._context.users.Where(q => q.Id == model.db.create_by).Select(q => q.full_name).FirstOrDefault();
            modelN.ten_su_kien = su_kien.title ?? "";

            //await firebaseAPI.send_notificatin_web(getUserId(), type, modelN, user_recive);
            //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");


            //Quản lý sự kiện_Thành viên hủy bỏ tham gia sự kiện
            var type2 = "18";
            modelN.ho_va_ten_nguoi_tham_gia_su_kien = repo._context.users.Where(q => q.Id == model.db.create_by).Select(q => q.full_name).FirstOrDefault();
            modelN.ten_su_kien = su_kien.title ?? "";
            //await firebaseAPI.send_notificatin_web(getUserId(), type2, modelN, user_recive);
            //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");
            return Json("");

        }
        [HttpPost]
        public async Task<IActionResult> thamgia([FromBody] JObject json)
        {
            //Quản lý sự kiện_Lời mời tham gia sự kiện được chấp nhận
            var type = "16";
            //await firebaseAPI.sendNotification(model.db.Id, model.db.full_name, epochTime, model, user_recive, _appsetting.domain, model, "");

            var id = json.GetValue("id").ToString();
                var db = repo._context.sys_event_khach_mois.Where(d => d.id == id).FirstOrDefault();
                db.update_date = DateTime.Now;
                db.check_in_status = 3;
                repo._context.SaveChanges();
                var model = await repo.getElementById(db.id);
                send_mail(model, "24");

            var loi_moi = repo._context.sys_event_khach_mois.Where(q => q.id == id).SingleOrDefault();
            var user_id = repo._context.users.Where(q => q.email == loi_moi.email).Select(q => q.Id).FirstOrDefault();
            var su_kien = repo._context.sys_events.Where(q => q.id == loi_moi.id_su_kien).FirstOrDefault();

            var user_recive = new List<String>();
            user_recive.Add(user_id);


            var modelN = new sys_notification_ref_model();
            modelN.ho_va_ten_nguoi_nhan_loi_moi_tham_gia_su_kien = repo._context.users.Where(q => q.Id == model.db.create_by).Select(q => q.full_name).FirstOrDefault();
            modelN.ten_su_kien = su_kien.title ?? "";
            //await firebaseAPI.send_notificatin_web(getUserId(), type, modelN, user_recive);
            return Json("");
        }



        [HttpGet]
        public async Task<IActionResult> thamgia(string token)
        {
            try
            {


                var decrypt = CMAESCrypto.DecryptText(token);
                var email = decrypt.Split("@@")[0];
                var id_su_kien = decrypt.Split("@@")[1];
                var ten_su_kien = repo._context.sys_events.Where(d => d.id == id_su_kien && d.status_del == 1 && d.ngay_den_han_dang_ky >= DateTime.Now.Date).Select(d => d.title).SingleOrDefault();
                var db = repo._context.sys_event_khach_mois.Where(d => d.id_su_kien == id_su_kien && d.email == email).FirstOrDefault();
                if (db.check_in_status != 1 || string.IsNullOrEmpty(ten_su_kien)) return Json("Đường dẫn này đã hết hiệu lực");
                db.update_date = DateTime.Now;
                db.check_in_status = 3;
                repo._context.SaveChanges();

               
                var model = await repo.getElementById(db.id);
                send_mail(model, "24");
                var type_mail = repo._context.sys_type_mails.Where(t => t.code == "25").FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();
                var body = generateEmailContent(model, maumail.template).Normalize();
                return Content(body, "text/html", System.Text.Encoding.UTF8);

                
            }
            catch (Exception e)
            {
                return Json("invalid");
            }
            return Json("invalid");
        
        }

        [HttpGet]
        public async Task<IActionResult> tuchoi(string token)
        {
            try
            {


                var decrypt = CMAESCrypto.DecryptText(token);
                var email = decrypt.Split("@@")[0];
                var id_su_kien = decrypt.Split("@@")[1];

                var ten_su_kien = repo._context.sys_events.Where(d => d.id == id_su_kien && d.status_del ==1 && d.ngay_den_han_dang_ky >= DateTime.Now.Date).Select(d => d.title).SingleOrDefault();
                var db = repo._context.sys_event_khach_mois.Where(d => d.id_su_kien == id_su_kien && d.email == email).FirstOrDefault();
                if(db.check_in_status !=1 || string.IsNullOrEmpty(ten_su_kien)) return Json("Đường dẫn này đã hết hiệu lực");
                db.check_in_status = 2;
                db.update_date = DateTime.Now;
                repo._context.SaveChanges();

                var model = await repo.getElementById(db.id);
                var type_mail = repo._context.sys_type_mails.Where(t => t.code == "28").FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();
                var body = generateEmailContent(model, maumail.template);
                return Content(body, "text/html", System.Text.Encoding.UTF8);
            }
            catch (Exception e)
            {
                return Json("invalid");
            }
            return Json("invalid");
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
                var event_id = dictionary["event_id"];
                var check_in_status = int.Parse(dictionary["check_in_status"]);
                var query = repo.FindAll()
                     .Where(d => d.db.name.Contains(search) || d.db.dien_thoai.Contains(search) || d.db.email.Contains(search))
                     ;
                query = query.Where(d => d.db.check_in_status == check_in_status || check_in_status==-1)
                    .Where(d => d.db.id_su_kien == event_id  )
                    ;

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderBy(d => d.db.update_date).Skip(param.Start).Take(param.Length)
        .ToList());
                DTResult<sys_event_khach_moi_model> result = new DTResult<sys_event_khach_moi_model>
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
