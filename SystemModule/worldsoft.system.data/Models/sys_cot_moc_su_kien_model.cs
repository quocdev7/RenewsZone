using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cot_moc_su_kien_model
    {
        public sys_cot_moc_su_kien_model()
        {
            db = new sys_cot_moc_su_kien_db();
        }
        public string id { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string note { get; set; }
        public string note_mobile { get; set; }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string ten_su_kien { get; set; }
        public string name_language { get; set; }

        public string time_language { get; set; }
        public string note_language { get; set; }
        public string note_mobile_language { get; set; }
        public sys_cot_moc_su_kien_db db { get; set; }
    }
}
