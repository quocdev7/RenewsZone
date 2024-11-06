using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace worldsoft.DataBase.System
{
    [Table("sys_notification")]
    public class sys_notification_db
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }

        //1 unread,2 read
        public int? status_read { get; set; }
        public string menu { get; set; }

        public string param { get; set; }
        public string logo { get; set; }
        public string contenthtml { get; set; }
        public DateTime? date_send { get; set; }
    }
}
