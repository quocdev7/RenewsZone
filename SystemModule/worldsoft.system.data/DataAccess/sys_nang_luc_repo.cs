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
    public class sys_nang_luc_repo 
    {
        public worldsoftDefautContext _context;

        public sys_nang_luc_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_nang_luc_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_nang_luc_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.update_date;
            model.db.create_date = model.db.update_date;
            await _context.sys_nang_luc.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_nang_luc_model model)
        {
           var db= await _context.sys_nang_luc.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.tieu_de = model.db.tieu_de;
            db.tieu_de_tieng_anh = model.db.tieu_de_tieng_anh;
            db.noi_dung = model.db.noi_dung;
            db.noi_dung_tieng_anh = model.db.noi_dung_tieng_anh;
            db.hinh_anh = model.db.hinh_anh;

            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_nang_luc_model> FindAll()
        {
            var result = _context.sys_nang_luc.Select(d=> new sys_nang_luc_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_nang_luc.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_nang_luc.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
