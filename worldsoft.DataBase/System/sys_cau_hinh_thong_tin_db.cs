using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_cau_hinh_thong_tin")]
    public class sys_cau_hinh_thong_tin_db
    {
        public string id { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung { get; set; }
        public string noi_dung_mobile { get; set; }
        public int? status_del { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string create_by { get; set; }
        public string update_by { get; set; }
        public DateTime? ngay_dang { get; set; }
        public int? status { get; set; }
        public string id_loai { get; set; }
        public string tieu_de_en { get; set; }
        public string noi_dung_en { get; set; }
        public string noi_dung_mobile_en { get; set; }
        public int? stt { get; set; }

    }
}
