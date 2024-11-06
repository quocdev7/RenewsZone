using System;
using System.Collections.Generic;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_event_program_model
    {
        public sys_event_program_model()
        {
            db = new sys_event_program_db();
        }
        public sys_event_program_db db { get; set; }
        public string ten_su_kien { get; set; }
        public string ten_dien_gia { get; set; }
        public string chuc_danh { get; set; }
        public string ten_dien_gia_en { get; set; }
        public string chuc_danh_en { get; set; }
        public string anh_dai_dien { get; set; }
        public DateTime? time_start { get; set; }
        public DateTime? time_end { get; set; }
        public string name_en { get; set; }
        public string description_en { get; set; }


    }
    public class sys_event_program_ref_model
    {
        public sys_event_program_ref_model()
        {
            list_chuong_trinh = new List<sys_event_program_model>();
        }
        public int? start_time_day { get; set; }
        public int? start_time_month { get; set; }
        public int? start_time_year { get; set; }
        public string start_time_str { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public DateTime? time_start { get; set; }
        public DateTime? time_end { get; set; }

        public List<sys_event_program_model> list_chuong_trinh { get;set;}
    }

}
