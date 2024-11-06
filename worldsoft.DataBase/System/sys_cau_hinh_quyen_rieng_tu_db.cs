using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_cau_hinh_quyen_rieng_tu")]
    public class sys_cau_hinh_quyen_rieng_tu_db
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public int? setting_phone { get; set; }
        public int? setting_email { get; set; }
        public int? setting_ngay_sinh { get; set; }
        public int? setting_dia_chi { get; set; }
        public int? setting_trang_thai { get; set; }
        public int? setting_chuc_danh { get; set; }
        public int? setting_cong_ty { get; set; }

        public int? setting_hoc_van { get; set; }
        public int? setting_thanh_tuu { get; set; }
        public int? setting_bang_cap { get; set; }
        public int? setting_kinh_nghiem { get; set; }
        public int? setting_ky_nang { get; set; }
        public int? setting_mang_xa_hoi { get; set; }
        public int? setting_gioi_tinh { get; set; }
        public int? setting_khoa { get; set; }
        public int? setting_nien_khoa { get; set; }
   
    }
}
