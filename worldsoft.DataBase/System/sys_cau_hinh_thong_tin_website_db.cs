using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_cau_hinh_thong_tin_website")]
    public class sys_cau_hinh_thong_tin_website_db
    {
        public string id { get; set; }
        public string tieu_de { get; set; }
        public string dia_chi { get; set; }
        public string email { get; set; }
        public string ten_truong { get; set; }
        public string dien_thoai { get; set; }
        public string di_dong { get; set; }
        public string facebook_link { get; set; }
        public string linked_link { get; set; }
        public string youtube_link { get; set; }
    }

}