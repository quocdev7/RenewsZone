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
    public class sys_event_program_repo 
    {
        public worldsoftDefautContext _context;

        public sys_event_program_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_event_program_model> getElementById(string id)
        {
            var obj= await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        
        public async Task<int> insert(sys_event_program_model model)
        {

            model.db.update_by = model.db.create_by;
            model.db.update_date = model.db.create_date;
            model.time_start = model.time_start;
            model.time_end = model.time_end;
            model.db.end_time = model.db.end_time;
            model.db.start_time = model.db.start_time;
            await _context.sys_event_programs.AddAsync(model.db);
            _context.SaveChanges();
            return 1;
        }
        public async Task<int> insert_language(sys_event_program_en_model model_language)
        {
            await _context.sys_event_program_ens.AddAsync(model_language.db);
            _context.SaveChanges();

            return 1;
        }
        public async Task<int> update(sys_event_program_model model)
        {
           var db= await _context.sys_event_programs.Where(d=>d.id ==  model.db.id).FirstOrDefaultAsync();
          
          
            db.update_date = model.db.update_date;
            db.status_del = model.db.status_del;

            db.name = model.db.name;
            db.description = model.db.description;
            db.start_time = model.db.start_time;
            db.end_time = model.db.end_time;
            db.update_date = model.db.update_date;
            db.max_person_participate = model.db.max_person_participate;
            db.update_by = model.db.update_by;
            db.presenter = model.db.presenter;
            db.location = model.db.location;

            db.stt = model.db.stt;

            db.id_dien_gia = model.db.id_dien_gia;
     
            _context.SaveChanges();
            return 1;
        }
        public IQueryable<sys_event_ref_model> FindAllSuKienThamGia()
        {
            var result = _context.sys_events.Select(d => new sys_event_ref_model()
            {
                id_su_kien = d.id,
                ten_su_kien = d.title,
                time_start = d.time_start,
                time_end = d.time_end,
                status_del = d.status_del
            });

            return result;
        }
        public IQueryable<sys_event_program_model> FindAllEventProgram()
        {


            var result = _context.sys_event_programs.Select(d => new sys_event_program_model()
            {

                db = d,
                ten_dien_gia = _context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.name).SingleOrDefault(),
                ten_su_kien = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.title).SingleOrDefault(),
                time_end = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.time_end).SingleOrDefault(),
                time_start = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.time_start).SingleOrDefault(),

            });

            return result;
        }
        public IQueryable<sys_event_program_model> FindAllItem(string id)
        {
            var result = _context.sys_event_programs
                .Where(t => t.event_id == id)
                .Select(d => new sys_event_program_model()
                {
                    time_end = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.time_end).SingleOrDefault(),
                    time_start = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.time_start).SingleOrDefault(),
                    //user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.full_name).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public IQueryable<sys_event_program_model> FindAll()
        {
            var result = _context.sys_event_programs.Select(d=> new sys_event_program_model()
            {
                ten_dien_gia = _context.sys_dien_gias.Where(q => q.id == d.id_dien_gia).Select(q => q.name).SingleOrDefault(),
                ten_su_kien = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.title).SingleOrDefault(),
                time_end = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.time_end).SingleOrDefault(),
                time_start = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.time_start).SingleOrDefault(),
                db = d,
            });
         
            return result;
        }
        public int delete(string id,string userid)
        {
            var itemToRemove =  _context.sys_event_programs.Where(x => x.id ==id).FirstOrDefault();
            itemToRemove.status_del = 2;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        public int revert(string id, string userid)
        {
            var itemToRemove = _context.sys_event_programs.Where(x => x.id == id).FirstOrDefault();
            itemToRemove.status_del = 1;
            itemToRemove.update_by = userid;
            itemToRemove.update_date = DateTime.Now;
            _context.SaveChanges();
            return 1;
        }
        
    }
}
