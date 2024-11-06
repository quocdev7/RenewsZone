using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using worldsoft.common.Helpers;
using worldsoft.common.Services;
using worldsoft.DataBase.Provider;
using worldsoft.system.web.Controller;

namespace worldsoft.system.web.ControllerView
{
    public class sys_scanController : Microsoft.AspNetCore.Mvc.Controller
    {
        private worldsoftDefautContext context;
        public sys_scanController(IUserService userService, worldsoftDefautContext _context)
        {
            context = _context;
        }
        public IActionResult index(string id)
        {
            var eventinfo = context.sys_events.Where(t => t.id == id).FirstOrDefault();
            ViewBag.eventinfo = eventinfo;
            return View();
        }
        public IActionResult empty()
        {
            return View();
        }
        public IActionResult userview(string q,string id)
        {
            try
            {


                var decrypt = CMAESCrypto.DecryptText(q);
                var eventid = decrypt.Replace("check_in", "").Split("@@")[0];
                var to_user_id = decrypt.Replace("check_in", "").Split("@@")[1];
                var update = context.sys_event_khach_mois.Where(t => t.id_su_kien == eventid && t.email == to_user_id ).FirstOrDefault();
                var eventinfo = context.sys_events.Where(t => t.id == eventid).FirstOrDefault();
                if (
                    eventid==id &&
                    (update.check_in_status == 3 || update.check_in_status == 4) && DateTime.Now.Date >= eventinfo.time_start.Value.Date && DateTime.Now.Date <= eventinfo.time_end.Value.Date)
                {
                    if (update.check_in_status == 3)
                    {
                        update.check_in_status = 4;
                        update.check_in_date = DateTime.Now;
                        context.SaveChanges();
                    }
                    var userinfo = context.users.Where(t => t.Id == to_user_id).FirstOrDefault(); ;
                    ViewBag.tencongty = userinfo.company;
                    ViewBag.eventinfo = eventinfo;
                    ViewBag.userinfo = userinfo;
                }
                else
                {
                    return View("userviewnotvalid");
                }
              
            }
            catch(Exception e){
                return View("userviewnotvalid");
            }
            return View();
        }
        public IActionResult phoneemail(string id,string ph)
        {
            try
            {
                var eventid = id;
                ph = (ph ?? "").Trim().ToLower();
                var update = context.sys_event_khach_mois.Where(t => t.id_su_kien == eventid  &&  ((t.dien_thoai??"").Trim().ToLower() == ph || (t.email??"").Trim().ToLower() == ph) && ( t.check_in_status == 3 ||t.check_in_status==4)).FirstOrDefault();
                if (update == null) return View("userviewnotvalid");
                var eventinfo = context.sys_events.Where(t => t.id == eventid).FirstOrDefault();
                if (
                    eventid == id &&
                    (update.check_in_status == 3 || update.check_in_status == 4) && DateTime.Now.Date >= eventinfo.time_start.Value.Date && DateTime.Now.Date <= eventinfo.time_end.Value.Date)
                {
                    if (update.check_in_status == 3)
                    {
                        update.check_in_status = 4;
                        update.check_in_date = DateTime.Now;
                        context.SaveChanges();
                    }
                 
                    ViewBag.eventinfo = eventinfo;
                    ViewBag.userinfo = update;
                }
                else
                {
                    return View("userviewnotvalid");
                }
            }
            catch (Exception e)
            {
                return View("userviewnotvalid");
            }
            return View("userview");
        }
    }
}
