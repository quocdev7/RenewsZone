using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_like_san_pham_model
    {
        public sys_like_san_pham_model()
        {
            db = new sys_like_san_pham_db();
        }

        public sys_like_san_pham_db db { get; set; }
        public string capcha { get; set; }
    }
}
