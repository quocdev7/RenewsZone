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
    public class sys_dien_gia_repo
    {
        public worldsoftDefautContext _context;

        public sys_dien_gia_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_dien_gia_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_dien_gia_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_dien_gias.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }
        public async Task<int> insert_en(sys_dien_gia_en_model model_language)
        {
            await _context.sys_dien_gia_ens.AddAsync(model_language.db);
            _context.SaveChanges();

            return 1;
        }
        public async Task<int> update(sys_dien_gia_model model)
        {
            var db = await _context.sys_dien_gias.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.chuc_danh = model.db.chuc_danh;
            db.cong_ty = model.db.cong_ty;
            db.image = model.db.image;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.stt = model.db.stt;
            db.id_su_kien = model.db.id_su_kien;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_dien_gia_model> FindAll()
        {
            var result = _context.sys_dien_gias.Select(d => new sys_dien_gia_model()
            {
                //field_name = _context.sys_fields.Where(t => t.id == d.id_field).Select(d => d.name).SingleOrDefault(),
                //createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                ten_su_kien = _context.sys_events.Where(t => t.id == d.id_su_kien).Select(d => d.title).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_dien_gias.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
