using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_help_detail_model
    {
        public sys_help_detail_model()
        {
            db = new sys_help_detail_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string help_name { get; set; }
        public sys_help_detail_db db { get; set; }
    }
}
