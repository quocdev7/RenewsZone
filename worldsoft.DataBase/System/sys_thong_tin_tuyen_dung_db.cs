using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_thong_tin_tuyen_dung")]
    public class sys_thong_tin_tuyen_dung_db
    {
        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }

        public string company_id { get; set; }
        public string title { get; set; }

        public string noi_dung_trang_bia { get; set; }

        public bool? is_hot_job { get; set; }
        public string id_nhom_nganh { get; set; }

        public string id_loai_nganh { get; set; }
        public string id_vi_tri_tuyen_dung { get; set; }
        public string id_khu_vuc { get; set; }

        public string muc_luong { get; set; }
        public DateTime? ngay_dang_tuyen { get; set; }

        public string dia_diem_lam_viec { get; set; }
        public string phuc_loi { get; set; }
        public string mo_ta_cong_viec { get; set; }
        public string yeu_cau_cong_viec { get; set; }

        public string note { get; set; }
        public int? status_del { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string create_by { get; set; }
        public string update_by { get; set; }
        public int? stt { get; set; }
    }
}
