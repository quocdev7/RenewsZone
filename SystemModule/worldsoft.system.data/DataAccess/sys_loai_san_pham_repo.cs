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
    public class sys_loai_san_pham_repo 
    {
        public worldsoftDefautContext _context;

        public sys_loai_san_pham_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_loai_san_pham_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_loai_san_pham_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_loai_san_phams.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_loai_san_pham_model model)
        {
           var db= await _context.sys_loai_san_phams.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.ten_loai = model.db.ten_loai;
            db.ma_loai = model.db.ma_loai;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.ten_loai_en = model.db.ten_loai_en;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_loai_san_pham_model> FindAll()
        {
            var result = _context.sys_loai_san_phams.Select(d=> new sys_loai_san_pham_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_loai_san_phams.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_loai_san_phams.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
