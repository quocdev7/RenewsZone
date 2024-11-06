using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;
using worldsoft.common.Helpers;
using worldsoft.DataBase.System;
using System.Text.RegularExpressions;
using worldsoft.DataBase.Helper;
using System.Web;

namespace worldsoft.system.data.DataAccess
{
    public class sys_news_repo 
    {
        public worldsoftDefautContext _context;

        public sys_news_repo(worldsoftDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_news_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            obj.hinhthuc = (obj.db.hinh_thuc_user ?? "").Split(",").ToList();
            obj.khoa = (obj.db.id_khoa ?? "").Split(",").ToList();
            return obj;
        }

        public async Task<int> insert(sys_news_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
          
                model.db.hinh_thuc_user = string.Join(",", model.hinhthuc);
                model.db.id_khoa = string.Join(",", model.khoa);
          
            await _context.sys_news.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> insert_language(sys_news_language_model model_language)
        {
           
          
            await _context.sys_news_languages.AddAsync(model_language.db);
            _context.SaveChanges();

            return 1;
        }
      

        public async Task<int> update(sys_news_model model)
        {
            var db = await _context.sys_news.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
          
            db.id_tiente = model.db.id_tiente;
            db.tieu_de = model.db.tieu_de;
            db.noi_dung = model.db.noi_dung;
            db.noi_dung_trang_bia = model.db.noi_dung_trang_bia;
            db.image = model.db.image;
            db.id_group_news = model.db.id_group_news;
            db.id_type_news = model.db.id_type_news;
            db.nguon_tin_tuc = model.db.nguon_tin_tuc;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.is_hot = model.db.is_hot;
            db.ngay_dang = model.db.ngay_dang;
            db.is_comment = model.db.is_comment;
            db.quyen_rieng_tu = model.db.quyen_rieng_tu;
            db.noi_dung_mobile = model.db.noi_dung_mobile;
            db.vi_tri_tin_noi_bat = model.db.vi_tri_tin_noi_bat;
            db.so_tien = model.db.so_tien;
          
                model.db.hinh_thuc_user = string.Join(",", model.hinhthuc);
                model.db.id_khoa = string.Join(",", model.khoa);
            
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_news_model> FindAll()
        {
            var result = _context.sys_news.Select(d => new sys_news_model() {


                ten_tien_te = _context.sys_tien_tes.Where(t => t.id == d.id_tiente).Select(d => d.name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                aprroval_by_name = _context.users.Where(t => t.Id == d.id_user_approval).Select(d => d.full_name).SingleOrDefault(),
                create_by_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
                group_news_name = _context.sys_group_news.Where(q => q.id == d.id_group_news).Select(q => q.name).FirstOrDefault(),
                type_news_name = _context.sys_type_news.Where(q => q.id == d.id_type_news).Select(q => q.name).FirstOrDefault(),
                avatar = _context.users.Where(t => t.Id == d.create_by).Select(d => d.avatar_path).SingleOrDefault(),
                hinh_anh = _context.sys_news.Select(d => d.image).SingleOrDefault(),
                position = _context.users.Where(s => s.Id == d.create_by).Select(q => q.position).FirstOrDefault() ?? "",
                ten_khoa =_context.sys_khoas.Where(q => q.id == d.id_khoa).Select(q => q.name).FirstOrDefault(),
            });

            return result;
        }
        public IQueryable<sys_news_comment_model> FindAllComment()
        {
            var result = _context.sys_news_comments.Select(d => new sys_news_comment_model()
            {

                id =d.id,
                id_news = d.id_news,
                comment= d.comment,
                user_id = d.user_id,
                comment_date = d.comment_date,
                news_name = _context.sys_news.Where(q=>q.id == d.id_news).Select(q=>q.tieu_de).FirstOrDefault(),
                user_comment = _context.users.Where(q => q.Id == d.user_id).Select(q => q.full_name).FirstOrDefault(),

            });

            return result;
        }
        public IQueryable<sys_news_model> FindAllNewPortalXelex()
        {
            var result = _context.sys_news.Select(d => new sys_news_model()
            {
                ten_tien_te = _context.sys_tien_tes.Where(t => t.id == d.id_tiente).Select(d => d.name).SingleOrDefault(),
                create_by_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
                group_news_name = _context.sys_group_news.Where(q => q.id == d.id_group_news).Select(q => q.name).FirstOrDefault(),
                type_news_name = _context.sys_type_news.Where(q => q.id == d.id_type_news).Select(q => q.name).FirstOrDefault(),
                avatar = _context.users.Where(t => t.Id == d.create_by).Select(d => d.avatar_path).SingleOrDefault(),
                ten_khoa = _context.sys_khoas.Where(q => q.id == d.id_khoa).Select(q => q.name).FirstOrDefault(),

            });
            return result;
        }

            public IQueryable<sys_news_model> FindAllNewPortal(string user_id)
        {
            var result = _context.sys_news.Where(d=>d.status_del==1).Select(d => new sys_news_model()
            {
                ten_tien_te = _context.sys_tien_tes.Where(t => t.id == d.id_tiente).Select(d => d.name).SingleOrDefault(),
                create_by_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
                group_news_name = _context.sys_group_news.Where(q => q.id == d.id_group_news).Select(q => q.name).FirstOrDefault(),
                type_news_name = _context.sys_type_news.Where(q => q.id == d.id_type_news).Select(q => q.name).FirstOrDefault(),
                avatar = _context.users.Where(t => t.Id == d.create_by).Select(d => d.avatar_path).SingleOrDefault(),
                ten_khoa = _context.sys_khoas.Where(q => q.id == d.id_khoa).Select(q => q.name).FirstOrDefault(),
            });


            //1.Công khai, 2.Thành viên, 3.Bạn bè, 4.Khoa, 5.Trả phí
            var listRiengTuDuocXem = new List<int>();
            listRiengTuDuocXem.Add(1);
            if (string.IsNullOrEmpty(user_id))
            {
                result = result.Where(d => d.db.id_khoa == "-1");
                result = result.Where(d => d.db.hinh_thuc_user == "-1");
            }
            else
            {
                var user = _context.users.Where(d => d.Id == user_id)
                    .Select(d => new
                    {
                        status_del = d.status_del,
                        hinh_thuc = d.status_graduate,
                        id_khoa = d.id_khoa
                    })
                    .SingleOrDefault();

                // là thành viên
                if (user.status_del == 1)
                {
                    // thanh vien
                    listRiengTuDuocXem.Add(2);
                    // ban be
                    listRiengTuDuocXem.Add(3);
                    var banbe = _context.sys_user_ban_bes.Where(d => d.user_id == user_id && d.status_del == 1);
                    result = result.Where(d => (d.db.quyen_rieng_tu == 3 && banbe.Where(t => t.user_id_ban_be == d.db.create_by).Count() > 0) || d.db.quyen_rieng_tu != 3);
                }
                else
                {
                    listRiengTuDuocXem.Add(4);
                }

                if (!string.IsNullOrEmpty(user.id_khoa))
                {
                    result = result.Where(d => d.db.id_khoa.Contains(user.id_khoa) || d.db.id_khoa == "-1");
                }
                else
                {
                    result = result.Where(d => d.db.id_khoa == "-1");
                }

                if (user.hinh_thuc != null)
                {
                    result = result.Where(d => d.db.hinh_thuc_user.Contains(user.hinh_thuc + "") || d.db.hinh_thuc_user == "-1");
                }
                else
                {
                    result = result.Where(d => d.db.hinh_thuc_user == "-1");
                }



            }
            result = result.Where(d => listRiengTuDuocXem.Contains(d.db.quyen_rieng_tu ?? 0));

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }

        public int hien_binh_luan(string id)
        {
            var itemToRemove = _context.sys_news_comments.Where(x => x.id == long.Parse(id)).FirstOrDefault();
            itemToRemove.status = 1;
            _context.SaveChanges();

            return 1;
        }
        public int an_binh_luan(string id)
        {
            var itemToRemove = _context.sys_news_comments.Where(x => x.id == long.Parse(id)).FirstOrDefault();
            itemToRemove.status = 2;
            _context.SaveChanges();

            return 1;
        }
        public int cancel(string id, string userid,string  reason)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 4;
            itemToRemove.reason_return = reason;
            itemToRemove.id_user_approval = userid;
            _context.SaveChanges();

            return 1;
        }
        public int approval(string id, string userid)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.id_user_approval = userid;
            itemToRemove.approval_date = DateTime.Now;
            _context.SaveChanges();



            insert_search(itemToRemove);
            return 1;
        }
        public void insert_search(sys_news_db model)
        {
            string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.tieu_de ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.noi_dung ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

            var news_language = _context.sys_news_languages.Where(x => x.id_news == model.id).FirstOrDefault();

            string t_language = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(news_language.tieu_de ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            string c_language = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(news_language.noi_dung ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);

            var db = _context.sys_searchs.Where(d => d.id_ref == model.id && d.type == 2).FirstOrDefault();
            if (db != null)
            {
                db.create_date = DateTime.Now;
                db.order_date = model.ngay_dang;
                db.search_text = t + " " + c;
                _context.SaveChanges();
            }
            else
            {
                var db1 = new sys_search_db()
                {
                    create_date = DateTime.Now,
                    order_date = model.ngay_dang,
                    id = 0,
                    id_ref = model.id,
                    search_text = t + " " + c,
                    search_text_language = t_language +" " + c_language,
                    type = 1,
                };

                _context.Add(db1);
                _context.SaveChanges();

            }

        }


    }
}
