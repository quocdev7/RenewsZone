using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_quan_huyen_model
    {
        public sys_quan_huyen_model()
        {
            db = new sys_quan_huyen_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string quoc_gia { get; set; }
        public string tinh { get; set; }
        public sys_quan_huyen_db db { get; set; }
    }
}
