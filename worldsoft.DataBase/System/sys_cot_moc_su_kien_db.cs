using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_cot_moc_su_kien")]
    public class sys_cot_moc_su_kien_db
    {
       
        public string id { get; set; }
        public string name { get; set; }

     
        public string time { get; set; }
        public string note { get; set; }
        public string note_mobile { get; set; }
        public int? stt { get; set; }
        
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }

    }
   
}
