using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_user_fileupload")]
    public class sys_user_fileupload_db
    {
       
        public string id { get; set; }
        public string user_id { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string upload_by { get; set; }
  

        public DateTime? upload_date { get; set; }

        public long? file_size { get; set; }
        public string file_type { get; set; }

    }
   
}
