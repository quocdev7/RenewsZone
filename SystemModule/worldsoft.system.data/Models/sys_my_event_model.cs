using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_my_event_model
    {
        public string id { get; set; }
        public string id_su_kien { get; set; }
        public string ly_do { get; set; }
        public string anh_su_kien { get; set; }
        public string ten_su_kien { get; set; }
        public int? check_in_status { get; set; }
        public DateTime? ngay_bat_dau { get; set; }
        public DateTime? ngay_ket_thuc { get; set; }
        public DateTime? ngay_thuc_hien { get; set; }

    }

}
