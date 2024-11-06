using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_khuyen_mai_model
    {
        public sys_khuyen_mai_model()
        {
            db = new sys_khuyen_mai_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_khuyen_mai_db db { get; set; }
    }
}
