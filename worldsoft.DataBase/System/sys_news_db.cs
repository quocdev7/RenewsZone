using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_news")]
    public class sys_news_db
    {
        public string id { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung_trang_bia { get; set; }
        public string noi_dung { get; set; }
        public string image { get; set; }
        public string id_group_news { get; set; }
        public string id_type_news { get; set; }
        public string id_tiente { get; set; }
        public string nguon_tin_tuc { get; set; }
        public int? status_del { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public string create_by { get; set; }
        public string update_by { get; set; }
        public DateTime? ngay_dang { get; set; }
 
        public long? view_count { get; set; }
        public long? comment_count { get; set; }
        public string id_user_approval { get; set; }
        public DateTime? approval_date { get; set; }
        public string reason_return { get; set; }
        public bool? is_hot { get; set; }
        public bool? is_comment { get; set; }
        public string id_khoa { get; set; }
        public string hinh_thuc_user { get; set; }
        public int? stt { get; set; }
        //1.Công khai, 2.Thành viên, 3.Bạn bè, 4.Khoa, 5.Trả phí
        public int? quyen_rieng_tu { get; set; }
        public DateTime? ngay_ngung_dang { get; set; }
        public string nguoi_ngung_dang { get; set; }
         public int? vi_tri_tin_noi_bat { get; set; }

        public decimal? so_tien { get; set; }
        public string noi_dung_mobile { get; set; }
        

    }
}
