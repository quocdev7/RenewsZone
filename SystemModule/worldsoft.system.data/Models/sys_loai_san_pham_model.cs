﻿using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_loai_san_pham_model
    {
        public sys_loai_san_pham_model()
        {
            db = new sys_loai_san_pham_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_loai_san_pham_db db { get; set; }
    }
}
