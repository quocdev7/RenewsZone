﻿using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_nhom_thu_vien_hinh_anh_model
    {
        public sys_nhom_thu_vien_hinh_anh_model()
        {
            db = new sys_nhom_thu_vien_hinh_anh_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_nhom_thu_vien_hinh_anh_db db { get; set; }
    }
}
