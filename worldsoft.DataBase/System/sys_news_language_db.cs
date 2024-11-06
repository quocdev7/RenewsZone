using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_news_language")]
    public class sys_news_language_db
    {
        public string id { get; set; }
        public string tieu_de { get; set; }
        public string noi_dung_trang_bia { get; set; }
        public string noi_dung { get; set; }

        public string nguon_tin_tuc { get; set; }
        public string id_news { get; set; }

        public string noi_dung_mobile { get; set; }
    }
}
