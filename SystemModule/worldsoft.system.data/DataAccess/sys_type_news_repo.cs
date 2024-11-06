﻿using Microsoft.EntityFrameworkCore;
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
    public class sys_type_news_repo 
    {
        public worldsoftDefautContext _context;

        public sys_type_news_repo(worldsoftDefautContext context)
        {
            _context = context;
        }
        public async Task<sys_type_news_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_type_news_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_type_news.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_type_news_model model)
        {
            var db = await _context.sys_type_news.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.note = model.db.note;
            db.name = model.db.name;
            db.name_en = model.db.name_en;
            db.image = model.db.image;
            db.id_group_news = model.db.id_group_news;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            db.stt = model.db.stt;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_type_news_model> FindAll()
        {
            var result = _context.sys_type_news.Select(d => new sys_type_news_model()
            {
                create_by_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                group_news_name = _context.sys_group_news.Where(t => t.id == d.id_group_news).Select(t=>t.name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_type_news.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }
    }
}
