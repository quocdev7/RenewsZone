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
using worldsoft.common.Common;
using worldsoft.common.Models;

namespace worldsoft.system.web.Controller
{
    public partial class sys_approval_newsController : BaseAuthenticationController
    {
        public sys_approval_news_repo repo;
        private IMailService _mailService;
        public sys_approval_newsController(IUserService userService, worldsoftDefautContext context, IMailService mailService) : base(userService)
        {
            repo = new sys_approval_news_repo(context);
            _mailService = mailService;
        }
        //public IActionResult getCountNews([FromBody] JObject json)
        //{

        //    var user_id = getUserId();
        //    var user_name = repo._context.users.Where(d => d.Id == user_id).Select(d => d.Username).FirstOrDefault();
        //    var result = new List<sys_type_news_ref>();
        //    var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == user_id).Select(a => a.id_type_news).ToList();
        //    if (user_name == "adminstrator")
        //    {
        //        result = repo._context.sys_type_news
        //      .Where(d => d.status_del == 1 && d.id_group_news == id_group).
        //       Select(d => new sys_type_news_ref
        //       {
        //           id = d.id,
        //           name = d.name,
        //           type = repo._context.sys_group_news.Where(q => q.status_del == 1 && q.id == d.id_group_news).Select(q => q.code).FirstOrDefault()
        //       }).ToList();

        //    }
        //    else
        //    {
        //        var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == user_id).Select(a => a.id_type_news).ToList();
        //        result = repo._context.sys_type_news
        //        .Where(d => list_type_news.Contains(d.id))
        //        .Where(d => d.status_del == 1 && d.id_group_news == id_group).
        //         Select(d => new sys_type_news_ref
        //         {
        //             id = d.id,
        //             name = d.name,
        //             type = repo._context.sys_group_news.Where(q => q.status_del == 1 && q.id == d.id_group_news).Select(q => q.code).FirstOrDefault()
        //         }).ToList();
        //    }
        //    var countChuaDuyet = repo._context.sys_news

        //        .Select(q => q.status == 1).Count();

        //    var result = new
        //    {
        //        countDaDuyet = countDaDuyet,
        //        countChuaDuyet = countChuaDuyet
        //    };
        //    return Json(result);
        //}
        //public IActionResult getCountNews()
        //{
        //    var result = from p in repo._context.sys_news
        //                 select p.status 
        //                 group p by p.status;

        //    return Json(result);
        //}


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_approval_news_model>(json.GetValue("data").ToString());
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



        public async Task<IActionResult> getTypeNewsByGroupName([FromBody] JObject json)
        {
            var code = json.GetValue("code").ToString();
            var group_news = repo._context.sys_group_news.Where(q => q.code == code && q.status_del == 1).FirstOrDefault();
  
            var result = repo._context.sys_type_news
                .Where(q=>q.id_group_news == group_news.id && q.status_del ==1 ) 
                .Select(d => new
                {
                   id_type_news = d.id,
                   type_news_name = d.name,
                   lst_news =  repo._context.sys_news.Where(q=>q.id_type_news== d.id).OrderByDescending(q=>q.ngay_dang).Select(q=> new
                   {
                       id = q.id,
                       tieu_de = q.tieu_de,
                       noi_dung_trang_bia = q.noi_dung_trang_bia,
                       noi_dung = q.noi_dung,
                       hinh_anh = q.image,
                       create_by = repo._context.users.Where(s=>s.Id ==q.create_by).Select(q=>q.LastName).FirstOrDefault(),
                       chuc_danh = repo._context.sys_job_titles.Where(s=>s.id == repo._context.users.Where(s => s.Id == q.create_by)
                       .Select(q => q.id_job_title).FirstOrDefault()).Select(q=>q.name).FirstOrDefault(),
                       ngay_dang = q.ngay_dang
                   }).Skip(0).Take(4).ToList()

                }).ToList();


            var model = new
            {
                data = result,
                group_name = group_news.name
            };
            return Json(model);

          
        }
        public async Task<IActionResult> getNewsByTypeNews([FromBody] JObject json)
        {
            //var type_news = repo._context.sys_user_typenews.Where(a => a.id_user == getUserId()).Select(a => a.id_type_news).FirstOrDefault();
            var id_type_news = json.GetValue("id_type_news").ToString();
            var type_news_name = repo._context.sys_type_news.Where(q => q.id == id_type_news).Select(q => q.name).FirstOrDefault();
            var result =
                    repo._context.sys_news.Where(q => q.id_type_news == id_type_news).OrderByDescending(q => q.ngay_dang).Select(q => new
                    {
                        id = q.id,
                        tieu_de = q.tieu_de,
                        noi_dung_trang_bia = q.noi_dung_trang_bia,
                        noi_dung = q.noi_dung,
                        hinh_anh = q.image,
                        create_by = repo._context.users.Where(s => s.Id == q.create_by).Select(q => q.LastName).FirstOrDefault(),
                        chuc_danh = repo._context.sys_job_titles.Where(s => s.id == repo._context.users.Where(s => s.Id == q.create_by).Select(q => q.id_job_title).FirstOrDefault()).Select(q => q.name).FirstOrDefault(),
                        ngay_dang = q.ngay_dang
                    }).ToList();

            var model = new
            {
                data = result,
                type_news_name = type_news_name
            };
            return Json(model);


        }


