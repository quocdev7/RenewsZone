using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_san_pham_language")]
    public class sys_san_pham_language_db
    {
        public long? id { get; set; }
        public string id_san_pham { get; set; }
        public string ten_san_pham { get; set; }
        public string thong_so_ky_thuat { get; set; }
        public string thong_so_ky_thuat_mobile { get; set; }
        public string mo_ta { get; set; }
        public string mo_ta_mobile { get; set; }
        public string thong_tin_bo_sung { get; set; }
        public string thong_tin_bo_sung_mobile { get; set; }
    }
}
