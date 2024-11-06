using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_cuu_sinh_vien")]
    public class sys_cuu_sinh_vien_db
    {
       
        public string id { get; set; }
        public string nien_khoa { get; set; }
        
        public string name { get; set; }
        public string name_company { get; set; }
        public string id_nhom_hoi_dong { get; set; }
        public bool? hien_thi_trang_chu { get; set; }
        public string chuc_danh_hoi_dong { get; set; }
        public string image { get; set; }
        public string update_by { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public int? stt { get; set; }
    }
   
}
