using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_anh_san_pham_model
    {
        public sys_anh_san_pham_model()
        {
            db = new sys_anh_san_pham_db();
            list_file = new List<sys_san_pham_file_upload_model>();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public string ten_san_pham { get; set; }
        public sys_anh_san_pham_db db { get; set; }
        public List<sys_san_pham_file_upload_model> list_file { get; set; }
    }

    public class ZipItem
    {
        public string name { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }

    }

}
