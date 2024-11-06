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

namespace worldsoft.system.web.Controller
{
    public partial class sys_cau_hinh_thong_tinController : BaseAuthenticationController
    {
        public sys_cau_hinh_thong_tin_repo repo;

        public sys_cau_hinh_thong_tinController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_cau_hinh_thong_tin_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_cau_hinh_thong_tins
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.tieu_de,
                     name_en=d.tieu_de_en,
                     noi_dung = d.noi_dung,
                     noi_dung_en=d.noi_dung_en,
                     noi_dung_mobile = d.noi_dung_mobile,
                     noi_dung_mobile_en = d.noi_dung_mobile_en,
                 }).ToList();
            return Json(result);
        }

        public IActionResult getListUseNew()
        {
            var id_loai = repo._context.sys_loai_cau_hinh_dbs.Where(q => q.code == "30").Select(q => q.id).FirstOrDefault();
            var result = repo._context.sys_cau_hinh_thong_tins
                .Where(d => d.status_del == 1 && d.id_loai == id_loai)
                 .OrderBy(d => d.stt).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.tieu_de,
                     name_en = d.tieu_de_en,
                     noi_dung = d.noi_dung,
                     noi_dung_en = d.noi_dung_en,
                     noi_dung_mobile = d.noi_dung_mobile,
                     noi_dung_mobile_en = d.noi_dung_mobile_en,
                 }).ToList();
            return Json(result);
        }


        public IActionResult getInfo()
        {
            var result = repo.FindAll().Where(q => q.db.status_del == 1 && 
                    (q.db.id_loai == "1"
                    || q.db.id_loai == "2"
                    || q.db.id_loai == "3"
                    || q.db.id_loai == "7"
                    || q.db.id_loai == "8"
                    )
            ).OrderBy(d => d.db.id_loai).ToList();
            return Json(result);
        }
        public IActionResult getLoaiThongTin()
        {
            var result = repo._context.sys_loai_cau_hinh_dbs
             
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     //tieu_de_en = repo._context.sys_cau_hinh_thong_tin_languages.Where(t => t.id_cau_hinh_thong_tin == d.id).Select(d => d.tieu_de).SingleOrDefault(),
                     //noi_dung_en = repo._context.sys_cau_hinh_thong_tin_languages.Where(t => t.id_cau_hinh_thong_tin == d.id).Select(d => d.noi_dung).SingleOrDefault(),
                     //noi_dung_mobile_en = repo._context.sys_cau_hinh_thong_tin_languages.Where(t => t.id_cau_hinh_thong_tin == d.id).Select(d => d.noi_dung_mobile).SingleOrDefault(),
                 }).ToList();
            return Json(result);
        }
        
        public IActionResult getListUseDetail()
        {
            var result = repo._context.sys_cau_hinh_thong_tins
                .Where(d => d.status_del == 1).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.tieu_de,
                     list_types= repo._context.sys_type_news
                     .Where(td=>td.id_group_news==d.id)
                    .Where(td => td.status_del == 1).Select(td => new
                    {
                        id = td.id,
                        name = td.name,
                    }).ToList(),
                }).ToList();
            return Json(result);
        }
        

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_cau_hinh_thong_tin_model>(json.GetValue("data").ToString());
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


            model.loai_cau_hinh = repo._context.sys_loai_cau_hinh_dbs.Where(t => t.id == model.db.id_loai).Select(d => d.name).SingleOrDefault();
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cau_hinh_thong_tin_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);

            model.loai_cau_hinh = repo._context.sys_loai_cau_hinh_dbs.Where(t => t.id == model.db.id_loai).Select(d => d.name).SingleOrDefault();
            return Json(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> edit_language([FromBody] JObject json)
        //{
        //    var model = JsonConvert.DeserializeObject<sys_cau_hinh_thong_tin_language_model>(json.GetValue("data").ToString());
        //    var check = checkModelStateEdit(model);
        //    if (!check)
        //    {
        //        return generateError();
        //    }
         //   await repo.insert_language(model);
        //    return Json(model);
        //}


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

        public async Task<IActionResult> getCauHinhThongTin([FromBody] JObject json)

        {
            var loai = json.GetValue("loai").ToString();
            //var result = repo.FindAll().Where(q => q.db.status_del == 1).OrderByDescending(d => d.db.update_date).FirstOrDefault();
            var result = repo.FindAll().Where(q => q.db.status_del == 1 && q.db.id_loai== loai).OrderByDescending(d => d.db.update_date).FirstOrDefault();
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
                     .Where(d => d.db.tieu_de.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_cau_hinh_thong_tin_model> result = new DTResult<sys_cau_hinh_thong_tin_model>
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
