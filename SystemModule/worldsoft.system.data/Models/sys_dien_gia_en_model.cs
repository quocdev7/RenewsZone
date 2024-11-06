using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_dien_gia_en_model
    {
        public sys_dien_gia_en_model()
        {
            db = new sys_dien_gia_en_db();
        }
        public sys_dien_gia_en_db db { get; set; }
    }
}
