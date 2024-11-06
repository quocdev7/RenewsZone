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
    public class sys_nghi_quyet_repo
    {
        public worldsoftDefautContext _context;

        public sys_nghi_quyet_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_nghi_quyet_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_nghi_quyet_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_nghi_quyets.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_nghi_quyet_model model)
        {
            var db = await _context.sys_nghi_quyets.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.status_del = model.db.status_del;
            db.title = model.db.title;
            db.noi_dung = model.db.noi_dung;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.note = model.db.note;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_nghi_quyet_model> FindAll()
        {
            var result = _context.sys_nghi_quyets.Select(d => new sys_nghi_quyet_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_nghi_quyets.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }

    }
}
