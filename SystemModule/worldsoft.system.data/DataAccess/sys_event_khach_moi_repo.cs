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
    public class sys_event_khach_moi_repo
    {
        public worldsoftDefautContext _context;

        public sys_event_khach_moi_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_event_khach_moi_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_event_khach_moi_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_event_khach_mois.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_event_khach_moi_model model)
        {
            var db = await _context.sys_event_khach_mois.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.dien_thoai = model.db.dien_thoai;
            db.avatar_path = model.db.avatar_path;
            db.position = model.db.position;
            db.company = model.db.company;

            db.email = model.db.email;
            db.status_del = model.db.status_del;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.id_su_kien = model.db.id_su_kien;
            db.check_in_status = model.db.check_in_status;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_event_khach_moi_model> FindAll()
        {
            var result = _context.sys_event_khach_mois.Select(d => new sys_event_khach_moi_model()
            {
                //field_name = _context.sys_fields.Where(t => t.id == d.id_field).Select(d => d.name).SingleOrDefault(),
                //createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
                ten_dien_gia = _context.sys_events.Where(t => t.id == d.update_by).Select(d => d.title).SingleOrDefault(),
            });

            return result;
        }
        public int update_status(string id, string status,string ly_do, string user_id)
        {
            var itemToRemove = _context.sys_event_khach_mois.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.ly_do = ly_do;
            itemToRemove.check_in_status = int.Parse(status);
            itemToRemove.update_date = DateTime.Now;
            itemToRemove.update_by = user_id;
            _context.SaveChanges();

            return 1;
        }

    }
}
