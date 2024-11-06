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
    public class sys_user_typenews_repo 
    {
        public worldsoftDefautContext _context;

        public sys_user_typenews_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_user_typenews_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_user_typenews_model model)
        {
            model.db.id_type_news = string.Join(",", model.types);
            model.db.id_khoa = string.Join(",", model.khoa);
            await _context.sys_user_typenews.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_user_typenews_model model)
        {
           var db= await _context.sys_user_typenews.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.id_user = model.db.id_user;
            db.id_type_news = model.db.id_type_news;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.id_type_news = string.Join(",", model.types);
            db.id_khoa = string.Join(",", model.khoa);
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_user_typenews_model> FindAll()
        {
            var result = _context.sys_user_typenews.Select(d=> new sys_user_typenews_model()
            {
             

                full_name =  _context.users.Where(t=>t.Id ==  d.id_user).Select(d=>d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),

               

                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_user_typenews.Where(x => x.id ==id).FirstOrDefault();
        

            _context.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
       
        
    }
}
