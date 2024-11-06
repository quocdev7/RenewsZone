using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_linh_vuc_model
    {
        public sys_linh_vuc_model()
        {
            db = new sys_linh_vuc_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_linh_vuc_db db { get; set; }
    }
}
