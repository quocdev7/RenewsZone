using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_dien_gia_model
    {
        public sys_dien_gia_model()
        {
            db = new sys_dien_gia_db();
        }
        public sys_dien_gia_db db { get; set; }
        public string updateby_name { get; set; }
        public string ten_su_kien { get; set; }
        public string name_en { get; set; }
        public string chuc_danh_en { get; set; }
        public string cong_ty_en { get; set; }
    }
}
