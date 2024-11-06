using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;

namespace worldsoft.system.data.DataAccess
{
    public class sys_event_email_repo
    {
        public worldsoftDefautContext _context;

        public sys_event_email_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_event_email_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public IQueryable<sys_event_email_model> FindAllItem(string id)
        {
            var result = _context.sys_event_emails
                .Where(t => t.event_id == id)
                .Select(d => new sys_event_email_model()
                {
                    //user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.full_name).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public async Task<int> insert(sys_event_email_model model)
        {

            await _context.sys_event_emails.AddAsync(model.db);
            _context.SaveChanges();
            //saveDetail(model);


            return 1;
        }
        public async Task<int> saveKhachMoiEvent(List<sys_event_email_model> list_khach_moi, string event_id)
        {
            //1 khach moi ; 2 ban to chuc
            var delete1 = _context.sys_event_emails.Where(t => t.event_id == event_id);
            _context.RemoveRange(delete1);
            _context.SaveChanges();

            list_khach_moi.ForEach(t =>
            {
                t.db.id = DateTime.Now.Ticks + "";
                t.db.event_id = event_id;
                //t.db.user_id = t.id_khach_moi;
                //t.db.date_add = DateTime.Now;
                //t.db.check_in_date = DateTime.Now;

            });
            var listInsertKhachMoi = list_khach_moi.Select(d => d.db).ToList();
            _context.sys_event_emails.AddRange(listInsertKhachMoi);
            _context.SaveChanges();

            return 1;

        }
        public async Task<int> saveBanToChucEvent(List<sys_event_email_model> list_ban_to_chuc, string event_id)
        {
            //1 khach moi ; 2 ban to chuc
            var delete1 = _context.sys_event_emails.Where(t => t.event_id == event_id);
            _context.RemoveRange(delete1);
            _context.SaveChanges();

            list_ban_to_chuc.ForEach(t =>
            {
                t.db.id = DateTime.Now.Ticks + "";
                t.db.event_id = event_id;
                //t.db.user_id = t.id_ban_to_chuc;
                //t.db.date_add = DateTime.Now;
                //t.db.check_in_date = DateTime.Now;
            });
            var listInsertBanToChuc = list_ban_to_chuc.Select(d => d.db).ToList();
            _context.sys_event_emails.AddRange(listInsertBanToChuc);
            _context.SaveChanges();
            return 1;

        }



        public async Task<int> update(sys_event_email_model model)
        {
            var db = await _context.sys_event_emails.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            //db.title = model.db.title;
            //db.intro = model.db.intro;
            //db.logo = model.db.logo;
            //db.location = model.db.location;
            //db.update_date = model.db.update_date;
            //db.max_person_participate = model.db.max_person_participate;
            //db.update_by = model.db.update_by;
            //db.type = model.db.type;
            //db.mo_ta = model.db.mo_ta;
            _context.SaveChanges();
            //saveDetail(model);
            return 1;
        }


        public IQueryable<sys_event_email_model> FindAll()
        {
            var result = _context.sys_event_emails.Select(d => new sys_event_email_model()
            {

                db = d,
                ten_su_kien = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.title).SingleOrDefault(),
                ten_loai_email = _context.sys_type_news.Where(q => q.id == d.id_type_email).Select(q => q.name).SingleOrDefault(),
                ten_mau_email = _context.sys_template_mails.Where(q => q.id == d.id_template).Select(q => q.name).SingleOrDefault(),
                ten_nguoi_lap = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + d.LastName).SingleOrDefault(),
            });

            return result;
        }

        public int delete(string id, string user_id)
        {
            var itemToRemove = _context.sys_event_emails.Where(x => x.id == id).SingleOrDefault();
            //itemToRemove.status_del =2;


            _context.SaveChanges();
            return 1;
        }
        public int sudung(string id, string user_id)
        {
            var itemToRemove = _context.sys_event_emails.Where(x => x.id == id).SingleOrDefault();
            //itemToRemove.status_del = 1;


            _context.SaveChanges();
            return 1;
        }

    }
}
