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
using Microsoft.AspNetCore.Http;
using System.Data.Entity;
using System.Text.RegularExpressions;
using worldsoft.DataBase.Helper;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace worldsoft.system.web.Controller
{

    public partial class sys_newsController : BaseAuthenticationController
    {
        public sys_news_repo repo;

        public sys_newsController(IUserService userService, worldsoftDefautContext context) : base(userService)
        {
            repo = new sys_news_repo(context);
        }

        [AllowAnonymous]
        public async Task<IActionResult> sync_search_all()
        {

            //1 tin tức  2 sự kiện 3 thành viên 4 tuyển dụng 5 sản phẩm
            insert_search_news();
            insert_search_event();
            insert_search_user();
            //insert_search_tuyen_dung();
            insert_search_san_pham();

            return Json("");
        }
        public IActionResult get_title_news([FromBody] JObject json)
        {
            var id_news = json.GetValue("id_news").ToString();
            var title_event = repo._context.sys_news.Where(d => d.id == id_news).Select(d => d.tieu_de).SingleOrDefault()
                .Replace("<", "").Replace(">", "").Replace("&", "").Replace("\'", "").Replace("\"", "")
                .Replace("- ".ToString(), string.Empty).Replace("-".ToString(), " ");
            var tieu_de = title_event.Trim();
            tieu_de = StringFunctions.NonUnicode(tieu_de).Replace(' ', '-');
            if (tieu_de.Length > 150)
                tieu_de = tieu_de.Substring(0, 150);
            return Json(tieu_de);
        }
        public async Task<IActionResult> load_icon([FromBody] JObject json)
        {
            var icon_facebook = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(d => d.type == 5).Select(q => q.image).FirstOrDefault() ?? "";
            var icon_linkedin = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(d => d.type == 7).Select(q => q.image).FirstOrDefault() ?? "";
            var icon_twitter = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(d => d.type == 8).Select(q => q.image).FirstOrDefault() ?? "";
            var icon_zalo = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(d => d.type == 6).Select(q => q.image).FirstOrDefault() ?? "";
            var icon_binh_luan = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(d => d.type == 9).Select(q => q.image).FirstOrDefault() ?? "";
            var model = new
            {
                icon_facebook = icon_facebook,
                icon_linkedin = icon_linkedin,
                icon_twitter = icon_twitter,
                icon_zalo = icon_zalo,
                icon_binh_luan = icon_binh_luan,
            };
            return Json(model);


        }
        public void insert_search_news()
        {


            var lst = repo._context.sys_news.Where(q => q.status_del == 1).ToList();

            for (int i = 0; i < lst.Count; i++)
            {
                var data = lst[i];

                string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.tieu_de ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
                string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.noi_dung ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

                var db = repo._context.sys_searchs
                    .Where(d => d.id_ref == data.id && d.type == 1).FirstOrDefault();
                if (db != null)
                {
                    db.create_date = DateTime.Now;
                    db.order_date = data.ngay_dang;
                    db.search_text = t + " " + c;
                    repo._context.SaveChanges();
                }
                else
                {
                    var db1 = new sys_search_db()
                    {
                        create_date = DateTime.Now,
                        order_date = data.ngay_dang,
                        id = 0,
                        id_ref = data.id,
                        search_text = t + " " + c,
                        type = 1,
                    };

                    repo._context.Add(db1);
                    repo._context.SaveChanges();

                }

            }


        }
        public void insert_search_event()
        {


            var lst = repo._context.sys_events.Where(q => q.status_del == 1).ToList();

            for (int i = 0; i < lst.Count; i++)
            {
                var data = lst[i];

                string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.title ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
                string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.mo_ta ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

                var db = repo._context.sys_searchs
                    .Where(d => d.id_ref == data.id && d.type == 2).FirstOrDefault();
                if (db != null)
                {
                    db.create_date = DateTime.Now;
                    db.order_date = data.time_start;
                    db.search_text = t + " " + c;
                    repo._context.SaveChanges();
                }
                else
                {
                    var db1 = new sys_search_db()
                    {
                        create_date = DateTime.Now,
                        order_date = data.time_start,
                        id = 0,
                        id_ref = data.id,
                        search_text = t + " " + c,
                        type = 2,
                    };

                    repo._context.Add(db1);
                    repo._context.SaveChanges();

                }

            }


        }
        public void insert_search_user()
        {


            var lst = repo._context.users.Where(q => q.status_del == 1).ToList();

            for (int i = 0; i < lst.Count; i++)
            {
                var data = lst[i];

                string full_name = (data.full_name ?? "").ToLower().Normalize();
                string email = (data.email ?? "").ToLower().Normalize();
                string phone = (data.phone ?? "").ToLower().Normalize();

                var db = repo._context.sys_searchs
                    .Where(d => d.id_ref == data.Id && d.type == 3).FirstOrDefault();
                if (db != null)
                {
                    db.create_date = DateTime.Now;
                    db.order_date = data.time_input;
                    db.search_text = full_name + " " + email + " " + phone;
                    repo._context.SaveChanges();
                }
                else
                {
                    var db1 = new sys_search_db()
                    {
                        create_date = DateTime.Now,
                        order_date = data.time_input,
                        id = 0,
                        id_ref = data.Id,
                        search_text = full_name + " " + email + " " + phone,
                        type = 3,
                    };

                    repo._context.Add(db1);
                    repo._context.SaveChanges();

                }

            }
        }

        public void insert_search_tuyen_dung()
        {


            var lst = repo._context.sys_thong_tin_tuyen_dungs.Where(q => q.status_del == 1).ToList();

            for (int i = 0; i < lst.Count; i++)
            {
                var data = lst[i];

                string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.name ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
                string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.mo_ta_cong_viec ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
                string e = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.yeu_cau_cong_viec ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
                var db = repo._context.sys_searchs.Where(d => d.id_ref == data.id && d.type == 4).FirstOrDefault();
                if (db != null)
                {
                    db.create_date = DateTime.Now;
                    db.order_date = data.ngay_dang_tuyen;
                    db.search_text = t + " " + c + " " + e;
                    repo._context.SaveChanges();
                }
                else
                {
                    var db1 = new sys_search_db()
                    {
                        create_date = DateTime.Now,
                        order_date = data.ngay_dang_tuyen,
                        id = 0,
                        id_ref = data.id,
                        search_text = t + " " + c + " " + e,
                        type = 4,
                    };

                    repo._context.Add(db1);
                    repo._context.SaveChanges();

                }

            }

        }

        public void insert_search_san_pham()
        {


            var lst = repo._context.sys_san_phams.Where(q => q.status_del == 1).ToList();

            for (int i = 0; i < lst.Count; i++)
            {
                var data = lst[i];

                string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.ten_san_pham ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
                string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(data.mo_ta ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

                var db = repo._context.sys_searchs
                    .Where(d => d.id_ref == data.id && d.type == 4).FirstOrDefault();
                if (db != null)
                {
                    db.create_date = DateTime.Now;
                    db.order_date = data.create_date;
                    db.search_text = t + " " + c;
                    repo._context.SaveChanges();
                }
                else
                {
                    var db1 = new sys_search_db()
                    {
                        create_date = DateTime.Now,
                        order_date = data.create_date,
                        id = 0,
                        id_ref = data.id,
                        search_text = t + " " + c,
                        type = 4,
                    };

                    repo._context.Add(db1);
                    repo._context.SaveChanges();

                }

            }


        }


        [HttpPost]
        public async Task<IActionResult> create([FromBody] JObject json)
        {

            var model = JsonConvert.DeserializeObject<sys_news_model>(json.GetValue("data").ToString());
            var check = checkModelStateCreate(model);
            if (!check)
            {
                return generateError();
            }
            model.db.create_by = getUserId();




            model.db.id = Guid.NewGuid().ToString();
            model.db.update_date = DateTime.Now;
            model.db.create_date = DateTime.Now;

            var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == getUserId()).Select(a => a.id_type_news).FirstOrDefault();
            var quantri = "eacf8b68-47bf-4f71-a987-1befb4dad9df";

            var admin = repo._context.sys_group_user_details.Where(q => q.user_id == getUserId() && q.id_group_user == quantri).Count() > 0;

            if (admin)
            {
                model.db.status_del = 1;
                model.db.id_user_approval = getUserId();
                model.db.approval_date = DateTime.Now;


            }
            else
            {
                if (list_type_news == null)
                {
                    model.db.status_del = 3;

                }
                else
                {
                    var type_news = list_type_news.Split(",").ToList();

                    if (type_news.Contains(model.db.id_type_news))
                    {
                        model.db.status_del = 1;
                        model.db.id_user_approval = getUserId();
                        model.db.approval_date = DateTime.Now;

                    }
                    else
                    {
                        model.db.status_del = 3;
                    }
                }



            }


            if (model.db.image == null)
            {
                model.db.image = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 2).Select(q => q.image).FirstOrDefault() ?? null; //_appsetting.avatar;
            }


            await repo.insert(model);

            model = await repo.getElementById(model.db.id);
            return Json(model);
        }


        [HttpPost]
        public IActionResult update_vi_tri_tin_noi_bat([FromBody] JObject json)
        {

            var lst = JsonConvert.DeserializeObject<List<sys_news_model>>(json.GetValue("data").ToString());

            for (int i = 0; i < lst.Count(); i++)
            {
                var model = lst[i];
                var db = repo._context.sys_news.Where(q => q.id == model.db.id).FirstOrDefault();
                db.stt = model.db.stt;
                repo._context.SaveChanges();
            }

            return Json("");
        }
        [HttpPost]
        public IActionResult save_tin_tuc_noi_bat([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var type = int.Parse(json.GetValue("type").ToString());
            var id_group_news = json.GetValue("id_group_news").ToString();




            var count_tin_noi_bat = repo._context.sys_news.Where(q => q.status_del == 1 && q.id_group_news == id_group_news && q.is_hot == true).Count();

            var model = repo._context.sys_news.Where(x => x.id == id).FirstOrDefault();
            if (type == 1)
            {
                if (count_tin_noi_bat > 4)
                {
                    ModelState.AddModelError("tin_noi_bat", "msg_tin_noi_bat");
                }
                if (!ModelState.IsValid)
                {
                    return generateError();
                }
                model.is_hot = true;
                model.stt = count_tin_noi_bat + 1;
                repo._context.SaveChanges();
            }
            else
            {
                model.is_hot = false;
                model.stt = null;
                repo._context.SaveChanges();
                var lst_tin_noi_bat = repo._context.sys_news.Where(q => q.status_del == 1 && q.id_group_news == id_group_news && q.is_hot == true).OrderBy(q => q.stt).ToList();
                for (int i = 0; i < lst_tin_noi_bat.Count(); i++)
                {
                    var item = lst_tin_noi_bat[i];
                    var db = repo._context.sys_news.Where(x => x.id == item.id).FirstOrDefault();
                    db.stt = i + 1;
                    repo._context.SaveChanges();
                }
            }



            return Json("");
        }
        public IActionResult getListTinNoiBat([FromBody] JObject json)
        {
            var id = json.GetValue("id_nhom").ToString();
            var lst_hot_news = new List<sys_news_model>();
            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var query = repo.FindAll().Where(d => d.db.id_group_news == id);
            lst_hot_news = query
                .Where(q => (q.db.is_hot ?? false) == true && q.db.status_del == 1 && q.db.quyen_rieng_tu == 1)
               .OrderBy(q => q.db.stt)
               .Skip(0).Take(4).ToList();


            return Json(lst_hot_news);
        }
        public IActionResult load_ngon_ngu([FromBody] JObject json)
        {
            var id_news = json.GetValue("id_news").ToString();
            var id_user = getUserId();
            //var query = repo.FindAllNewPortal(id_user).Where(d => d.db.id_group_news == id);
            var model = new sys_news_language_model();

            model = repo._context.sys_news_languages.Where(d => d.id_news == id_news).Select(d => new sys_news_language_model()
            {
                db = d
            }).FirstOrDefault();

            if (model == null)
            {
                model = new sys_news_language_model();
                model.db.id_news = id_news;
            }
            return Json(model);
        }


        public IActionResult getHomePageHotNews([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var lst_hot_news = new List<sys_news_model>();
            var id_user = getUserId();
            var query = repo.FindAllNewPortal(id_user)
                .Where(d => d.db.id_group_news == id)
                .Where(d => d.db.ngay_dang <= DateTime.Now.Date)
                ;


            lst_hot_news = query
               .Where(q => (q.db.is_hot ?? false) == true)
               .OrderBy(q => q.db.stt)
               .Skip(0).Take(4).ToList();
            var length_hot_news = lst_hot_news.Count();
            if (length_hot_news == 0 || length_hot_news < 4)
            {

                lst_hot_news.AddRange(
                    query.Where(q => (q.db.is_hot ?? false) == false)
                .OrderByDescending(q => q.db.ngay_dang)
                .Skip(0).Take(4 - length_hot_news).ToList());
            }

            lst_hot_news.ForEach(q =>
            {
                var language = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).FirstOrDefault();

                if (language != null)
                {
                    q.tieu_de_language = language.tieu_de ?? "";
                    q.noi_dung_trang_bia_language = language.noi_dung_trang_bia ?? "";
                    q.nguon_tin_tuc_language = language.nguon_tin_tuc ?? "";
                    q.noi_dung_language = language.noi_dung ?? "";
                    q.noi_dung_language_mobile = language.noi_dung_mobile;
                }


            });
            return Json(lst_hot_news);
        }
        public IActionResult getNewsTuyenDung([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var lst_hot_news = new List<sys_news_model>();
            var id_user = getUserId();
            var query = repo.FindAllNewPortal(id_user)
                  .Where(d => d.db.id_group_news == id)
                  .Where(d => d.db.ngay_dang <= DateTime.Now.Date)
                  ;

            lst_hot_news = query
                .Where(q => (q.db.is_hot ?? false) == true)
               .OrderBy(q => q.db.stt)
               .Skip(0).Take(4).ToList();
            var length_hot_news = lst_hot_news.Count();
            if (length_hot_news == 0 || length_hot_news < 4)
            {

                lst_hot_news.AddRange(
                    query.Where(q => (q.db.is_hot ?? false) == false)
                .OrderByDescending(q => q.db.ngay_dang)
                .Skip(0).Take(4 - length_hot_news).ToList());
            }
            lst_hot_news.ForEach(q =>
            {
                var language = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).FirstOrDefault();

                if (language != null)
                {
                    q.tieu_de_language = language.tieu_de ?? "";
                    //q.noi_dung_trang_bia_language = language.noi_dung_trang_bia ?? "";
                    //q.nguon_tin_tuc_language = language.nguon_tin_tuc ?? "";
                    //q.noi_dung_language = language.noi_dung ?? "";

                }


            });
            return Json(lst_hot_news);
        }
        public async Task<IActionResult> approval([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            repo.approval(id, getUserId());

            return Json("");
        }
        public async Task<IActionResult> cancel([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();

            var reason = json.GetValue("reason").ToString();
            repo.cancel(id, getUserId(), reason);
            return Json("");
        }
        public IActionResult search_news_common([FromBody] JObject json)
        {



            var filter = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("filter").ToString());
            var search_key = (filter["search_key"] ?? "").Trim().ToLower();
            var type_info = (filter["type_info"] ?? "").Trim().ToLower();
            var id_group_news = (filter["id_group_news"] ?? "").Trim().ToLower();
            var id_type_news = (filter["id_type_news"] ?? "").Trim().ToLower();
            var page = int.Parse(json.GetValue("page").ToString());
            var id_user = getUserId();
            var lst_hot_news = repo.FindAllNewPortal(id_user)
                .Where(q => q.db.tieu_de.ToLower().Contains(search_key) || q.db.noi_dung.ToLower().Contains(search_key))
                 .Where(q => id_type_news == "-1" || q.db.id_type_news == id_type_news)
                   .Where(q => id_group_news == "-1" || q.db.id_group_news == id_group_news)
                .Where(q => q.db.status_del == 1)
                .Select(d => new sys_search_info_model()
                {
                    id = d.db.id,
                    avatar = d.avatar,
                    code = d.code,
                    content = d.db.noi_dung,
                    content_brief = d.db.noi_dung_trang_bia ?? "",
                    title = d.db.tieu_de,
                    image = d.db.image,
                    create_by_name = d.create_by_name,
                    group_news_name = d.group_news_name,
                    post_date = d.db.ngay_dang,
                    type_info = 1,
                    view_count = d.db.view_count,
                    comment_count = d.db.comment_count,
                    type_news_name = d.type_news_name,
                }).AsNoTracking();


            var lst = lst_hot_news.OrderByDescending(d => d.post_date).Skip(page * 5).Take(5).ToList();
            lst.ForEach(q =>
            {
                var language = repo._context.sys_news_languages.Where(d => d.id_news == q.id).FirstOrDefault();

                if (language != null)
                {
                    q.tieu_de_language = language.tieu_de ?? "";
                    q.noi_dung_trang_bia_language = language.noi_dung_trang_bia ?? "";
                    q.nguon_tin_tuc_language = language.nguon_tin_tuc ?? "";
                    q.noi_dung_language = language.noi_dung ?? "";
                }


            });
            var total = lst_hot_news.Count();
            var result = new
            {
                lst_news = lst,
                total_item = total,
                total_page = Math.Round((decimal)(total / 5)) + (total % 5 == 0 ? 0 : 1),
                page = page,
            };
            return Json(result);
        }


        public IActionResult search_common([FromBody] JObject json)
        {
            var filter = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("filter").ToString());
            var search_key_nonuni = StringFunctions.NonUnicode((filter["search_key"] ?? "").Trim().ToLower().Normalize());
            var search_key = (filter["search_key"] ?? "").Trim().ToLower().Normalize();
            var type_info = int.Parse((filter["type_info"] ?? "").Trim().ToLower());
            var id_group_news = (filter["id_group_news"] ?? "").Trim().ToLower();
            var id_type_news = (filter["id_type_news"] ?? "").Trim().ToLower();
            var page = int.Parse(json.GetValue("page").ToString());
            var id_user = getUserId();
            var search_query = repo._context.sys_searchs.Where(q => q.search_text.Contains(search_key_nonuni) || q.search_text.Contains(search_key))
                 .Where(q => type_info == -1 || q.type == type_info).Select(d => new sys_search_info_model()
                 {
                     id = d.id_ref,
                     type_info = d.type,
                     post_date = d.order_date,
                 });
            search_query = search_query.OrderByDescending(d => d.post_date).OrderByDescending(d => d.type_info);
            var total = search_query.Count();
            var lst = search_query.Skip(page * 5).Take(5).ToList();
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].type_info == 1)
                {
                    var news = repo.FindAll().Where(d => d.db.id == lst[i].id).FirstOrDefault();
                    if (news != null)
                    {
                        lst[i].avatar = news.avatar;
                        lst[i].code = news.code;
                        lst[i].content_brief = news.db.noi_dung_trang_bia ?? "";
                        lst[i].title = news.db.tieu_de ?? "";
                        lst[i].image = news.db.image;
                        lst[i].create_by_name = news.create_by_name;
                        lst[i].group_news_name = news.group_news_name;
                        lst[i].view_count = news.db.view_count;
                        lst[i].comment_count = news.db.comment_count;
                        lst[i].type_news_name = news.type_news_name;
                    }
                       
                }
                else if (lst[i].type_info == 2)
                {
                    var data = repo._context.sys_events.Where(d => d.id == lst[i].id).FirstOrDefault();
                    if(data != null)
                    {
                        lst[i].avatar = "";
                        lst[i].code = "";
                        lst[i].content_brief = "";
                        lst[i].title = data.title ?? "";
                        lst[i].image = data.logo ?? "";
                        lst[i].create_by_name = "";
                        lst[i].group_news_name = "";

                    }
               
                }
                else if (lst[i].type_info == 3)
                {
                    var news = repo._context.users.Where(d => d.Id == lst[i].id).FirstOrDefault();
                    lst[i].avatar = news.avatar_path;
                    lst[i].content_brief = news.school_year + "";
                    lst[i].content = repo._context.sys_khoas.Where(d => d.id == news.id_khoa).Select(d => d.name).SingleOrDefault();
                    if (string.IsNullOrEmpty(id_user))
                    {
                        lst[i].status_del = -1;
                    }
                    else
                    {
                        lst[i].status_del = repo._context.sys_user_ban_bes.Where(d => d.user_id == id_user && d.user_id_ban_be == lst[i].id).Select(d => d.status_del).FirstOrDefault() ?? 0;
                        lst[i].id_invite = repo._context.sys_user_ban_bes.Where(d => d.user_id == id_user && d.user_id_ban_be == lst[i].id).Select(d => d.id).FirstOrDefault();
                    }

                    lst[i].title = news.full_name;
                }
                else if (lst[i].type_info == 4)
                {
                    var sp = repo._context.sys_san_phams.Where(d => d.id == lst[i].id).FirstOrDefault();
                    if (sp != null)
                    {
                        lst[i].avatar = "";
                        lst[i].code = "";
                        lst[i].content_brief = sp.mo_ta ?? "";
                        lst[i].title = sp.ten_san_pham ?? "";
                        lst[i].image = sp.hinh_anh ?? "";
                        lst[i].create_by_name = "";
                        lst[i].group_news_name = "";

                    }

                }
            }
            var result = new
            {
                lst_news = lst,
                total_item = total,
                total_page = Math.Round((decimal)(total / 5)) + (total % 5 == 0 ? 0 : 1),
                page = page,
            };
            return Json(result);
        }






        public async Task<ActionResult> getStats()
        {
            var result = new
            {
                count_post = repo._context.sys_news.Where(d => d.status_del == 1).Count(),
                count_event = repo._context.sys_events.Where(d => d.status_del == 1).Count(),
                count_people = repo._context.users.Where(d => d.status_del == 1).Count(),
            };
            return Json(result);

        }

        public async Task<IActionResult> getAllTypeNewsDetail([FromBody] JObject json)
        {

            var id_user = getUserId();
            var news_query = repo.FindAllNewPortal(id_user);

            var result = repo._context.sys_group_news
                .Where(d => d.status_del == 1).
                OrderBy(d => d.stt).
                 Select(d => new
                 {
                     id = d.id,
                     name = d.name,
                     image = d.image,
                     list_types = repo._context.sys_type_news
                         .OrderBy(d => d.stt)
                     .Where(td => td.id_group_news == d.id)

                    .Where(td => td.status_del == 1).Select(td => new
                    {
                        id = td.id,
                        name = td.name,
                        image = d.image,
                        lst_news = news_query
                          .Where(q => q.db.status_del == 1)
                        .Where(q => q.db.id_type_news == td.id).OrderByDescending(q => q.db.ngay_dang).Select(q => new
                        {
                            id = q.db.id,
                            tieu_de = q.db.tieu_de,
                            noi_dung_trang_bia = q.db.noi_dung_trang_bia,
                            noi_dung = q.db.noi_dung,
                            hinh_anh = q.db.image,
                            create_by = q.db.create_by,
                            create_by_name = q.create_by_name,
                            avatar = q.avatar,

                            ngay_dang = q.db.ngay_dang,
                            view_count = q.db.view_count ?? 0,
                            comment_count = q.db.comment_count ?? 0,
                        }).Skip(0).Take(4).ToList()
                    }).ToList(),
                 }).ToList();

            result = result.Where(d => d.list_types.Where(t => t.lst_news.Count > 0).Count() > 0).ToList();

            return Json(result);
        }
        public async Task<IActionResult> getGroupNewsInfo([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var id_user = getUserId();
            var news_query = repo.FindAllNewPortal(id_user);
            var group_news = repo._context.sys_group_news.Where(q => q.id == id && q.status_del == 1).FirstOrDefault();
            var result = repo._context.sys_type_news.OrderBy(d => d.stt)
                .Where(q => q.id_group_news == group_news.id && q.status_del == 1)
                .Select(d => new
                {
                    id_type_news = d.id,
                    image = d.image,
                    type_news_name = d.name,
                    type_news_name_en = d.name_en,
                    lst_news = news_query
                    .Where(q => q.db.id_type_news == d.id).OrderByDescending(q => q.db.ngay_dang).Select(q => new
                    {
                        id = q.db.id,
                        tieu_de = q.db.tieu_de,
                        noi_dung_trang_bia = q.db.noi_dung_trang_bia,
                        noi_dung = q.db.noi_dung,
                        noi_dung_mobile = q.db.noi_dung_mobile,
                        hinh_anh = q.db.image,
                        create_by = q.db.create_by,
                        create_by_name = q.create_by_name,
                        avatar = q.avatar,
                        ngay_dang = q.db.ngay_dang,
                        view_count = q.db.view_count ?? 0,
                        comment_count = q.db.comment_count ?? 0,
                        tieu_de_language = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).Select(q => q.tieu_de).FirstOrDefault() ?? "",
                        noi_dung_trang_bia_language = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).Select(q => q.noi_dung_trang_bia).FirstOrDefault() ?? "",
                        noi_dung_language_mobile = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).Select(q => q.noi_dung_mobile).FirstOrDefault() ?? "",
                        nguon_tin_tuc_language = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).Select(q => q.nguon_tin_tuc).FirstOrDefault() ?? "",
                        noi_dung_language = repo._context.sys_news_languages.Where(d => d.id_news == q.db.id).Select(q => q.noi_dung).FirstOrDefault() ?? "",
                    }).Skip(0).Take(4).ToList()
                }).ToList();


            var model = new
            {
                data = result,
                group_name = group_news.name,
                id_group_news_name = id
            };

            return Json(model);


        }
        public async Task<IActionResult> getNews([FromBody] JObject json)
       {
            var id_news = json.GetValue("id_news").ToString();


            var user_id = getUserId();
            var model = repo.FindAll().Where(d => d.db.id == id_news).FirstOrDefault();
            model.db.view_count  = (repo._context.sys_news.Where(d => d.id == id_news).Select(q=>q.view_count).FirstOrDefault()??0) + 1;

            var db = repo._context.sys_news.Where(d => d.id == id_news).FirstOrDefault();
            db.view_count = model.db.view_count;
            repo._context.SaveChanges();

            //if (!string.IsNullOrEmpty(user_id))
            //{
            //    var check = repo._context.sys_news_log_views.Where(d => d.user_id == user_id && d.id_news == id_news).Count();
            //    if (check == 0)
            //    {
            //        var db = new sys_news_log_view_db()
            //        {
            //            id_news = id_news,
            //            user_id = user_id,
            //            view_date = DateTime.Now,
            //        };
            //        repo._context.sys_news_log_views.Add(db);
            //        model.db.view_count = repo._context.sys_news_log_views.Where(d => d.id_news == id_news).Count() + 1;
            //        repo._context.SaveChanges();
            //    }
            //}




            var language = repo._context.sys_news_languages.Where(d => d.id_news == id_news).FirstOrDefault();
            if (language != null)
            {
                model.tieu_de_language = language.tieu_de ?? "";
              
                model.noi_dung_trang_bia_language = language.noi_dung_trang_bia ?? "";
                model.nguon_tin_tuc_language = language.nguon_tin_tuc ?? "";
                model.noi_dung_language = language.noi_dung ?? "";
                model.noi_dung_language_mobile = language.noi_dung_mobile ?? "";
            }
            var hinhanh = repo._context.sys_news.Where(d => d.id == id_news).FirstOrDefault();
            if (hinhanh != null)
            {
                model.hinh_anh = hinhanh.image ?? "";

            }

            var avt = repo._context.users.Where(d => d.Id == id_news).FirstOrDefault();
            if (avt != null)
            {
                model.avatar = avt.avatar_path ?? "";

            }
            
            var type_news = repo._context.sys_type_news.Where(q => q.id == model.db.id_type_news).FirstOrDefault();
            var id_group_news = repo._context.sys_type_news.Where(q => q.id == model.db.id_type_news).Select(q => q.id_group_news).FirstOrDefault();
            var group_news = repo._context.sys_group_news.Where(q => q.id == id_group_news).FirstOrDefault();

            var modelNew = new
            {
                data = model,
                type_news = type_news,
                id_group_news = id_group_news,
                group_news = group_news

            };

            return Json(modelNew);
        }

        public async Task<IActionResult> getNewsByUser([FromBody] JObject json)
        {
            var id_user = "";
            try
            {
                id_user = json.GetValue("id_user").ToString();
            }
            catch
            {
                id_user = getUserId();
            }

            var a = id_user;
            var result =
                    repo._context.sys_news.Where(q => q.create_by == id_user).OrderByDescending(q => q.ngay_dang).Select(q => new
                    {
                        id = q.id,
                        tieu_de = q.tieu_de,
                        noi_dung_trang_bia = q.noi_dung_trang_bia,
                        noi_dung = q.noi_dung,
                        hinh_anh = q.image,
                        create_by = q.create_by,
                        create_by_name = repo._context.users.Where(s => s.Id == q.create_by).Select(q => q.full_name).FirstOrDefault(),
                        avatar = repo._context.users.Where(s => s.Id == q.create_by).Select(q => q.avatar_path).FirstOrDefault(),
                        view_count = q.view_count ?? 0,
                        comment_count = q.comment_count ?? 0,
                        ngay_dang = q.ngay_dang
                    }).ToList();


            var user = repo._context.users.Where(q => q.Id == id_user).FirstOrDefault();
            var model = new
            {
                data = result,
                user = user

            };
            return Json(model);


        }


        public IActionResult get_comment([FromBody] JObject json)
        {
            var id_news = json.GetValue("id_news").ToString();

            var result =
                  repo._context.sys_news_comments.Where(q => q.id_news == id_news).OrderByDescending(q => q.comment_date).Select(q => new
                  {
                      id = q.id,
                      user_id = q.user_id,
                      comment = q.comment,
                      comment_date = q.comment_date,
                      create_by_name = repo._context.users.Where(s => s.Id == q.user_id).Select(q => q.full_name).FirstOrDefault(),
                      position = repo._context.users.Where(s => s.Id == q.user_id).Select(q => q.position).FirstOrDefault() ?? "",
                      avatar = repo._context.users.Where(s => s.Id == q.user_id).Select(q => q.avatar_path).FirstOrDefault(),
                  }).ToList();


            return Json(result);
        }




        public IActionResult generateSearch_text()
        {
            var news = repo._context.sys_news.Where(d => d.status_del == 1).ToList();
            var reponews = new sys_approval_news_repo(repo._context);
            for (int i = 0; i < news.Count; i++)
            {
                reponews.insert_search(news[i]);
            }


            var events = repo._context.sys_events.Where(d => d.status_del == 1).ToList();
            var repoevent = new sys_event_repo(repo._context);
            for (int i = 0; i < events.Count; i++)
            {
                repoevent.insert_search(events[i]);
            }


            var user = repo._context.users.Where(d => d.status_del == 1).ToList();
            var userrepo = new sys_user_repo(repo._context);
            for (int i = 0; i < user.Count; i++)
            {
                userrepo.insert_search(user[i]);
            }


            return Json("");
        }

        public IActionResult add_comment([FromBody] JObject json)
        {
            var id_news = json.GetValue("id_news").ToString();
            var comment = json.GetValue("comment").ToString();
            var captcha = json.GetValue("captcha").ToString();
            var CaptchaCode = HttpContext.Session.GetString("CaptchaCode");
            if (string.IsNullOrEmpty(comment))
            {
                ModelState.AddModelError("comment", "required");

            }
            else
            {
                var error = "";
                var msg = "";
                var lst_tu_ngu_cam = repo._context.sys_tu_ngu_cams.Where(q => q.status_del == 1).ToList();
                for (int i = 0; i < lst_tu_ngu_cam.Count; i++)
                {
                    var lst = lst_tu_ngu_cam[i].note.Split(';').ToList();

                    for (int j = 0; j < lst.Count; j++)
                    {
                        if (comment.ToString().ToLower().Contains(lst[j].ToLower()))
                        {
                            error += lst[j] + ';';
                        }
                    }
                    msg = error;

                };
                if (!String.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("comment", "Bình luận  có chứa từ ngữ nhạy cảm " + msg);
                }
            }

            if (captcha.ToLower() != CaptchaCode.ToLower())
            {
                ModelState.AddModelError("captcha", "captcha_invalid");

            }

            if (!ModelState.IsValid)
            {
                return generateError();
            }

            var db = new sys_news_comment_db();
            db.id = 0;
            db.user_id = getUserId();
            db.id_news = id_news;
            db.comment = comment;
            db.comment_date = DateTime.Now;
            repo._context.Add(db);
            repo._context.SaveChanges();

            var news = repo._context.sys_news.Where(d => d.id == id_news).SingleOrDefault();
            news.comment_count = repo._context.sys_news_comments.Where(d => d.id_news == id_news).Count();
            repo._context.SaveChanges();
            return Json(0);
        }

        public IActionResult getListNews()
        {

            var lstGroup = repo._context.sys_news.Select(q => q.id_group_news).Distinct().ToList();




            return Json(0);
        }



        [HttpPost]
        public async Task<IActionResult> edit([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_news_model>(json.GetValue("data").ToString());
            var check = checkModelStateEdit(model);
            if (!check)
            {
                return generateError();
            }

            model.db.update_by = getUserId();
            model.db.update_date = DateTime.Now;

            var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == getUserId()).Select(a => a.id_type_news).FirstOrDefault();
            var quantri = "eacf8b68-47bf-4f71-a987-1befb4dad9df";

            var admin = repo._context.sys_group_user_details.Where(q => q.user_id == getUserId() && q.id_group_user == quantri).Count() > 0;

            if (admin)
            {
                model.db.status_del = 1;
                model.db.id_user_approval = getUserId();
                model.db.approval_date = DateTime.Now;


            }
            else
            {
                if (list_type_news == null)
                {
                    model.db.status_del = 3;

                }
                else
                {
                    var type_news = list_type_news.Split(",").ToList();

                    if (type_news.Contains(model.db.id_type_news))
                    {
                        model.db.status_del = 1;
                        model.db.id_user_approval = getUserId();
                        model.db.approval_date = DateTime.Now;

                    }
                    else
                    {
                        model.db.status_del = 3;
                    }
                }



            }

            if (model.db.image == null)
            {
                model.db.image = repo._context.sys_cau_hinh_anh_mac_dinhs.Where(q => q.type == 2).Select(q => q.image).FirstOrDefault(); //_appsetting.avatar;
            }

            await repo.update(model);
            model = await repo.getElementById(model.db.id);
            return Json(model);
        }


        [HttpPost]
        public async Task<IActionResult> edit_language([FromBody] JObject json)
        {
            var model = JsonConvert.DeserializeObject<sys_news_language_model>(json.GetValue("data").ToString());
            var check = checkModelStateEditLanguage(model);
            if (!check)
            {
                return generateError();
            }

            var data = repo._context.sys_news_languages.Where(q => q.id_news == model.db.id_news).FirstOrDefault();
            if (data == null)
            {
                var model_language = new sys_news_language_model();
                model_language.db.id = Guid.NewGuid().ToString();
                model_language.db.id_news = model.db.id_news;
                model_language.db.tieu_de = model.db.tieu_de;
                model_language.db.noi_dung = model.db.noi_dung;
                model_language.db.noi_dung_trang_bia = model.db.noi_dung_trang_bia;
                model_language.db.nguon_tin_tuc = model.db.nguon_tin_tuc;
                model_language.db.noi_dung_mobile = model.db.noi_dung_mobile;
                await repo.insert_language(model_language);
            }
            else
            {
                data.tieu_de = model.db.tieu_de;
                data.noi_dung = model.db.noi_dung;
                data.noi_dung_trang_bia = model.db.noi_dung_trang_bia;
                data.nguon_tin_tuc = model.db.nguon_tin_tuc;
                data.noi_dung_mobile = model.db.noi_dung_mobile;
                repo._context.SaveChanges();
            }


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
                     name = d.full_name,
                 }).ToList();
            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> DataHandlerPersonNews([FromBody] JObject json)
        {
            try
            {
                var user_id = getUserId();
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = (dictionary["search"] ?? "").Trim().ToLower();
                var id_group_news = dictionary["id_group_news"];
                var id_type_news = dictionary["id_type_news"];
                var status_del = int.Parse(dictionary["status_del"]);





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



                var query = Enumerable.Empty<sys_news_model>().AsQueryable();

                query = repo.FindAll()
                .Where(d => d.db.tieu_de.ToLower().Contains(search) || d.db.noi_dung.ToLower().Contains(search))
             .Where(d => d.db.create_by == getUserId())
                    .Where(d => d.db.status_del == status_del || status_del == -1)
                .Where(d => d.db.id_group_news == id_group_news || id_group_news == "-1")
        .Where(d => d.db.id_type_news == id_type_news || id_type_news == "-1");




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
                dataList.ForEach(t =>
                {
                    t.khoa = (t.db.id_khoa ?? "").Split(",").ToList();
                    t.hinhthuc = (t.db.hinh_thuc_user ?? "").Split(",").ToList();
                });
                DTResult<sys_news_model> result = new DTResult<sys_news_model>
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

        public async Task<IActionResult> hien_binh_luan([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();

            repo.hien_binh_luan(id);
            return Json("");
        }

        public async Task<IActionResult> an_binh_luan([FromBody] JObject json)
        {
            var id = json.GetValue("id").ToString();
            var user_id = getUserId();
            repo.an_binh_luan(id);
            return Json("");
        }



        [HttpPost]
        public async Task<IActionResult> DataHandlerComment([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = (dictionary["search"] ?? "").Trim().ToLower();
                var id_news = (dictionary["id_news"] ?? "").Trim().ToLower();
                var query = repo.FindAllComment()
                      .Where(d => d.id_news == id_news)
                      .Where(d => d.comment.ToLower().Contains(search));




                var count = query.Count();
                var dataList = await Task.Run(() => query.OrderByDescending(d => d.comment_date).Skip(param.Start).Take(param.Length).ToList());
                dataList.ForEach(t =>
                {
                });
                DTResult<sys_news_comment_model> result = new DTResult<sys_news_comment_model>
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

        [HttpPost]
        public async Task<IActionResult> DataHandler([FromBody] JObject json)
        {
            try
            {
                var a = Request;
                var param = JsonConvert.DeserializeObject<DTParameters>(json.GetValue("param1").ToString());
                var dictionary = new Dictionary<string, string>();
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json.GetValue("data").ToString());

                var search = (dictionary["search"] ?? "").Trim().ToLower();
                var id_group_news = dictionary["id_group_news"];
                var id_type_news = dictionary["id_type_news"];
                var status_del = int.Parse(dictionary["status_del"]);

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


                var query = Enumerable.Empty<sys_news_model>().AsQueryable();


                var list_type_news = repo._context.sys_user_typenews.Where(a => a.id_user == getUserId()).Select(a => a.id_type_news).FirstOrDefault();
                var quantri = "eacf8b68-47bf-4f71-a987-1befb4dad9df";

                var admin = repo._context.sys_group_user_details.Where(q => q.user_id == getUserId() && q.id_group_user == quantri).Count() > 0;

                ////nếu nhóm admin
                //if (admin)
                //{

                //}
                //else
                //{
                //    //nếu chưa cấu hình
                //    if (list_type_news == null)
                //    {
                //        model.db.status_del = 3;

                //    }
                //    else
                //    {
                //        //nếu có cấu hình và chứa loại đó
                //        var type_news = list_type_news.Split(",").ToList();

                //        if (type_news.Contains(model.db.id_type_news))
                //        {


                //        }
                //        else  //nếu có cấu hình và ko chứa loại đó
                //        {

                //        }
                //    }



                //}


                query = repo.FindAll()
                      .Where(d => d.db.tieu_de.ToLower().Contains(search) || d.db.noi_dung.ToLower().Contains(search))
                        .Where(d => d.db.status_del == status_del || status_del == -1)
                    .Where(d => d.db.id_group_news == id_group_news || id_group_news == "-1")
            .Where(d => d.db.id_type_news == id_type_news || id_type_news == "-1")
            .Where(d => (d.db.is_hot ?? false) == Boolean.Parse(is_hot));







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
                dataList.ForEach(t =>
                {
                    t.khoa = (t.db.id_khoa ?? "").Split(",").ToList();
                    t.hinhthuc = (t.db.hinh_thuc_user ?? "").Split(",").ToList();
                });
                DTResult<sys_news_model> result = new DTResult<sys_news_model>
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
