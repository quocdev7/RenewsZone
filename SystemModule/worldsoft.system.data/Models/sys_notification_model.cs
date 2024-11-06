using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_notification_model
    {
        public sys_notification_model()
        {
            db = new sys_notification_db();
            
        }
        public sys_notification_db db { get; set; }

    }
    public class sys_notification_ref_model
    {
        public string ly_do_khong_duyet_profile { get; set; }
        public string ho_va_ten_nguoi_gui_loi_moi_ket_ban { get; set; }
        public string ho_va_ten_nguoi_nhan_loi_moi_ket_ban { get; set; }
        public string ten_tin_tuc { get; set; }
        public string ly_do_khong_duyet_tin { get; set; }
        public string ten_khoa { get; set; }
        public string ten_loai_tin_tuc { get; set; }
        public string ly_do_xoa_binh_luan { get; set; }
        public string ho_va_ten_nguoi_gui_loi_moi_tham_gia_su_kien { get; set; }
        public string ten_su_kien { get; set; }
        public string ly_do_khong_duyet_dang_ky_tham_gia { get; set; }
        public string ho_va_ten_nguoi_nhan_loi_moi_tham_gia_su_kien { get; set; }
        public string ho_va_ten_nguoi_tham_gia_su_kien { get; set; }
        public string ly_do_huy_bo_su_kien { get; set; }

    }
}
