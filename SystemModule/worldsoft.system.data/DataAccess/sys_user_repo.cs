using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;
using worldsoft.DataBase.System;
using worldsoft.common.Common;
using worldsoft.common.Models;
using System.Text.RegularExpressions;
using worldsoft.DataBase.Helper;
using System.Web;

namespace worldsoft.system.data.DataAccess
{
    public class sys_user_repo 
    {
        public worldsoftDefautContext _context;
        private IMailService _mailService;
        public sys_user_repo(worldsoftDefautContext context)
        {
            _context = context;
          
        }

        public async Task<sys_user_model> getElementById(string id)
        {
        
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.Id == id);

           
            
            return obj;
        }
        
        public async Task<int> insert(sys_user_model model)
        {
            model.db.FirstName = model.db.FirstName ?? "";
            model.db.LastName = model.db.LastName ?? "";
            model.db.time_input = DateTime.Now;
            await _context.users.AddAsync(model.db);
            _context.SaveChanges();
           
            return 1;
        }
    

        public async Task<String> send_otp(string id)
        {

            var rnd = new Random();
            var code = rnd.Next(11111, 99999).ToString();
            try
            {


                var user = _context.users.Where(q => q.Id == id).FirstOrDefault();


                user.otp = code;
                _context.SaveChanges();


                //var lst_email = repo._context.doc_tailieu_fileuploads.Where(q => q.id_du_an == id_du_an ||q.id_du_an=="-1" ).Select(
                //    q => new sys_cau_hinh_otp_model
                //    {
                //        email = repo._context.users.Where(d => d.Id == q.id_user).Select(q => q.email).FirstOrDefault()
                //    }
                //    ).Distinct().ToList();

                var email = user.email;
                var msg = "";


                var body = "";

                var type_mail = _context.sys_type_mails.Where(t => t.code == "01").FirstOrDefault();
                var maumail = _context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();
                //maumail.noidung ?? "";
                body = maumail.template;
                body = body.Replace("@@user_name@@", user.email);
                body = body.Replace("@@otp@@", user.otp);
                body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
                //body = body.Replace("@@url_reset@@", _appsetting.domain + "/systemverify.ctr/confirmResetPass?q=" + HttpUtility.UrlEncode(encryptconfirmparam));

                var dblogmail = new sys_log_mail_db();

                dblogmail.tieu_de = type_mail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = user.email;

                dblogmail.send_date = DateTime.Now;
                dblogmail.user_id = id;
                dblogmail.ket_qua = 0;
                //dblogmail.db.ngay_gui = DateTime.Now;
                //dblogmail.db.nguoi_gui = getUserId();
                _context.sys_log_mails.Add(dblogmail);
                _context.SaveChanges();
                var id_log_mail = dblogmail.id;
                try
                {
                    await _mailService.SendEmailAsync(new MailRequest
                    {
                        Body = body,
                        Subject = dblogmail.tieu_de,
                        ToEmail = email, //CMAESCrypto.DecryptText(),
                        CCEmail = "",
                    });
                    var dblog = _context.sys_log_mails.Where(q => q.id == id_log_mail).FirstOrDefault();

                    if (dblog != null)
                    {

                        dblog.ket_qua = 1;
                        _context.SaveChanges();
                    }
                }
                catch (Exception e)
                {

                    return "error:" + e.ToString();

                }




            }
            catch
            {
                return "error";

            }
            return "";


        }

