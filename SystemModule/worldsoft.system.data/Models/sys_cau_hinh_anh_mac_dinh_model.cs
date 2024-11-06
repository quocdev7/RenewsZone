using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cau_hinh_anh_mac_dinh_model
    {
        public sys_cau_hinh_anh_mac_dinh_model()
        {
            db = new sys_cau_hinh_anh_mac_dinh_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public string ten_loai { get; set; }
        public sys_cau_hinh_anh_mac_dinh_db db { get; set; }
    }
}
