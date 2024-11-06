using System.Collections.Generic;

using worldsoft.common.Models;
using worldsoft.system.web.Controller;

namespace worldsoft.system.web.MenuAndRole
{
    public static class SystemListController
    {
        public static List<ControllerAppModel> listController = new List<ControllerAppModel>()

        {

            sys_khuyen_maiController.declare,
            sys_tinh_thanhController.declare,
            sys_quan_huyenController.declare,
            sys_doi_tacController.declare,
            sys_loai_doi_tacController.declare,
            sys_nang_luc_hinh_anhController.declare,
            sys_nang_lucController.declare,
            sys_loai_cau_hinhController.declare,
            sys_cau_hinh_thong_tin_websiteController.declare,
             sys_person_eventController.declare,

            sys_approved_eventController.declare,

               sys_event_nguoi_nhan_hoc_bongController.declare,
               sys_event_nha_tai_troController.declare,
              sys_cot_moc_su_kienController.declare,
            sys_degreeController.declare,
            sys_tien_teController.declare,

             sys_cau_hinh_anh_mac_dinhController.declare,


            sys_tu_ngu_camController.declare,
            sys_nhom_hoi_dongController.declare,
            sys_cuu_sinh_vienController.declare,
            sys_quyen_loiController.declare,

               sys_videoController.declare,
               sys_nhom_thu_vien_hinh_anhController.declare,
                    sys_thu_vien_hinh_anhController.declare,
                  sys_loai_nganh_ngheController.declare,
                sys_nhom_nganh_ngheController.declare,
                sys_vi_tri_tuyen_dungController.declare,
                sys_thong_tin_tuyen_dungController.declare,
                sys_khu_vucController.declare,

            sys_userController.declare,
            sys_cau_hinh_duyet_userController.declare,
            sys_approval_userController.declare,

            sys_nhom_thanh_vienController.declare,
            sys_thanh_vien_thuoc_nhomController.declare,

            sys_departmentController.declare,
            sys_job_titleController.declare,
            sys_linh_vucController.declare,
            sys_companyController.declare,


            sys_group_userController.declare,
            sys_user_companyController.declare,

            sys_khoaController.declare,
            sys_nghi_quyetController.declare,
            sys_cau_hinh_thong_tinController.declare,

            //sys_anh_san_phamController.declare,
            sys_loai_san_phamController.declare,
            sys_san_phamController.declare,
            sys_dat_hangController.declare,
       sys_like_san_phamController.declare,

            sys_group_newsController.declare,
            sys_type_newsController.declare,
            sys_user_typenewsController.declare,
            sys_newsController.declare,
            sys_approval_newsController.declare,
            sys_template_notificationController.declare,
            sys_type_notificationController.declare,

            sys_type_mailController.declare,
            sys_template_mailController.declare,
            sys_helpController.declare,
            sys_help_detailController.declare,
            sys_contactusController.declare,
            sys_bannerController.declare,
            sys_eventController.declare,
            sys_dien_giaController.declare,
            sys_cau_hinh_duyet_su_kienController.declare,
            sys_event_programController.declare,
            sys_event_contactController.declare,
            //sys_event_participateController.declare,
            sys_event_qaController.declare,
            sys_anh_noi_bat_su_kienController.declare,
            sys_event_emailController.declare,
              sys_event_khach_moiController.declare,

        };


    }
}
