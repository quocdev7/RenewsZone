using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cau_hinh_duyet_su_kien_model
    {
        public sys_cau_hinh_duyet_su_kien_model()
        {
            db = new sys_cau_hinh_duyet_su_kien_db();
        }
        public string full_name { get; set; }
        public string type_su_kien_name { get; set; }
        public string khoa_name { get; set; }
        public string updateby_name { get; set; }
  
        public List<string> khoa { get; set; }

        public string type_su_kien { get; set; }
        public sys_cau_hinh_duyet_su_kien_db db { get; set; }
    }
}
