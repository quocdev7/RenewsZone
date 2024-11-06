using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;

namespace worldsoft.system.data.Models
{
    public class sys_video_model
    {
        public sys_video_model()
        {
            db = new sys_video_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_video_db db { get; set; }
    }
}
