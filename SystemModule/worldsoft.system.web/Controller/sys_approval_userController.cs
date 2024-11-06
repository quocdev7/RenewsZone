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
namespace worldsoft.system.web.Controller
{
    public partial class sys_approval_userController : BaseAuthenticationController
    {
        private sys_user_repo repo;
        private IUserService _userService;
        private IMailService _mailService;
        public sys_approval_userController(IUserService userService, IMailService mailService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_user_repo(context);
            _userService = userService;
            _mailService = mailService;
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

    
        [HttpPost]
        public async Task<IActionResult> getUserByCompany([FromBody] JObject json)

        { 
            var search = json.GetValue("search").ToString();
            var id_company = json.GetValue("id_company").ToString();

            var result = repo.FindAll()
                .Where(q=>q.db.id_company== id_company )
                .Where(q => q.db.email.Contains(search) || q.full_name.Contains(search))

                .FirstOrDefault();

            return Json(result);
        }



        public IActionResult getUserInfo()
        {
            var user_id = getUserId();
            var result = repo.FindAll().Where(d => d.db.Id == user_id).FirstOrDefault();
            return Json(result);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.users
                .Where(d=>d.status_del==1)
                 .Select(d => new
                 {
                     id = d.Id,
                     name = d.FirstName + " "+ d.LastName
                 }).ToList();
            return Json(result);
        }
     
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
            await repo.insert(model);


            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> register([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
          
            model.db.Id = Guid.NewGuid().ToString();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(model.password, out passwordHash, out passwordSalt);
        
            model.db.PasswordHash = passwordHash;
            model.db.PasswordSalt = passwordSalt;
            model.db.status_del = 1;
            model.db.type = 2;
            await repo.insert(model);

            return Json(model);
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
            await repo.update(model);
            return Json(model);
        }


        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id,getUserId());
            return Json("");
        }

