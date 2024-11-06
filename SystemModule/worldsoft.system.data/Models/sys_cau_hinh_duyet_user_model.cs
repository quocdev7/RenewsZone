using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cau_hinh_duyet_user_model
    {
        public sys_cau_hinh_duyet_user_model()
        {
            db = new sys_cau_hinh_duyet_user_db();
        }
        public string full_name { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public List<string> hinh_thuc { get; set; }
        public string ten_hinh_thuc { get; set; }

        public string ten_khoa { get; set; }

        public List<string> khoa { get; set; }
        public sys_cau_hinh_duyet_user_db db { get; set; }
    }
}
