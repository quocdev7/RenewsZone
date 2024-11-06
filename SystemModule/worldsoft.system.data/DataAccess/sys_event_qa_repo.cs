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
    public class sys_event_qa_repo
    {
        public worldsoftDefautContext _context;

        public sys_event_qa_repo(worldsoftDefautContext context)
        {
            _context = context;
        }

        public async Task<sys_event_qa_model> getElementById(string id)
        {
            var obj = await FindAll().FirstOrDefaultAsync(m => m.db.id == id);
            return obj;
        }
        public IQueryable<sys_event_qa_model> FindAllItem(string id)
        {
            var result = _context.sys_event_qas
                .Where(t => t.event_id == id)
                .Select(d => new sys_event_qa_model()
                {
                    //user_name = _context.users.Where(t => t.Id == d.user_id).Select(d => d.full_name).SingleOrDefault(),
                    db = d,
                });

            return result;
        }
        public async Task<int> insert(sys_event_qa_model model)
        {

            await _context.sys_event_qas.AddAsync(model.db);
            _context.SaveChanges();
            //saveDetail(model);


            return 1;
        }

        public async Task<int> insert_language(sys_event_qa_en_model model_language)
        {
            await _context.sys_event_qa_ens.AddAsync(model_language.db);
            _context.SaveChanges();

            return 1;
        }

        public async Task<int> update(sys_event_qa_model model)
        {
            var db = await _context.sys_event_qas.Where(d => d.id == model.db.id).FirstOrDefaultAsync();
            db.question = model.db.question;
            db.update_by = model.db.update_by;
            db.update_date = DateTime.Now;
            db.answer = model.db.answer;

          
            _context.SaveChanges();
            //saveDetail(model);
            return 1;
        }


        public IQueryable<sys_event_qa_model> FindAll()
        {
            var result = _context.sys_event_qas.Select(d => new sys_event_qa_model()
            {

                db = d,
                ten_su_kien = _context.sys_events.Where(q => q.id == d.event_id).Select(q => q.title).SingleOrDefault(),
                user_answer = _context.users.Where(t => t.Id == d.user_id_answer).Select(d => d.FirstName +" " +d.LastName).SingleOrDefault(),
                user_question = _context.users.Where(t => t.Id == d.user_id_question).Select(d => d.full_name).SingleOrDefault(),
                create_by_name = _context.users.Where(t => t.Id == d.create_by).Select(d => d.full_name).SingleOrDefault(),
            });

            return result;
        }

        public int delete(string id, string user_id)
        {
            var itemToRemove = _context.sys_event_qas.Where(x => x.id == id).SingleOrDefault();

            _context.RemoveRange(itemToRemove);

            _context.SaveChanges();
            return 1;



        }
        public int sudung(string id, string user_id)
        {
            var itemToRemove = _context.sys_event_qas.Where(x => x.id == id).SingleOrDefault();
            //itemToRemove.status_del = 1;


            _context.SaveChanges();
            return 1;
        }

    }
}
