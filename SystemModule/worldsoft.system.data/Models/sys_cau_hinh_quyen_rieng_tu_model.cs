using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cau_hinh_quyen_rieng_tu_model
    {
        public sys_cau_hinh_quyen_rieng_tu_model()
        {
            db = new sys_cau_hinh_quyen_rieng_tu_db();
            
        }
        public sys_cau_hinh_quyen_rieng_tu_db db { get; set; }
        public string create_by_name { get; set; }
        public int? status_user { get; set; }

    }
}
