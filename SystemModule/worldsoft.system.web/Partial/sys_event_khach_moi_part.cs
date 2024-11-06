using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.DataBase.System;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_event_khach_moiController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_event_khach_moi",
            icon = "badge",
            module = "system",
            id = "sys_event_khach_moi",
            url = "/sys_event_khach_moi_index",
            title = "sys_event_khach_moi",
            translate = "NAV.sys_event_khach_moi",
            type = "item",
            list_controller_action_public = new List<string>(){
                "sys_event_khach_moi;getListUse",
                "sys_event_khach_moi;getElementById",
                            "sys_event_khach_moi;gui_thu_cam_on"
                     

            },
            list_controller_action_publicNonLogin = new List<string>(){

                       "sys_event_khach_moi;getListUseDangKyVaDuocMoi",
                "sys_event_khach_moi;getListUse",
                                "sys_event_khach_moi;getListUseDuocMoi",
                                                "sys_event_khach_moi;getListUseThamGia",
                                                                "sys_event_khach_moi;getListUseDangKy",
                  "sys_event_khach_moi;thamgia",
                   "sys_event_khach_moi;tuchoi",

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_event_khach_moi;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_khach_moi;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_khach_moi;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_khach_moi;edit",
                           "sys_event_khach_moi;update_status",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_event_khach_moi;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_khach_moi;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_event_khach_moi;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_event_khach_moi;DataHandler",
                    }
                }
            }
        };
        private async Task send_list_mail(string id_su_kien,List<sys_event_khach_moi_model> lst, string type)
        {

            var rnd = new Random();
            var code = rnd.Next(11111, 99999).ToString();


          
            var msg = "";

            var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
            var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();

            for(int i=0;i< lst.Count; i++)
            {

                var model = lst[i];
                var body = generateListEmailContent(id_su_kien, model, maumail.template);


                var dblogmail = new sys_log_mail_db();

                dblogmail.tieu_de = type_mail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = model.db.email;
                dblogmail.id = Guid.NewGuid().ToString();
                dblogmail.send_date = DateTime.Now;
                dblogmail.ket_qua = 0;

                repo._context.sys_log_mails.Add(dblogmail);
                repo._context.SaveChanges();
                var id_log_mail = dblogmail.id;
                try
                {
                    _mailService.SendEmailAsync(new MailRequest
                    {
                        Body = body,
                        Subject = dblogmail.tieu_de,
                        ToEmail = model.db.email, //CMAESCrypto.DecryptText(),
                        CCEmail = "",
                    });

                }
                catch (Exception e)
                {



                }
            }

          


        }
        private async Task send_mail(sys_event_khach_moi_model model, string type)
        {

            var rnd = new Random();
            var code = rnd.Next(11111, 99999).ToString();
           

                var email = model.db.email;
                var msg = "";

                var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();
                var body = generateEmailContent(model, maumail.template);


               var dblogmail = new sys_log_mail_db();

                dblogmail.tieu_de = type_mail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = model.db.email;
                dblogmail.id = Guid.NewGuid().ToString();
                dblogmail.send_date = DateTime.Now;
                dblogmail.ket_qua = 0;

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



                }

          
       }

       private string  generateEmailContent(sys_event_khach_moi_model model, string template)
        {
            var body = "";
            var event1 = repo._context.sys_events.Where(t => t.id == model.db.id_su_kien).FirstOrDefault();
           
            //maumail.noidung ?? "";
            body = template;
            body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);



            if (model.db.check_in_status == 1)
            {
                var encryptconfirmparam = CMAESCrypto.EncryptText(model.db.email + "@@" + model.db.id_su_kien);
                body = body.Replace("@@link_tham_gia@@", "https://" + Request.Host.Value + "/sys_event_khach_moi.ctr/thamgia?token=" + HttpUtility.UrlEncode(encryptconfirmparam));
                body = body.Replace("@@link_tu_choi@@", "https://" + Request.Host.Value + "/sys_event_khach_moi.ctr/tuchoi?token=" + HttpUtility.UrlEncode(encryptconfirmparam));
            }
            body = body.Replace("@@link_su_kien@@", "https://" + Request.Host.Value + "/eventDetail/" + event1.id);

            body = body.Replace("@@full_name@@", model.db.name);
            body = body.Replace("@@ban_to_chuc@@", event1.ban_to_chuc);
            body = body.Replace("@@ten_su_kien@@", event1.title);
            body = body.Replace("@@ly_do@@", model.db.ly_do);
            body = body.Replace("@@email@@", model.db.email);
            body = body.Replace("@@sdt@@", model.db.dien_thoai);
            body = body.Replace("@@dia_diem@@", event1.location);
            body = body.Replace("@@ngay_bat_dau@@", event1.time_start.Value.ToString("dd/MM/yyyy HH:mm"));
            body = body.Replace("@@link_logo@@", event1.logo);
            body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
            return body;
        }

        private string generateListEmailContent(string id_su_kien,sys_event_khach_moi_model model , string template)
        {
            var body = "";
            var event1 = repo._context.sys_events.Where(t => t.id == id_su_kien).FirstOrDefault();

            //maumail.noidung ?? "";
            body = template;
            body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);



           
            var encryptconfirmparam = CMAESCrypto.EncryptText(model.db.email + "@@" + model.db.id_su_kien);
            body = body.Replace("@@link_tham_gia@@", "https://" + Request.Host.Value + "/sys_event_khach_moi.ctr/thamgia?token=" + HttpUtility.UrlEncode(encryptconfirmparam));
            body = body.Replace("@@link_tu_choi@@", "https://" + Request.Host.Value + "/sys_event_khach_moi.ctr/tuchoi?token=" + HttpUtility.UrlEncode(encryptconfirmparam));
            
            body = body.Replace("@@link_su_kien@@", "https://" + Request.Host.Value + "/eventDetail/" + event1.id);

            body = body.Replace("@@full_name@@", model.db.name);
            body = body.Replace("@@ban_to_chuc@@", event1.ban_to_chuc);
            body = body.Replace("@@ten_su_kien@@", event1.title);
            body = body.Replace("@@email@@", model.db.email);
            body = body.Replace("@@sdt@@", model.db.dien_thoai);
            body = body.Replace("@@dia_diem@@", event1.location);
            body = body.Replace("@@ngay_bat_dau@@", event1.time_start.Value.ToString("dd/MM/yyyy HH:mm"));
            body = body.Replace("@@link_logo@@", event1.logo);
            body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
            return body;
        }


        private bool checkModelStateCreate(sys_event_khach_moi_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_event_khach_moi_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_event_khach_moi_model item)
        {
             if (string.IsNullOrEmpty(item.db.name))
            {
                ModelState.AddModelError("db.name", "required");
            }
            //if (item.db.stt==null)
            //{
            //    ModelState.AddModelError("db.stt", "required");
            //}
            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {
                if (!string.IsNullOrEmpty(item.db.email))
                {

                    var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                       + "@"
                                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                    var checkEmail = rgEmail.IsMatch(item.db.email);
                    if (checkEmail == false)
                    {
                        ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                    }
                    else
                    {
                        var email = item.db.email;// CMAESCrypto.EncryptText(item.db.email);
                        var checkemail = repo.FindAll().Where(d => d.db.email == email && d.db.id_su_kien == item.db.id_su_kien && d.db.id != item.db.id).Count();
                        if (checkemail > 0)
                        {
                            ModelState.AddModelError("db.email", "existed");
                        }
                    }


                }
            }
          
           

            if (!string.IsNullOrEmpty(item.db.dien_thoai))
            {
                if(item.db.dien_thoai.Length > 10)
                {
                    ModelState.AddModelError("db.dien_thoai", "system.msgdienthoaiqua10kytu");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
                    |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
                    |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
                    |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(item.db.dien_thoai);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.dien_thoai", "system.soDienThoaiKhongHopLe");
                    }
                    else
                    {
                        var search1 = repo.FindAll().Where(d => d.db.dien_thoai == item.db.dien_thoai && d.db.id_su_kien == item.db.id_su_kien && d.db.id != item.db.id).Count();
                        if (search1 > 0)
                        {
                            ModelState.AddModelError("db.dien_thoai", "existed");
                        }

                    }
                }
            
            }


           

            return ModelState.IsValid;
        }

    }
}
