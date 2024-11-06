using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace worldsoft.DataBase.System
{
    [Table("sys_user_ban_be")]
    public class sys_user_ban_be_db
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string user_id { get; set; }
        public string user_id_ban_be { get; set; }
        public DateTime? date_action { get; set; }

        // 1 ban be, 2 lời mời kết ban, 3 chờ ket ban, 4 mời qua email (user chưa tồn tại trên hệ thống).
        public int? status_del { get; set; }
        public string email_invite { get; set; }
    }
}
