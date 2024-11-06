using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_nhom_hoi_dong")]
    public class sys_nhom_hoi_dong_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }

        public string noi_dung { get; set; }
        public string noi_dung_mobile { get; set; }
        public int? stt { get; set; }

        public string name_en { get; set; }
        public string noi_dung_en { get; set; }
        public string noi_dung_mobile_en { get; set; }



    }
   
}
