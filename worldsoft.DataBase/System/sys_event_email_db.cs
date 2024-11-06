using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_email")]
    public class sys_event_email_db
    {
        public string id { get; set; }
        public string event_id { get; set; }
        public string title { get; set; }
        public string mailto { get; set; }
        public string ccto { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }


        // 0 la chuan bi gui, 1 thanh cong, 2 that bai
        public int? send_status { get; set; }

        public DateTime? send_time { get; set; }
        public string id_template { get; set; }
        public string id_type_email { get; set; }
        public string send_by_user_id { get; set; }
        public string content { get; set; }
        public int? count_false { get; set; }
        public string to_user_id { get; set; }
        


    }
}