        public async Task<IActionResult> approval([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.approval(id, getUserId());
            var tai_khoan_duoc_phe_duyet = "03";
             send_mail(id, tai_khoan_duoc_phe_duyet);
            return Json("");
        }

        public async Task<IActionResult> cancel([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var reason = json.GetValue("reason").ToString();
            repo.cancel(id, getUserId(),reason);
            var tai_khoan_khong_duoc_duyet_duyet = "04";
            send_mail(id, tai_khoan_khong_duoc_duyet_duyet);
            return Json("");
        }

        [HttpPost]
        public async Task<IActionResult> send_mail(string id, string type)
        {

        
            try
            {


                var user = repo._context.users.Where(q => q.Id == id).FirstOrDefault();
                var email = user.email;
                var msg = "";
                var body = "";
                var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();

                //maumail.noidung ?? "";
                body = maumail.template;
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@email@@", user.email);
                body = body.Replace("@@ly_do_ho_so_khong_hop_le@@", user.ly_do);
                body = body.Replace("@@full_name@@", user.full_name);
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
        //[HttpPost]
        //public async Task<IActionResult> changePasswordNonLogin([FromBody] JObject json)
        //{
        //    var password = json.GetValue("password").ToString();
        //    var idtoken = json.GetValue("idtoken").ToString();
        //    var idUser = CMAESCrypto.DecryptText(idtoken);
        //    var user = repo._context.users.Where(d => d.Id == idUser).SingleOrDefault();
        //    if(user.token_reset_pass==null)
        //    {
        //        ModelState.AddModelError("token", "system.tokendahethan");
        //        return generateError();
        //    }    
        //    sys_user_model model = new sys_user_model();
        //    if (!string.IsNullOrWhiteSpace(password))
        //    {
        //        byte[] passwordHash, passwordSalt;
        //        CreatePasswordHash(password, out passwordHash, out passwordSalt);
        //        model.db.PasswordHash = passwordHash;
        //        model.db.PasswordSalt = passwordSalt;
        //        model.db.Id = idUser;
        //        model.password = password;
        //    }
        //    await repo.updatePassword(model);

        //    return Json(model);

        //}
        //[HttpPost]
        //public IActionResult checkResetPass([FromBody] JObject json)
        //{
        //    var q = json.GetValue("token").ToString();
        //    try
        //    {
        //        var decrypt = CMAESCrypto.DecryptText(q);
        //        var id = decrypt.Replace("confirm", "").Split("@@")[0];
        //        var token = decrypt.Replace("confirm", "").Split("@@")[1];
        //        var update = repo._context.users.Where(t => t.Id == id).FirstOrDefault();
        //        if(update.expiration_date_reset_pass.Value.AddMinutes(15).Ticks<DateTime.Now.Ticks)
        //        {
        //            return Json(false);
        //        }    
        //        if(update.token_reset_pass!=token)
        //        {
        //            return Json(false);
        //        }
        //        else
        //        {
        //            return Json(CMAESCrypto.EncryptText(id));
        //        }
                
        //    }
        //    catch (Exception e)
        //    {
        //        return  Json(false);
        //    }

        //}
        //public async Task<IActionResult> forgot_pass([FromBody] JObject json)
        //{
        //    var email = json.GetValue("email").ToString();
        //    //var checkEmail = CMAESCrypto.EncryptText(email);
        //    var query = repo._context.users.Where(t => t.email == email).FirstOrDefault();
        //    var maumail = repo._context.sys_template_mails.Where(t => t.id_type == 2+"").FirstOrDefault();
        //    if (query == null)
        //    {
        //        ModelState.AddModelError("email", "notvalid");
        //        return generateError();
        //    }
        //    else
        //    {
        //        var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
        //                         + "@"
        //                         + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
        //        var checkEmail = rgEmail.IsMatch(query.email);
        //        if (checkEmail == false)
        //        {
        //            ModelState.AddModelError("email", "system.emailKhongHopLe");
        //            return generateError();
        //        }
        //    }

        //    query.token_reset_pass = DateTime.Now.Ticks.ToString();
        //    query.expiration_date_reset_pass = DateTime.Now;
        //    repo._context.SaveChanges();
        //    var encryptconfirmparam = CMAESCrypto.EncryptText(query.Id + "@@" + query.token_reset_pass);
        //    //encryptconfirmparam = encryptconfirmparam.Replace("%", "");
        //    var body = maumail.template ?? "";
        //    body = body.Replace("@@user_name@@", query.FirstName +" "+query.LastName);
        //    body = body.Replace("@@url_reset@@", "https://customercare.worldsoft.com.vn/" + "reset-password?token="+HttpUtility.UrlEncode(encryptconfirmparam));
        //    await _mailService.SendEmailAsync(new MailRequest
        //    {
        //        Body = body,
        //        Subject = maumail.name,
        //        ToEmail = query.email, //CMAESCrypto.DecryptText(),
        //        CCEmail = "",
        //    });

        //    return generateSuscess();
        //}
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
                var user_id = getUserId();
                var query = Enumerable.Empty<sys_user_model>().AsQueryable();
                var config = repo._context.sys_cau_hinh_duyet_users.Where(d => d.user_id == user_id).FirstOrDefault();
                var is_quan_tri = repo._context.users.Where(d => d.Id == user_id).Select(q=>q.type).FirstOrDefault();
                if(is_quan_tri == 1) {
                    query = repo.FindAll()

                          .Where(d => d.db.status_del == 4 || d.db.status_del == 5)
                       .Where(d => d.db.status_del == status_del)

                      .Where(d => d.full_name.Contains(search)
                      || d.db.Username.Contains(search) || d.db.email.Contains(search) || d.db.dia_chi.Contains(search) || d.db.phone.Contains(search)
                      )
                      ;
                }
                else
                {
                    query = repo.FindAll()

                         .Where(d => d.db.status_del == 4 || d.db.status_del == 5)
                      .Where(d => d.db.status_del == status_del)

                     .Where(d => d.full_name.Contains(search)
                     || d.db.Username.Contains(search) || d.db.email.Contains(search) || d.db.dia_chi.Contains(search) || d.db.phone.Contains(search)
                     )
                     ;
                    if (config != null)
                    {
                        query = query.Where(d => (config.hinh_thuc == "-1" || config.hinh_thuc.Contains(d.db.status_graduate + "")) && (config.id_khoa.Contains(d.db.id_khoa) || config.id_khoa == "-1"));

                    }

                }
               
            


                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .ToList());

                dataList.ForEach(q => {
                    q.list_user_success =  repo._context.sys_success_users.Where(d => d.user_id == q.db.Id).
                    Select(d => new sys_success_user_model
                    {
                        db = d
                    }).ToList();
                    q.list_user_education  = repo._context.sys_education_users.Where(d => d.user_id == q.db.Id).
               Select(d => new sys_education_user_model
               {
                   db = d,

                   degree_name = repo._context.sys_degrees.Where(q => q.id == d.degree).Select(q => q.name).FirstOrDefault(),

               }).OrderBy(q => q.db.to_year).ToList();
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

                q.file = repo._context.sys_user_fileuploads.Where(d => d.user_id == q.db.Id).
                  Select(d => new sys_user_fileupload_model
                  {
                      db = d
                  }).OrderByDescending(q => q.db.upload_date).FirstOrDefault();

                q.user_ung_tuyen = repo._context.sys_user_ung_tuyens.Where(d => d.user_id == q.db.Id).
                Select(d => new sys_user_ung_tuyen_model
                {
                    db = d,
                    ten_tien_te = repo._context.sys_tien_tes.Where(s => s.id == d.tien_te).Select(q=>q.name).FirstOrDefault()
                }).FirstOrDefault();

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

                        var ho = (fileImport[0].value.ToString() ?? "").Trim();
                        var ten = (fileImport[1].value.ToString() ?? "").Trim();
                        var noi_xu_ly = (fileImport[2].value.ToString() ?? "").Trim();
                        var cong_ty = (fileImport[3].value.ToString() ?? "").Trim();
                        var user = (fileImport[4].value.ToString() ?? "").Trim();
                        var password = (fileImport[5].value.ToString() ?? "").Trim();
                        var phongban = (fileImport[6].value.ToString() ?? "").Trim();
                        var id_cong_ty = repo._context.sys_companys.Where(d => d.name == cong_ty).Select(d => d.id).SingleOrDefault();
                        var id_phong_ban = repo._context.sys_departments.Where(d => d.name == phongban).Select(d => d.id).SingleOrDefault();
                        model.db.FirstName = ho;
                        model.db.LastName = ten;
                        model.db.id_department = id_phong_ban;
                        model.db.id_company = id_cong_ty;
                        model.db.Username = user;
                        model.db.type = 1;
                        error = CheckErrorImport(model, ct + 1, error);
                        if (!string.IsNullOrEmpty(error))
                        {

                        }
                        else
                        {
                            byte[] passwordHash, passwordSalt;
                            CreatePasswordHash(password, out passwordHash, out passwordSalt);
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
        public ActionResult Download()
        {
            string folderName = "template";
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "file_upload", folderName);
            string Files = newPath + "\\sys_user_file_import.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_serial_number.xlsx");
        }
        public string CheckErrorImport(sys_user_model model, int ct, string error)
        {


            if (String.IsNullOrEmpty(model.db.Username))
            {
                error += "Phải nhập Username tại dòng" + (ct + 1) + "<br />";
            }
            else
            {
                var check_exited = repo._context.users.Where(q => q.Username == model.db.Username).Count() > 0;
                if (check_exited)
                {
                    error += "Tên Username  đã tồn tại trong hệ thống tại dòng" + (ct + 1) + "<br />";
                }

            }

            
            if (String.IsNullOrEmpty(model.db.FirstName))
            {
                error += "Phải nhập họ tại dòng" + (ct + 1) + "<br />";
            }
            if (String.IsNullOrEmpty(model.db.LastName))
            {
                error += "Phải nhập tên tại dòng" + (ct + 1) + "<br />";
            }
            if (String.IsNullOrEmpty(model.db.id_department))
            {
                error += "Phải nhập phòng ban tại dòng" + (ct + 1) + "<br />";
            }
            else
            {
                var check_exited = repo._context.users.Where(q => q.Username == model.db.Username).Count();

                if (check_exited==0)
                {

                }
                else
                {
                    error += "Tài khoản  tại dòng" + (ct + 1) + " đã tồn tại trong hệ thống " + "<br />";
                }
            }

           
            return error;
        }

    }
    //public static class CMAESCrypto
    //{
    //    static string key = "123xxczxcxxzzMCbsJs4LRzjBHUFM";
    //    static byte[] IVAES = new byte[] { };
    //    static byte[] KEYAES = new byte[] { };


    //    public static string EncryptText(string input)
    //    {
    //        // Get the bytes of the string
    //        byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);

    //        byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, key);

    //        string result = Convert.ToBase64String(bytesEncrypted);

    //        return result;
    //    }
    //    public static byte[] GetMD5Hash2(byte[] bytes)
    //    {
    //        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    //        byte[] encoded = md5.ComputeHash(bytes);
    //        return encoded;
    //    }
    //    static void deriveKeyAndIV(String passphrase, byte[] salt)
    //    {
    //        var password = Encoding.UTF8.GetBytes(passphrase);
    //        Console.WriteLine(string.Join(",", password.Select(d => Convert.ToInt32(d))));
    //        Console.WriteLine(string.Join(",", salt.Select(d => Convert.ToInt32(d))));
    //        List<byte> concatenatedHashes = new List<byte>();
    //        List<byte> currentHash = new List<byte>();
    //        bool enoughBytesForKey = false;
    //        List<byte> preHash = new List<byte>();
    //        MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
    //        while (!enoughBytesForKey)
    //        {
    //            if (currentHash.Count > 0)
    //            {
    //                preHash = currentHash;
    //                preHash.AddRange(password);
    //                preHash.AddRange(salt);
    //            }
    //            else
    //            {
    //                preHash = password.ToList();
    //                preHash.AddRange(salt);
    //            }
    //            Console.WriteLine(string.Join(",", preHash.Select(d => Convert.ToInt32(d))));
    //            currentHash = GetMD5Hash2(preHash.ToArray()).ToList();

    //            concatenatedHashes.AddRange(currentHash);
    //            if (concatenatedHashes.Count >= 48) enoughBytesForKey = true;
    //        }

    //        KEYAES = concatenatedHashes.Take(32).ToArray();
    //        IVAES = concatenatedHashes.Skip(32).Take(16).ToArray();
    //    }
    //    public static string DecryptText(string input)
    //    {
    //        if (string.IsNullOrEmpty(input)) return "";
    //        // Get the bytes of the string
    //        byte[] bytesToBeDecrypted = Convert.FromBase64String(input);

    //        byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, key);

    //        string result = Encoding.UTF8.GetString(bytesDecrypted);

    //        return result;
    //    }
    //    static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, string passwordBytes)
    //    {
    //        byte[] decryptedBytes = null;

    //        // Set your salt here, change it to meet your flavor:
    //        // The salt bytes must be at least 8 bytes.
    //        byte[] saltBytes = { 1, 5, 4, 2, 3, 2, 1, 5 };
    //        deriveKeyAndIV(passwordBytes, saltBytes);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (RijndaelManaged AES = new RijndaelManaged())
    //            {
    //                AES.KeySize = 256;
    //                AES.BlockSize = 128;
    //                AES.Key = KEYAES;
    //                AES.IV = IVAES;

    //                AES.Mode = CipherMode.CBC;
    //                AES.Padding = PaddingMode.PKCS7;
    //                using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
    //                {
    //                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
    //                    cs.Close();
    //                }
    //                decryptedBytes = ms.ToArray();
    //            }
    //        }

    //        return decryptedBytes;
    //    }
    //    static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, string passwordBytes)
    //    {
    //        byte[] encryptedBytes = null;

    //        // Set your salt here, change it to meet your flavor:
    //        // The salt bytes must be at least 8 bytes.
    //        byte[] saltBytes = { 1, 5, 4, 2, 3, 2, 1, 5 };

    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (RijndaelManaged AES = new RijndaelManaged())
    //            {
    //                AES.KeySize = 256;
    //                AES.BlockSize = 128;
    //                deriveKeyAndIV(passwordBytes, saltBytes);
    //                AES.Key = KEYAES;
    //                AES.IV = IVAES;
    //                AES.Mode = CipherMode.CBC;
    //                AES.Padding = PaddingMode.PKCS7;


    //                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
    //                {
    //                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
    //                    cs.Close();
    //                }
    //                encryptedBytes = ms.ToArray();
    //            }
    //        }

    //        return encryptedBytes;
    //    }
        
    
    //}

}
