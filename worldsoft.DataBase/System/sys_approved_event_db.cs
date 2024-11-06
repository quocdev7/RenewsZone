using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event")]
    public class sys_approved_event_db
    {
        public string id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }

        public DateTime? time_start { get; set; }
        public DateTime? time_end { get; set; }
        public DateTime? ngay_den_han_dang_ky { get; set; }

        public string location { get; set; }
        public int? max_person_participate { get; set; }
        public string type { get; set; }
        public string id_tiente { get; set; }
      
        public string hinh_thuc_user { get; set; }
        public int? status_del { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string update_by { get; set; }
        public string mo_ta { get; set; }
        public string logo { get; set; }
        public string id_template_invite { get; set; }
        public string id_template_thanks { get; set; }
        public int? is_register_event { get; set; }
        public string id_khoa { get; set; }
        public string ban_to_chuc { get; set; }
        public int? quyen_rieng_tu { get; set; }
        public decimal? so_tien { get; set; }
        public string luu_y_tham_gia { get; set; }
        public string li_do { get; set; }



    }
}
