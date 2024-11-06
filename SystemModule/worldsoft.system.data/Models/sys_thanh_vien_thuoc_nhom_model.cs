using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_thanh_vien_thuoc_nhom_model
    {
        public sys_thanh_vien_thuoc_nhom_model()
        {
            db = new sys_thanh_vien_thuoc_nhom_db();

        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public sys_thanh_vien_thuoc_nhom_db db { get; set; }

        public string ten_nhom { get; set; }
        public string ten_nguoi_dung { get; set; }

        public string user_name { get; set; }

        public string position { get; set; }

        public string avatar_link { get; set; }
        public string ten_cong_ty { get; set; }
        public int? school_year { get; set; }
        public string faculty { get; set; }
        public string ten_quoc_gia { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }


    }
}
