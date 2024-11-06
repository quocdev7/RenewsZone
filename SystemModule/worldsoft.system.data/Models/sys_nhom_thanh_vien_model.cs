using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_nhom_thanh_vien_model
    {
        public sys_nhom_thanh_vien_model()
        {
            db = new sys_nhom_thanh_vien_db();
           
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public sys_nhom_thanh_vien_db db { get; set; }

        public string ten_nhom { get; set; }
      
    }
}
