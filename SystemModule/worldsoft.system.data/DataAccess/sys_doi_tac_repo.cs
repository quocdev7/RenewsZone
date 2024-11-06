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
    public class sys_doi_tac_repo
    {
        public worldsoftDefautContext _context;

        public sys_doi_tac_repo(worldsoftDefautContext context)
        {
            _context = context;
        }
        public async Task<int> insert_language(sys_doi_tac_language_model model_language)
        {
            await _context.sys_doi_tac_languages.AddAsync(model_language.db);
            _context.SaveChanges();

            return 1;
        }
        public async Task<sys_doi_tac_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_doi_tac_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            model.db.note = model.db.note;
            model.db.image = model.db.image;
            await _context.sys_doi_tacs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_doi_tac_model model)
        {
            var db = await _context.sys_doi_tacs.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.status_del = model.db.status_del;
            db.name = model.db.name;
            db.note = model.db.note;
            db.image=model.db.image;
            db.id_loai_doi_tac = model.db.id_loai_doi_tac;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.stt = model.db.stt;
            db.link = model.db.link;

            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_doi_tac_model> FindAll()
        {
            var result = _context.sys_doi_tacs.Select(d => new sys_doi_tac_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                loai_doi_tac = _context.sys_loai_doi_tacs.Where(t => t.id == d.id_loai_doi_tac).Select(d => d.name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_doi_tacs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }

    }
}
