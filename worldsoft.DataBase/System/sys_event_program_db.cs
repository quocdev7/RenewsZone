using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_program")]
    public class sys_event_program_db
    {
      
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string presenter { get; set; }
        public int? max_person_participate { get; set; }
        public string location { get; set; }
        public int? stt { get; set; }
        public int? status_del { get; set; }
        public string event_id { get; set; }

        public string update_by { get; set; }
        public DateTime? update_date { get; set; }

     

        public string id_dien_gia { get; set; }

    
    }
}
