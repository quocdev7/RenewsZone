using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_work_history_user")]
    public class sys_work_history_user_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string description { get; set; }
        public string user_id { get; set; }
        public string image { get; set; }


        public string position { get; set; }
        public DateTime? from_date { get; set; }

        public DateTime? to_date { get; set; }
        public string company { get; set; }


        public string update_by { get; set; }

        public DateTime? update_date { get; set; }
    
    }
   
}
