using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_dat_hang_chi_tiet")]
    public class sys_dat_hang_chi_tiet_db
    {
        public string id { get; set; }
        public string khuyen_mai { get; set; }
        public string id_don_hang { get; set; }
        public string id_san_pham { get; set; }
        public string ten_san_pham { get; set; }
        public string ma_san_pham { get; set; }
        public string mo_ta { get; set; }
        public decimal? don_gia { get; set; }
        public decimal? so_tien { get; set; }
        public int? so_luong { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    }
}