        public async Task<sys_user_model> infoUserLogin(string iduser)
        {
            var db = _context.users.Where(d => d.Id == iduser).Select(
                d => new sys_user_model()
                {
                    db=d,
                    company_name= _context.sys_companys.Where(s=>s.id==d.id_company).Select(s=>s.name).SingleOrDefault(),
                    department_name= _context.sys_departments.Where(s=>s.id==d.id_department).Select(s=>s.name).SingleOrDefault(),
                }).SingleOrDefault();
            return db;
        }
        public async Task<int> update(sys_user_model model)
        {
           var db= await _context.users.Where(d=>d.Id ==  model.db.Id).FirstOrDefaultAsync();
            db.id_department = model.db.id_department;
            db.id_job_title = model.db.id_job_title;
            db.FirstName = model.db.FirstName ?? "";
            db.LastName = model.db.LastName??"";
            //db.Username = model.db.Username;
            db.email = model.db.email;
            db.phone = model.db.phone;
            db.type = model.db.type;
            db.sex = model.db.sex;
            db.date_of_birth = model.db.date_of_birth;
            db.id_company = model.db.id_company;
            db.school_year = model.db.school_year;
            db.id_khoa = model.db.id_khoa;
            db.avatar_path = model.db.avatar_path;
            db.status_graduate = model.db.status_graduate;
            db.school_year = model.db.school_year;
            db.facebook_link = model.db.facebook_link;
            db.linkedin_link = model.db.linkedin_link;
            db.youtube_link = model.db.youtube_link;
            db.website_link = model.db.website_link;
            db.instagram_link = model.db.instagram_link;
            db.twitter_link = model.db.twitter_link;
            db.cover_image = model.db.cover_image;
            db.full_name = model.db.full_name;
            db.position = model.db.position;
            db.dia_chi = model.db.dia_chi;
            db.cv_link = model.db.cv_link;
            db.ly_do = model.db.ly_do;
            db.company = model.db.company;
            db.status_del = model.db.status_del;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_user_model> FindAll()
        {
            var result = _context.users.Select(d=> new sys_user_model()
            {
                db = d,
                department_name = _context.sys_departments.Where(t => t.id == d.id_department).Select(d => d.name).SingleOrDefault(),
                job_title_name = _context.sys_job_titles.Where(t => t.id == d.id_job_title).Select(d => d.name).SingleOrDefault(),
                company_name = _context.sys_companys.Where(s=>s.id==d.id_company).Select(s=>s.name).SingleOrDefault(),
                company_logo = _context.sys_companys.Where(s => s.id == d.id_company).Select(s => s.logo).SingleOrDefault(),

                khoa_name = _context.sys_khoas.Where(s => s.id == d.id_khoa).Select(s => s.name).SingleOrDefault(),
                full_name = d.FirstName + " " +d.LastName

            });;
         
            return result;
        }
        
       public int guiDuyetHoSo(string id)
        {
            var itemToRemove = _context.users.Where(x => x.Id == id).FirstOrDefault();
            //chờ xét duyệt
            itemToRemove.status_del = 4;
            _context.SaveChanges();
            return 1;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.users.Where(x => x.Id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();
            return 1;
        }
        public int deleteFile(string id)
        {
            var itemToRemove = _context.sys_user_fileuploads.Where(x => x.id == id).FirstOrDefault();

            _context.sys_user_fileuploads.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int deleteEducation(string id, string userid)
        {
            var itemToRemove = _context.sys_education_users.Where(x => x.id == id).FirstOrDefault();
            _context.sys_education_users.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int deleteSucess(string id, string userid)
        {
            var itemToRemove = _context.sys_success_users.Where(x => x.id == id).FirstOrDefault();
            _context.sys_success_users.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int deleteUngTuyen(string id, string userid)
        {
            var itemToRemove = _context.sys_user_ung_tuyens.Where(x => x.id == id).FirstOrDefault();
            _context.sys_user_ung_tuyens.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int deleteCertificate(string id, string userid)
        {
            var itemToRemove = _context.sys_certificate_users.Where(x => x.id == id).FirstOrDefault();
            _context.sys_certificate_users.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int deleteWorkHistory(string id, string userid)
        {
            var itemToRemove = _context.sys_work_history_users.Where(x => x.id == id).FirstOrDefault();
            _context.sys_work_history_users.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int deleteExperience(string id, string userid)
        {
            var itemToRemove = _context.sys_experience_users.Where(x => x.id == id).FirstOrDefault();
            _context.sys_experience_users.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int approval(string id, string userid)
        {
            var item = _context.users.Where(x => x.Id == id).FirstOrDefault();
            item.status_del = 1;
            item.ngay_duyet = DateTime.Now;
            item.nguoi_duyet = userid;
            _context.SaveChanges();
            insert_search(item);
            return 1;
        }

        public void insert_search(User model)
        {
            string t = (model.full_name ?? "").ToLower().Normalize();

            var db = _context.sys_searchs.Where(d => d.id_ref == model.Id && d.type == 3).FirstOrDefault();
            if (db != null)
            {
                db.create_date = DateTime.Now;
                db.order_date = model.time_input;
                db.search_text = t;
                _context.SaveChanges();
            }
            else
            {
                var db1 = new sys_search_db()
                {
                    create_date = DateTime.Now,
                    order_date = model.time_input,
                    id = 0,
                    id_ref = model.Id,
                    search_text = t,
                    type = 3,
                };
                _context.Add(db1);
                _context.SaveChanges();

            }

        }

        public int cancel(string id, string userid,string reason)
        {
            var item = _context.users.Where(x => x.Id == id).FirstOrDefault();
            item.status_del = 5;
            item.ngay_duyet = DateTime.Now;
            item.ly_do = reason;
            item.nguoi_duyet = userid;
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> updatePassword(sys_user_model model)
        {
            var db = await _context.users.Where(d => d.Id == model.db.Id).FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                db.PasswordHash = model.db.PasswordHash;
                db.PasswordSalt = model.db.PasswordSalt;
                db.token_reset_pass = null;
                db.expiration_date_reset_pass = null;
            }
            _context.SaveChanges();
            return 1;
        }


    }
}
