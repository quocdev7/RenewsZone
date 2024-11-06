using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;
using worldsoft.common.Helpers;
using System.Text.RegularExpressions;
using worldsoft.DataBase.Helper;
using System.Web;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.DataAccess
{
    public class sys_approval_news_repo 
    {
        public worldsoftDefautContext _context;

        public sys_approval_news_repo(worldsoftDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_approval_news_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_approval_news_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            
            await _context.sys_news.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_approval_news_model model)
        {
            var db = await _context.sys_news.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
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
            model.db.ngay_dang = model.db.ngay_dang;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_approval_news_model> FindAll()
        {
            
            var result = _context.sys_news.Select(d => new sys_approval_news_model()
            {
                
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                aprroval_by_name = _context.users.Where(t => t.Id == d.id_user_approval).Select(d => d.full_name).SingleOrDefault(),

                group_news_name = _context.sys_group_news.Where(t => t.id == d.id_group_news).Select(d => d.name).SingleOrDefault(),
                type_news_name = _context.sys_type_news.Where(t => t.id == d.id_type_news).Select(d => d.name).SingleOrDefault(),
                create_by_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                 avatar  = _context.users.Where(s => s.Id == d.create_by).Select(q => q.avatar_path).FirstOrDefault(),
                position = _context.users.Where(s => s.Id == d.create_by).Select(q => q.position).FirstOrDefault(),
                db = d,
            });

            return result;
        }

        public IQueryable<sys_approval_news_group_model> FindAllNew()
        {
            var result = _context.sys_news.Select(d => new sys_approval_news_group_model()
            {
                id = d.id,
                id_group_news = d.id_group_news,
                id_type_news = d.id_type_news,
                status_del = d.status_del,
                code = _context.sys_group_news.Where(q => q.id == d.id_group_news).Select(q => q.code).FirstOrDefault(),
                group_news_name = _context.sys_group_news.Where(q=>q.id == d.id_group_news).Select(q=>q.name).FirstOrDefault(),
                type_news_name = _context.sys_type_news.Where(q => q.id == d.id_group_news).Select(q => q.name).FirstOrDefault(),

            });

            return result;
        }
        public int reject(string id, string userid,string reason)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del =4;
            itemToRemove.id_user_approval = userid;
            itemToRemove.reason_return = reason;
            _context.SaveChanges();

            return 1;
        }
        public int approval(string id, string userid)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del =1;
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
                    type = 1,
                };

                _context.Add(db1);
                _context.SaveChanges();

            }

        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }
        public int cancel(string id, string userid)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 5;
            itemToRemove.id_user_approval = userid;
            _context.SaveChanges();

            return 1;
        }
    }
}
