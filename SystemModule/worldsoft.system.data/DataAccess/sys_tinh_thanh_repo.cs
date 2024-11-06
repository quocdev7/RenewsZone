using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using worldsoft.DataBase.Provider;
using worldsoft.system.data.Models;

namespace worldsoft.system.data.DataAccess
{
    public class sys_tinh_thanh_repo
    {
        public worldsoftDefautContext _context;

        public sys_tinh_thanh_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_tinh_thanh_model> getElementById(long id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_tinh_thanh_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_tinh_thanhs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_tinh_thanh_model model)
        {
            var db = await _context.sys_tinh_thanhs.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.status_del = model.db.status_del;
            db.ten = model.db.ten;
            db.ten_khong_dau = model.db.ten_khong_dau;
            db.id_quoc_gia = model.db.id_quoc_gia;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.note = model.db.note;
            _context.SaveChanges();
            return 1;
        }


        public int update_status_del(string id, string userid, int status_del)
        {
            var itemToRemove = _context.sys_tinh_thanhs.Where(x => x.id == long.Parse(id)).FirstOrDefault();
            itemToRemove.status_del = status_del;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_tinh_thanh_model> FindAll()
        {
            var result = _context.sys_tinh_thanhs.Select(d => new sys_tinh_thanh_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                //quoc_gia = _context.sys_quoc_gias.Where(t => t.id == d.id_quoc_gia).Select(d => d.ten).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(long id, string userid)
        {
            var itemToRemove = _context.sys_tinh_thanhs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }

    }
}
