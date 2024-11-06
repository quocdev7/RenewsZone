using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_template_notification_model
    {
        public sys_template_notification_model()
        {
            db = new sys_template_notification_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string type_notification_name { get; set; }
        
        public sys_template_notification_db db { get; set; }
    }
}
