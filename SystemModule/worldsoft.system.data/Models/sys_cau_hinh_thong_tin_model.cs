using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_cau_hinh_thong_tin_model
    {
        public sys_cau_hinh_thong_tin_model()
        {
            db = new sys_cau_hinh_thong_tin_db();
            
        }
        public sys_cau_hinh_thong_tin_db db { get; set; }
        public string create_by_name { get; set; }

        public string loai_cau_hinh { get; set; }
    }
}
