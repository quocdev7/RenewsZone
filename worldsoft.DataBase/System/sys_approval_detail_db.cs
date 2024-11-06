using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_approval_detail")]
    public class sys_approval_detail_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public int? status_finish { get; set; }
        public string to_user { get; set; }
        public string from_user { get; set; }
        public int? step_num { get; set; }
        public string step_name { get; set; }
        public string id_approval_config { get; set; }
        public string note { get; set; }
        public DateTime? date_action { get; set; }
        public string user_action { get; set; }
        public DateTime? deadline { get; set; }
        public string id_approval { get; set; }
        public int? status_action { get; set; }

    }
}
