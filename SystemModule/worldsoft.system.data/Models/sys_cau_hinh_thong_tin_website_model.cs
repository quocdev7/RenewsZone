using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cau_hinh_thong_tin_website_model
    {
        public sys_cau_hinh_thong_tin_website_model()
        {
            db = new sys_cau_hinh_thong_tin_website_db();
        }
        public sys_cau_hinh_thong_tin_website_db db { get; set; }
    }
}
