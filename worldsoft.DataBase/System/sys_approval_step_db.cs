using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_approval_step")]
    public class sys_approval_step_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string id_approval_config { get; set; }
        public string id_approval { get; set; }
        public string user_id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public decimal? duration_hours { get; set; }
        public int? step_num { get; set; }
        public int? status { get; set; }
    }

}
