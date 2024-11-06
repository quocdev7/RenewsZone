using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_type_mail_model
    {
        public sys_type_mail_model()
        {
            db = new sys_type_mail_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_type_mail_db db { get; set; }
    }
}
