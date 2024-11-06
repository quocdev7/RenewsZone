using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_approval_config")]
    public class sys_approval_config_db
    {
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public int? step { get; set; }
        public int? status_del { get; set; }
        public string menu { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }

    }
}
