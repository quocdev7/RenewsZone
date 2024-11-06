using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_user_typenews")]
    public class sys_user_typenews_db
    {
        public string id_user { get; set; }
        public string id { get; set; }
        public string id_type_news { get; set; }
        public string id_khoa { get; set; }
        public string note { get; set; }
    
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }

    }

}
