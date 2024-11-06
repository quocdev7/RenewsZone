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
    public partial class sys_type_newsController : BaseAuthenticationController
    {
        public sys_type_news_repo repo;

        public sys_type_newsController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_type_news_repo(context);
        }
        
        public IActionResult getListUseByGroupNew([FromBody] JObject json)
        {
            var id_group = json.GetValue("id").ToString();
            var user_id = getUserId();
            var user_name = repo._context.users.Where(d => d.Id == user_id).Select(d => d.Username).FirstOrDefault();
            var result = new List<sys_type_news_ref>();
            if (user_name == "administrator")
            {
                result = repo._context.sys_type_news
              .Where(d => d.status_del == 1 && d.id_group_news == id_group).
               Select(d => new sys_type_news_ref
               {
                   id = d.id,
                   name = d.name,
                   type = repo._context.sys_group_news.Where(q => q.status_del == 1 && q.id == d.id_group_news).Select(q => q.code).FirstOrDefault(),
                   count = repo._context.sys_news.Where(q => q.status_del == 1 && q.id_type_news == d.id).Count(),

               }).ToList();

            }
            else
            {
                var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == user_id).Select(a => a.id_type_news).ToList();
                result = repo._context.sys_type_news
                .Where(d => list_type_news.Contains(d.id))
                .Where(d => d.status_del == 1 && d.id_group_news == id_group).
                 Select(d => new sys_type_news_ref
                 {
                     id = d.id,
                     name = d.name,
                     type = repo._context.sys_group_news.Where(q => q.status_del == 1 && q.id == d.id_group_news).Select(q => q.code).FirstOrDefault(),
                     count = repo._context.sys_news.Where(q => q.status_del == 1 && q.id_type_news == d.id).Count(),
                     color = getColor(),
                 }).ToList();
            }
            return Json(result);

        }
        public IActionResult getListUseByGroup([FromBody] JObject json)
        {
            var id_group = json.GetValue("id").ToString();
           var result = repo._context.sys_type_news
            .Where(d => d.status_del == 1 && d.id_group_news == id_group).
               Select(d => new sys_type_news_ref
               {
                   id = d.id,
                   name = d.name,
               }).ToList();
            return Json(result);

        }
        public IActionResult getInfoByTypeNews([FromBody] JObject json)
        {
            var id_type_new = json.GetValue("id").ToString();
            var type_news = repo._context.sys_type_news.Where(d => d.id == id_type_new).SingleOrDefault();
            var group_news = repo._context.sys_group_news.Where(d => d.id == type_news.id_group_news).SingleOrDefault();

            var result = repo._context.sys_type_news
             .Where(d => d.status_del == 1 && d.id_group_news == type_news.id_group_news).
                Select(d => new sys_type_news_ref
                {
                    id = d.id,
                    name = d.name,
                    name_en = d.name_en,
                }).ToList();

            var model = new
            {
                lst_type = result,
                type_news = type_news,
                group_news= group_news


            };
            return Json(model);

        }
        




        public string getColor()
        {
            string[] strMang = new string[] { "#FF0000", "#00FF00", "#FFCC00", "#00CCCC"};
            string str = "";
            Random r = new Random();
            int i = r.Next(0, 3);
            str = strMang[i];
            return str; 
        }

        public IActionResult getListUse()
        {
            var result = repo._context.sys_type_news
                .Where(d => d.status_del == 1)
                  .Select(d => new
                  {
                      id = d.id,
                      name = d.name,
                      img = d.image,
                      stt = d.stt
                  }).OrderByDescending(d => d.stt).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_type_news_model>(json.GetValue("data").ToString());
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
            var model = JsonConvert.DeserializeObject<sys_type_news_model>(json.GetValue("data").ToString());
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
                var query = repo.FindAll()
                    //.Where(d=>d.db.status_del==1)
                     .Where(d => d.db.name.Contains(search))
                     ;
                var status_del = int.Parse(dictionary["status_del"]);
                query = query.Where(d => d.db.status_del == status_del);

                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.stt).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_type_news_model> result = new DTResult<sys_type_news_model>
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
