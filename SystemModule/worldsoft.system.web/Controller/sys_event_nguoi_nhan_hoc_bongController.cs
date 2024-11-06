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
using Microsoft.AspNetCore.Http;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace worldsoft.system.web.Controller
{
    public partial class sys_event_nguoi_nhan_hoc_bongController : BaseAuthenticationController
    {
        public sys_event_nguoi_nhan_hoc_bong_repo repo; 

        public sys_event_nguoi_nhan_hoc_bongController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_event_nguoi_nhan_hoc_bong_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_event_nguoi_nhan_hoc_bongs
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     mssv = d.mssv
                  
                 }).ToList();
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> ImportFromExcel()
        {
            var error = "";
            IFormFile file = Request.Form.Files[0];
            var id_su_kien = "";
            try
            {
                id_su_kien = Request.Form["id_su_kien"].ToString();
            }
            catch { }
               

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


                        var model = new sys_event_nguoi_nhan_hoc_bong_model();

                        var stt = (fileImport[0].value.ToString() ?? "").Trim();
                        var mssv = (fileImport[1].value.ToString() ?? "").Trim();
                        var name = (fileImport[2].value.ToString() ?? "").Trim();
                        //var dien_thoai = (fileImport[4].value.ToString() ?? "").Trim();
                        var so_tien = decimal.Parse((fileImport[3].value.ToString() ?? "").Trim());
                        var ma_tien_te = (fileImport[4].value.ToString() ?? "").Trim();

                       
                        model.db.id_su_kien = id_su_kien;
                        model.db.mssv = mssv;
                        model.db.name = name;
                        //model.db.dien_thoai = dien_thoai;
                        model.db.id_tien_te = repo._context.sys_tien_tes.Where(q=>q.code== ma_tien_te).Select(q=>q.id).FirstOrDefault();
                        model.db.so_tien = so_tien;
                      
                        //user import
                        error = CheckErrorImport(model, ct + 1, error);
                        if (!string.IsNullOrEmpty(error))
                        {

                        }
                        else
                        {
                         
                            model.db.id = Guid.NewGuid().ToString();
                            model.db.create_date = DateTime.Now;
                            model.db.create_by = getUserId();
                            model.db.update_by = getUserId();
                            model.db.update_date = DateTime.Now;
                            model.db.status_del = 1;
                            model.db.stt = repo._context.sys_event_nguoi_nhan_hoc_bongs.Where(q => q.id_su_kien == model.db.id_su_kien).Max(q=>q.stt) + 1;

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
            var currentpath = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentpath, "wwwroot", "assets", "template");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            string Files = newPath + "\\sys_event_nguoi_nhan_hoc_bong.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(Files);
            System.IO.File.WriteAllBytes(Files, fileBytes);
            MemoryStream ms = new MemoryStream(fileBytes);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "sys_event_nguoi_nhan_hoc_bong.xlsx");
        }
        public string CheckErrorImport(sys_event_nguoi_nhan_hoc_bong_model model, int ct, string error)
        {
            if (String.IsNullOrEmpty(model.db.mssv))
            {
                error += "Phải nhập mã số sinh viên tại dòng " + (ct + 1) + "<br />";
            }
            else
            {


                var check_exited_mssv = repo._context.sys_event_nguoi_nhan_hoc_bongs.Where(q => q.id_su_kien == model.db.id_su_kien && q.mssv == model.db.mssv).Count() > 0;
                if (check_exited_mssv)
                {
                    error += "Mã số sinh viên tồn tại trong sự kiện tại dòng " + (ct + 1) + "<br />";
                }



            }
        
            if (String.IsNullOrEmpty(model.db.name))
            {
                error += "Phải nhập tên sinh viên tại dòng " + (ct + 1) + "<br />";
            }


            if (!string.IsNullOrEmpty(model.db.dien_thoai))
            {

                if (model.db.dien_thoai.Length > 10)
                {
                    error += "Số điện thoại tối đa 10 số " + (ct + 1) + "<br />";

                }
                else
                {
                    var rgSoDienThoai = new Regex(@"(^[\+]?[0-9]{10,13}$) 
            |(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)
            |(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)
            |(^[(]?[\+]?[\s]?[(]?[0-9]{2,3}[)]?[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{2,4}[-\s\.]?[0-9]{0,4}[-\s\.]?$)");

                    //var rgSoDienThoai = new Regex(@"(^[0-9]{10,13}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^\+[0-9]{2}\s+[0-9]{4}\s+[0-9]{3}\s+[0-9]{3}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)|(^[0-9]{4}\.[0-9]{3}\.[0-9]{3}$)");
                    var checkSDT = rgSoDienThoai.IsMatch(model.db.dien_thoai);
                    if (checkSDT == false)
                    {
                        error += "Số điện thoại không hợp lệ " + (ct + 1) + "<br />";
                    }
                    else
                    {
                        //var dienthoai = model.db.dien_thoai; 

                        //var checkdienthoai = repo.FindAll().Where(d => d.db.phone == dienthoai && d.db.Id != model.db.Id).Count();
                        //if (checkdienthoai > 0)
                        //{
                        //    error += "Số điện thoại tồn tại trong hệ thống" + (ct + 1) + "<br />";
                        //}
                    }
                }


            }




            if (model.db.so_tien == null  || model.db.so_tien < 0)
            {
                error += "Phải nhập số tiền tại dòng " + (ct + 1) + "<br />";
            }

            if (String.IsNullOrEmpty(model.db.id_tien_te))
            {
                error += "Phải nhập tiền tệ tại dòng " + (ct + 1) + "<br />";
            }
            else
            {
                var check_exited = repo._context.sys_tien_tes.Where(q => q.id == model.db.id_tien_te && q.status_del==1).Count() > 0;
                if (!check_exited)
                {
                    error += "Tiền tệ không tồn tại trong hệ thống tại dòng " + (ct + 1) + "<br />";
                }

            }

          
            return error;
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_event_nguoi_nhan_hoc_bong_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            
            if (!check)
            {
                return generateError();
            }
            
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_event_nguoi_nhan_hoc_bong_model>(json.GetValue("data").ToString());
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


        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
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

                var search = dictionary["search"];

                var event_id = dictionary["event_id"];
                var query = repo.FindAll()
                    //.Where(d=>d.db.status_del==1)
                        .Where(d =>d.db.id_su_kien == event_id || event_id == "-1" )
                     .Where(d => d.db.name.Contains(search) || d.db.mssv.Contains(search))
                     //.Where(d => d.db.mssv.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_event_nguoi_nhan_hoc_bong_model> result = new DTResult<sys_event_nguoi_nhan_hoc_bong_model>
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
