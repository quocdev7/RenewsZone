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
    public class sys_event_contact_repo
    {
        public worldsoftDefautContext _context;

        public sys_event_contact_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_event_contact_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public IQueryable<sys_event_contact_model> FindAllItem(string id)
        {
            var result = _context.sys_event_participates
                .Where(t => t.event_id == id)
                .Select(d => new sys_event_contact_model()
                {
                    user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.FirstName +" " +d.LastName).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public async Task<int> insert(sys_event_contact_model model, string user_create)
        {


            await _context.sys_event_participates.AddAsync(model.db);
            _context.SaveChanges();

            saveEvenEmail(model, user_create);

            return 1;
        }

        public async Task<int> saveEvenEmail(sys_event_contact_model model_nguoi_tham_du, string user_create)
        {
            var email = _context.users.Where(q => q.Id == model_nguoi_tham_du.db.user_id).Select(q => q.email).SingleOrDefault();
            var model = new sys_event_email_model();
            var id_template = _context.sys_events.Where(q => q.id == model_nguoi_tham_du.db.event_id).Select(q => q.id_template_invite).SingleOrDefault();
            var template_mail = _context.sys_template_mails.Where(q => q.id == id_template).SingleOrDefault();
            var id_type_email = template_mail.id_type;
            var title = template_mail.name;
            var content = template_mail.name;


            model.db.id = DateTime.Now.Ticks + "";
            model.db.event_id = model_nguoi_tham_du.db.event_id;
            model.db.title = title;
            model.db.content = content;
            model.db.id_template = id_template;
            model.db.id_type_email = id_type_email;

            model.db.create_date = DateTime.Now;
            model.db.create_by = user_create;

            model.db.send_by_user_id = user_create;
            //ten nguoi dung
            model.db.to_user_id = model_nguoi_tham_du.db.user_id;

            model.db.mailto = email;
            model.db.send_time = null;
            model.db.send_status = 0;


            await _context.sys_event_emails.AddAsync(model.db);
            _context.SaveChanges();



            return 1;
        }

        public async Task<int> update(sys_event_contact_model model)
        {
            var db = await _context.sys_event_participates.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            //db.title = model.db.title;
            //db.intro = model.db.intro;
            //db.logo = model.db.logo;
            //db.location = model.db.location;
            //db.update_date = model.db.update_date;
            //db.max_person_participate = model.db.max_person_participate;
            //db.update_by = model.db.update_by;
            //db.type = model.db.type;
            //db.mo_ta = model.db.mo_ta;
            _context.SaveChanges();
            //saveDetail(model);
            return 1;
        }


        public IQueryable<sys_event_contact_model> FindAll()
        {
            var result = _context.sys_event_participates.Select(d => new sys_event_contact_model()
            {
                //ten_nguoi_lap = _context.users.Where(t => t.Id == d.create_by).Select(d => d.FullName).SingleOrDefault(),
                //ten_hinh_thuc = d.type == '1' ? "Offline" : "Online",
                db = d,
                user_name = _context.users.Where(q => q.Id == d.user_id).Select(q => q.FirstName +" " +q.LastName).SingleOrDefault(),
                position = _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_job_title).SingleOrDefault(),
                avatar_link = _context.users.Where(q => q.Id == d.user_id).Select(q => q.avatar_path).SingleOrDefault(),
                ten_cong_ty =
                    _context.sys_companys.Where(q => q.id == _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_company).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                school_year = _context.users.Where(q => q.Id == d.user_id).Select(q => q.school_year ).SingleOrDefault(),
                faculty = _context.sys_khoas.Where(q => q.id == _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_khoa).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                role_view = d.role,
                //ten_quoc_gia = _context.sys_countrys.Where(q => q.id == _context.users.Where(q => q.Id == d.user_id).Select(q => q.id_country).SingleOrDefault()).Select(q => q.name).SingleOrDefault(),
                ten_su_kien = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.title).SingleOrDefault(),
            });

            return result;
        }

        public int delete(string id, string user_id)
        {
            var itemToRemove = _context.sys_event_participates.Where(x => x.id == id && x.check_in_status == 1).SingleOrDefault();

            _context.RemoveRange(itemToRemove);
            _context.SaveChanges();
            return 1;
        }
        public int sudung(string id, string user_id)
        {
            var itemToRemove = _context.sys_event_participates.Where(x => x.id == id).SingleOrDefault();
            //itemToRemove.status_del = 1;


            _context.SaveChanges();
            return 1;
        }

    }
}
