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
    public partial class sys_user_typenewsController : BaseAuthenticationController
    {
        private sys_user_typenews_repo repo;

        public sys_user_typenewsController(IUserService userService,worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_user_typenews_repo(context);
        }

        


        [HttpPost]
        public async Task<IActionResult> add_nguoi_dung([FromBody] JObject json)
        {

            var user_id = json.GetValue("user_id").ToString();
            var id_type_news = json.GetValue("id_type_news").ToString();
            var model = new sys_user_typenews_model();
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }


            model.db.id = DateTime.Now.Ticks + "";
            model.db.id_user = user_id;
            model.db.id_type_news = id_type_news;
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo._context.sys_user_typenews.AddAsync(model.db);
            repo._context.SaveChanges();

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_user_typenews_model>(json.GetValue("data").ToString());
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
            var model = JsonConvert.DeserializeObject<sys_user_typenews_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }
            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;
            await repo.update(model);
            model = await repo.getElementById(model.db.id);
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


                var query = Enumerable.Empty<sys_user_typenews_model>().AsQueryable();


                var id_type_news = dictionary["id_type_news"];
                query = repo.FindAll()   
                     .Where(d => d.full_name.Contains(search))
                     ;

                if (id_type_news =="-1")
                {
                    query = query.Where(d => id_type_news == "-1");
                }
                else 
                {
                    var lst_attr = id_type_news.Split(",").ToList();
                    lst_attr.ForEach(t =>
                    {
                        query = query.Where(d => d.db.id_type_news.Contains(t));
                    });
                  
                };
                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
       .ToList());

                dataList.ForEach(q =>
                {

                    q.types = (q.db.id_type_news ?? "").Split(",").ToList();
                    q.khoa = (q.db.id_khoa ?? "").Split(",").ToList();
                    if (!String.IsNullOrEmpty(q.db.id_type_news))
                     {
                        if(q.db.id_type_news == "-1")
                        {
                            q.type_news_name = "Tất cả";
                        }
                        else
                        {
                            var type_name = "";
                            var lst = q.db.id_type_news.Split(",").ToList();
                            for (int i = 0; i < lst.Count(); i++)
                            {
                                var id_type_news = lst[i];
                                var name = repo._context.sys_type_news.Where(t => t.id == id_type_news).Select(d => d.name).SingleOrDefault();
                                type_name += name + ";";
                            }
                                q.type_news_name = type_name;
                            q.types = (q.db.id_type_news ?? "").Split(",").ToList();

                        }

                      
                        
                    }
                
                        if (q.db.id_khoa == "-1")
                        {
                            q.khoa_name = "Tất cả";
                        }
                        else
                        {
                        if (!String.IsNullOrEmpty(q.db.id_khoa))
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
                           
                        }

                });
                DTResult<sys_user_typenews_model> result = new DTResult<sys_user_typenews_model>
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
