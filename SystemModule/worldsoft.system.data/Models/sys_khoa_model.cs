using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_khoa_model
    {
        public sys_khoa_model()
        {
            db = new sys_khoa_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_khoa_db db { get; set; }
    }
}
