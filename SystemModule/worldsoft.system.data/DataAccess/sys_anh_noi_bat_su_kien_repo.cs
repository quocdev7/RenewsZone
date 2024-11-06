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
    public class sys_anh_noi_bat_su_kien_repo 
    {
        public worldsoftDefautContext _context;

        public sys_anh_noi_bat_su_kien_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_anh_noi_bat_su_kien_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_anh_noi_bat_su_kien_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_anh_noi_bat_su_kiens.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_anh_noi_bat_su_kien_model model)
        {
           var db= await _context.sys_anh_noi_bat_su_kiens.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.stt = model.db.stt;
            db.link = model.db.link;
            db.image = model.db.image;   
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.image_mobile = model.db.image_mobile;
            db.id_su_kien = model.db.id_su_kien;
            db.name_en = model.db.name_en;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_anh_noi_bat_su_kien_model> FindAll()
        {
            var result = _context.sys_anh_noi_bat_su_kiens.Select(d=> new sys_anh_noi_bat_su_kien_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                ten_su_kien = _context.sys_events.Where(t => t.id == d.id_su_kien).Select(d => d.title).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_anh_noi_bat_su_kiens.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_anh_noi_bat_su_kiens.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
