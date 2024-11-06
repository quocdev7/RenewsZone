using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_approval")]
    public class sys_approval_db
    {
        public string id { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string create_by_record { get; set; }
        public DateTime? create_date_record { get; set; }
        public string start_by { get; set; }
        public DateTime? start_date { get; set; }

        public string id_sys_approval_config { get; set; }
        // 1 start, 2 in process, 3 finish, 4 cancel,5 return
        public int? status_finish { get; set; }
        public string to_user { get; set; }
        public string from_user { get; set; }
        public int? step_num { get; set; }
        public string last_note { get; set; }
        public DateTime? last_date_action { get; set; }
        public string last_user_action { get; set; }
        public int? total_step { get; set; }
        public string menu { get; set; }
        public string id_record { get; set; }
        public DateTime? deadline { get; set; }
        public int? status_action { get; set; }
        public string step_name { get; set; }
    }
}
