using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_san_pham")]
    public class sys_san_pham_db
    {

        public string id { get; set; }
        public string id_khuyen_mai { get; set; }
        public string ten_san_pham { get; set; }
        public string ma_san_pham { get; set; }
        public string hinh_anh { get; set; }
        public string hinh_anh_mobile { get; set; }
        public string id_loai { get; set; }
        public decimal? so_tien { get; set; }
        public string create_by { get; set; }
        public string update_by { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public int? stt { get; set; }
        public string mo_ta { get; set; }
        public string thong_so_ky_thuat { get; set; }
        public string thong_tin_bo_sung { get; set; }
        public string thong_so_ky_thuat_mobile { get; set; }
        public string thong_tin_bo_sung_mobile { get; set; }
        public string mo_ta_mobile { get; set; }
        public string mo_ta_mobile_en { get; set; }
        public string ten_san_pham_en { get; set; }
        public string mo_ta_en { get; set; }
        public string thong_so_ky_thuat_en { get; set; }
        public string thong_tin_bo_sung_en { get; set; }
    }
}
