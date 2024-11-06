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
    public class sys_cau_hinh_anh_mac_dinh_repo 
    {
        public worldsoftDefautContext _context;

        public sys_cau_hinh_anh_mac_dinh_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_cau_hinh_anh_mac_dinh_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_cau_hinh_anh_mac_dinh_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_cau_hinh_anh_mac_dinhs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_cau_hinh_anh_mac_dinh_model model)
        {
           var db= await _context.sys_cau_hinh_anh_mac_dinhs.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.image = model.db.image;
            db.avatar = model.db.avatar;
            db.type = model.db.type;
 
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.create_date = model.db.create_date;
            db.create_by = model.db.create_by;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_cau_hinh_anh_mac_dinh_model> FindAll()
        {
            var result = _context.sys_cau_hinh_anh_mac_dinhs.Select(d=> new sys_cau_hinh_anh_mac_dinh_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
               
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_cau_hinh_anh_mac_dinhs.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_cau_hinh_anh_mac_dinhs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
