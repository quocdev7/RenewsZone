using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cot_moc_su_kien_language_model
    {
        public sys_cot_moc_su_kien_language_model()
        {
            db = new sys_cot_moc_su_kien_language_db();
        }
      
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public string ten_su_kien { get; set; }
        public sys_cot_moc_su_kien_language_db db { get; set; }
    }
}
