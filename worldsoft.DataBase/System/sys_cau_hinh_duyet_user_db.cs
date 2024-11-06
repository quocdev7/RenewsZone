﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_cau_hinh_duyet_user")]
    public class sys_cau_hinh_duyet_user_db
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string id_khoa { get; set; }
        public string hinh_thuc { get; set; }
        public string create_by { get; set; }
        public DateTime? create_date { get; set; }
        public string update_by { get; set; }
        public DateTime? update_date { get; set; }
        public int? status_del { get; set; }
    }

}