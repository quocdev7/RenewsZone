using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using worldsoft.system.data.Models;
using worldsoft.DataBase.Provider;
using System.Text.RegularExpressions;
using worldsoft.DataBase.Helper;
using System.Web;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.DataAccess
{
    public class person_event_repo
    {
        public worldsoftDefautContext _context;

        public person_event_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<person_event_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            obj.hinhthuc = (obj.db.hinh_thuc_user ?? "").Split(",").ToList();
            obj.khoa = (obj.db.id_khoa ?? "").Split(",").ToList();
            obj.types = (obj.db.type ?? "").Split(",").ToList();
            return obj;
        }

        public async Task<int> insert(person_event_model model)
        {
            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            model.db.type = string.Join(",", model.types);
            model.db.hinh_thuc_user = string.Join(",", model.hinhthuc);
            model.db.id_khoa = string.Join(",", model.khoa);
            model.db.ngay_den_han_dang_ky = model.db.ngay_den_han_dang_ky ?? model.db.time_start;
            await _context.sys_events.AddAsync(model.db);
            _context.SaveChanges();
            insert_search(model.db);
            return 1;
        }

        public async Task<int> update(person_event_model model)
        {
            var db = await _context.sys_events.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.is_register_event = model.db.is_register_event;
            db.quyen_rieng_tu = model.db.quyen_rieng_tu;
            db.title = model.db.title;
            db.ban_to_chuc = model.db.ban_to_chuc;
            db.intro = model.db.intro;
            db.logo = model.db.logo;
            db.ngay_den_han_dang_ky = model.db.ngay_den_han_dang_ky ?? model.db.time_start;
            db.location = model.db.location;
            db.luu_y_tham_gia = model.db.luu_y_tham_gia;
            db.max_person_participate = model.db.max_person_participate;
            db.update_by = model.db.update_by;
            db.id_tiente = model.db.id_tiente;
            db.mo_ta = model.db.mo_ta;
            db.id_template_invite = model.db.id_template_invite;
            db.id_template_thanks = model.db.id_template_thanks;
            db.time_end = model.db.time_end;
            db.time_start = model.db.time_start;
            db.type = string.Join(",", model.types);
            db.hinh_thuc_user = string.Join(",", model.hinhthuc);
            db.id_khoa = string.Join(",", model.khoa);
            db.so_tien = model.db.so_tien;
            db.update_by = model.db.update_by;
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;
            _context.SaveChanges();
            insert_search(db);
            return 1;
        }

        public void insert_search(sys_event_db model)
        {
            string t = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.title ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            string c = Regex.Replace(StringFunctions.NonUnicode(HttpUtility.HtmlDecode(model.mo_ta ?? "")).ToLower().Normalize(), "<.*?>|&.*?;", String.Empty);
            var db = _context.sys_searchs.Where(d => d.id_ref == model.id && d.type == 2).FirstOrDefault();
            if (db != null)
            {
                db.create_date = DateTime.Now;
                db.order_date = model.time_start;
                db.search_text = t + " " + c;
                _context.SaveChanges();
            }
            else
            {
                var db1 = new sys_search_db()
                {
                    create_date = DateTime.Now,
                    order_date = model.time_start,
                    id = 0,
                    id_ref = model.id,
                    search_text = t + " " + c,
                    type = 2,
                };

                _context.Add(db1);
                _context.SaveChanges();

            }

        }



        public IQueryable<person_event_model> FindAll()
        {
            var result = _context.sys_events.Select(d => new person_event_model()
            {
                ten_khoa = _context.sys_khoas.Where(t => t.id == d.id_khoa).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                ten_tien_te = _context.sys_tien_tes.Where(t => t.id == d.id_tiente).Select(d => d.name).SingleOrDefault(),
                db = d,
            });

            return result;
        }
        public IQueryable<person_event_model> FindAllNewPortal(string user_id)
        {
            var result = _context.sys_events.Select(d => new person_event_model()
            {
                ten_khoa = _context.sys_khoas.Where(t => t.id == d.id_khoa).Select(d => d.name).SingleOrDefault(),
                createby_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
                updateby_name = _context.users.Where(t => t.Id == d.update_by).Select(d => d.full_name).SingleOrDefault(),
                ten_tien_te = _context.sys_tien_tes.Where(t => t.id == d.id_tiente).Select(d => d.name).SingleOrDefault(),
                db = d,

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
            var itemToRemove = _context.sys_events.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_events.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }

        public int approval(string id, string userid)
        {
            var itemToRemove = _context.sys_events.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;

        }
        public int delete(string id, string userid, string reason)
        {
            var itemToRemove = _context.sys_events.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            itemToRemove.li_do = reason;
            _context.SaveChanges();
            return 1;
        }

    }
}
