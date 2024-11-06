using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using worldsoft.DataBase.Provider;
using worldsoft.DataBase.System;
using worldsoft.system.data.Models;

namespace worldsoft.system.data.DataAccess
{
    public class sys_dat_hang_repo
    {
        public worldsoftDefautContext _context;

        public sys_dat_hang_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_dat_hang_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }

        public async Task<int> insert(sys_dat_hang_model model)
        {
            model.db.thanh_tien = model.list_product_card.Sum(q => q.so_luong * q.db.so_tien);
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            await _context.sys_dat_hangs.AddAsync(model.db);
            _context.SaveChanges();
            await insert_detail(model);
            return 1;
        }

        public async Task<int> insert_detail(sys_dat_hang_model model)
        {

            foreach (var item in model.list_product_card)
            {
                sys_dat_hang_chi_tiet_db detail = new sys_dat_hang_chi_tiet_db();


                detail.id = Guid.NewGuid().ToString();
                detail.id_don_hang = model.db.id;
                detail.id_san_pham = item.db.id;
                detail.khuyen_mai = item.khuyen_mai;
                detail.ten_san_pham = item.db.ten_san_pham;
                detail.ma_san_pham = item.db.ma_san_pham;
                detail.mo_ta = item.db.mo_ta;
                detail.don_gia = item.db.so_tien;
                detail.so_tien = item.db.so_tien * item.so_luong;
                detail.so_luong = item.so_luong;

                detail.update_by = model.db.create_by;
                detail.update_date = model.db.create_date;
                detail.create_by = model.db.create_by;
                detail.create_date = model.db.create_date;
                detail.status_del = 1;
                await _context.sys_dat_hang_chi_tiets.AddAsync(detail);
                _context.SaveChanges();
            }
            return 1;
        }
        public async Task<int> update(sys_dat_hang_model model)
        {
            var db = await _context.sys_dat_hangs.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.full_name = model.db.full_name;
            db.email = model.db.email;
            db.phone = model.db.phone;
            db.update_by = model.db.update_by;
            model.db.thanh_tien = model.list_product_card.Sum(q => q.so_luong * q.db.so_tien);
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            _context.SaveChanges();
            return 1;
        }

        public IQueryable<sys_dat_hang_model> FindAll()
        {
            var result = _context.sys_dat_hangs.Select(d => new sys_dat_hang_model()
            {
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FirstName + " " + d.LastName).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                tinh_thanh_user = _context.sys_tinh_thanhs.Where(t => t.id == d.tinh_thanh).Select(d => d.ten).SingleOrDefault(),
                quan_huyen_user = _context.sys_quan_huyens.Where(t => t.id == d.quan_huyen).Select(d => d.ten).SingleOrDefault(),
                tinh_thanh_cong_ty = _context.sys_tinh_thanhs.Where(t => t.id == d.tinh_thanh_cong_ty).Select(d => d.ten).SingleOrDefault(),
                quan_huyen_cong_ty = _context.sys_quan_huyens.Where(t => t.id == d.quan_huyen_cong_ty).Select(d => d.ten).SingleOrDefault(),
                db = d,
                list_detail = _context.sys_dat_hang_chi_tiets.Where(q => q.id_don_hang == d.id).ToList(),
            });

            return result;
        }
        public int delete(string id, string userid)
        {
            var itemToRemove = _context.sys_dat_hangs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_dat_hangs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

    }
}
