using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_nhom_hoi_dong_model
    {
        public sys_nhom_hoi_dong_model()
        {
            db = new sys_nhom_hoi_dong_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_nhom_hoi_dong_db db { get; set; }
    }
}
