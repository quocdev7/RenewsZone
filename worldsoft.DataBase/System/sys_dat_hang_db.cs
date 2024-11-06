using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_dat_hang")]
    public class sys_dat_hang_db
    {
        public string id { get; set; }
        public int? gioi_tinh { get; set; }
        public decimal? thanh_tien { get; set; }
        public decimal? thanh_tien_van_chuyen { get; set; }
        public decimal? thanh_tien_thu_ho { get; set; }
        public decimal? thanh_tien_giam_gia { get; set; }
        public string ma_giam_gia { get; set; }
        public string ma_don_hang { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public long? tinh_thanh { get; set; }
        public long? quan_huyen { get; set; }
        public string dia_chi { get; set; }
        public string ghi_chu { get; set; }
        public int? phuong_thuc_thanh_toan { get; set; }
        public string ten_cong_ty { get; set; }
        public long? tinh_thanh_cong_ty { get; set; }
        public long? quan_huyen_cong_ty { get; set; }
        public string dia_chi_cong_ty { get; set; }
        public string ma_so_thue { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    }
}

