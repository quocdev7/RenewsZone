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
    public class sys_like_san_pham_repo
    {
        public worldsoftDefautContext _context;

        public sys_like_san_pham_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_like_san_pham_model> getElementById(long id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_like_san_pham_model model)
        {
            await _context.sys_like_san_phams.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_like_san_pham_model model)
        {
            var db = await _context.sys_like_san_phams.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            
            db.email= model.db.email;

            db.create_date = DateTime.Now;
         
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_like_san_pham_model> FindAll()
        {
            var result = _context.sys_like_san_phams.Select(d => new sys_like_san_pham_model()
            {
             
                db = d,
            });

            return result;
        }
        public int delete(long id, string userid)
        {
            var itemToRemove = _context.sys_like_san_phams.Where(x => x.id == id).FirstOrDefault();
         
            _context.SaveChanges();

            return 1;
        }

    }
}
