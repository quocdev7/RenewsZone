using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_nhom_thanh_vien")]
    public class sys_nhom_thanh_vien_db
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string note { get; set; }
   

        public int? status_del { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string update_by { get; set; }


    }
}