        public IActionResult getListNews()
        {

            var lstGroup = repo._context.sys_news.Select(q => q.id_group_news).Distinct().ToList();


            return Json(0);
        }



        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_approval_news_model>(json.GetValue("data").ToString());
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


        public async Task<IActionResult> reject([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var reason = json.GetValue("reason").ToString();
            repo.reject(id, getUserId(), reason);

          
          
            return Json("");
        }
        public async Task<IActionResult> approval([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.approval(id, getUserId());

          
            var tin_tuc_duoc_phe_duyet = "20";
            await send_mail( tin_tuc_duoc_phe_duyet, id);
            return Json("");
        }
        public async Task<IActionResult> cancel([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.cancel(id, getUserId());
            return Json("");
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
                var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == getUserId()).Select(a => a.id_type_news).FirstOrDefault();
                var search = dictionary["search"];
                var id_group_news = dictionary["id_group_news"];
                var id_type_news = dictionary["id_type_news"];
                var status_del = int.Parse(dictionary["status_del"]);


                var user_id = getUserId();

                var id_phamvi = dictionary["id_phamvi"];

                var quyen_rieng_tu = dictionary["quyen_rieng_tu"];
                var is_hot = dictionary["is_hot"];

                var tuNgay = new DateTime(1900, 1, 1);
                var denNgay = new DateTime(1900, 1, 1);
                if (!string.IsNullOrEmpty(dictionary["tu_ngay"]))
                {
                    tuNgay = DateTime.Parse(dictionary["tu_ngay"]);
                }
                if (!string.IsNullOrEmpty(dictionary["tu_ngay"]))
                {
                    denNgay = DateTime.Parse(dictionary["den_ngay"]);
                }

                var query = Enumerable.Empty<sys_approval_news_model>().AsQueryable();



                //{
                //        id: "1",
                //    name: this._translocoService.translate('system.da_duyet')
                //},

                //{
                //        id: "3",
                //    name: this._translocoService.translate('system.cho_xet_duyet')
                //},
                //{
                //        id: "4",
                //    name: this._translocoService.translate('system.bi_tra_lai')
                //}

                query = repo.FindAll().Where(d => d.db.id_user_approval == user_id && (d.db.status_del == 1 || d.db.status_del ==4) || d.db.status_del==3);


                var listTypeAllow = repo._context.sys_user_typenews.Where(d => d.id_user == user_id).FirstOrDefault();

                query = query.Where(d =>(listTypeAllow.id_type_news =="-1"|| listTypeAllow.id_type_news.Contains(d.db.id_type_news)) && (listTypeAllow.id_khoa.Contains(d.db.id_khoa) ||listTypeAllow.id_khoa == "-1"  ));
                query = query.Where(d => d.db.status_del == status_del || status_del ==-1);


                //duyệt bài viet trong cau hinh


                query = query

                        .Where(d => d.db.id_group_news == id_group_news || id_group_news == "-1")
                .Where(d => d.db.id_type_news == id_type_news || id_type_news == "-1");
                //.Where(d => (d.db.is_hot ?? false) == Boolean.Parse(is_hot));

                if (tuNgay != new DateTime(1900, 1, 1))
                {
                    tuNgay = new DateTime(tuNgay.Year, tuNgay.Month, tuNgay.Day, 0, 0, 0);
                    denNgay = new DateTime(denNgay.Year, denNgay.Month, denNgay.Day, 23, 59, 59);

                    //ngày tạo
                    if (id_phamvi == "1")
                    {
                        query = query.Where(d => tuNgay <= d.db.ngay_dang && denNgay >= d.db.ngay_dang);
                    }
                    //ngày cập nhật
                    if (id_phamvi == "2")
                    {
                        query = query.Where(d => tuNgay <= d.db.update_date && denNgay >= d.db.update_date);
                    }
                    //ngày duyệt
                    if (id_phamvi == "3")
                    {
                        query = query.Where(d => tuNgay <= d.db.approval_date && denNgay >= d.db.approval_date);
                    }
                    //ngày ngừng đăng
                    if (id_phamvi == "4")
                    {
                        query = query.Where(d => tuNgay <= d.db.update_date && denNgay >= d.db.update_date);
                    }

                    query = query.Where(d => d.db.quyen_rieng_tu == int.Parse(quyen_rieng_tu) || quyen_rieng_tu == "-1");

                }


                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.db.update_date).Skip(param.Start).Take(param.Length)
      .ToList());
                DTResult<sys_approval_news_model> result = new DTResult<sys_approval_news_model>
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
