using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_loai_nganh_nghe_model
    {
        public sys_loai_nganh_nghe_model()
        {
            db = new sys_loai_nganh_nghe_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string ten_nhom { get; set; }
        public sys_loai_nganh_nghe_db db { get; set; }
    }
}
