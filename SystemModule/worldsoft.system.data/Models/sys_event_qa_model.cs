using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_qa_model
    {
        public sys_event_qa_model()
        {
            db = new sys_event_qa_db();
        }
        public sys_event_qa_db db { get; set; }

        public string user_answer { get; set; }
        public string user_question { get; set; }
        public string ten_su_kien { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string question_en { get; set; }
        public string answer_en { get; set; }
        public string create_by_name { get; set; }
    }
}
