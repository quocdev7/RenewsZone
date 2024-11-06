using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_khach_moi_model
    {
        public sys_event_khach_moi_model()
        {
            db = new sys_event_khach_moi_db();
        }
        public sys_event_khach_moi_db db { get; set; }
        public string updateby_name { get; set; }
        public string ten_dien_gia { get; set; }
      
    }
}
