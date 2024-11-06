using System.Collections.Generic;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_dat_hang_model
    {
        public sys_dat_hang_model()
        {
            db = new sys_dat_hang_db();
            list_detail = new List<sys_dat_hang_chi_tiet_db>();
            list_product_card = new List<sys_san_pham_model>();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string tinh_thanh_user { get; set; }
        public string quan_huyen_user { get; set; }
        public string tinh_thanh_cong_ty { get; set; }
        public string quan_huyen_cong_ty { get; set; }
        public string captcha { get; set; }
        public sys_dat_hang_db db { get; set; }
        public List<sys_dat_hang_chi_tiet_db> list_detail { get; set; }
        public List<sys_san_pham_model> list_product_card { get; set; }
    }
}
