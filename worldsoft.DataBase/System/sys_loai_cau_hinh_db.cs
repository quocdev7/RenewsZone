using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_loai_cau_hinh")]
    public class sys_loai_cau_hinh_db
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
     
    }

}
