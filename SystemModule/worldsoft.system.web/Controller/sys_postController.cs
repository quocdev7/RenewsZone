using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldsoft.common.Services;
using worldsoft.DataBase.Provider;

namespace worldsoft.system.web.ControllerView
{
    public class sys_postController : Microsoft.AspNetCore.Mvc.Controller
    {
        private worldsoftDefautContext context;
        public sys_postController(IUserService userService, worldsoftDefautContext _context)
        {
            context = _context;
        }
        public IActionResult index(string id, string t)
        {
            if (t == "1")
            {
                var news = context.sys_news.Where(q => q.id == id).SingleOrDefault();
                ViewBag.id = news.id;
                ViewBag.title = news.tieu_de;
                ViewBag.image = news.image;
                ViewBag.description = news.noi_dung;
                ViewBag.Host = Request.Host.Value;
                return View("news_detail");
            }
            else if (t == "2")
            {
                var news = context.sys_events.Where(q => q.id == id).SingleOrDefault();
                ViewBag.id = news.id;
                ViewBag.title = news.title;
                ViewBag.image = news.logo;
                ViewBag.description = news.mo_ta;

                ViewBag.Host = Request.Host.Value;
                return View("event_detail");
            }
            else
            {
                var news = context.sys_san_phams.Where(q => q.id == id).SingleOrDefault();
                ViewBag.id = news.id;
                ViewBag.title = news.ten_san_pham;
                ViewBag.image = news.hinh_anh;
                ViewBag.description = news.mo_ta;

                ViewBag.Host = Request.Host.Value;
                return View("san_pham");
            }
        }
        public IActionResult san_pham(string id)
        {
            var news = context.sys_san_phams.Where(q => q.id == id).SingleOrDefault();
            ViewBag.id = news.id;
            ViewBag.title = news.ten_san_pham;
            ViewBag.image = news.hinh_anh;
            ViewBag.description = news.mo_ta;

            ViewBag.Host = Request.Host.Value;
            return View();
        }
        public IActionResult event_detail(string id)
        {
            var news = context.sys_events.Where(q => q.id == id).SingleOrDefault();
            ViewBag.id = news.id;
            ViewBag.title = news.title;
            ViewBag.image = news.logo;
            ViewBag.description = news.mo_ta;

            ViewBag.Host = Request.Host.Value;
            return View();
        }
        public IActionResult news_detail(string id)
        {
            var news = context.sys_news.Where(q => q.id == id).SingleOrDefault();
            ViewBag.id = news.id;
            ViewBag.title = news.tieu_de;
            ViewBag.image = news.image;
            ViewBag.description = news.tieu_de;

            ViewBag.Host = Request.Host.Value;
            return View();
        }
    }
}
