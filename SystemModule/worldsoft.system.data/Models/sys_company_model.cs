using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_company_model
    {
        public sys_company_model()
        {
            db = new sys_company_db();
        }
        public string field_name { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_company_db db { get; set; }
    }
}
