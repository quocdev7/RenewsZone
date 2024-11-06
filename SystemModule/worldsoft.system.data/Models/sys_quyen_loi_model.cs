using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_quyen_loi_model
    {
        public sys_quyen_loi_model()
        {
            db = new sys_quyen_loi_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_quyen_loi_db db { get; set; }
    }
}
