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
    public class sys_cau_hinh_duyet_su_kien_repo
    {
        public worldsoftDefautContext _context;

        public sys_cau_hinh_duyet_su_kien_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_cau_hinh_duyet_su_kien_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_cau_hinh_duyet_su_kien_model model)
        {
            
            model.db.id_khoa = string.Join(",", model.khoa);
            await _context.sys_cau_hinh_duyet_su_kiens.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_cau_hinh_duyet_su_kien_model model)
        {
            var db = await _context.sys_cau_hinh_duyet_su_kiens.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            model.db.id_khoa = string.Join(",", model.khoa);
            db.user_id = model.db.user_id;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_cau_hinh_duyet_su_kien_model> FindAll()
        {
            var result = _context.sys_cau_hinh_duyet_su_kiens.Select(d => new sys_cau_hinh_duyet_su_kien_model()
            {


                full_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),



                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_cau_hinh_duyet_su_kiens.Where(x => x.id == id).FirstOrDefault();


            _context.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }


    }
}
