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
    public partial class sys_cau_hinh_duyet_su_kienController : BaseAuthenticationController
    {
        private sys_cau_hinh_duyet_su_kien_repo repo;

        public sys_cau_hinh_duyet_su_kienController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_cau_hinh_duyet_su_kien_repo(context);
        }




        [HttpPost]
        public async Task<IActionResult> add_nguoi_dung([FromBody] JObject json)
        {

            var user_id = json.GetValue("user_id").ToString();
            var id_khoa = json.GetValue("id_khoa").ToString();
            var model = new sys_cau_hinh_duyet_su_kien_model();
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }


            model.db.id = DateTime.Now.Ticks + "";
            model.db.user_id = user_id;
            model.db.id_khoa = id_khoa;
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo._context.sys_cau_hinh_duyet_su_kiens.AddAsync(model.db);
            repo._context.SaveChanges();

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_cau_hinh_duyet_su_kien_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }

            model.db.id = Guid.NewGuid().ToString();
            model.db.update_date = DateTime.Now;
            model.db.update_by = getUserId();
          
            await repo.insert(model);



            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_cau_hinh_duyet_su_kien_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);
            model = await repo.getElementById(model.db.id);


            if (model.db.id_khoa == "-1")
            {
                model.khoa_name = "Tất cả";
            }
            else
            {
                var khoa_name = "";
                var lst = model.db.id_khoa.Split(",").ToList();
                for (int i = 0; i < lst.Count(); i++)
                {

                    var name = repo._context.sys_khoas.Where(t => t.id == lst[i]).Select(d => d.name).SingleOrDefault();
                    khoa_name += name + ";";
                }
                model.khoa_name = khoa_name;
            }
            return Json(model);
        }

        public async Task<IActionResult> delete([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.delete(id, getUserId());
            return Json("");
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


                var query = Enumerable.Empty<sys_cau_hinh_duyet_su_kien_model>().AsQueryable();


                var id_khoa = dictionary["id_khoa"];
                query = repo.FindAll()
                     .Where(d => d.full_name.Contains(search))
                     ;

                if (id_khoa == "-1")
                {
                    query = query.Where(d => id_khoa == "-1");
                }
                else
                {
                    var lst_attr = id_khoa.Split(",").ToList();
                    lst_attr.ForEach(t =>
                    {
                        query = query.Where(d => d.db.id_khoa.Contains(t));
                    });

                };
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
       .ToList());

                dataList.ForEach(q =>
                {

                    
                    q.khoa = (q.db.id_khoa ?? "").Split(",").ToList();
                  

                    if (q.db.id_khoa == "-1")
                    {
                        q.khoa_name = "Tất cả";
                    }
                    else
                    {
                        var khoa_name = "";
                        var lst = q.db.id_khoa.Split(",").ToList();
                        for (int i = 0; i < lst.Count(); i++)
                        {

                            var name = repo._context.sys_khoas.Where(t => t.id == lst[i]).Select(d => d.name).SingleOrDefault();
                            khoa_name += name + ";";
                        }
                        q.khoa_name = khoa_name;
                    }

                });
                DTResult<sys_cau_hinh_duyet_su_kien_model> result = new DTResult<sys_cau_hinh_duyet_su_kien_model>
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
