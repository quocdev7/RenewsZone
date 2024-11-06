using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_email_model
    {
        public sys_event_email_model()
        {
            db = new sys_event_email_db();
        }
        public sys_event_email_db db { get; set; }

        public string ten_su_kien { get; set; }
        public string ten_mau_email { get; set; }
        public string ten_loai_email { get; set; }
        public string ten_nguoi_lap { get; set; }

    }
}
