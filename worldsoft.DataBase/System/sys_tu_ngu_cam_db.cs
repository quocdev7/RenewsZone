using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_tu_ngu_cam")]
    public class sys_tu_ngu_cam_db
    {
       
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    }
   
}
