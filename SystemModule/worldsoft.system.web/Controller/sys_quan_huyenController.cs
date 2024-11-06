using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using worldsoft.common.BaseClass;
using worldsoft.common.common;
using worldsoft.common.Helpers;
using worldsoft.common.Services;
using worldsoft.DataBase.Provider;
using worldsoft.system.data.DataAccess;
using worldsoft.system.data.Models;

namespace worldsoft.system.web.Controller
{
    public partial class sys_quan_huyenController : BaseAuthenticationController
    {
        public sys_quan_huyen_repo repo;
        public AppSettings _appsetting;

        public sys_quan_huyenController(IUserService userService, worldsoftDefautContext context, IOptions<AppSettings> appsetting) : base(userService)
        {
            repo = new sys_quan_huyen_repo(context);
            _appsetting = appsetting.Value;
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_quan_huyens
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     id_tinh = d.id_tinh,
                     name = d.ten
                 }).ToList();
            return Json(result);
        }
        public IActionResult get_list_quan_huyen_multiple([FromBody] JObject json)
        {
            var id_tinh_thanh = json.GetValue("id_tinh_thanh").ToString();
            var list_id_tinh_thanh = id_tinh_thanh.Split(",").Where(t => !string.IsNullOrEmpty(t.Trim())).Select(q => long.Parse(q)).ToList();
            var list_tinh_thanh = repo._context.sys_quan_huyens
                       .Where(q => q.status_del == 1)
                       .Select(q => (long)q.id_tinh).ToList();
            if (list_id_tinh_thanh.Contains(-1))
                list_id_tinh_thanh = list_tinh_thanh;

            var result = repo._context.sys_quan_huyens
                .Where(q => list_id_tinh_thanh.Contains(q.id_tinh ?? 0))
                .Where(d => d.status_del == 1)
                .Select(d => new
                {
                    id = d.id,
                    name = d.ten,
                    ten_khong_dau = d.ten_khong_dau,
                }).ToList();
            return Json(result);
        }
        public IActionResult getListQuanHuyen([FromBody] JObject json)
        {
            var id_tinh_thanh = "";
            try
            {
                id_tinh_thanh = json.GetValue("id_tinh_thanh").ToString() ?? "";
            }
            catch (Exception)
            {

                return Json("");
            }

            var lst_tinh = id_tinh_thanh.Split(",").Select(q => long.Parse(q)).ToList();
            var result = repo._context.sys_quan_huyens
              .Where(d => lst_tinh.Contains(d.id_tinh ?? 0) || id_tinh_thanh == "-1")
              .Where(d => d.status_del == 1).
               Select(d => new
               {
                   id = d.id,
                   name = d.ten,
                   id_tinh = d.id_tinh,
               });

            return Json(result);

        }


        public IActionResult getListQuanHuyenNonLog([FromBody] JObject json)
        {
            var id_tinh_thanh = "";
            try
            {
                id_tinh_thanh = json.GetValue("id_tinh_thanh").ToString() ?? "";
            }
            catch (Exception)
            {

                return Json("");
            }

            var lst_tinh = id_tinh_thanh.Split(",").Select(q => long.Parse(q)).ToList();
            var result = repo._context.sys_quan_huyens
              .Where(d => lst_tinh.Contains(d.id_tinh ?? 0) || id_tinh_thanh == "-1")
              .Where(d => d.status_del == 1).
               Select(d => new
               {
                   id = d.id,
                   name = d.ten,
                   id_tinh = d.id_tinh,
               });

            return Json(result);

        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_quan_huyen_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_quan_huyen_model>(json.GetValue("data").ToString());
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
        public async Task<IActionResult> update_status_del([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var status_del = int.Parse(json.GetValue("status_del").ToString());
            repo.update_status_del(id, getUserId(), status_del);
            return Json("");
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(long.Parse(id), getUserId());
            return Json("");
        }
        //public async Task<FileStreamResult> exportExcel(string search, int status_del, string id_quoc_gia, string id_tinh)
        //{

        //    var excel = new ExcelHelper(_appsetting);

        //    search = search ?? "";

        //    var query = repo.FindAll()
        //             //.Where(d=>d.db.status_del==1)
        //             .Where(d => d.db.ten.Contains(search))
        //             ;
        //    query = query.Where(d => d.db.status_del == status_del);
        //    if (id_quoc_gia != "" && id_quoc_gia != null)
        //    {
        //        query = query.Where(d => d.db.id_quoc_gia == int.Parse(id_quoc_gia));
        //    }
        //    if (id_tinh != "" && id_quoc_gia != null)
        //    {
        //        query = query.Where(d => d.db.id_tinh == int.Parse(id_tinh));
        //    }
        //    var count = query.Count();
        //    var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).ToList());

        //    string[] header = new string[] {
        //        "STT (No.)","Tên","ID quận huyện","Tỉnh thành","ID tỉnh thành","Quốc gia","Ghi chú","Người cập nhật","Ngày cập nhật"
        //    };

        //    string[] listKey = new string[]
        //    {
        //       "db.ten","StrExcel_db.id","tinh","StrExcel_db.id_tinh","quoc_gia","db.note","updateby_name","db.update_date"
        //    };

        //    return await exportFileExcel(_appsetting, header, listKey, dataList, "sys_quan_huyen");
        //}

        public async Task<IActionResult> getElementById(string id)
        {
            var model = await repo.getElementById(long.Parse(id));
            return Json(model);
        }
        public IActionResult getListNghiQuyetInfo()
        {
            var result = repo.FindAll().Where(q => q.db.status_del == 1).ToList();
            return Json(result);
        }



        public async Task<IActionResult> getNghiQuyetInfo([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var model = await repo.getElementById(long.Parse(id));
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
                var query = repo.FindAll()
                     //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.ten.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);
                var id_quoc_gia = dictionary["id_quoc_gia"];
                if (id_quoc_gia != "")
                {
                    query = query.Where(d => d.db.id_quoc_gia == int.Parse(id_quoc_gia));
                }
                var id_tinh = dictionary["id_tinh"];
                if (id_tinh != "")
                {
                    query = query.Where(d => d.db.id_tinh == int.Parse(id_tinh));
                }
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
                   .ToList());
                DTResult<sys_quan_huyen_model> result = new DTResult<sys_quan_huyen_model>
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
