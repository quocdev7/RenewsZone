using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using worldsoft.common.BaseClass;
using worldsoft.common.common;
using worldsoft.common.Services;
using worldsoft.DataBase.Helper;
using worldsoft.DataBase.Provider;
using worldsoft.system.data.DataAccess;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    public partial class sys_san_phamController : BaseAuthenticationController
    {
        private sys_san_pham_repo repo;

        public sys_san_phamController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_san_pham_repo(context);
        }
        public IActionResult get_title_san_pham([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var title_event = repo._context.sys_san_phams.Where(d => d.id == id).Select(d => d.ten_san_pham).SingleOrDefault()
                .Replace("<", "").Replace(">", "").Replace("&", "").Replace("\'", "").Replace("\"", "")
                .Replace("- ".ToString(), string.Empty).Replace("-".ToString(), " ");
            var tieu_de = title_event.Trim();
            tieu_de = StringFunctions.NonUnicode(tieu_de).Replace(' ', '-');
            if (tieu_de.Length > 150)
                tieu_de = tieu_de.Substring(0, 150);
            return Json(tieu_de);
        }
        [HttpPost]
        public async Task<IActionResult> delete_file([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();

            var file = repo._context.sys_san_pham_file_uploads.Where(q => q.id == id).SingleOrDefault();

            //var check = checkModelStateEdit(model);
            //if (!check)
            //{
            //    return generateError();
            //}
            var currentpath = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentpath, "file_upload", "image_upload", "sys_san_pham", file.id_san_pham);
            var pathsave = Path.Combine(path, file.uuid + "." + file.file_name.Split(".").Last());
            //string[] files = Directory.GetFiles(path, file.uuid + "*.*", SearchOption.AllDirectories);
            //if (files.Length > 0)
            //{
            //    foreach (string item in files)
            //        try
            //        {
            //            System.IO.File.Delete(item);
            //        }
            //        catch { };

            //}



            if (file != null)
            {
                repo._context.Remove(file);
                repo._context.SaveChanges();

            }

            return Json("");
        }
        public IActionResult getListSanPhamQuanTam([FromBody] JObject json)
        {
            var id_san_pham = json.GetValue("id_san_pham").ToString();

            var result = repo._context.sys_san_phams
                .Where(d => d.id != id_san_pham)
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.ten_san_pham,
                     name_en = d.ten_san_pham_en,
                     so_tien = d.so_tien,
                     id_loai = d.id_loai,
                     hinh_anh = d.hinh_anh,
                     hinh_anh_mobile = d.hinh_anh_mobile,
                     mo_ta = d.mo_ta,
                     mo_ta_en = d.mo_ta_en,
                     mo_ta_mobile = d.mo_ta_mobile,
                     mo_ta_mobile_en = d.mo_ta_mobile_en,
                     thong_tin_bo_sung = d.thong_tin_bo_sung,
                     ma_san_pham = d.ma_san_pham,
                     image = repo._context.sys_san_pham_file_uploads.Where(t => t.id_san_pham == d.id).Select(d => d.file_name).SingleOrDefault(),
                     ma_loai = repo._context.sys_loai_san_phams.Where(t => t.id == d.id_loai).Select(d => d.ma_loai).SingleOrDefault(),
                 }).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> edit_language([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_san_pham_language_model>(json.GetValue("data").ToString());
            var check = checkModelStateEditLanguage(model);
            if (!check)
            {
                return generateError();
            }

            var data = repo._context.sys_san_pham_languages.Where(q => q.id_san_pham == model.db.id_san_pham).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_san_pham_language_model();

                model_language.db.id_san_pham = model.db.id_san_pham;
                model_language.db.ten_san_pham = model.db.ten_san_pham;
                model_language.db.thong_so_ky_thuat = model.db.thong_so_ky_thuat;
                model_language.db.thong_so_ky_thuat_mobile = model.db.thong_so_ky_thuat_mobile;
                model_language.db.mo_ta = model.db.mo_ta;
                model_language.db.mo_ta_mobile = model.db.mo_ta_mobile;
                model_language.db.thong_tin_bo_sung = model.db.thong_tin_bo_sung;
                model_language.db.thong_tin_bo_sung_mobile = model.db.thong_tin_bo_sung_mobile;
                await repo._context.sys_san_pham_languages.AddAsync(model_language.db);
                repo._context.SaveChanges();
            }
            else
            {
                data.ten_san_pham = model.db.ten_san_pham;
                data.thong_so_ky_thuat = model.db.thong_so_ky_thuat;
                data.thong_so_ky_thuat_mobile = model.db.thong_so_ky_thuat_mobile;
                data.mo_ta = model.db.mo_ta;
                data.mo_ta_mobile = model.db.mo_ta_mobile;
                data.thong_tin_bo_sung = model.db.thong_tin_bo_sung;
                data.thong_tin_bo_sung_mobile = model.db.thong_tin_bo_sung_mobile;
                repo._context.SaveChanges();
            }


            return Json(model);
        }

        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_san_pham_language_model();

            model = repo._context.sys_san_pham_languages.Where(d => d.id_san_pham == id).Select(d => new sys_san_pham_language_model()
            {
                db = d
            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_san_pham_language_model();
                model.db.id_san_pham = id;
            }
            return Json(model);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.sys_san_phams
                .Where(d => d.status_del == 1).
                OrderBy(d => d.stt)
                 .Select(d => new
                 {
                     id = d.id,
                     is_check = false,
                     name = d.ten_san_pham,
                     name_en = repo._context.sys_san_pham_languages.Where(q => q.id_san_pham == d.id).Select(d => d.ten_san_pham).FirstOrDefault(),
                     so_tien = d.so_tien,
                     id_loai = d.id_loai,
                     hinh_anh = d.hinh_anh,
                     hinh_anh_mobile = d.hinh_anh_mobile,
                     mo_ta = d.mo_ta,
                     mo_ta_en = repo._context.sys_san_pham_languages.Where(q => q.id_san_pham == d.id).Select(d => d.mo_ta).FirstOrDefault(),
                     mo_ta_mobile = d.mo_ta_mobile,
                     mo_ta_mobile_en = repo._context.sys_san_pham_languages.Where(q => q.id_san_pham == d.id).Select(d => d.mo_ta_mobile).FirstOrDefault(),
                     thong_tin_bo_sung = d.thong_tin_bo_sung,
                     thong_tin_bo_sung_en = repo._context.sys_san_pham_languages.Where(q => q.id_san_pham == d.id).Select(d => d.thong_tin_bo_sung).FirstOrDefault(),
                     ma_san_pham = d.ma_san_pham,
                     image = repo._context.sys_san_pham_file_uploads.Where(t => t.id_san_pham == d.id).Select(d => d.file_name).SingleOrDefault(),
                     ma_loai = repo._context.sys_loai_san_phams.Where(t => t.id == d.id_loai).Select(d => d.ma_loai).SingleOrDefault(),
                 }).ToList();
            return Json(result);
        }
        public async Task<IActionResult> get_san_pham([FromBody] JObject json)
        {
            var id_san_pham = json.GetValue("id_san_pham").ToString();



            var san_pham = repo.FindAll().Where(q => q.db.id == id_san_pham).FirstOrDefault();
            san_pham.ten_loai = repo._context.sys_loai_san_phams.Where(q => q.id == san_pham.db.id_loai).Select(q => q.ten_loai).FirstOrDefault();
            san_pham.image = repo._context.sys_san_pham_file_uploads.Where(q => q.id_san_pham == san_pham.db.id).Select(q => q.file_path).FirstOrDefault();



            var language = repo._context.sys_san_pham_languages.Where(d => d.id_san_pham == san_pham.db.id).FirstOrDefault();

            if (language != null)
            {
                san_pham.ten_san_pham_language = language.ten_san_pham ?? "";
                san_pham.thong_so_ky_thuat_language = language.thong_so_ky_thuat ?? "";
                san_pham.thong_so_ky_thuat_language_mobile = language.thong_so_ky_thuat_mobile ?? "";
                san_pham.thong_tin_bo_sung_language = language.thong_tin_bo_sung ?? "";
                san_pham.thong_tin_bo_sung_language_mobile = language.thong_tin_bo_sung_mobile ?? "";
                san_pham.mo_ta_language = language.mo_ta ?? "";
                san_pham.mo_ta_language_mobile = language.mo_ta_mobile ?? "";

            }



            return Json(san_pham);


        }

        [HttpPost]
        public async Task<IActionResult> get_list_hinh_anh([FromBody] JObject json)
        {
            var id_san_pham = json.GetValue("id_san_pham").ToString();
            var list = repo._context.sys_san_pham_file_uploads.Where(d => d.id_san_pham == id_san_pham)
                .Select(d => new sys_san_pham_file_upload_model()
                {
                    db = d,


                }).ToList();

            return Json(list);

        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_san_pham_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.id = Guid.NewGuid().ToString();
            model.db.create_date = DateTime.Now;
            model.db.update_date = DateTime.Now;
            model.db.status_del = 1;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_san_pham_model>(json.GetValue("data").ToString());
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
        public async Task<IActionResult> getElementByIdNew([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();

            var model = await repo.getElementByIdNew(id);
            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> save_image([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_san_pham_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }


            var file = repo._context.sys_san_pham_file_uploads.Where(q => q.id_san_pham == model.db.id).ToList();
            if (file.Count > 0)
            {
                repo._context.RemoveRange(file);
                repo._context.SaveChanges();

            }


            for (int i = 0; i < model.list_file.Count; i++)
            {
                var file_path = "/FileManager/Download/?filename=" + HttpUtility.UrlEncode(model.list_file[i].db.file_path);
                model.list_file[i].db.id = DateTime.Now.Ticks.ToString();
                model.list_file[i].db.status_del = 1;
                model.list_file[i].db.stt = i + 1;
                model.list_file[i].db.id_san_pham = model.db.id;
                model.list_file[i].db.file_path = model.list_file[i].newfile != true ? model.list_file[i].db.file_path : file_path;
                repo._context.sys_san_pham_file_uploads.Add(model.list_file[i].db);
            }
            repo._context.SaveChanges();
            return Json(model);
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
            return Json("");
        }
        public async Task<IActionResult> revert([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.revert(id, getUserId());
            return Json("");
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

                var id_loai = dictionary["id_loai"];
                var search = dictionary["search"];
                var query = repo.FindAll()
                     //.Where(d => d.db.status_del == 1)
                     .Where(d => d.db.ten_san_pham.Contains(search))
                       .Where(d => d.db.id_loai.Equals(id_loai) || id_loai == "-1")
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderBy(d => d.db.id_loai + d.db.stt).Skip(param.Start).Take(param.Length)
          .ToList());
                DTResult<sys_san_pham_model> result = new DTResult<sys_san_pham_model>
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
