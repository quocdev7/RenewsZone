using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_anh_noi_bat_su_kien_model
    {
        public sys_anh_noi_bat_su_kien_model()
        {
            db = new sys_anh_noi_bat_su_kien_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public string ten_su_kien { get; set; }
        public sys_anh_noi_bat_su_kien_db db { get; set; }
    }
}
