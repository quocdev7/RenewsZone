using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.DataBase.System;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_userController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "person_outline",
            icon = "menu",
            module = "system",
            id = "sys_user",
            url = "/sys_user_index",
            title = "sys_user",
            translate = "NAV.sys_user",
            type = "item",
            list_controller_action_publicNonLogin = new List<string>() {
                "sys_user;forgot_pass",
                "sys_user;checkResetPass",
                "sys_user;changePasswordNonLogin",
                 "sys_user;register",
                 "sys_user;getExperienceUser",
                 "sys_user;getSuccessUser",
                 "sys_user;getCertificateUser",
                 "sys_user;getWorkHistoryUser",
                 "sys_user;updateProfile",
                     "sys_user;getUserOtp",
                         "sys_user;getAnotherUserInfo",
                  "sys_user;xac_thuc",
                   "sys_user;send_otp",
                     "sys_user;Authenticate",
                      "sys_user;downloadtemp"
            },
            list_controller_action_public = new List<string>() {
                    "sys_user;get_list_ban_be",
           "sys_user;search_common",
                      "sys_user;getBanbeInfo",
                    
                 "sys_user;invite_user",
                   "sys_user;action_invite",
                  "sys_user;getNewsInfo",
                     "sys_user;getEventInfo",
                  "sys_user;getUserLogin",
                "sys_user;getListUse",
                "sys_user;forgot_pass",
                 "sys_user;changePassword",
                "sys_user;getName",
                "sys_user;changePasswordByAdmin",
                "sys_user;changePasswordCustomer",
                "sys_user;ImportFromExcel",
                "sys_user;Download",
                "sys_user;getInfomationUserLogin",
                         "sys_user;getUserByCompany",
                         "sys_user;getListUseInfo",
                           "sys_user;getListUserSearch",
                "sys_user;getUserInfo",
                "sys_user;createExperience",
                "sys_user;createSuccess",
                "sys_user;createCertificate",
                "sys_user;createWorkHistory",
                 "sys_user;createEducation",
                 "sys_user;getListDegree",
                 "sys_user;getEducationUser",
                 "sys_user;deleteEducation",
                 "sys_user;deleteSuccess",
                 "sys_user;deleteCertificate",
                   "sys_user;deleteExperience",
                 "sys_user;deleteWorkHistory",
                   "sys_user;guiDuyetHoSo",
                   "sys_user;upload_file",

                 "sys_user;getCVUser",
                 "sys_user;deleteFile",
             
                   "sys_user;createQuyenRiengTu",
                    "sys_user;getQuyenRiengTu",
                       "sys_user;createUngTuyen",
                          "sys_user;getUngTuyen",
           "sys_user;getListUseNew",
            "sys_user;deleteUngTuyen",
         

            },
            list_role = new List<ControllerRoleModel>()
            {
                 new ControllerRoleModel()
                {
                    id="sys_user;create",
                    name="create",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;create",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user;edit",
                    name="edit",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;edit",
                          "sys_user;revert",
                    }
                },
                new ControllerRoleModel()
                {
                    id="sys_user;delete",
                    name="delete",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;delete",
                    }
                },
                  new ControllerRoleModel()
                {
                    id="sys_user;list",
                    name="list",
                    list_controller_action = new List<string>()
                    {
                          "sys_user;DataHandler",
                    }
                }
            }
        };
        private bool checkModelStateCreate(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_user_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }

        private async Task<IActionResult> send_mail(string id, string type)
        {

            var rnd = new Random();
            var code = rnd.Next(11111, 99999).ToString();
            try
            {


                var user = repo._context.users.Where(q => q.Id == id).FirstOrDefault();
                user.status_del = 3;
                repo._context.SaveChanges();
                //var lst_email = repo._context.doc_tailieu_fileuploads.Where(q => q.id_du_an == id_du_an ||q.id_du_an=="-1" ).Select(
                //    q => new sys_cau_hinh_otp_model
                //    {
                //        email = repo._context.users.Where(d => d.Id == q.id_user).Select(q => q.email).FirstOrDefault()
                //    }
                //    ).Distinct().ToList();

                var email = user.email;
                var msg = "";
                var body = "";
                var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();
                //maumail.noidung ?? "";
                body = maumail.template;
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
                body = body.Replace("@@user_name@@", user.email);
                body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
                //body = body.Replace("@@url_reset@@", _appsetting.domain + "/systemverify.ctr/confirmResetPass?q=" + HttpUtility.UrlEncode(encryptconfirmparam));

                var dblogmail = new sys_log_mail_db();

                dblogmail.tieu_de = type_mail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = user.email;
                dblogmail.id = Guid.NewGuid().ToString();
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
        private void removeUserModel(sys_user_model obj)
        {
            obj.db.otp = null;
            obj.db.PasswordHash = null;
            obj.db.PasswordSalt = null;
            obj.db.token_notification = null;
            obj.db.token_reset_pass = null;
            obj.db.token_reset_pass = null;
            obj.db.expiration_date_reset_pass = null;


        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_user_model item)
        {

            if (item.db.sex==null || item.db.sex == 0)
            {
                ModelState.AddModelError("db.sex", "required");
            }
            if (string.IsNullOrEmpty(item.db.full_name))
            {
                ModelState.AddModelError("db.full_name", "required");
            }

            if (item.db.status_graduate== null || item.db.status_graduate == 0)
            {
                ModelState.AddModelError("db.status_graduate", "required");
            }
            //if (string.IsNullOrEmpty(item.db.position))

            //{
            //    ModelState.AddModelError("db.position", "required");
            //}
            //if (string.IsNullOrEmpty(item.db.company))

            //{
            //    ModelState.AddModelError("db.company", "required");
            //}
            //if (string.IsNullOrEmpty(item.db.dia_chi))

            //{
            //    ModelState.AddModelError("db.dia_chi", "required");
            //}
            //if (item.db.sex==0)
            //{
            //    ModelState.AddModelError("db.sex", "required");
            //}
            //if (item.db.sex==3)
            //{
            //    if(string.IsNullOrEmpty(item.db.position))
            //    {
            //        ModelState.AddModelError("db.position", "required");
            //    }
            //}

            if (item.db.status_graduate == 1 || item.db.status_graduate == 2)
            {
                if (item.db.school_year == 0)
                {
                    ModelState.AddModelError("db.school_year", "required");
                }
                if (string.IsNullOrEmpty(item.db.id_khoa))
                {
                    ModelState.AddModelError("db.id_khoa", "required");
                }
            }



            //if (string.IsNullOrEmpty(item.password) && action == ActionEnumForm.create)
            //{
            //    ModelState.AddModelError("db.password", "required");
            //}


            if (string.IsNullOrEmpty(item.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            //            if (item.db.status_graduate == 0)
            //            {
            //                ModelState.AddModelError("db.status_graduate", "required");
            //            }

            if (!string.IsNullOrEmpty(item.db.phone))
            {
               
                    if (item.db.phone.Length > 10)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(item.db.phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }
                    else
                    {
                        var dienthoai = item.db.phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

                        var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.Id != item.db.Id).Count();
                        if (checkdienthoai > 0)
                        {
                            ModelState.AddModelError("db.phone", "existed");
                        }
                    }
                }

                
            }

           
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
                    var checkemail = repo.FindAll().Where(d => d.db.email == email && d.db.Id != item.db.Id).Count();
                    if (checkemail > 0)
                    {
                        ModelState.AddModelError("db.email", "existed");
                    }
                }


            }

            return ModelState.IsValid;
        }

    }
}
