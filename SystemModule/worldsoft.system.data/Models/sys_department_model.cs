using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_department_model
    {
        public sys_department_model()
        {
            db = new sys_department_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_department_db db { get; set; }
    }
}
