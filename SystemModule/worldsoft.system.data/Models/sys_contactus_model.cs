using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_contactus_model
    {
        public sys_contactus_model()
        {
            db = new sys_contactus_db();
        }
        public string captcha { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string ten_khoa { get; set; }
        public sys_contactus_db db { get; set; }
    }
}
