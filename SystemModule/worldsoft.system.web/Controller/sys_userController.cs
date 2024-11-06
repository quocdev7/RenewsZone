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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using worldsoft.common.Common;
using worldsoft.common.Models;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Globalization;
using worldsoft.common.Helpers;
using Microsoft.Extensions.Options;
using worldsoft.DataBase.Helper;
using System.Threading;
using worldsoft.common.Models.Users;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace worldsoft.system.web.Controller
{
    public partial class sys_userController : BaseAuthenticationController
    {
        private sys_user_repo repo;
        private IUserService _userService;
        private IMailService _mailService;
        public AppSettings _appsetting;
        public sys_userController(IUserService userService, IMailService mailService, worldsoftDefautContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            _appsetting = appsetting.Value;
            repo = new sys_user_repo(context);
            _userService = userService;
            _mailService = mailService;
        }
       public class ZipItem
        {
            public string doc_no { get; set; }
            public string file_name { get; set; }
            public string file_path { get; set; }

        }
        public async Task<IActionResult> authenticate([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            //var check = checkModelStateCreate(model);
            var user = new User();
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("password", "required");
            }

            if (string.IsNullOrEmpty(model.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            else
            {
                var email = model.db.email.Trim();
                if (email == "administrator" || email == "btt")
                {
                    user = repo._context.users.Where(d => d.Username == email).SingleOrDefault();
                    if (!string.IsNullOrEmpty(model.password))
                    {
                        if (!VerifyPasswordHash(model.password, user.PasswordHash, user.PasswordSalt))
                            ModelState.AddModelError("password", "Mật khẩu không chính xác");
                    }
                    

                }
                else
                {
                   
                    user = repo._context.users.Where(d => (d.status_del != 0 && d.status_del != 2) && d.Username == email).SingleOrDefault();
                    if (user == null)
                    {
                        var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                      + "@"
                                      + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                        var checkEmail = rgEmail.IsMatch(email);
                        if (checkEmail == false)
                        {
                            ModelState.AddModelError("db.email", "Email không hợp lệ");

                        }
                        else
                        {
                            ModelState.AddModelError("db.email", "Email này chưa đăng ký");

                        }

                    }
                    else
                    {
                        if (!VerifyPasswordHash(model.password, user.PasswordHash, user.PasswordSalt))
                            ModelState.AddModelError("password", "Mật khẩu không chính xác");
                    }


                   
             }

              
            }
           
           
            if(model.showCaptcha == 1)
            {
                if (string.IsNullOrEmpty(model.capcha))
                {
                    ModelState.AddModelError("capcha", "required");
                }
                else
                {
                    var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                    if (CaptchaCode.ToLower() != model.capcha.ToLower())
                    {
                        ModelState.AddModelError("capcha", "captcha_invalid");
                    }

                }


            }







            if (!ModelState.IsValid)
            {
                return generateError();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsetting.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            HttpContext.Session.Remove("CaptchaCode");
            // return basic user info and authentication token
            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                fullName = user.full_name,
                status_del = user.status_del,
                token = tokenString,
                type = user.type,
            });
        }
        public IActionResult getSex()
        {
            var sex_id = repo._context.users.Where(d => d.status_del == 1).Select(d => new
            {
                id = d.Id,
                Sex = d.sex,
            }).ToList();
            return Json(sex_id);
        }
        public IActionResult getListDegree()
        {
            var degree = repo._context.sys_degrees.Select(d => new
            {
                id = d.id,
                name = d.name,
            }).ToList();
            return Json(degree);
        }
        

        [HttpPost]
        public async Task<IActionResult> getUserByCompany([FromBody] JObject json)

        { 
            var search = json.GetValue("search").ToString();
            var id_company = json.GetValue("id_company").ToString();

            var result = repo.FindAll()
                .Where(q=>q.db.id_company== id_company )
                .Where(q => q.db.email.Contains(search) || q.full_name.Contains(search))

                .FirstOrDefault();
            removeUserModel(result);
            return Json(result);
        }
        public IActionResult getEducationUser()
        {
            var user_id = getUserId();
            var result = repo._context.sys_education_users.Where(d => d.user_id == user_id).
                Select(d => new sys_education_user_model
                {
                    db = d,

                    degree_name = repo._context.sys_degrees.Where(q => q.id == d.degree).Select(q => q.name).FirstOrDefault(),

                }).OrderBy(q => q.db.to_year).ToList();
            return Json(result);
        }
        public IActionResult getExperienceUser()
        {
            var user_id = getUserId();
            var result = repo._context.sys_experience_users.Where(d => d.user_id == user_id).
                Select(d => new sys_experience_user_model
                {
                    db = d
                }).OrderByDescending(q=>q.db.update_by).ToList();
            return Json(result);
        }
        public IActionResult getWorkHistoryUser()
        {
            var user_id = getUserId();
            var result = repo._context.sys_work_history_users.Where(d => d.user_id == user_id).
                Select(d => new sys_work_history_user_model
                {
                    db = d
                }).OrderBy(q=>q.db.to_date).ToList();
            return Json(result);
        }
        public IActionResult getQuyenRiengTu()
        {
            var user_id = getUserId();
            var status_user = repo._context.users.Where(q => q.Id == user_id).Select(q => q.status_del).FirstOrDefault();
            var result = repo._context.sys_cau_hinh_quyen_rieng_tus.Where(d => d.user_id == user_id).
                Select(d => new sys_cau_hinh_quyen_rieng_tu_model
                {
                    db = d,
                    status_user = status_user
                }).FirstOrDefault();
            if(result == null)
            {
                result = new sys_cau_hinh_quyen_rieng_tu_model();

                result.db.setting_phone = 1;
                result.db.setting_email = 1;
                result.db.setting_ngay_sinh = 1;
                result.db.setting_dia_chi = 1;
                result.db.setting_trang_thai = 1;
                result.db.setting_chuc_danh = 1;
                result.db.setting_cong_ty = 1;
                result.db.setting_hoc_van = 1;
                result.db.setting_thanh_tuu = 1;
                result.db.setting_bang_cap = 1;
                result.db.setting_kinh_nghiem = 1;
                result.db.setting_ky_nang = 1;
                result.db.setting_mang_xa_hoi =1;
                result.db.setting_nien_khoa = 1;
                result.db.setting_khoa = 1;
                result.db.setting_gioi_tinh = 1;

                result.db.id = Guid.NewGuid().ToString();
                result.db.user_id = user_id;
                repo._context.sys_cau_hinh_quyen_rieng_tus.Add(result.db);
                repo._context.SaveChanges();
                result.status_user = status_user;
            }
            return Json(result);
        }
        public IActionResult getCertificateUser()
        {
            var user_id = getUserId();
            var result = repo._context.sys_certificate_users.Where(d => d.user_id == user_id).
                Select(d => new sys_certificate_user_model
                {
                    db = d
                }).OrderBy(q => q.db.to_date).ToList();
            return Json(result);
        }
        public IActionResult getUngTuyen()
        {
            var user_id = getUserId();
            var result = repo._context.sys_user_ung_tuyens.Where(d => d.user_id == user_id).
                Select(d => new sys_user_ung_tuyen_model
                {
                    db = d,
                    ten_tien_te = repo._context.sys_tien_tes.Where(q=>q.id== d.tien_te).Select(q=>q.name).FirstOrDefault()
                    
                }).FirstOrDefault();
          
            return Json(result);
        }
        public IActionResult getSuccessUser()
        {
            var user_id = getUserId();
            var result = repo._context.sys_success_users.Where(d => d.user_id == user_id).
                Select(d => new sys_success_user_model
                {
                    db = d
                }).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> download(string id)
        {
            var ids = id.Split(",").ToList();

          
                var filedb = repo._context.sys_user_fileuploads.Where(t => t.id == id).FirstOrDefault();

                var path = filedb.file_path;

                return new PhysicalFileResult(path,
                            File_content_type.GetContentType(path))
                { FileDownloadName = filedb.file_name };

        }

        public IActionResult getUserLogin()
        {
            var user_id = getUserId();
            var is_quan_tri = repo._context.users.Where(q => q.Id == user_id).Select(q => q.type).FirstOrDefault();
            var result = repo.FindAll().Where(d => d.db.Id == user_id)
                .Select(d=>new
                {
                    full_name= d.db.full_name ??d.db.email,
                    last_name = d.db.full_name ?? d.db.email,

                    avatar_path = d.db.avatar_path,
                    Username =d.db.Username,
                    status_del = d.db.status_del,
                    ly_do = d.db.ly_do,
                    check_aprroval_event = repo._context.sys_cau_hinh_duyet_su_kiens.Where(d => d.user_id == user_id).Count() > 0 ||is_quan_tri == 1,
                    check_aprroval_user = repo._context.sys_cau_hinh_duyet_users.Where(d => d.user_id == user_id).Count() > 0 || is_quan_tri==1,
                    check_aprroval_news =  repo._context.sys_user_typenews.Where(d=>d.id_user == user_id).Count()>0 || is_quan_tri == 1
                })
                .ToList();

            var result1 = result.Select(d =>new 
            {
                full_name = d.full_name,
                    last_name = (d.last_name??"").Trim().Split(" ").Last(),

                avatar_path = d.avatar_path,
                Username = d.Username,
                status_del = d.status_del,
                ly_do = d.ly_do,
                check_aprroval_event = d.check_aprroval_event,
                check_aprroval_user = d.check_aprroval_user,
                check_aprroval_news = d.check_aprroval_news,
            }).FirstOrDefault();
            return Json(result1);
        }

        public IActionResult getNewsInfo()
        {
            var user_id = getUserId();

            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();

            var badges = new Dictionary<string, long>();

            // 1 Mời tham gia sự kiện, 2 Từ chối tham gia sư kiện, 3 Sẽ tham gia sự kiện, 4 Đã tham gia sự kiện, 5 Không đủ điều kiện tham dựn, 6 Đăng ký tham gia sự kiện
            badges["tin_tuc_dang_cho_duyet"] = repo._context.sys_news.Where(d => d.create_by == user_id).Where(d => d.status_del == 3).Count();
            badges["tin_tuc_khong_duoc_duyet"] = repo._context.sys_news.Where(d => d.create_by == user_id).Where(d => d.status_del == 4).Count();
            badges["tin_tuc_ngung_dang"] = repo._context.sys_news.Where(d => d.create_by == user_id).Where(d => d.status_del == 2).Count();
            badges["tin_tuc_da_dang"] = repo._context.sys_news.Where(d => d.create_by == user_id).Where(d => d.status_del == 1).Count();

            return Json(badges);
        }
        public IActionResult getEventInfo()
        {
            var user_id = getUserId();

            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();

            var badges = new Dictionary<string, long>();

            // 1 Mời tham gia sự kiện, 2 Từ chối tham gia sư kiện, 3 Sẽ tham gia sự kiện, 4 Đã tham gia sự kiện, 5 Không đủ điều kiện tham dựn, 6 Đăng ký tham gia sự kiện
            badges["event_duoc_moi"] = repo._context.sys_event_khach_mois.Where(d => d.email == email).Where(d => d.check_in_status == 1).Count();
            badges["event_da_dang_ky"] = repo._context.sys_event_khach_mois.Where(d => d.email == email).Where(d => d.check_in_status == 6).Count();
            badges["event_se_tham_gia"] = repo._context.sys_event_khach_mois.Where(d => d.email == email).Where(d => d.check_in_status == 3).Count();
            badges["event_da_tham_du"] = repo._context.sys_event_khach_mois.Where(d => d.email == email).Where(d => d.check_in_status == 4).Count();
            badges["event_khong_du_dieu_kien_tham_du"] = repo._context.sys_event_khach_mois.Where(d => d.email == email).Where(d => d.check_in_status == 5).Count();
            badges["event_tu_choi_tham_du"] = repo._context.sys_event_khach_mois.Where(d => d.email == email).Where(d => d.check_in_status == 2).Count();
            return Json(badges);
        }
        public IActionResult getBanbeInfo()
        {
            var user_id = getUserId();

            var badges = new Dictionary<string, long>();

            // 1 ban be, 2 lời mời kết ban, 3 chờ ket ban, 4 mời qua email (user chưa tồn tại trên hệ thống).
            badges["ban_be"] = repo._context.sys_user_ban_bes.Where(d => d.user_id == user_id).Where(d => d.status_del == 1).Count();
            badges["loi_moi_ket_ban"] = repo._context.sys_user_ban_bes.Where(d => d.user_id == user_id).Where(d => d.status_del ==  2).Count();
            badges["da_gui_loi_moi"] = repo._context.sys_user_ban_bes.Where(d => d.user_id == user_id).Where(d => d.status_del == 3).Count();
          

            return Json(badges);
        }
        

        public IActionResult getUserInfo()
        {
            var user_id = getUserId();

            var email = repo._context.users.Where(d => d.Id == user_id).Select(d => d.email).SingleOrDefault();
            var result = repo.FindAll().Where(d => d.db.Id == user_id).FirstOrDefault();
            removeUserModel(result);
            result.file = repo._context.sys_user_fileuploads.Where(d => d.user_id == user_id).
                Select(d => new sys_user_fileupload_model
                {
                    db = d
                }).OrderByDescending(q=>q.db.upload_date).FirstOrDefault();
            removeUserModel(result);
            result.cau_hinh_quyen_rieng_tu = repo._context.sys_cau_hinh_quyen_rieng_tus.Where(d => d.user_id == user_id).
               Select(d => new sys_cau_hinh_quyen_rieng_tu_model
               {
                   db = d
               }).FirstOrDefault();
            result.user_ung_tuyen = repo._context.sys_user_ung_tuyens.Where(d => d.user_id == user_id).
            Select(d => new sys_user_ung_tuyen_model
            {
                db = d,
                ten_tien_te = repo._context.sys_tien_tes.Where(q=>q.id == d.tien_te).Select(q=>q.name).FirstOrDefault()
            }).FirstOrDefault();


            return Json(result);
        }




        [HttpPost]
        public async Task<IActionResult> getAnotherUserInfo([FromBody] JObject json)
        {
            var user_id = json.GetValue("user_id").ToString();
            var user_id_current = getUserId();
            var khonglaBanBe = repo._context.sys_user_ban_bes.Where(d => d.user_id == user_id && d.user_id_ban_be == user_id_current && d.status_del == 1).Count()==0;
            var result = repo.FindAll().Where(d => d.db.Id == user_id).FirstOrDefault();
            var another_user = user_id != user_id_current;
            removeUserModel(result);
          
            result.cau_hinh_quyen_rieng_tu = repo._context.sys_cau_hinh_quyen_rieng_tus.Where(d => d.user_id == user_id).
             Select(d => new sys_cau_hinh_quyen_rieng_tu_model
             {
                 db = d
             }).FirstOrDefault();
            if(result.db.status_del == 1)
            {

              
                if (another_user)
                {
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_gioi_tinh == 3 ||  (result.cau_hinh_quyen_rieng_tu.db.setting_gioi_tinh==2 && khonglaBanBe) )
                    {
                        result.db.sex = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_ngay_sinh == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_ngay_sinh == 2 && khonglaBanBe))
                    {
                        result.db.date_of_birth = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_email == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_email == 2 && khonglaBanBe))
                    {
                        result.db.email = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_phone == 3|| (result.cau_hinh_quyen_rieng_tu.db.setting_phone == 2 && khonglaBanBe))
                    {
                        result.db.phone = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_trang_thai == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_trang_thai == 2 && khonglaBanBe))
                    {
                        result.db.status_graduate = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_dia_chi == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_dia_chi == 2 && khonglaBanBe))
                    {
                        result.db.dia_chi = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_khoa == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_khoa == 2 && khonglaBanBe))
                    {
                        result.db.id_khoa = null;
                    }

                    if (result.cau_hinh_quyen_rieng_tu.db.setting_nien_khoa == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_nien_khoa == 2 && khonglaBanBe))
                    {
                        result.db.school_year = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_chuc_danh == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_chuc_danh == 2 && khonglaBanBe))
                    {
                        result.db.position = null;
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_cong_ty == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_cong_ty == 2 && khonglaBanBe))
                    {
                        result.db.company = null;
                    }

                    if (result.cau_hinh_quyen_rieng_tu.db.setting_mang_xa_hoi == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_mang_xa_hoi == 2 && khonglaBanBe))
                    {
                        result.db.linkedin_link = null;
                        result.db.facebook_link = null;
                        result.db.instagram_link = null;
                        result.db.youtube_link = null;
                        result.db.website_link = null;
                        result.db.twitter_link = null;

                    }

                    if (result.cau_hinh_quyen_rieng_tu.db.setting_hoc_van == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_hoc_van == 2 && khonglaBanBe))
                    {
                        result.list_user_education = new List<sys_education_user_model>();
                    }
                    else
                    {
                        result.list_user_education = repo._context.sys_education_users.Where(d => d.user_id == user_id).
                        Select(d => new sys_education_user_model
                        {
                            db = d,

                            degree_name = repo._context.sys_degrees.Where(q => q.id == d.degree).Select(q => q.name).FirstOrDefault(),

                        }).OrderBy(q => q.db.to_year).ToList();

                    }


                    if (result.cau_hinh_quyen_rieng_tu.db.setting_thanh_tuu == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_thanh_tuu == 2 && khonglaBanBe))
                    {
                        result.list_user_success = new List<sys_success_user_model>();
                    }
                    else
                    {
                        result.list_user_success = repo._context.sys_success_users.Where(d => d.user_id == user_id).
                          Select(d => new sys_success_user_model
                          {
                              db = d
                          }).ToList();
                    }
                    if (result.cau_hinh_quyen_rieng_tu.db.setting_bang_cap == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_bang_cap == 2 && khonglaBanBe))
                    {
                        result.list_user_certificate = new List<sys_certificate_user_model>();
                    }
                    else
                    {
                        result.list_user_certificate = repo._context.sys_certificate_users.Where(d => d.user_id == user_id).
                   Select(d => new sys_certificate_user_model
                   {
                       db = d
                   }).OrderBy(q => q.db.to_date).ToList();

                    }


                    if (result.cau_hinh_quyen_rieng_tu.db.setting_kinh_nghiem == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_kinh_nghiem == 2 && khonglaBanBe))
                    {
                        result.list_user_work_history = new List<sys_work_history_user_model>();
                    }
                    else
                    {
                        result.list_user_work_history = repo._context.sys_work_history_users.Where(d => d.user_id == user_id).
                    Select(d => new sys_work_history_user_model
                    {
                        db = d
                    }).OrderBy(q => q.db.to_date).ToList();


                    }

                    if (result.cau_hinh_quyen_rieng_tu.db.setting_ky_nang == 3 || (result.cau_hinh_quyen_rieng_tu.db.setting_ky_nang == 2 && khonglaBanBe))
                    {
                        result.list_user_experience = new List<sys_experience_user_model>();
                    }
                    else
                    {

                        result.list_user_experience = repo._context.sys_experience_users.Where(d => d.user_id == user_id).
                           Select(d => new sys_experience_user_model
                           {
                               db = d
                           }).OrderByDescending(q => q.db.update_by).ToList();

                    }


                }

                result.file = repo._context.sys_user_fileuploads.Where(d => d.user_id == user_id).
                    Select(d => new sys_user_fileupload_model
                    {
                        db = d
                    }).OrderByDescending(q => q.db.upload_date).FirstOrDefault();
            }


            return Json(result);
        }




        [HttpPost]
        public async Task<IActionResult> getUserOtp([FromBody] JObject json)
        {

            var user_id = json.GetValue("id").ToString();

            var result = repo.FindAll().Where(d => d.db.Id == user_id).FirstOrDefault();
            removeUserModel(result);
            return Json(result);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.users
                .Where(d=>d.status_del==1 && d.type==1)
                 .Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " "+ d.LastName
                 }).ToList();
            return Json(result);
        }

        
        [HttpPost]
        public async Task<IActionResult> getListUseNew([FromBody] JObject json)
        {
            var search = "";
            try
            {
                search = (json.GetValue("search").ToString() ?? "").ToLower();
            }
            catch
            {

            }
            var result = repo._context.users
                .Where(d => d.status_del == 1)
                 .Where(t => t.full_name.ToLower().Equals(search)  || t.phone.ToLower().Equals(search) || t.email.ToLower().Equals(search))
                 .Select(d => new
                 {
                     id = d.Id,
                     name = d.full_name
                 }).Take(5).ToList();
            return Json(result);
        }
        //[HttpGet]
        //public async Task<IActionResult> download(string id)
        //{
        //    var ids = id.Split(",").ToList();

        //    if (ids.Count() == 1)
        //    {
        //        var filedb = repo._context.users.Where(t => t.Id == id).FirstOrDefault();

        //        var path = "";// _appSettings.folder_path + filedb.file_path;

        //        return new PhysicalFileResult(path,
        //                    File_content_type.GetContentType(path))
        //        { FileDownloadName = filedb.cv_link };





        //    }
        //    else
        //    {

        //        //List<ZipItem> zipItems = new List<ZipItem>();
        //        //var zipStream = new MemoryStream();

        //        //for (int i = 0; i < ids.Count; i++)
        //        //{
        //        //    var filedb = repo._context.users.Where(t => t.Id == ids[i]).FirstOrDefault();
        //        //    var path = ""; //_appSettings.folder_path + filedb.file_path;
        //        //    var model = new ZipItem();
        //        //    model.file_name = filedb.file_name;
        //        //    model.file_path = path;
        //        //    model.doc_no = repo._context.users.Where(t => t.Id == filedb.id_doc_tailieu).Select(q => q.code).FirstOrDefault();

        //        //    zipItems.Add(model);
        //        //}
        //        //var path1 = Path.Combine(_appSettings.folder_path, "file_upload", "zip");
        //        //if (!Directory.Exists(path1))
        //        //{
        //        //    Directory.CreateDirectory(path1);
        //        //}
        //        //var webRoot = _appSettings.folder_path;
        //        //var tick = DateTime.Now.Ticks;
        //        //var filename = tick + "." + "zip";
        //        //var pathsave = Path.Combine(path1, filename);
        //        //using (var archive = System.IO.Compression.ZipFile.Open(pathsave, ZipArchiveMode.Create))
        //        //{
        //        //    foreach (var fileToZip in zipItems)
        //        //    {

        //        //        archive.CreateEntryFromFile(fileToZip.file_path, Path.GetFileName(fileToZip.file_name));
        //        //    }

        //        //}
        //        //return new PhysicalFileResult(pathsave,
        //        //         File_content_type.GetContentType(pathsave))
        //        //{ FileDownloadName = zipItems[0].doc_no + "." + "zip" };
        //        return new PhysicalFileResult("",
        //              File_content_type.GetContentType(""))
        //        { FileDownloadName = "" + "." + "zip" };

        //    }

        //}
        public IActionResult getInfomationUserLogin()
        {
            var user_id = getUserId();
            var resuft = repo.infoUserLogin(user_id);
            return Json(resuft.Result);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {
          
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check= checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.Id = Guid.NewGuid().ToString();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
            model.db.Id = Guid.NewGuid().ToString();
            model.db.PasswordHash = passwordHash;
            model.db.PasswordSalt = passwordSalt;
            model.db.status_del = 1;
            model.db.type = model.type == true ? 1 : 0;

            if (model.db.avatar_path == null)
            {
                model.db.avatar_path = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.avatar).FirstOrDefault(); //_appsetting.avatar;
            }
            if (model.db.cover_image == null)
            {
                model.db.cover_image = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.image).FirstOrDefault();//_appsetting.cover;
            }

            await repo.insert(model);


            return Json(model);
        }
        
       
        [HttpPost]
        public async Task<IActionResult> createExperience([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_experience_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.db.skill))
            {
                ModelState.AddModelError("db.skill", "required");
            }
           
            if (!ModelState.IsValid)
            {
                return generateError();
            }

            if (model.db.id == "0")
            {
                var db = new sys_experience_user_db();
                db.id = Guid.NewGuid().ToString();
                db.description = model.db.description;
                db.skill = model.db.skill;
                db.user_id = getUserId();
                db.update_by = getUserId();
                db.update_date = DateTime.Now;
                await repo._context.sys_experience_users.AddAsync(db);
                repo._context.SaveChanges();
            }
            else
            {
                var db = repo._context.sys_experience_users.Where(d => d.id == model.db.id).SingleOrDefault();
                db.description = model.db.description;
                db.skill = model.db.skill;
                db.update_date = DateTime.Now;
                repo._context.SaveChanges();
            }
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> createUngTuyen([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_user_ung_tuyen_model>(json.GetValue("data").ToString());
            if (model.db.muc_luong==null)
            {
                ModelState.AddModelError("db.muc_luong", "required");
            }
            if (string.IsNullOrEmpty(model.db.tien_te))
            {
                ModelState.AddModelError("db.tien_te", "required");
            }
            if (string.IsNullOrEmpty(model.db.vi_tri))
            {
                ModelState.AddModelError("db.vi_tri", "required");
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (model.db.id == "0")
            {
                var db = new sys_user_ung_tuyen_db();
                db.id = Guid.NewGuid().ToString();
                db.muc_luong = model.db.muc_luong;
                db.hinh_thuc = model.db.hinh_thuc;
                db.vi_tri = model.db.vi_tri;
                db.user_id = getUserId();
                db.update_date = DateTime.Now;
                db.update_by = getUserId();
                db.tien_te = model.db.tien_te;
             
                await repo._context.sys_user_ung_tuyens.AddAsync(db);
                repo._context.SaveChanges();
            }
            else
            {
                var db = repo._context.sys_user_ung_tuyens.Where(d => d.id == model.db.id).SingleOrDefault();
                db.muc_luong = model.db.muc_luong;
                db.hinh_thuc = model.db.hinh_thuc;
                db.vi_tri = model.db.vi_tri;
                db.tien_te = model.db.tien_te;
                db.update_date = DateTime.Now;
                db.update_by = getUserId();

                repo._context.SaveChanges();
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> createSuccess([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_success_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.db.description))
            {
                ModelState.AddModelError("db.description", "required");
            }
            if (String.IsNullOrEmpty(model.db.url)){
                if (model.db.url.StartsWith("http://") || model.db.url.StartsWith("https://"))
                {

                }
                else
                {
                    ModelState.AddModelError("db.url", "system.khong_hop_le");
                }
            }
           
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (model.db.id == "0")
            {
                var db = new sys_success_user_db();
                db.id = Guid.NewGuid().ToString();
                db.description = model.db.description;
                db.field_name = model.db.field_name;
                db.image = model.db.image;
                db.user_id = getUserId();
                db.update_date = DateTime.Now;
                db.update_by = getUserId();
                db.url = model.db.url;
                db.noi_cap = model.db.noi_cap;
                await repo._context.sys_success_users.AddAsync(db);
                repo._context.SaveChanges();
            }
            else
            {
                var db = repo._context.sys_success_users.Where(d => d.id == model.db.id).SingleOrDefault();
                db.description = model.db.description;
                db.field_name = model.db.field_name;
                db.image = model.db.image;
                db.noi_cap = model.db.noi_cap;
                db.update_date = model.db.update_date;
                db.url = model.db.url;
            
                repo._context.SaveChanges();
            }

            return Json(model);
        }




        [HttpPost]
        public async Task<IActionResult> upload_file()
        {

            var request = Request;

            //pass model
            //var model = JsonConvert.DeserializeObject<doc_tailieu_model>(Request.Form["model"]);
            var userid = getUserId();
            var model = repo.FindAll().Where(q=>q.db.Id == userid).FirstOrDefault();

            foreach (var formFile in Request.Form.Files)
            {

                var filename = formFile.FileName.Trim('"') + "";
                filename = StringFunctions.NonUnicode(filename);
                filename = Regex.Replace(filename, "[^A-Za-z0-9.]", "");

                var currentpath = Directory.GetCurrentDirectory();
                var path = Path.Combine(currentpath, "file_upload","cv_user", model.db.Id);
           
                Thread.Sleep(1);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                var tick = Guid.NewGuid().ToString();
                var pathsave = Path.Combine(path, tick + "." + filename.Split(".").Last());
                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(pathsave.Replace(Path.Combine(currentpath, "file_upload"), ""));
                using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var item = new sys_user_fileupload_model();
                item.db.id = tick;
                item.db.user_id = model.db.Id;          
                item.db.upload_by = model.db.Id;
                item.db.upload_date = DateTime.Now;
                item.db.file_name = formFile.FileName;
                item.db.file_size = formFile.Length;
                item.db.file_type = formFile.ContentType;
                item.db.file_path = file_path;
                repo._context.sys_user_fileuploads.Add(item.db);
                repo._context.SaveChanges();
                model.file = item;
            }



            return Json(model);
        }


        
        [HttpPost]
        public async Task<IActionResult> createQuyenRiengTu([FromBody] JObject json)
        {
            
                var model = JsonConvert.DeserializeObject<sys_cau_hinh_quyen_rieng_tu_model>(json.GetValue("data").ToString());



                if (model.db.id == "0")
                {
                    var db = new sys_cau_hinh_quyen_rieng_tu_db();
                    db.id = Guid.NewGuid().ToString();

                    db.user_id = getUserId();

                    db.setting_phone = model.db.setting_phone;
                    db.setting_email = model.db.setting_email;
                    db.setting_ngay_sinh = model.db.setting_ngay_sinh;
                    db.setting_dia_chi = model.db.setting_dia_chi;
                    db.setting_trang_thai = model.db.setting_trang_thai;
                    db.setting_chuc_danh = model.db.setting_chuc_danh;
                    db.setting_cong_ty = model.db.setting_cong_ty;
                    db.setting_hoc_van = model.db.setting_hoc_van;
                    db.setting_thanh_tuu = model.db.setting_thanh_tuu;
                    db.setting_bang_cap = model.db.setting_bang_cap;
                    db.setting_kinh_nghiem = model.db.setting_kinh_nghiem;
                    db.setting_ky_nang = model.db.setting_ky_nang;
                    db.setting_gioi_tinh = model.db.setting_gioi_tinh;
                    db.setting_khoa = model.db.setting_khoa;
                    db.setting_nien_khoa = model.db.setting_nien_khoa;
                    db.setting_mang_xa_hoi= model.db.setting_mang_xa_hoi;

                await repo._context.sys_cau_hinh_quyen_rieng_tus.AddAsync(db);
                    repo._context.SaveChanges();
                }
                else
                {
                    var db = repo._context.sys_cau_hinh_quyen_rieng_tus.Where(d => d.id == model.db.id).SingleOrDefault();
                    db.setting_phone = model.db.setting_phone;
                    db.setting_email = model.db.setting_email;
                    db.setting_ngay_sinh = model.db.setting_ngay_sinh;
                    db.setting_dia_chi = model.db.setting_dia_chi;
                    db.setting_trang_thai = model.db.setting_trang_thai;
                    db.setting_chuc_danh = model.db.setting_chuc_danh;
                    db.setting_cong_ty = model.db.setting_cong_ty;
                    db.setting_hoc_van = model.db.setting_hoc_van;
                    db.setting_thanh_tuu = model.db.setting_thanh_tuu;
                    db.setting_bang_cap = model.db.setting_bang_cap;
                    db.setting_kinh_nghiem = model.db.setting_kinh_nghiem;
                    db.setting_ky_nang = model.db.setting_ky_nang;
                    db.setting_gioi_tinh = model.db.setting_gioi_tinh;
                    db.setting_khoa = model.db.setting_khoa;
                    db.setting_nien_khoa = model.db.setting_nien_khoa;
                    db.setting_mang_xa_hoi = model.db.setting_mang_xa_hoi;
                    repo._context.SaveChanges();
                }
                return Json(model);
        
        
        }
        [HttpPost]
        public async Task<IActionResult> createCertificate([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_certificate_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.db.description))
            {
                ModelState.AddModelError("db.description", "required");
            }
            if (!String.IsNullOrEmpty(model.db.url))
            {
                if (model.db.url.StartsWith("http://") || model.db.url.StartsWith("https://"))
                {

                }
                else
                {
                    ModelState.AddModelError("db.url", "system.khong_hop_le");
                }
            }
            if(model.db.from_date!= null && model.db.to_date!= null)
            {
                if(model.db.from_date > model.db.to_date)
                {
                    ModelState.AddModelError("db.from_date", "system.thoi_gian_bat_dau_phai_nho_hon_thoi_gian_ket_thuc");
                }
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }

            if (model.db.id == "0")
            {
                var db = new sys_certificate_user_db();
                db.id =  Guid.NewGuid().ToString();
                db.description = model.db.description;
                db.image = model.db.image;
                db.issued_by = model.db.issued_by;          
                db.user_id = getUserId();
                
                db.update_date =DateTime.Now;
                db.update_by = model.db.update_by;
                db.url = model.db.url;
                db.hinh_thuc = model.db.hinh_thuc;
                if(db.hinh_thuc == 2)
                {
                    db.from_date =null;
                    db.to_date = null;
                }
                else
                {
                    db.from_date = model.db.from_date;
                    db.to_date = model.db.to_date;
                }
             

                await repo._context.sys_certificate_users.AddAsync(db);
                repo._context.SaveChanges();
            }
            else
            {
                var db  = repo._context.sys_certificate_users.Where(d => d.id == model.db.id).SingleOrDefault();
                db.description = model.db.description;
                db.description = model.db.description;
                db.image = model.db.image;
                db.issued_by = model.db.issued_by;
             
                db.url = model.db.url;
                db.hinh_thuc = model.db.hinh_thuc;
                db.update_date = DateTime.Now;
                if (db.hinh_thuc == 2)
                {
                    db.from_date = null;
                    db.to_date = null;
                }
                else
                {
                    db.from_date = model.db.from_date;
                    db.to_date = model.db.to_date;
                }
                repo._context.SaveChanges();
            }
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> createWorkHistory([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_work_history_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.db.company))
            {
                ModelState.AddModelError("db.company", "required");
            }
            if (model.db.from_date != null && model.db.to_date != null)
            {
                if (model.db.from_date > model.db.to_date)
                {
                    ModelState.AddModelError("db.from_date", "system.thoi_gian_bat_dau_phai_nho_hon_thoi_gian_ket_thuc");
                }
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (model.db.id == "0")
            {
                var db = new sys_work_history_user_db();
                db.id = Guid.NewGuid().ToString();
                db.company = model.db.company;
                db.description = model.db.description;
                db.position = model.db.position;
                db.from_date = model.db.from_date;
                db.to_date = model.db.to_date;
                db.update_date = DateTime.Now;
                db.update_by = model.db.update_by;
                db.user_id = getUserId();
                await repo._context.sys_work_history_users.AddAsync(db);
                repo._context.SaveChanges();
            }
            else
            {
                var db = repo._context.sys_work_history_users.Where(d => d.id == model.db.id).SingleOrDefault();
                db.company = model.db.company;
                db.description = model.db.description;
                db.position = model.db.position;
                db.from_date = model.db.from_date;
                db.to_date = model.db.to_date;
                db.update_date = DateTime.Now;
                db.update_by = model.db.update_by;
                repo._context.SaveChanges();
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> createEducation([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_education_user_model>(json.GetValue("data").ToString());

            if(model.db.from_year >model.db.to_year)
            {
                ModelState.AddModelError("db.from_year", "system.nam_bat_dau_phai_nho_hon_nam_ket_thuc");
              
            }    

            if (string.IsNullOrEmpty(model.db.school))
            {
                ModelState.AddModelError("db.school", "required");
            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (model.db.id == "0")
            {
                var db = new sys_education_user_db();
                db.id = Guid.NewGuid().ToString();
                db.school = model.db.school;
                db.degree = model.db.degree;
                db.speciality = model.db.speciality;
                db.from_year = model.db.from_year;
                db.to_year = model.db.to_year;
                db.user_id = getUserId();
                await repo._context.sys_education_users.AddAsync(db);
                repo._context.SaveChanges();
            }
            else
            {
                var db = repo._context.sys_education_users.Where(d => d.id == model.db.id).SingleOrDefault();
                db.school = model.db.school;
                db.degree = model.db.degree;
                db.speciality = model.db.speciality;
                db.from_year = model.db.from_year;
                db.to_year = model.db.to_year;
                repo._context.SaveChanges();
            }




            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> register([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            //var check = checkModelStateCreate(model);

            if ((model.agree ?? false) == false) { }
            if (string.IsNullOrEmpty(model.password) )
            {
                ModelState.AddModelError("db.password", "required");
            }
            else
            {
                if(model.password.Length < 8)
                {
                    ModelState.AddModelError("db.password", "msgmatkhau");
                }
            }

            if (string.IsNullOrEmpty(model.repassword))
            {
                ModelState.AddModelError("db.repass", "required");
               
            }
            else
            {

                if (model.password != model.repassword && model.password!="")
                {
                    ModelState.AddModelError("db.repass", "matkhaukhongkhop");
                }

            }

           
            if (string.IsNullOrEmpty(model.db.email))
            {
                ModelState.AddModelError("db.email", "required");
            }
            if (!string.IsNullOrEmpty(model.db.email))
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(model.db.email);
                if (checkEmail == false)
                {
                    ModelState.AddModelError("db.email", "system.emailKhongHopLe");
                }
                else
                {
                    var email = model.db.email;// CMAESCrypto.EncryptText(item.db.email);
                    var checkemail = repo.FindAll().Where(d => d.db.email == email && d.db.Id != model.db.Id && d.db.status_del !=0).Count();
                    if (checkemail > 0)
                    {
                        ModelState.AddModelError("db.email", "existed");
                    }
                }


            }
            if (string.IsNullOrEmpty(model.capcha))
            {
                ModelState.AddModelError("capcha", "required");
            }
            else
            {
                var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
                if (CaptchaCode.ToLower() != model.capcha.ToLower())
                {
                    ModelState.AddModelError("capcha", "captcha_invalid");
                }

            }


            if (!ModelState.IsValid)
            {
                return generateError();
            }
           
            var modelN= repo.FindAll().Where(u => u.db.email == model.db.email && u.db.status_del==0).FirstOrDefault();

            var xac_thuc_dang_ky_tai_khoan = "01";
            if(modelN == null)
            {
                model.db.Id = Guid.NewGuid().ToString();
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);

                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;

                model.db.status_del = 0;
                model.db.Username = model.db.email;
                //1 admin 0 user thường
                model.db.type = 0;
                if (model.db.avatar_path == null)
                {
                    model.db.avatar_path = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.avatar).FirstOrDefault(); //_appsetting.avatar;
                }
                if (model.db.cover_image == null)
                {
                    model.db.cover_image = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.image).FirstOrDefault();//_appsetting.cover;
                }
                var i=await repo.insert(model);
                if (i == 1)
                {
                    await send_otp(model.db.Id, xac_thuc_dang_ky_tai_khoan);
                }

           


            }
            else
            {
                model.db.Id = modelN.db.Id;
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);

                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;

                model.db.status_del = 0;
                model.db.Username = model.db.email;
                if (model.db.avatar_path == null)
                {
                    model.db.avatar_path = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.avatar).FirstOrDefault();
                }
                if (model.db.cover_image == null)
                {
                    model.db.cover_image = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.image).FirstOrDefault();
                }
                var u = await repo.update(modelN);
                if (u == 1)
                {
                    await send_otp(modelN.db.Id, xac_thuc_dang_ky_tai_khoan);
                }
            }

              
               
            

            return Json(model);

            //var query = repo._context.users.Where(t => t.email == model.db.email).FirstOrDefault();
            //var maumail = repo._context.sys_template_mails.Where(t => t.id_type == 1 + "").FirstOrDefault();
            //var body = maumail.template ?? "";
            //body = body.Replace("@@full_name@@", query.FirstName + " " + query.LastName);
            //body = body.Replace("@@user_name@@", query.Username);
            //body = body.Replace("@@url_reset@@", "https://customercare.worldsoft.com.vn/" + "reset-password?token=" + HttpUtility.UrlEncode(encryptconfirmparam));

            //await _mailService.SendEmailAsync(new MailRequest
            //{
            //    Body = body,
            //    Subject = maumail.name,
            //    ToEmail = query.email, //CMAESCrypto.DecryptText(),
            //    CCEmail = "",
            //});

        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            if (!string.IsNullOrEmpty(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
            }
            if (model.db.avatar_path == null)
            {
                model.db.avatar_path = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.avatar).FirstOrDefault(); //_appsetting.avatar;
            }
            if (model.db.cover_image == null)
            {
                model.db.cover_image = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 1).Select(q => q.image).FirstOrDefault();//_appsetting.cover;
            }
            model.db.type = model.type == true ? 1 : 0;

            await repo.update(model);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> xac_thuc([FromBody] JObject json)
        {

            var user_id = "";
            var code = "";
            try { user_id = json.GetValue("user_id").ToString(); } catch { }

            try { code = json.GetValue("code").ToString(); } catch { }

     
            var user = repo._context.users.Where(t => t.Id == user_id).FirstOrDefault();

            if (user !=null && user.otp == code)
            {

                var xac_thuc_tai_khoan_thanh_cong = "02";
                send_mail(user_id, xac_thuc_tai_khoan_thanh_cong);
                return Json("");
            }
            else
            {
                return Json("Mã xác thực không đúng");
            }
        }
       
        public async Task send_otp(string id,string type) { 

            var rnd = new Random();
            var code = rnd.Next(11111, 99999).ToString();
           
              
                var user = repo._context.users.Where(q => q.Id == id).FirstOrDefault();


                user.otp = code;
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
 
                body = body.Replace("@@otp@@", user.otp);
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

                }

          

        }
       

        [HttpPost]
        public async Task<IActionResult> invite_user([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            // 1 mời user trên hệ thống , 2 gửi email mời người dùng gia nhập
            var type = json.GetValue("type").ToString();
            var email = json.GetValue("email").ToString();
            var user_id = getUserId();
            if (user_id == id) return Json("");
            long id_invite = 0;
            var check = repo._context.sys_user_ban_bes.Where(d => d.user_id_ban_be == id && d.user_id == user_id).Count();
            if(check == 0)
            {
                var db = new sys_user_ban_be_db
                {
                    date_action = DateTime.Now,
                    email_invite = "",
                     status_del = 3,
                    user_id = user_id,
                    user_id_ban_be = id,
                };

                var db1 = new sys_user_ban_be_db
                {
                    date_action = DateTime.Now,
                    email_invite = "",
                    status_del = 2,
                    user_id = id,
                    user_id_ban_be = user_id,
                };
                repo._context.sys_user_ban_bes.Add(db);
                repo._context.sys_user_ban_bes.Add(db1);
                repo._context.SaveChanges();
                id_invite = db.id;
            }
            else
            {
                if (type == "2")
                {

                    var db = new sys_user_ban_be_db
                    {
                        date_action = DateTime.Now,
                        email_invite = email,
                        status_del = 4,
                        user_id = user_id,
                        user_id_ban_be = "",
                    };
                    repo._context.sys_user_ban_bes.Add(db);
                    repo._context.SaveChanges();
                    id_invite = db.id;

                }

            }

            return Json(id_invite);
        }
        public IActionResult search_common([FromBody] JObject json)
        {



            var filter = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("filter").ToString());
            var search_key = (filter["search_key"] ?? "").Trim().ToLower().Normalize();
            var id_khoa = (filter["id_khoa"] ?? "");
            var nien_khoa = (filter["nien_khoa"] ?? "");
            var page = int.Parse(json.GetValue("page").ToString());

            var id_user = getUserId();
            var search_query = repo._context.users
              .Where(d => d.status_del == 1)
                 .Where(d => d.Id != id_user)
                    .Where(d => d.id_khoa == id_khoa || id_khoa=="-1")
                       .Where(d => d.school_year == int.Parse(nien_khoa) || nien_khoa == "-1")
              .Where(d => (d.Username ?? "").ToLower().Equals(search_key) || (d.phone ?? "").ToLower().Equals(search_key) || (d.full_name ?? "").ToLower().Contains(search_key))
               .Select(t => new
               {
                   id = t.Id,
                   status_graduate=t.status_graduate,
                   avatar_path =t.avatar_path,
                   status_del =  repo._context.sys_user_ban_bes.Where(d=>d.user_id ==  id_user && d.user_id_ban_be ==  t.Id).Select(d=>d.status_del).FirstOrDefault()??0,
                   name = (t.full_name ?? t.email) ,
                   khoa_name = repo._context.sys_khoas.Where(d => d.id == t.id_khoa).Select(d => d.name).SingleOrDefault() ?? "",
                   school_year = t.school_year ,
                   id_invite = repo._context.sys_user_ban_bes.Where(d => d.user_id == id_user && d.user_id_ban_be == t.Id).Select(d => d.id).FirstOrDefault() ,
               });
            search_query = search_query.OrderBy(d => d.name);
            var total = search_query.Count();
            var lst = search_query.Skip(page * 5).Take(5).ToList();
            var result = new
            {
                lst_ban_be = lst,
                total_item = total,
                total_page = Math.Round((decimal)(total / 5)) + (total % 5 == 0 ? 0 : 1),
                page = page,
            };
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> get_list_ban_be([FromBody] JObject json)
        {
            Random rnd = new Random();
            var user_id = getUserId();
            var check = repo._context.sys_user_ban_bes.Where(d=>d.user_id ==  user_id).Select(t => new
            {
                id = t.id,
                user_id = t.user_id_ban_be,
                status_del = t.status_del,
                full_name = repo._context.users.Where(d => d.Id == t.user_id_ban_be).Select(d => d.full_name ?? d.email).SingleOrDefault(),
                avatar_path = repo._context.users.Where(d => d.Id == t.user_id_ban_be).Select(d => d.avatar_path).SingleOrDefault(),
            }).OrderBy(d=>d.full_name).ToList();
            return Json(check);
        }


        [HttpPost]
        public async Task<IActionResult> action_invite([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();

            // 1 đồng y , 2 không đồng ý
            var action = json.GetValue("action").ToString();
            var user_id = getUserId();
            var check = repo._context.sys_user_ban_bes.Where(d => d.id==long.Parse(id)).FirstOrDefault();
            if (check != null)
            {
                if (action == "1")
                {
                    check.status_del = 1;
                    check.date_action = DateTime.Now;
                    var db_user = repo._context.sys_user_ban_bes.Where(d => d.user_id == check.user_id_ban_be && d.user_id_ban_be ==  check.user_id ).FirstOrDefault();
                    db_user.status_del = 1;
                    db_user.date_action = DateTime.Now;
                    repo._context.SaveChanges();
                }
                else
                {
                    var db_user = repo._context.sys_user_ban_bes.Where(d => d.user_id == check.user_id_ban_be && d.user_id_ban_be == check.user_id).FirstOrDefault();
                    repo._context.sys_user_ban_bes.Remove(check);
                    repo._context.sys_user_ban_bes.Remove(db_user);
                    repo._context.SaveChanges();
                }
            }
          

            return Json("");
        }



        [HttpPost]
        public async Task<IActionResult> updateProfile([FromBody] JObject json)
        {
            var user_id = getUserId();
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());



            //type_update
            //1 main image , 2 avatar , 3 info , 4 link youtobe 
          
                model.db.Id = user_id;
                var db = repo._context.users.Where(d => d.Id == model.db.Id).FirstOrDefault();
                if (model.type_update == 1)
                {

                    db.cover_image = model.db.cover_image;
                    repo._context.SaveChanges();

                }
                else if (model.type_update == 2)
                {
                    db.avatar_path = model.db.avatar_path;
                    repo._context.SaveChanges();
                }
                else if (model.type_update == 3)
                {
                //var check = checkModelStateEdit(model);

                if (!string.IsNullOrEmpty(model.db.phone))
                {
                    if (model.db.phone.Length != 10)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }
                }
                else
                {


                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
                        |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
                        |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
                        |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(model.db.phone);
                    if (checkSDT == false)
                    {
                        ModelState.AddModelError("db.phone", "system.soDienThoaiKhongHopLe");
                    }
                    else
                    {
                        var dienthoai = model.db.phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

                        var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.Id != model.db.Id).Count();
                        if (checkdienthoai > 0)
                        {
                            ModelState.AddModelError("db.phone", "existed");
                        }
                    }
                }
                       
                    
                   

                if (model.db.date_of_birth != null)
                {
                    if (model.db.date_of_birth > DateTime.Now)
                    {
                        ModelState.AddModelError("db.date_of_birth", "system.khong_duoc_phep_chon_ngay_tuong_lai");
                    }
                }

              


                if (model.db.sex== null ||model.db.sex==0)
                    {
                        ModelState.AddModelError("db.sex", "required");
                    }
                    //if (model.db.status_graduate == null || model.db.status_graduate == 0) 
                    //{
                    //    ModelState.AddModelError("db.status_graduate", "required");
                    //}
                    //if (model.db.status_graduate == 1 || model.db.status_graduate == 2)
                    //{
                    //    if (model.db.school_year == 0)
                    //    {
                    //        ModelState.AddModelError("db.school_year", "required");
                    //    }
                    //    if (string.IsNullOrEmpty(model.db.id_khoa))
                    //    {
                    //        ModelState.AddModelError("db.id_khoa", "required");
                    //    }
                    //}



                if (string.IsNullOrEmpty(model.db.full_name))

                    {
                        ModelState.AddModelError("db.full_name", "required");
                    }
                    if (!ModelState.IsValid)
                    {
                        return generateError();
                    }
                    if (!string.IsNullOrEmpty(model.password))
                    {
                        byte[] passwordHash, passwordSalt;
                        CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                        model.db.PasswordHash = passwordHash;
                        model.db.PasswordSalt = passwordSalt;
                    }
                    await repo.update(model);
                    if (model.db.status_del == 3|| model.db.status_del==5)
                    {
                        guiDuyetHoSo(model.db.Id);
                    }
                   
                }
                else
                {


                    if (!model.db.facebook_link.StartsWith("https://www.facebook.com/") && !String.IsNullOrEmpty(model.db.facebook_link))
                    {
                        ModelState.AddModelError("db.facebook_link", "system.duong_link_facebook_khong_hop_le");
                    }
                    if (!model.db.linkedin_link.StartsWith("https://www.linkedin.com") && !String.IsNullOrEmpty(model.db.linkedin_link))
                    {
                        ModelState.AddModelError("db.linkedin_link", "system.duong_link_linkedin_khong_hop_le");
                    }
                    if (!model.db.youtube_link.StartsWith("https://www.youtube.com/") && !String.IsNullOrEmpty(model.db.youtube_link))
                    {
                        ModelState.AddModelError("db.youtube_link", "system.duong_link_youtube_khong_hop_le");
                    }
                    if (!model.db.website_link.StartsWith("https://") && !model.db.website_link.StartsWith("http://") && !String.IsNullOrEmpty(model.db.website_link))
                    {
                        ModelState.AddModelError("db.website_link", "system.duong_link_website_khong_hop_le");
                    }
                    if (!model.db.instagram_link.StartsWith("https://www.instagram.com/") && !String.IsNullOrEmpty(model.db.instagram_link))
                    {
                        ModelState.AddModelError("db.instagram_link", "system.duong_link_instagram_khong_hop_le");
                    }
                    if (!model.db.twitter_link.StartsWith("https://twitter.com") && !String.IsNullOrEmpty(model.db.twitter_link))
                    {
                        ModelState.AddModelError("db.twitter_link", "system.duong_link_twitter_khong_hop_le");
                    }
                if (!ModelState.IsValid)
                {
                    return generateError();
                }
                db.facebook_link = model.db.facebook_link;
                    db.linkedin_link = model.db.linkedin_link;
                    db.youtube_link = model.db.youtube_link;
                    db.website_link = model.db.website_link;
                    db.instagram_link = model.db.instagram_link;
                    db.twitter_link = model.db.twitter_link;
                    repo._context.SaveChanges();

                }

         
          

            return Json(model);
        }

      

        public async Task<IActionResult> deleteEducation([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteEducation(id, getUserId());
            return Json("");
        }
        
        public async Task<IActionResult> deleteUngTuyen([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteUngTuyen(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> deleteSuccess([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteSucess(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> deleteCertificate([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteCertificate(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> deleteWorkHistory([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteWorkHistory(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> deleteExperience([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteExperience(id, getUserId());
            return Json("");
        }


        
        public async Task<IActionResult> guiDuyetHoSo(string id)
        {
            //var id = json.GetValue("id").ToString();
            repo.guiDuyetHoSo(id);
            return Json("");
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id,getUserId());
            return Json("");
        }
        public async Task<IActionResult> deleteFile([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.deleteFile(id);
            return Json("");
        }

        

        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(id);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> changePasswordByAdmin([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("new_password", "required");
            }
            //if (ú)

            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
            }
            await repo.updatePassword(model);

            return Json(model);
        }
      
        [HttpPost]
        public async Task<IActionResult> changePassword([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());


            var UserId = getUserId();
            model.db.Id = UserId;
            var user = AuthenticateId(UserId, model.oldPassword);
            if (user == null)
                return BadRequest(new { message = "Old Password is incorrect" });
            var user_login = repo._context.users.Where(q => q.Id == UserId).SingleOrDefault();
            if (string.IsNullOrEmpty(model.password))
            {
                ModelState.AddModelError("new_password", "required");
            }
            //if (ú)

            if (!ModelState.IsValid)
            {
                return generateError();
            }
            if (!string.IsNullOrWhiteSpace(model.password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
                model.db.Id = UserId;
            }
            



            await repo.updatePassword(model);

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> changePasswordNonLogin([FromBody] JObject json)
        {
            var password = json.GetValue("password").ToString();
            var repassword = json.GetValue("repassword").ToString();
            var idtoken = "";
            try {  idtoken = json.GetValue("idtoken").ToString(); } catch { }
           
            var idUser = CMAESCrypto.DecryptText(idtoken);
            var user = repo._context.users.Where(d => d.Id == idUser).SingleOrDefault();
            if(user.token_reset_pass==null)
            {
               ModelState.AddModelError("token", "system.tokendahethan");
                
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "required");
            }
            else
            {
                if (password.Length < 8)
                {
                    ModelState.AddModelError("password", "msgmatkhau");
                }
            }

            if (string.IsNullOrEmpty(repassword))
            {
                ModelState.AddModelError("repass", "required");

            }
            else
            {

                if (password != repassword && password != "")
                {
                    ModelState.AddModelError("repass", "matkhaukhongkhop");
                }

            }
            if (!ModelState.IsValid)
            {
                return generateError();
            }
            sys_user_model model = new sys_user_model();
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                model.db.PasswordHash = passwordHash;
                model.db.PasswordSalt = passwordSalt;
                model.db.Id = idUser;
                model.password = password;
            }
            await repo.updatePassword(model);

            return Json(model);

        }

        public IActionResult getListUserSearch([FromBody] JObject json)
        {

            var id = json.GetValue("search").ToString().ToLower().Trim();
            var result = repo._context.users
                .Where(d=>d.status_del ==1)
                .Where(d => (d.Username ?? "").ToLower().Equals(id) || (d.phone ?? "").ToLower().Equals(id) || (d.full_name ?? "").ToLower().Contains(id))
                 .Select(t => new
                 {
                     id = t.Id,
                     name = (t.full_name?? t.email)  +   (string.IsNullOrEmpty(t.id_khoa)?"" : "( " + (repo._context.sys_khoas.Where(d=>d.id ==t.id_khoa ).Select(d=>d.name).SingleOrDefault()??"")+   (t.school_year==0?"":" - Khóa "+t.school_year )  +" )"),
                 }).Take(5).ToList();
            return Json(result);
        }
        [HttpPost]
        public IActionResult checkResetPass([FromBody] JObject json)
        {
            var q = json.GetValue("token").ToString();
            try
            {
                var decrypt = CMAESCrypto.DecryptText(q);
                var id = decrypt.Replace("confirm", "").Split("@@")[0];
                var token = decrypt.Replace("confirm", "").Split("@@")[1];
                var update = repo._context.users.Where(t => t.Id == id).FirstOrDefault();
                if(update.expiration_date_reset_pass.Value.AddMinutes(15).Ticks<DateTime.Now.Ticks)
                {
                    return Json(false);
                }    
                if(update.token_reset_pass!=token)
                {
                    return Json(false);
                }
                else
                {
                    return Json(CMAESCrypto.EncryptText(id));
                }
                
            }
            catch (Exception e)
            {
                return  Json(false);
            }

        }
        public async Task<IActionResult> forgot_pass([FromBody] JObject json)
        {
            var email = json.GetValue("email").ToString();
            var capcha = json.GetValue("capcha").ToString();
            var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");

            if (String.IsNullOrEmpty(capcha))
            {
                ModelState.AddModelError("capcha", "required");
            }
            else
            {
                if (CaptchaCode.ToLower() != capcha.ToLower())
                {
                    ModelState.AddModelError("capcha", "captcha_invalid");
                }
            }
       
            //var checkEmail = CMAESCrypto.EncryptText(email);
          
            var user = repo._context.users.Where(d => d.status_del != 0 && d.status_del != 2).SingleOrDefault(x => x.Username == email);
            var quen_mat_khau = "05";
            var type_mail = repo._context.sys_type_mails.Where(t => t.code == quen_mat_khau).FirstOrDefault();
            var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();

            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("db.email", "required");

            }
            else
            {
               


                if (user == null)
                {
                    var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                  + "@"
                                  + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                    var checkEmail = rgEmail.IsMatch(email);
                    if (checkEmail == false)
                    {
                        ModelState.AddModelError("db.email", "Email không hợp lệ");

                    }
                    else
                    {
                        ModelState.AddModelError("db.email", "Email này chưa đăng ký");

                    }
                 
                }
             

            }

            if (!ModelState.IsValid)
            {
                return generateError();
            }

            user.token_reset_pass = DateTime.Now.Ticks.ToString();
            user.expiration_date_reset_pass = DateTime.Now;
            repo._context.SaveChanges();
            var encryptconfirmparam = CMAESCrypto.EncryptText(user.Id + "@@" + user.token_reset_pass);
            //encryptconfirmparam = encryptconfirmparam.Replace("%", "");
            var body = maumail.template ?? "";
            body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
            body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value +"/sign-in");
            body = body.Replace("@@url@@", "https://" + Request.Host.Value);
            body = body.Replace("@@user_name@@", user.FirstName +" "+ user.LastName);
            body = body.Replace("@@url_reset@@", "https://" + Request.Host.Value + "/reset-password?token="+HttpUtility.UrlEncode(encryptconfirmparam));
            _mailService.SendEmailAsync(new MailRequest
            {
                Body = body,
                Subject = maumail.name,
                ToEmail = user.email, //CMAESCrypto.DecryptText(),
                CCEmail = "",
            });

            return generateSuscess();
        }
        public User AuthenticateId(string userid, string password)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(password))
                return null;

            var user = repo._context.users.SingleOrDefault(x => x.Id == userid);
            //var user = _context.Users.SingleOrDefault(x => x.Username == "Administrator");
            // check if username exists
            if (user == null)
                return null;
                
            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
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
                var status_del = int.Parse(dictionary["status_del"]);
                //var type = int.Parse(dictionary["type"]);
                var user = getUser();
                var query = Enumerable.Empty<sys_user_model>().AsQueryable();
               
                    query= repo.FindAll()
                      .Where(d => d.db.status_del == status_del)
                         //.Where(d => d.db.type == type)
                     .Where(d => d.db.phone.Contains(search) || d.db.email.Contains(search)|| d.db.full_name.Contains(search)|| d.db.Username.Contains(search));
              
                
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(q=>q.db.time_input).Skip(param.Start).Take(param.Length)
                .ToList());

                dataList.ForEach(q =>
                {
                    q.type = q.db.type == 1 ? true : false;
                    q.file = repo._context.sys_user_fileuploads.Where(d => d.user_id == q.db.Id).
                    Select(d => new sys_user_fileupload_model
                    {
                        db = d
                    }).OrderByDescending(q => q.db.upload_date).FirstOrDefault();
                  
                    q.cau_hinh_quyen_rieng_tu = repo._context.sys_cau_hinh_quyen_rieng_tus.Where(d => d.user_id == q.db.Id).
                       Select(d => new sys_cau_hinh_quyen_rieng_tu_model
                       {
                           db = d
                       }).FirstOrDefault();
                    q.user_ung_tuyen = repo._context.sys_user_ung_tuyens.Where(d => d.user_id == q.db.Id).
                    Select(d => new sys_user_ung_tuyen_model
                    {
                        db = d,
                        ten_tien_te = repo._context.sys_tien_tes.Where(q => q.id == d.tien_te).Select(q => q.name).FirstOrDefault()
                    }).FirstOrDefault();
                    removeUserModel(q);

             
                    
                        q.list_user_education = repo._context.sys_education_users.Where(d => d.user_id == q.db.Id).
                        Select(d => new sys_education_user_model
                        {
                            db = d,

                            degree_name = repo._context.sys_degrees.Where(q => q.id == d.degree).Select(q => q.name).FirstOrDefault(),

                        }).OrderBy(q => q.db.to_year).ToList();

                    


               
                        q.list_user_success = repo._context.sys_success_users.Where(d => d.user_id == q.db.Id).
                          Select(d => new sys_success_user_model
                          {
                              db = d
                          }).ToList();
                    




                    
                        q.list_user_certificate = repo._context.sys_certificate_users.Where(d => d.user_id == q.db.Id).
                   Select(d => new sys_certificate_user_model
                   {
                       db = d
                   }).OrderBy(q => q.db.to_date).ToList();

                    


                    
                        q.list_user_work_history = repo._context.sys_work_history_users.Where(d => d.user_id == q.db.Id).
                    Select(d => new sys_work_history_user_model
                    {
                        db = d
                    }).OrderBy(q => q.db.to_date).ToList();


                    

                 

                        q.list_user_experience = repo._context.sys_experience_users.Where(d => d.user_id == q.db.Id).
                           Select(d => new sys_experience_user_model
                           {
                               db = d
                           }).OrderByDescending(q => q.db.update_by).ToList();

                  

                });

                DTResult<sys_user_model> result = new DTResult<sys_user_model>
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
        public async Task<IActionResult> ImportFromExcel()
        {
            var error = "";
            IFormFile file = Request.Form.Files[0];

            string folderName = "import_excel";

            var currentpath = Directory.GetCurrentDirectory();

            string newPath = Path.Combine(currentpath, "file_upload", folderName);
            var tick = Guid.NewGuid();
            //var tick = Guid.NewGuid();
            //var filename = formFile.FileName.Trim('"') + "";
            //filename = StringFunctions.NonUnicode(filename);
            //var currentpath = Directory.GetCurrentDirectory();
            //var path = Path.Combine(currentpath, "file_upload", "avatar");
            //if (!Directory.Exists(path))
            //    Directory.CreateDirectory(path);
            //var pathsave = Path.Combine(path, tick + "." + filename.Split(".").Last());
            //using (System.IO.Stream stream = new FileStream(pathsave, FileMode.Create))
            //{
            //    await formFile.CopyToAsync(stream);
            //}


            //StringBuilder sb = new StringBuilder();
        
            if (!Directory.Exists(newPath))

            {

                Directory.CreateDirectory(newPath);

            }

            var list_cell = new List<cell>();

            var list_row = new List<row>();
            if (file.Length > 0)

            {

                string sFileExtension = Path.GetExtension(file.FileName).ToLower();

                ISheet sheet;

                string fullPath = Path.Combine(newPath, tick + "." + file.FileName.Split(".").Last());

                try
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))

                    {

                        file.CopyTo(stream);

                        stream.Position = 0;

                        if (sFileExtension == ".xls")

                        {

                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  

                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  

                        }

                        else

                        {

                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  

                            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

                        }

                        IRow headerRow = sheet.GetRow(0); //Get Header Row

                        int cellCount = headerRow.LastCellNum;

                        //sb.Append("<table class='table table-bordered'><tr>");

                        //Header
                        //for (int j = 0; j < cellCount; j++)

                        //{

                        //    NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);

                        //   if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;

                        //   sb.Append("<th>" + cell.ToString() + "</th>");

                        //}
                        //sb.Append("</tr>");
                        //sb.AppendLine("<tr>");

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File

                        {

                            IRow row = sheet.GetRow(i);

                            if (row == null) continue;

                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                            for (int j = row.FirstCellNum; j < cellCount; j++)

                            {

                                if (row.GetCell(j) != null)
                                {
                                    var cell = new cell();

                                    var value = row.GetCell(j).ToString();


                                    cell.value = value;
                                    list_cell.Add(cell);
                                }

                                //sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");

                            }

                            var data_row = new row();


                            data_row.key = i.ToString();
                            data_row.list_cell = list_cell;
                            list_cell = new List<cell>();
                            list_row.Add(data_row);
                            //sb.AppendLine("</tr>");

                        }

                        //sb.Append("</table>");

                    }

                  
                    for (int ct = 0; ct < list_row.Count(); ct++)
                    {
                        var fileImport = list_row[ct].list_cell.ToList();


                        var model = new sys_user_model();

                        var stt = (fileImport[0].value.ToString() ?? "").Trim();
                        var full_name = (fileImport[1].value.ToString() ?? "").Trim();
                        var sex =(fileImport[2].value.ToString() ?? "").Trim();
                        var date_of_birth = (fileImport[3].value.ToString() ?? "").Trim();
                        var email = (fileImport[4].value.ToString() ?? "").Trim();
                        var phone = (fileImport[5].value.ToString() ?? "").Trim();
                        var status_graduate = (fileImport[6].value.ToString() ?? "").Trim();
                        var id_khoa = (fileImport[7].value.ToString() ?? "").Trim();
                        var school_year = (fileImport[8].value.ToString() ?? "").Trim();
                        var company = (fileImport[9].value.ToString() ?? "").Trim();
                        var position =  (fileImport[10].value.ToString() ?? "").Trim();
                        var dia_chi = (fileImport[11].value.ToString() ?? "").Trim();

                        model.password = "";
                        model.db.full_name = full_name;
                        model.db.email = email;
                        model.db.phone = phone;
                        if (sex == "") {
                            model.db.sex  = null;
                        } 
                        else
                        {
                            model.db.sex  = int.Parse(sex);
                        }

                        if (date_of_birth == "")
                        {
                            model.db.date_of_birth = null;
                        }
                        else
                        {
                            model.db.date_of_birth = DateTime.Parse(date_of_birth);
                        }
                        if (status_graduate == "")
                        {
                            model.db.status_graduate = null;
                        }
                        else
                        {
                            model.db.status_graduate = int.Parse(status_graduate);
                        }
                        model.db.id_khoa = repo._context.sys_khoas.Where(q => q.code == id_khoa).Select(q => q.id).FirstOrDefault();
                        if (school_year == "")
                        {
                            model.db.school_year = null;
                        }
                        else
                        {
                            model.db.school_year = int.Parse(school_year);
                        }
                        model.db.company = company;
                        model.db.position = position;
                        model.db.dia_chi = dia_chi;
                        model.db.Username = email;

                        //user import
                        model.db.type = 1;
                        error = CheckErrorImport(model, ct + 1, error);
                        if (!string.IsNullOrEmpty(error))
                        {

                        }
                        else
                        {
                            byte[] passwordHash, passwordSalt;
                            CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
                            model.db.Id = Guid.NewGuid().ToString();
                            model.db.PasswordHash = passwordHash;
                            model.db.PasswordSalt = passwordSalt;
                            model.db.status_del = 1;
                            await repo.insert(model);




                        }


                    }
                    return Json(error);
                }
                catch
                {
                    return Json("File không đúng định dạng");
                }


            }
            else
            {
                return Json("File không đúng định dạng");

            }

        }
       
        [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            string folderName = "template";
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "file_upload", folderName);

            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_user_file_import.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_user.xlsx");
        }
        public string CheckErrorImport(sys_user_model model, int ct, string error)
        {
            if (String.IsNullOrEmpty(model.db.email))
            {
                error += "Phải nhập email  tại dòng" + (ct + 1) + "<br />";
            }
            else
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(model.db.email);
                if (checkEmail == false)
                {
                    error += "Email không hợp lệ" + (ct + 1) + "<br />";
                }
                else
                {
                    var check_exited = repo._context.users.Where(q => q.email == model.db.email).Count() > 0;
                    if (check_exited)
                    {
                        error += "Email đã tồn tại trong hệ thống tại dòng" + (ct + 1) + "<br />";
                    }
                }

               
            }



            if (!string.IsNullOrEmpty(model.db.phone))
            {

                if (model.db.phone.Length > 10)
                {
                    error += "Số điện thoại tối đa 10 số" + (ct + 1) + "<br />";
               
                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(model.db.phone);
                    if (checkSDT == false)
                    {
                        error += "Số điện thoại không hợp lệ" + (ct + 1) + "<br />";
                    }
                    else
                    {
                        var dienthoai = model.db.phone;  //CMAESCrypto.EncryptText(item.db.dienthoai);

                        var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.Id != model.db.Id).Count();
                        if (checkdienthoai > 0)
                        {
                            error += "Số điện thoại tồn tại trong hệ thống" + (ct + 1) + "<br />";
                        }
                    }
                }


            }






            if (String.IsNullOrEmpty(model.db.full_name))
            {
                error += "Phải nhập Họ tên tại dòng" + (ct + 1) + "<br />";
            }


            if (model.db.sex==null)
            {
                error += "Phải nhập giới tính tại dòng" + (ct + 1) + "<br />";
            }
            if (model.db.status_graduate == null)
            {
                error += "Phải nhập trạng thái tại dòng" + (ct + 1) + "<br />";
            }
            else
            {
                if (model.db.status_graduate == 1 || model.db.status_graduate == 2)
                {
                    if (String.IsNullOrEmpty(model.db.id_khoa))
                    {
                        error += "Phải nhập tên khoa tại dòng" + (ct + 1) + "<br />";
                    }
                    if (model.db.school_year == null)
                    {
                        error += "Phải nhập niên khóa tại dòng" + (ct + 1) + "<br />";
                    }
                }
              
            }





   

           
            return error;
        }

    }
    public static class CMAESCrypto
    {
        static string key = "123xxczxcxxzzMCbsJs4LRzjBHUFM";
        static byte[] IVAES = new byte[] { };
        static byte[] KEYAES = new byte[] { };


        public static string EncryptText(string input)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, key);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }
        public static byte[] GetMD5Hash2(byte[] bytes)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] encoded = md5.ComputeHash(bytes);
            return encoded;
        }
        static void deriveKeyAndIV(String passphrase, byte[] salt)
        {
            var password = Encoding.UTF8.GetBytes(passphrase);
            Console.WriteLine(string.Join(",", password.Select(d => Convert.ToInt32(d))));
            Console.WriteLine(string.Join(",", salt.Select(d => Convert.ToInt32(d))));
            List<byte> concatenatedHashes = new List<byte>();
            List<byte> currentHash = new List<byte>();
            bool enoughBytesForKey = false;
            List<byte> preHash = new List<byte>();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            while (!enoughBytesForKey)
            {
                if (currentHash.Count > 0)
                {
                    preHash = currentHash;
                    preHash.AddRange(password);
                    preHash.AddRange(salt);
                }
                else
                {
                    preHash = password.ToList();
                    preHash.AddRange(salt);
                }
                Console.WriteLine(string.Join(",", preHash.Select(d => Convert.ToInt32(d))));
                currentHash = GetMD5Hash2(preHash.ToArray()).ToList();

                concatenatedHashes.AddRange(currentHash);
                if (concatenatedHashes.Count >= 48) enoughBytesForKey = true;
            }

            KEYAES = concatenatedHashes.Take(32).ToArray();
            IVAES = concatenatedHashes.Skip(32).Take(16).ToArray();
        }
        public static string DecryptText(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, key);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
        static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, string passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = { 1, 5, 4, 2, 3, 2, 1, 5 };
            deriveKeyAndIV(passwordBytes, saltBytes);
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = KEYAES;
                    AES.IV = IVAES;

                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;
                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, string passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = { 1, 5, 4, 2, 3, 2, 1, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    deriveKeyAndIV(passwordBytes, saltBytes);
                    AES.Key = KEYAES;
                    AES.IV = IVAES;
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;


                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        
    
    }

}
