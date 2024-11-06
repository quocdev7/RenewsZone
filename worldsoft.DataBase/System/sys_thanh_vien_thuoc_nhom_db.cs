using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_thanh_vien_thuoc_nhom")]
    public class sys_thanh_vien_thuoc_nhom_db
    {
        public string id { get; set; }
        public string id_nhom { get; set; }
        public string user_id { get; set; }
        public string note { get; set; }

        public DateTime? create_date { get; set; }

        public string create_by { get; set; }


    }
}
