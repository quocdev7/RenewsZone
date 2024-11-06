using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.DataAccess
{
    public class sys_group_user_repo 
    {
        public worldsoftDefautContext _context;

        public sys_group_user_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_group_user_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_group_user_model model)
        {
            await _context.sys_group_users.AddAsync(model.db);
            _context.SaveChanges();
            saveDetail(model);
            return 1;
        }

        public async Task<int> update(sys_group_user_model model)
        {
           var db= await _context.sys_group_users.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.status_del = model.db.status_del;
            db.name = model.db.name;
            db.note = model.db.note;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            _context.SaveChanges();
            saveDetail(model);
            return 1;
        }
        public void saveDetail(sys_group_user_model model)
        {
            var delete1 = _context.sys_group_user_details.Where(t => t.id_group_user == model.db.id);
            _context.RemoveRange(delete1);
            _context.SaveChanges();
            var listdetail = model.list_item.Where(t => t.isCheck == true).ToList();
            var listinsert = new List<sys_group_user_detail_db>();
            for (int i = 0; i < listdetail.Count; i++)
            {
                var item = new sys_group_user_detail_db()
                {
                    id = 0,
                    id_group_user = model.db.id,
                    user_id = listdetail[i].user_id,
                };
                listinsert.Add(item);
            }
            _context.sys_group_user_details.AddRange(listinsert);
            _context.SaveChanges();


            var delete = _context.sys_group_user_roles.Where(t => t.id_group_user == model.db.id);
            _context.RemoveRange(delete);
            _context.SaveChanges();
            model.list_role.ForEach(t =>
            {
                t.db.id = 0;
                t.db.id_group_user = model.db.id;
            });
            var listInsert = model.list_role.Select(d => d.db).ToList();
            _context.sys_group_user_roles.AddRange(listInsert);
            _context.SaveChanges();


        }
        public IQueryable<sys_group_user_model> FindAll()
        {
            var result = _context.sys_group_users.Select(d=> new sys_group_user_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.FirstName+ " "+d.LastName).SingleOrDefault(),
                
                db = d,
            });
         
            return result;
        }
        public IQueryable<sys_group_user_role_model> FindAllRole(string id)
        {
            var result = _context.sys_group_user_roles
                .Where(t => t.id_group_user == id)
                .Select(d => new sys_group_user_role_model()
                {
                    db = d,
                });

            return result;
        }
        public IQueryable<sys_group_user_detail_model> FindAllItem(string id)
        {
            var result = _context.users
                 .Where(d => d.status_del == 1 )
                 .OrderBy(d=>d.id_department)
                .Select(d => new sys_group_user_detail_model()
                {
                    user_name = d.Username,
                    department_name = _context.sys_departments.Where(t => t.id == d.id_department).Select(t => t.name).SingleOrDefault(),
                    position_name = _context.sys_job_titles.Where(t => t.id == d.id_job_title).Select(t => t.name).SingleOrDefault(),
                    user_id = d.Id,
                    isCheck = _context.sys_group_user_details.Where(t => t.user_id == d.Id && t.id_group_user == id).Count() > 0,
                    type_user =d.type
                });

            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_group_users.Where(x => x.id ==id).SingleOrDefault(); ;
            itemToRemove.status_del =2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_group_users.Where(x => x.id == id).SingleOrDefault(); ;
            itemToRemove.status_del =1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

        

    }
}
