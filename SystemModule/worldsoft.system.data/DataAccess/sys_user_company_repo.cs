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
    public class sys_user_company_repo 
    {
        public worldsoftDefautContext _context;

        public sys_user_company_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_user_company_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_user_company_model model)
        {
         
            await _context.sys_user_companys.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_user_company_model model)
        {
           var db= await _context.sys_user_companys.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.user_id = model.db.user_id;
            db.company_id = model.db.company_id;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;

            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_user_company_model> FindAll()
        {
            var result = _context.sys_user_companys.Select(d=> new sys_user_company_model()
            {
                company_name =   _context.sys_companys.Where(t => t.id == d.company_id).Select(d => d.name).SingleOrDefault(),
        
                full_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_user_companys.Where(x => x.id ==id).FirstOrDefault();


            _context.Remove(itemToRemove);
            _context.SaveChanges();

            return 1;
        }
     
        
    }
}
