using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_quan_huyen")]
    public class sys_quan_huyen_db
    {
        public long id { get; set; }
        public long? id_quoc_gia { get; set; }
        public long? id_tinh { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }
        public string note { get; set; }
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public string ten_khong_dau { get; set; }
    }
}
