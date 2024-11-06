﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_nang_luc")]
    public class sys_nang_luc_db
    {
        public string id { get; set; }
        public string tieu_de { get; set; }
        public string tieu_de_tieng_anh { get; set; }
        public string hinh_anh { get; set; }
        public string noi_dung { get; set; }
        public string noi_dung_tieng_anh { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
        public int? stt { get; set; }

    }

}
