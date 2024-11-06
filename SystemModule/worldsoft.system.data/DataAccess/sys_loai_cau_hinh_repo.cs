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
    public class sys_loai_cau_hinh_repo 
    {
        public worldsoftDefautContext _context;

        public sys_loai_cau_hinh_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_loai_cau_hinh_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_loai_cau_hinh_model model)
        {
            await _context.sys_loai_cau_hinh_dbs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_loai_cau_hinh_model model)
        {
           var db= await _context.sys_loai_cau_hinh_dbs.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.code = model.db.code;

            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_loai_cau_hinh_model> FindAll()
        {
            var result = _context.sys_loai_cau_hinh_dbs.Select(d=> new sys_loai_cau_hinh_model()
            {
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_loai_cau_hinh_dbs.Where(x => x.id ==id).FirstOrDefault();
            //itemToRemove.id = id;
            _context.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_loai_cau_hinh_dbs.Where(x => x.id == id).FirstOrDefault();


            itemToRemove.id = id;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
