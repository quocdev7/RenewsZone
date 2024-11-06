using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;
using worldsoft.common.Helpers;

namespace worldsoft.system.data.DataAccess
{
    public class sys_cau_hinh_thong_tin_repo 
    {
        public worldsoftDefautContext _context;

        public sys_cau_hinh_thong_tin_repo(worldsoftDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_cau_hinh_thong_tin_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_cau_hinh_thong_tin_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            
            await _context.sys_cau_hinh_thong_tins.AddAsync(model.db);
            _context.SaveChanges();


            return 1;
        }
        //public async Task<int> insert_language(sys_cau_hinh_thong_tin_language_model model)
        //{
        //    var db = await _context.sys_cau_hinh_thong_tin_languages.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
        //    db.tieu_de = model.db.tieu_de;
        //    db.noi_dung = model.db.noi_dung;
        //    db.noi_dung_mobile = model.db.noi_dung_mobile;
        //    _context.SaveChanges();
        //    return 1;
        //}
        public async Task<int> update(sys_cau_hinh_thong_tin_model model)
        {
            var db = await _context.sys_cau_hinh_thong_tins.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.tieu_de = model.db.tieu_de;
            db.noi_dung = model.db.noi_dung;           
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.status = model.db.status;
            db.id_loai = model.db.id_loai;
            db.noi_dung_mobile = model.db.noi_dung_mobile;
            db.noi_dung_mobile_en = model.db.noi_dung_mobile_en;
            db.tieu_de_en = model.db.tieu_de_en;
            db.noi_dung_en = model.db.noi_dung_en;
            db.stt = model.db.stt;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_cau_hinh_thong_tin_model> FindAll()
        {
            var result = _context.sys_cau_hinh_thong_tins.Select(d => new sys_cau_hinh_thong_tin_model()
            {
                create_by_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                loai_cau_hinh = _context.sys_loai_cau_hinh_dbs.Where(t => t.id == d.id_loai).Select(d => d.name ).SingleOrDefault(),
                db = d,
            });

            return result;
        }      
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_cau_hinh_thong_tins.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }
    }
}
