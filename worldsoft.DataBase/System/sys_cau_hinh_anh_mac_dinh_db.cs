using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_cau_hinh_anh_mac_dinh")]
    public class sys_cau_hinh_anh_mac_dinh_db
    {
       
        public string id { get; set; }
        public string image { get; set; }
        
        public string avatar { get; set; }

        public int? type { get; set; }
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    
    }
   
}
