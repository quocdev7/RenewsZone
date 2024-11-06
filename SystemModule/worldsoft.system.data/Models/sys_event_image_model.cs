using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_image_model
    {
        public sys_event_image_model()
        {
            db = new sys_event_image_db();

        }

        public string ten_nguoi_lap { get; set; }
        public string ten_hinh_thuc { get; set; }
        public string ten_nguoi_cap_nhat { get; set; }

        public sys_event_image_db db { get; set; }

        public string ten_cong_ty { get; set; }
    }
}
