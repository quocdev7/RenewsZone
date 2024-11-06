using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using worldsoft.DataBase.Provider;
using worldsoft.system.data.Models;

namespace worldsoft.system.data.DataAccess
{
    public class sys_khuyen_mai_repo
    {
        public worldsoftDefautContext _context;

        public sys_khuyen_mai_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_khuyen_mai_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_khuyen_mai_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_khuyen_mais.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_khuyen_mai_model model)
        {
            var db = await _context.sys_khuyen_mais.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.status_del = model.db.status_del;
            db.content = model.db.content;
            db.name = model.db.name;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_khuyen_mai_model> FindAll()
        {
            var result = _context.sys_khuyen_mais.Select(d => new sys_khuyen_mai_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_khuyen_mais.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            _context.SaveChanges();

            return 1;
        }

    }
}
