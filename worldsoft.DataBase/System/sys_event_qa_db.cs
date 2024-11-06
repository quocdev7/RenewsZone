using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_Q_A")]
    public class sys_event_qa_db
    {
      
        public string id { get; set; }
        public string event_id { get; set; }
        public string question { get; set; }
        public DateTime? time_question { get; set; }
        public string user_id_question { get; set; }
        public string answer { get; set; }
        public DateTime? time_answer { get; set; }
        public string user_id_answer { get; set; }

        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }

    }
}
