using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_image")]
    public class sys_event_image_db
    {
      
        public string id { get; set; }
        public string event_id { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string extension { get; set; }
        public DateTime? create_date { get; set; }
        public string upload_by_userid { get; set; }
        public long? size { get; set; }
        public string note { get; set; }
    }
}
