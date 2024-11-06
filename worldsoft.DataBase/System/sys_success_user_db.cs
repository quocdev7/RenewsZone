using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_success_user")]
    public class sys_success_user_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string description { get; set; }
        public string user_id { get; set; }
        public string image { get; set; }
        public string field_name { get; set; }
        public string url { get; set; }
        public DateTime? update_date { get; set; }
        public string update_by { get; set; }
        public string noi_cap { get; set; }

        
    }
   
}
