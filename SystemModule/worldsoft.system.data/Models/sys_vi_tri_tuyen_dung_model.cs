using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_vi_tri_tuyen_dung_model
    {
        public sys_vi_tri_tuyen_dung_model()
        {
            db = new sys_vi_tri_tuyen_dung_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_vi_tri_tuyen_dung_db db { get; set; }
    }
}
