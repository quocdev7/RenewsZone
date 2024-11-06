using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_thong_tin_tuyen_dung_model
    {
        public sys_thong_tin_tuyen_dung_model()
        {
            db = new sys_thong_tin_tuyen_dung_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }


        public string ten_cong_ty { get; set; }

        public string ten_nhom_nganh { get; set; }
        public string ten_loai_nganh { get; set; }
        public string ten_khu_vuc { get; set; }
        public string vi_tri_tuyen_dung { get; set; }
        public sys_thong_tin_tuyen_dung_db db { get; set; }
    }
}
