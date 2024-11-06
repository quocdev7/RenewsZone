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
    public class sys_cau_hinh_thong_tin_website_repo 
    {
        public worldsoftDefautContext _context;

        public sys_cau_hinh_thong_tin_website_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_cau_hinh_thong_tin_website_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_cau_hinh_thong_tin_website_model model)
        {
            await _context.sys_cau_hinh_thong_tin_website.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_cau_hinh_thong_tin_website_model model)
        {
           var db= await _context.sys_cau_hinh_thong_tin_website.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.tieu_de = model.db.tieu_de;
            db.dia_chi = model.db.dia_chi;
            db.email = model.db.email;
            db.ten_truong = model.db.ten_truong;
            db.facebook_link = model.db.facebook_link;
            db.youtube_link = model.db.youtube_link;
            db.linked_link = model.db.linked_link;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_cau_hinh_thong_tin_website_model> FindAll()
        {
            var result = _context.sys_cau_hinh_thong_tin_website.Select(d=> new sys_cau_hinh_thong_tin_website_model()
            {
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_cau_hinh_thong_tin_website.Where(x => x.id ==id).FirstOrDefault();
            //itemToRemove.id = id;
            _context.Remove(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_cau_hinh_thong_tin_website.Where(x => x.id == id).FirstOrDefault();


            itemToRemove.id = id;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
