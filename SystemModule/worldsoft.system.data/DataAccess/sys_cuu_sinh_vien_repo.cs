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
    public class sys_cuu_sinh_vien_repo
    {
        public worldsoftDefautContext _context;

        public sys_cuu_sinh_vien_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_cuu_sinh_vien_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_cuu_sinh_vien_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_cuu_sinh_viens.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_cuu_sinh_vien_model model)
        {
            var db = await _context.sys_cuu_sinh_viens.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.name_company = model.db.name_company;
            db.image = model.db.image;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.id_nhom_hoi_dong = model.db.id_nhom_hoi_dong;
            db.hien_thi_trang_chu = model.db.hien_thi_trang_chu;
            db.chuc_danh_hoi_dong = model.db.chuc_danh_hoi_dong;
            db.nien_khoa = model.db.nien_khoa;
            db.stt = model.db.stt;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_cuu_sinh_vien_model> FindAll()
        {
            var result = _context.sys_cuu_sinh_viens.Select(d => new sys_cuu_sinh_vien_model()
            {
                //field_name = _context.sys_fields.Where(t => t.id == d.id_field).Select(d => d.name).SingleOrDefault(),
                //createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                ten_nhom_hoi_dong = _context.sys_nhom_hoi_dongs.Where(t => t.id == d.id_nhom_hoi_dong).Select(d => d.name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_cuu_sinh_viens.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
