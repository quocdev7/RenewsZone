using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_cuu_sinh_vien_model
    {
        public sys_cuu_sinh_vien_model()
        {
            db = new sys_cuu_sinh_vien_db();
        }
        public sys_cuu_sinh_vien_db db { get; set; }

        public string updateby_name { get; set; }
        public string ten_nhom_hoi_dong { get; set; }

        

    }
}
