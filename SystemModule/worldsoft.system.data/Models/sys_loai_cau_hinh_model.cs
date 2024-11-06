using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_loai_cau_hinh_model
    {
        public sys_loai_cau_hinh_model()
        {
            db = new sys_loai_cau_hinh_db();
        }
        public sys_loai_cau_hinh_db db { get; set; }
    }
}
