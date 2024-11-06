using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_cau_hinh_thong_tin_language")]
    public class sys_cau_hinh_thong_tin_language_db
    {
        public string id { get; set; }
        public string id_cau_hinh_thong_tin { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung { get; set; }
        public string noi_dung_mobile { get; set; }
        
      
    }
}
