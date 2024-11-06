using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_nguoi_nhan_hoc_bong_model
    {
        public sys_event_nguoi_nhan_hoc_bong_model()
        {
            db = new sys_event_nguoi_nhan_hoc_bong_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_event_nguoi_nhan_hoc_bong_db db { get; set; }

        public List<string> su_kien { get; set; }
        public string ten_su_kien { get; set; }

        public List<string> tien_te { get; set; }

        public string ten_tien_te { get; set; }
    }

   
}
