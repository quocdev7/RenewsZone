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
    public partial class sys_cuu_sinh_vienController : BaseAuthenticationController
    {
        public sys_cuu_sinh_vien_repo repo;

        public sys_cuu_sinh_vienController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_cuu_sinh_vien_repo(context);
        }
        public IActionResult getListUse()
        {
            var result = repo._context.sys_cuu_sinh_viens
                .Where(d => d.status_del == 1 && (d.hien_thi_trang_chu ??false)== true)
                .OrderBy(d=>d.stt)
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     name_company = d.name_company,
                     chuc_danh_hoi_dong = d.chuc_danh_hoi_dong,
                     nien_khoa = d.nien_khoa,
                     image = d.image,

                 }).ToList();
            return Json(result);
        }
        public IActionResult getListGioiThieu([FromBody] JObject json)
        {
            var id_nhom_hoi_dong = json.GetValue("id_nhom_hoi_dong").ToString();
            var result = repo._context.sys_cuu_sinh_viens
                .Where(d=>d.id_nhom_hoi_dong == id_nhom_hoi_dong)
                .Where(d => d.status_del == 1)
                .OrderBy(d => d.stt)
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     name_company = d.name_company,
                     chuc_danh_hoi_dong = d.chuc_danh_hoi_dong,
                     nien_khoa = d.nien_khoa,
                     image = d.image,
                 }).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_cuu_sinh_vien_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();
            model.db.status_del = 1;
            model.db.id = Guid.NewGuid().ToString();
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;
            await repo.insert(model);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cuu_sinh_vien_model>(json.GetValue("data").ToString());
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
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status = int.Parse(dictionary["status"]);
                query = query.Where(d => d.db.status_del == status);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderBy(d => d.db.stt).Skip(param.Start).Take(param.Length)
        .ToList());
                DTResult<sys_cuu_sinh_vien_model> result = new DTResult<sys_cuu_sinh_vien_model>
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
