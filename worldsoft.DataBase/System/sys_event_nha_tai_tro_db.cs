using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{

    [Table("sys_event_nha_tai_tro")]
    public class sys_event_nha_tai_tro_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string name { get; set; }
        public int? stt { get; set; }
        public string note { get; set; }
        public string dien_thoai { get; set; }
        public decimal? so_tien { get; set; }
        public string id_tien_te { get; set; }
        public string id_su_kien { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }

        public  string logo { get; set; }
        public string ma_so_thue { get; set; }
        public bool? is_tai_tro { get; set; }
        
    }
}
