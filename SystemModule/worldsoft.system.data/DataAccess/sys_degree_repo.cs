﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;

namespace worldsoft.system.data.DataAccess
{
    public class sys_degree_repo
    {
        public worldsoftDefautContext _context;

        public sys_degree_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_degree_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_degree_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_degrees.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_degree_model model)
        {
            var db = await _context.sys_degrees.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.name = model.db.name;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_degree_model> FindAll()
        {
            var result = _context.sys_degrees.Select(d => new sys_degree_model()
            {
                //field_name = _context.sys_fields.Where(t => t.id == d.id_field).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_degrees.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();

            return 1;
        }

    }
}
