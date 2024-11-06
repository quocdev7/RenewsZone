using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_nghi_quyet_model
    {
        public sys_nghi_quyet_model()
        {
            db = new sys_nghi_quyet_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_nghi_quyet_db db { get; set; }
    }
}
