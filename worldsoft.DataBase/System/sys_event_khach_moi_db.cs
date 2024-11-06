using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_khach_moi")]
    public class sys_event_khach_moi_db
    {
       
        public string id { get; set; }
        public string name { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string position { get; set; }
        public string avatar_path { get; set; }
        public string company { get; set; }
        public string ly_do { get; set; }
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public string id_su_kien { get; set; }

        // 1 Mời tham gia sự kiện, 2 Từ chối tham gia sư kiện, 3 Sẽ tham gia sự kiện, 4 Đã tham gia sự kiện, 5 Không đủ điều kiện tham dựn, 6 Đăng ký tham gia sự kiện

        public int? check_in_status { get; set; }
        public DateTime? check_in_date { get; set; }
    }
   
}
