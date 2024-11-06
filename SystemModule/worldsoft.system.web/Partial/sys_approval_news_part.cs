using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using worldsoft.common.BaseClass;
using worldsoft.common.Models;
using worldsoft.DataBase.System;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    partial class sys_approval_newsController
    {
        public static ControllerAppModel declare = new ControllerAppModel()
        {
            controller = "sys_approval_news",
            icon = "badge",
            module = "system",
            id = "sys_approval_news",
            url = "/sys_approval_news_index",
            title = "sys_approval_news",
            translate = "NAV.sys_approval_news",
            type = "item",

            list_controller_action_public = new List<string>(){
                "sys_approval_news;getListUse",
                  "sys_approval_news;getTypeNewsByGroupName",
                        "sys_approval_news;getNewsByTypeNews",
                            "sys_approval_news;getCountNews",
                             "sys_approval_news;approval",
                              "sys_approval_news;DataHandler",
                                "sys_approval_news;edit",
                                 "sys_approval_news;reject",
                                 "sys_approval_news;delete",

            },
            
            list_role = new List<ControllerRoleModel>()
            {
            }
        };

        private bool checkModelStateCreate(sys_approval_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.create, item);
        }

        private bool checkModelStateEdit(sys_approval_news_model item)
        {
            return checkModelStateCreateEdit(ActionEnumForm.edit, item);
        }
        private bool checkModelStateCreateEdit(ActionEnumForm action, sys_approval_news_model item)
        {
            //if (string.IsNullOrEmpty(item.db.name))
            //{
            //    ModelState.AddModelError("db.name", "required");
            //}
            //var search = repo.FindAll().Where(d => d.db.name == item.db.name && d.db.id != item.db.id).Count();
            //if (search > 0)
            //{
            //    ModelState.AddModelError("db.name", "existed");
            //}


            return ModelState.IsValid;
        }
        private async Task send_mail(string type, string id_news)
        {



            //var lst_email = repo._context.doc_tailieu_fileuploads.Where(q => q.id_du_an == id_du_an ||q.id_du_an=="-1" ).Select(
            //    q => new sys_cau_hinh_otp_model
            //    {
            //        email = repo._context.users.Where(d => d.Id == q.id_user).Select(q => q.email).FirstOrDefault()
            //    }
            //    ).Distinct().ToList();


            var msg = "";


            var body = "";

            var domain = $"{Request.Scheme}://{Request.Host}";
            var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
            var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();
            var news = repo._context.sys_news.Where(t => t.id == id_news).FirstOrDefault();

            var user = repo._context.users.Where(q => q.Id == news.create_by).FirstOrDefault();
            var email = user.email;
            //maumail.noidung ?? "";
            body = maumail.template;
            body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
            body = body.Replace("@@full_name@@", user.full_name);
            body = body.Replace("@@user_name@@", user.email);
            body = body.Replace("@@tieu_de_tin_tuc@@", news.tieu_de);
            body = body.Replace("@@otp@@", user.otp);
            body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
            //body = body.Replace("@@url_reset@@", _appsetting.domain + "/systemverify.ctr/confirmResetPass?q=" + HttpUtility.UrlEncode(encryptconfirmparam));
            body = body.Replace("@@news_link@@", domain + "/portal-news-detail/" + id_news);
            body = body.Replace("@@ly_do_khong_duyet@@", news.reason_return);
            var dblogmail = new sys_log_mail_db();

            dblogmail.tieu_de = news.tieu_de;
            dblogmail.noi_dung = body;
            dblogmail.id_template = maumail.id;
            dblogmail.email = user.email;
            dblogmail.id = Guid.NewGuid().ToString();
            dblogmail.send_date = DateTime.Now;
            dblogmail.user_id = getUserId();
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



            }







        }

    }
}
