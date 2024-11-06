using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_nang_luc_file_upload_model
    {
        public sys_nang_luc_file_upload_model()
        {
            db = new sys_nang_luc_file_upload_db();
        }
     
        public sys_nang_luc_file_upload_db db { get; set; }

        public decimal? percent_complete { get; set; }

        public bool? isCheck { get; set; }
        public bool? newfile { get; set; }
    }
}
