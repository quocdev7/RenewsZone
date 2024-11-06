using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_job_title_model
    {
        public sys_job_title_model()
        {
            db = new sys_job_title_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_job_title_db db { get; set; }
    }
}
