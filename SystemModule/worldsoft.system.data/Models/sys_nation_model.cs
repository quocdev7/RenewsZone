using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_nation_model
    {
        public sys_nation_model()
        {
            db = new sys_nation_db();
        }
        public string nation_name { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_nation_db db { get; set; }
    }
}
