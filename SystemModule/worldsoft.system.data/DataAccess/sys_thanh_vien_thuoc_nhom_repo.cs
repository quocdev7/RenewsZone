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
    public class sys_thanh_vien_thuoc_nhom_repo 
    {
        public worldsoftDefautContext _context;

        public sys_thanh_vien_thuoc_nhom_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_thanh_vien_thuoc_nhom_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_thanh_vien_thuoc_nhom_model model)
        {
         
            await _context.sys_thanh_vien_thuoc_nhoms.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }

        public async Task<int> update(sys_thanh_vien_thuoc_nhom_model model)
        {
           var db= await _context.sys_thanh_vien_thuoc_nhoms.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
            db.id_nhom = model.db.id_nhom;
            db.user_id = model.db.user_id;
        
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_thanh_vien_thuoc_nhom_model> FindAll()
        {
            var result = _context.sys_thanh_vien_thuoc_nhoms.Select(d=> new sys_thanh_vien_thuoc_nhom_model()
            {
                createby_name =  _context.users.Where(t=>t.Id ==  d.create_by).Select(d=>d.full_name).SingleOrDefault(),
                ten_nhom = _context.sys_nhom_thanh_viens.Where(t => t.id == d.id_nhom).Select(d => d.name).SingleOrDefault(),

                dien_thoai = _context.users.Where(q => q.Id == d.user_id).Select(q => q.phone).SingleOrDefault(),
                email = _context.users.Where(q => q.Id == d.user_id).Select(q => q.email).SingleOrDefault(),

                user_name = _context.users.Where(q => q.Id == d.user_id).Select(q => q.full_name).SingleOrDefault(),
                position = _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_job_title).SingleOrDefault(),
                avatar_link = _context.users.Where(q => q.Id == d.user_id).Select(q => q.avatar_path).SingleOrDefault(),
                ten_cong_ty =
                    _context.sys_companys.Where(q => q.id == _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_company).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                school_year = _context.users.Where(q => q.Id == d.user_id).Select(q => q.school_year).SingleOrDefault(),
                faculty = _context.sys_khoas.Where(q => q.id == _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_khoa).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),

                //ten_quoc_gia = _context.sys_countrys.Where(q => q.id == _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_country).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_thanh_vien_thuoc_nhoms.Where(x => x.id ==id).FirstOrDefault();

            _context.sys_thanh_vien_thuoc_nhoms.Remove(itemToRemove);
            _context.SaveChanges();

            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_thanh_vien_thuoc_nhoms.Where(x => x.id == id).FirstOrDefault();
    
            _context.SaveChanges();
            return 1;
        }
        
    }
}
