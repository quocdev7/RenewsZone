using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_contact_model
    {
        public sys_event_contact_model()
        {
            db = new sys_event_participate_db();
        }
        public sys_event_participate_db db { get; set; }
        public string user_name { get; set; }
        public string id_khach_moi { get; set; }
        public string id_ban_to_chuc { get; set; }

        public string position { get; set; }
        public string avatar_link { get; set; }
        public string ten_cong_ty { get; set; }
        public int? school_year { get; set; }
        public string faculty { get; set; }
        public int? role_view { get; set; }
        public string ten_quoc_gia { get; set; }
        public string ten_su_kien { get; set; }
        public string email { get; set; }
        public string dienthoai { get; set; }

    }
}
