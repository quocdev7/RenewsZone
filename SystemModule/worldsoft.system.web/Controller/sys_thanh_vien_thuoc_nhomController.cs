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
using worldsoft.common.Models;
using worldsoft.common.Common;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace worldsoft.system.web.Controller
{
    public partial class sys_thanh_vien_thuoc_nhomController : BaseAuthenticationController
    {
        public sys_thanh_vien_thuoc_nhom_repo repo;

        private IMailService _mailService;
        public sys_thanh_vien_thuoc_nhomController(IUserService userService, IMailService mailService,worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_thanh_vien_thuoc_nhom_repo(context);
            _mailService = mailService;
        }


        
       
        [HttpPost]
        public async Task<IActionResult> add_nguoi_dung([FromBody] JObject json)
        {

            var user_id = json.GetValue("user_id").ToString();
            var id_nhom = json.GetValue("id_nhom").ToString();
            var model = new sys_thanh_vien_thuoc_nhom_model();
            var check = checkModelStateCreate(model, user_id);
           if (!check)
           {
               return generateError();
           }
           

            model.db.id = DateTime.Now.Ticks +"";
            model.db.user_id  = user_id;
            model.db.id_nhom = id_nhom;
            model.db.create_by = getUserId();
            model.db.create_date = DateTime.Now;
            await repo._context.sys_thanh_vien_thuoc_nhoms.AddAsync(model.db);
            repo._context.SaveChanges();

            var dang_ky_thanh_cong = "30";
            sendMail(user_id, dang_ky_thanh_cong);
            


            return Json(model);
        }
        public void sendMail(string user_id,string type)
        {
            var user = repo._context.users.Where(q => q.Id == user_id).FirstOrDefault();
            Random rnd1 = new Random(8);
            var password = rnd1.Next().ToString();
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            repo._context.SaveChanges();
           

                var type_mail = repo._context.sys_type_mails.Where(t => t.code == type).FirstOrDefault();
                var maumail = repo._context.sys_template_mails.Where(t => t.id_type == type_mail.id).FirstOrDefault();

                var body = "";
                body = maumail.template;
                body = body.Replace("@@link_home@@", "https://" + Request.Host.Value);
                body = body.Replace("@@link_dang_nhap@@", "https://" + Request.Host.Value + "/sign-in");
                body = body.Replace("@@full_name@@", user.full_name);
                body = body.Replace("@@tai_khoan@@", user.email);
                body = body.Replace("@@mat_khau@@", password);

                body = body.Replace("@@current_year@@", DateTime.Now.Year.ToString());
                var dblogmail = new sys_log_mail_db();

                dblogmail.tieu_de = type_mail.name;
                dblogmail.noi_dung = body;
                dblogmail.id_template = maumail.id;
                dblogmail.email = user.email;
                dblogmail.id = Guid.NewGuid().ToString();
                dblogmail.send_date = DateTime.Now;
                dblogmail.user_id = user_id;
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
                        ToEmail = user.email, //CMAESCrypto.DecryptText(),
                        CCEmail = "",
                    });

                }
                catch (Exception e)
                {

              

                }

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

                    if(list_row.Count > 500)
                    {
                        return Json("Hệ thống cho phép Import tối đa 500 thành viên 1 lần");
                    }
                    for (int ct = 0; ct < list_row.Count(); ct++)
                    {
                        var fileImport = list_row[ct].list_cell.ToList();


                        var model = new sys_thanh_vien_thuoc_nhom_model();

                        var stt = (fileImport[0].value.ToString() ?? "").Trim();
                        var ten_nhom = (fileImport[1].value.ToString() ?? "").Trim();
                        var email = (fileImport[2].value.ToString() ?? "").Trim();

                        if (!String.IsNullOrEmpty(ten_nhom))
                        {
                            model.ten_nhom = ten_nhom;
                            model.db.id_nhom = repo._context.sys_nhom_thanh_viens.Where(q=>q.name== ten_nhom).Select(q=>q.id).FirstOrDefault();
                          
                        }
                        if (!String.IsNullOrEmpty(email))
                        {
                            model.email = email;
                            model.db.user_id = repo._context.users.Where(q => q.Username == email).Select(q => q.Id).FirstOrDefault();
                           
                        }
                     
                       

                        error = CheckErrorImport(model, ct + 1, error);
                        if (!string.IsNullOrEmpty(error))
                        {

                        }
                        else
                        {

                            model.db.id = DateTime.Now.Ticks + "";
                        
                            model.db.create_by = getUserId();
                            model.db.create_date = DateTime.Now;
                            await repo._context.sys_thanh_vien_thuoc_nhoms.AddAsync(model.db);
                            repo._context.SaveChanges();

                            var dang_ky_thanh_cong = "30";
                            sendMail(model.db.user_id, dang_ky_thanh_cong);
 

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
        public string CheckErrorImport(sys_thanh_vien_thuoc_nhom_model model, int ct, string error)
        {
            if (String.IsNullOrEmpty(model.email))
            {
                error += "Phải nhập email  tại dòng " + (ct + 1) + "<br />";
            }
            else
            {

                var rgEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z");
                var checkEmail = rgEmail.IsMatch(model.email);
                if (checkEmail == false)
                {
                    error += "Email không hợp lệ tại dòng" + (ct + 1) + "<br />";
                }
                else
                {
                    var check_exited = repo._context.users.Where(q => q.email == model.email ).Count() > 0;
                    if (!check_exited)
                    {
                        error += "Email không tồn tại trong hệ thống tại dòng " + (ct + 1) + "<br />";
                    }
                }


            }



         



            if (String.IsNullOrEmpty(model.ten_nhom))
            {
                error += "Phải nhập Họ tên tại dòng " + (ct + 1) + "<br />";
            }
            else
            {
                var check_exited = repo._context.sys_nhom_thanh_viens.Where(q => q.name == model.ten_nhom).Count() > 0;
                if (!check_exited)
                {
                    error += "Nhóm " + model.ten_nhom +" không tồn tại trong hệ thống tại dòng" + (ct + 1) + "<br />";
                }
            }



            if(!String.IsNullOrEmpty(model.ten_nhom) && !String.IsNullOrEmpty(model.email))
            {
                var check_exited = repo._context.sys_thanh_vien_thuoc_nhoms.Where(q => q.id_nhom == model.db.id_nhom && q.user_id==model.db.user_id).Count() > 0;
                if (check_exited)
                {
                    error += "Thành viên"+model.email+ " đã tồn tại trong nhóm " +model.ten_nhom+" tại dòng " + (ct + 1) + "<br />";
                }
            }




            return error;
        }

    
    [AllowAnonymous]
        public ActionResult downloadtemp()
        {
            string folderName = "template";
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "file_upload", folderName);
            string Files = newPath + "\\sys_thanh_vien_thuoc_nhom.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_thanh_vien_thuoc_nhom.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_thanh_vien_thuoc_nhom_model>(json.GetValue("data").ToString());
            //var check = checkModelStateEdit(model);
            //if (!check)
            //{
            //    return generateError();
            //}
           

            await repo.update(model);
             return Json(model);
        }


        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var user_id = getUserId();
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
             
                var id_nhom = dictionary["id_nhom"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                     .Where(d=>d.db.id_nhom == id_nhom)
                    .Where(d => string.IsNullOrEmpty(search) || d.user_name.Contains(search) || d.position.Contains(search)
                    || d.ten_cong_ty.Contains(search) || d.ten_quoc_gia.Contains(search) || d.ten_nhom.Contains(search)
                    || d.faculty.Contains(search)
                    )
                     ;
                var count = query.Count();
                var dataList = await Task.Run(() => query.Skip(param.Start).Take(param.Length)
                .OrderByDescending(d => d.db.create_date).ToList());
                DTResult<sys_thanh_vien_thuoc_nhom_model> result = new DTResult<sys_thanh_vien_thuoc_nhom_model>
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
