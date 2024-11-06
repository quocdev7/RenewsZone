using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldsoft.DataBase.System;


namespace worldsoft.system.data.Models
{
    public class sys_approved_event_model
    {
        public sys_approved_event_model()
        {
            db = new sys_event_db();
            //list_khach_moi = new List<sys_event_participate_model>();
            //list_khach_moi = new List<sys_event_participate_model>();
            //list_ban_to_chuc = new List<sys_event_participate_model>();
            //list_chuong_trinh = new List<sys_event_program_model>();
            list_file = new List<sys_event_file_model>();
            list_image = new List<sys_event_image_model>();
        }
        //public List<sys_event_participate_model> list_khach_moi{ get; set; }
        //public List<sys_event_participate_model> list_ban_to_chuc { get; set; }
        //public List<sys_event_program_model> list_chuong_trinh { get; set; }
        public List<string> types { get; set; }

        public List<string> khoa { get; set; }


        public List<string> hinhthuc { get; set; }

        public string ten_khoa { get; set; }

        public List<sys_event_file_model> list_file { get; set; }
        public List<sys_event_image_model> list_image { get; set; }
        public string createby_name { get; set; }
        public string ten_tien_te { get; set; }

        public string updateby_name { get; set; }
        //1 .dang_dien_ra 2 .cancel  3 .ket_thuc 4 .sap_toi
        public int? trang_thai { get; set; }
        public sys_event_db db { get; set; }

        public string ten_cong_ty { get; set; }
        public int? cho_phep_dang_ky { get; set; }
        

        // 1 Mời tham gia sự kiện, 2 Từ chối tham gia sư kiện, 3 Sẽ tham gia sự kiện, 4 Đã tham gia sự kiện, 5 Không được chấp thuận tham gia sự kiện, 6 Đăng ký tham gia sự kiện
        public int? check_in_status { get; set; }
        public string ten_mau_thu_moi { get; set; }
        public string ten_mau_thu_cam_on { get; set; }


        public string tieu_de_mau_thu_moi { get; set; }
        public string tieu_de_thu_cam_on { get; set; }
        public string noi_dung_mau_thu_moi { get; set; }
        public string noi_dung_thu_cam_on { get; set; }
        public string trangthai_dangky { get; set; }
    }
    public class sys_approved_event_ref_model
    {
        public string id_su_kien { get; set; }
        public string ten_su_kien { get; set; }
        public DateTime? time_start { get; set; }
        public DateTime? time_end { get; set; }
        public int? status_del { get; set; }
    }
}
