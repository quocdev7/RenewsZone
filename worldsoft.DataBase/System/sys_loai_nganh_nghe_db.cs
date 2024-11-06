using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_loai_nganh_nghe")]
    public class sys_loai_nganh_nghe_db
    {
        public string id { get; set; }
        public string name { get; set; }
        public string note { get; set; }
        public string image { get; set; }
        public int? status_del { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string create_by { get; set; }
        public string update_by { get; set; }
        public string id_nhom { get; set; }
        public int? stt { get; set; }
    }
}
