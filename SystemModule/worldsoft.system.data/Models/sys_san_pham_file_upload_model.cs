using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_san_pham_file_upload_model
    {
        public sys_san_pham_file_upload_model()
        {
            db = new sys_san_pham_file_upload_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }

        public string ten_san_pham { get; set; }
        public sys_san_pham_file_upload_db db { get; set; }

        public decimal? percent_complete { get; set; }

        public bool? isCheck { get; set; }
        public bool? newfile { get; set; }
        public string uuid { get; set; }
        
    }
}
