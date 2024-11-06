using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_tien_te_model
    {
        public sys_tien_te_model()
        {
            db = new sys_tien_te_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_tien_te_db db { get; set; }
    }
}
