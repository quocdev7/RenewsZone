using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_tu_ngu_cam_model
    {
        public sys_tu_ngu_cam_model()
        {
            db = new sys_tu_ngu_cam_db();
        }
        public string createby_name { get; set; }
        public string updateby_name { get; set; }
        public sys_tu_ngu_cam_db db { get; set; }
    }
}
