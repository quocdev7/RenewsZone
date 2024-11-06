
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
    public class sys_person_news_repo
    {
        public worldsoftDefautContext _context;

        public sys_person_news_repo(worldsoftDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_person_news_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_person_news_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            
            await _context.sys_news.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_person_news_model model)
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
            db.ngay_dang = model.db.ngay_dang;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_person_news_model> FindAll()
        {
            var result = _context.sys_news.Select(d => new sys_person_news_model()
            {
                create_by_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
                group_news_name = _context.sys_group_news.Where(q => q.id == d.id_group_news).Select(q => q.name).FirstOrDefault(),
                type_news_name = _context.sys_type_news.Where(q => q.id == d.id_group_news).Select(q => q.name).FirstOrDefault(),
            });

            return result;
        }

        public IQueryable<sys_person_news_group_model> FindAllNew()
        {
            var result = _context.sys_news.Select(d => new sys_person_news_group_model()
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
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }
    }
}
