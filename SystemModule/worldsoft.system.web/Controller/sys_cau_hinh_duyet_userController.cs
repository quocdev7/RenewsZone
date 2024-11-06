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
    public partial class sys_cau_hinh_duyet_userController : BaseAuthenticationController
    {
        private sys_cau_hinh_duyet_user_repo repo;

        public sys_cau_hinh_duyet_userController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_cau_hinh_duyet_user_repo(context);
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_cau_hinh_duyet_users
                .Where(d => d.status_del == 1)
                 .Select(d => new
                 {
                     id = d.id,
                     name = d.user_id
                 }).ToList();
            return Json(result);
        }

        public string get_hinh_thuc(string hinh_thuc)
        {
            var ten_hinh_thuc = "";
            if (hinh_thuc == "1")
            {
                ten_hinh_thuc = "Sinh Viên";
            }
            else if (hinh_thuc == "2")
            {
                ten_hinh_thuc = "Cựu Sinh Viên";
            }
            else if (hinh_thuc == "3")
            {
                ten_hinh_thuc = "Giảng Viên";
            }
            else if (hinh_thuc == "4")
            {
                ten_hinh_thuc = "Cán bộ công nhân viên";
            }
            else
            {
                ten_hinh_thuc = "Cựu Giáo Chức";
            };

            return ten_hinh_thuc;
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_cau_hinh_duyet_user_model>(json.GetValue("data").ToString());
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
            var model = JsonConvert.DeserializeObject<sys_cau_hinh_duyet_user_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);
            if (model.db.id_khoa == "-1")
            {
                model.ten_khoa = "Tất cả";
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
                model.ten_khoa = khoa_name;
            }


            if (model.db.hinh_thuc == "-1")
            {
                model.ten_hinh_thuc = "Tất cả";
            }
            else
            {
                var ten_hinh_thuc = "";
                var lst = model.db.hinh_thuc.Split(",").ToList();
                for (int i = 0; i < lst.Count(); i++)
                {
                    var hinh_thuc = lst[i];
                    var name = get_hinh_thuc(hinh_thuc);
                    ten_hinh_thuc += name + ";";
                }
                model.ten_hinh_thuc = ten_hinh_thuc;
                model.hinh_thuc = (model.db.hinh_thuc ?? "").Split(",").ToList();

            }

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

                var search = dictionary["search"];
                var id_khoa = dictionary["id_khoa"];
                var id_hinh_thuc = dictionary["id_hinh_thuc"];


                var query = Enumerable.Empty<sys_cau_hinh_duyet_user_model>().AsQueryable();

                query = repo.FindAll()
                    //.Where(d => d.db.status_del == 1)

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

                if (id_hinh_thuc == "-1")
                {
                    query = query.Where(d => id_hinh_thuc == "-1");
                }
                else
                {
                    var lst_attr = id_hinh_thuc.Split(",").ToList();
                    lst_attr.ForEach(t =>
                    {
                        query = query.Where(d => d.db.hinh_thuc.Contains(t));
                    });

                };


                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
          .ToList());

                dataList.ForEach(q =>
                {

                    q.hinh_thuc = (q.db.hinh_thuc ?? "").Split(",").ToList();
                    if (!String.IsNullOrEmpty(q.db.hinh_thuc))
                    {
                        if (q.db.hinh_thuc == "-1")
                        {
                            q.ten_hinh_thuc = "Tất cả";
                        }
                        else
                        {
                            var ten_hinh_thuc = "";
                            var lst = q.db.hinh_thuc.Split(",").ToList();
                            for (int i = 0; i < lst.Count(); i++)
                            {
                                var hinh_thuc = lst[i];
                                var name = get_hinh_thuc(hinh_thuc);
                                ten_hinh_thuc += name + ";";
                            }
                            q.ten_hinh_thuc = ten_hinh_thuc;
                            q.hinh_thuc = (q.db.hinh_thuc ?? "").Split(",").ToList();

                        }
                    }


                    q.khoa = (q.db.id_khoa ?? "").Split(",").ToList();
                    if (q.db.id_khoa == "-1")
                    {
                        q.ten_khoa = "Tất cả";
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
                        q.ten_khoa = khoa_name;
                    }

                });
                DTResult<sys_cau_hinh_duyet_user_model> result = new DTResult<sys_cau_hinh_duyet_user_model>
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
