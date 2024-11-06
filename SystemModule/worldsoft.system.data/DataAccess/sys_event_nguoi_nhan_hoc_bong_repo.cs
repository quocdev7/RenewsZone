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
    public class sys_event_nguoi_nhan_hoc_bong_repo
    {
        public worldsoftDefautContext _context;

        public sys_event_nguoi_nhan_hoc_bong_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_event_nguoi_nhan_hoc_bong_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_event_nguoi_nhan_hoc_bong_model model)
        {
            model.db.name = model.db.name;
            model.db.so_tien = model.db.so_tien;
            model.db.dien_thoai = model.db.dien_thoai;
            model.db.stt = model.db.stt;
            model.db.id_tien_te = model.db.id_tien_te;
            model.db.id_su_kien = model.db.id_su_kien;
            model.db.mssv = model.db.mssv;
            model.db.note = model.db.note;
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_event_nguoi_nhan_hoc_bongs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_event_nguoi_nhan_hoc_bong_model model)
        {
            var db = await _context.sys_event_nguoi_nhan_hoc_bongs.Where(d => d.id == model.db.id).FirstOrDefaultAsync();


            db.name = model.db.name;
            db.so_tien = model.db.so_tien;
            db.dien_thoai = model.db.dien_thoai;
            db.stt = model.db.stt;
            db.note = model.db.note;
            db.id_tien_te = model.db.id_tien_te;
            db.id_su_kien = model.db.id_su_kien;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.mssv = model.db.mssv;
            db.status_del = model.db.status_del;

            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_event_nguoi_nhan_hoc_bong_model> FindAll()
        {
            var result = _context.sys_event_nguoi_nhan_hoc_bongs.Select(d => new sys_event_nguoi_nhan_hoc_bong_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),

                ten_su_kien = _context.sys_events.Where(t => t.id == d.id_su_kien).Select(d => d.title).SingleOrDefault(),
                ten_tien_te = _context.sys_tien_tes.Where(t => t.id == d.id_su_kien).Select(d => d.name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_event_nguoi_nhan_hoc_bongs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
