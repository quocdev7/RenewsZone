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
    public class sys_anh_san_pham_repo 
    {
        public worldsoftDefautContext _context;

        public sys_anh_san_pham_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_anh_san_pham_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_anh_san_pham_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_anh_san_phams.AddAsync(model.db);

            for (int i = 0; i < model.list_file.Count; i++)
            {

                model.list_file[i].db.id = DateTime.Now.Ticks.ToString();
                model.list_file[i].db.status_del = 1;
                model.list_file[i].db.stt = i + 1;
                model.list_file[i].db.id_san_pham = model.db.id_san_pham;
                _context.sys_san_pham_file_uploads.Add(model.list_file[i].db);

            }
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_anh_san_pham_model model)
        {
           var db= await _context.sys_anh_san_phams.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.stt = model.db.stt;
            db.link = model.db.link;
            db.image = model.db.image;   
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.image_mobile = model.db.image_mobile;
            db.id_san_pham = model.db.id_san_pham;
            _context.SaveChanges();



            var db_file = _context.sys_san_pham_file_uploads.Where(q => q.id_san_pham == db.id_san_pham).ToList();
            _context.RemoveRange(db_file);
            _context.SaveChanges();

            for (int i = 0; i < model.list_file.Count; i++)
            {

                model.list_file[i].db.id = DateTime.Now.Ticks.ToString();
                model.list_file[i].db.status_del = 1;
                model.list_file[i].db.stt = i + 1;
                model.list_file[i].db.id_san_pham = model.db.id_san_pham;
                _context.sys_san_pham_file_uploads.Add(model.list_file[i].db);

            }
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_anh_san_pham_model> FindAll()
        {
            var result = _context.sys_anh_san_phams.Select(d => new sys_anh_san_pham_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                ten_san_pham = _context.sys_san_phams.Where(t => t.id == d.id_san_pham).Select(d => d.ten_san_pham).SingleOrDefault(),
                db = d,
                list_file = _context.sys_san_pham_file_uploads.Where(q => q.id_san_pham == d.id_san_pham).Select(ct => new sys_san_pham_file_upload_model()
                {
                    db = ct

                }).ToList()

            });
         
            return result;
        }

        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_anh_san_phams.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_anh_san_phams.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
