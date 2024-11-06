using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using worldsoft.DataBase.Helper;
using worldsoft.DataBase.Provider;
using worldsoft.DataBase.System;
using worldsoft.system.data.Models;

namespace worldsoft.system.data.DataAccess
{
    public class sys_san_pham_repo
    {
        public worldsoftDefautContext _context;

        public sys_san_pham_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_san_pham_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);

            obj.list_file = _context.sys_san_pham_file_uploads.Select(d => new sys_san_pham_file_upload_model()
            {

                db = d,
            }).ToList();

            return obj;
        }
        public async Task<sys_san_pham_model> getElementByIdNew(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);

            obj.list_file = _context.sys_san_pham_file_uploads.Where(q => q.id_san_pham == obj.db.id).Select(d => new sys_san_pham_file_upload_model()
            {

                db = d,
            }).ToList();

            return obj;
        }


        public async Task<int> insert(sys_san_pham_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_san_phams.AddAsync(model.db);
            _context.SaveChanges();
            insert_search(model.db);
            return 1;
        }
        public void insert_search(sys_san_pham_db model)
        {
            string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.ten_san_pham ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.mo_ta ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

            var db = _context.sys_searchs.Where(d => d.id_ref == model.id && d.type == 4).FirstOrDefault();
            if (db != null)
            {
                db.create_date = DateTime.Now;
                db.order_date = model.create_date;
                db.search_text = t + " " + c;
                _context.SaveChanges();
            }
            else
            {
                var db1 = new sys_search_db()
                {
                    create_date = DateTime.Now,
                    order_date = model.create_date,
                    id = 0,
                    id_ref = model.id,
                    search_text = t + " " + c,
                    type = 4,
                };

                _context.Add(db1);
                _context.SaveChanges();

            }

        }
        public async Task<int> update(sys_san_pham_model model)
        {
            var db = await _context.sys_san_phams.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.ten_san_pham = model.db.ten_san_pham;
            db.stt = model.db.stt;
            db.ma_san_pham = model.db.ma_san_pham;
            db.id_khuyen_mai = model.db.id_khuyen_mai;

            db.hinh_anh = model.db.hinh_anh;
            db.hinh_anh_mobile = model.db.hinh_anh_mobile;
            db.so_tien = model.db.so_tien;

            db.thong_so_ky_thuat = model.db.thong_so_ky_thuat;
            db.thong_tin_bo_sung = model.db.thong_tin_bo_sung;
            db.thong_so_ky_thuat_mobile = model.db.thong_so_ky_thuat_mobile;
            db.thong_tin_bo_sung_mobile = model.db.thong_tin_bo_sung_mobile;
            db.mo_ta = model.db.mo_ta;
            db.mo_ta_mobile = model.db.mo_ta_mobile;

            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.id_loai = model.db.id_loai;
            _context.SaveChanges();
            insert_search(db);
            return 1;
        }

        public IQueryable<sys_san_pham_model> FindAll()
        {
            var result = _context.sys_san_phams.Select(d => new sys_san_pham_model()
            {
                khuyen_mai = _context.sys_khuyen_mais.Where(q => q.id == d.id_khuyen_mai).Select(q => q.content).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                ten_loai = _context.sys_loai_san_phams.Where(t => t.id == d.id_loai).Select(d => d.ten_loai).SingleOrDefault(),
                image = _context.sys_san_pham_file_uploads.Where(t => t.id_san_pham == d.id).Select(d => d.file_path).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_san_phams.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_san_phams.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
