using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_tinh_thanh_model
    {
        public sys_tinh_thanh_model()
        {
            db = new sys_tinh_thanh_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public string quoc_gia { get; set; }
        public sys_tinh_thanh_db db { get; set; }
    }
}
