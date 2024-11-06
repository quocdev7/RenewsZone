using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_user_company_model
    {
        public sys_user_company_model()
        {
            db = new sys_user_company_db();
        }
        public string full_name { get; set; }
        public string company_name { get; set; }
        public string updateby_name { get; set; }
        
        public sys_user_company_db db { get; set; }
    }
}
