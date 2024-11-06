using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_event_participate")]
    public class sys_event_participate_db
    {
      
        public string id { get; set; }
        public string event_id { get; set; }
        public string user_id { get; set; }
        public DateTime? date_add { get; set; }
    
        public string company_id { get; set; }
        public int? role { get; set; }
        public string note { get; set; }
        public string review_note { get; set; }
        public int? review_rate { get; set; }

        public string list_id_type_email_send { get; set; }

        // 1 Mời tham gia sự kiện, 2 Từ chối tham gia sư kiện, 3 Sẽ tham gia sự kiện, 4 Đã tham gia sự kiện, 5 Không được chấp thuận tham gia sự kiện, 6 Đăng ký tham gia sự kiện
        public int? check_in_status { get; set; }
        public DateTime? check_in_date { get; set; }
        public DateTime? confirm_date { get; set; }
        public int? send_thanks_mail { get; set; }
        public string qr_code { get; set; }
    }
}
