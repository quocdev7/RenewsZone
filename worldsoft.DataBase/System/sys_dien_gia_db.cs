using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_dien_gia")]
    public class sys_dien_gia_db
    {
       
        public string id { get; set; }
        public string name { get; set; }
        public string chuc_danh { get; set; }
        public string id_su_kien { get; set; }
        public string cong_ty { get; set; }
        public string image { get; set; }
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public int? stt { get; set; }

    }
   
}
