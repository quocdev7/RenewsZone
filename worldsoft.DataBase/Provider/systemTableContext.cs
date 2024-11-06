using Microsoft.EntityFrameworkCore;
using worldsoft.DataBase.Function;
using worldsoft.DataBase.System;

namespace worldsoft.DataBase.Provider
{
    public partial class worldsoftDefautContext
    {
        public virtual DbSet<sys_khuyen_mai_db> sys_khuyen_mais { get; set; }
        public virtual DbSet<sys_dat_hang_chi_tiet_db> sys_dat_hang_chi_tiets { get; set; }
        public virtual DbSet<sys_quan_huyen_db> sys_quan_huyens { get; set; }
        public virtual DbSet<sys_tinh_thanh_db> sys_tinh_thanhs { get; set; }
        public virtual DbSet<sys_like_san_pham_db> sys_like_san_phams { get; set; }
        public virtual DbSet<sys_dat_hang_db> sys_dat_hangs { get; set; }
        public virtual DbSet<sys_dien_gia_en_db> sys_dien_gia_ens { get; set; }
        public virtual DbSet<sys_event_program_en_db> sys_event_program_ens { get; set; }
        public virtual DbSet<sys_event_qa_en_db> sys_event_qa_ens { get; set; }
        public virtual DbSet<sys_san_pham_language_db> sys_san_pham_languages { get; set; }
        public virtual DbSet<sys_doi_tac_language_db> sys_doi_tac_languages { get; set; }
        public virtual DbSet<sys_doi_tac_db> sys_doi_tacs { get; set; }
        public virtual DbSet<sys_loai_doi_tac_language_db> sys_loai_doi_tac_languages { get; set; }
        public virtual DbSet<sys_loai_doi_tac_db> sys_loai_doi_tacs { get; set; }
        public virtual DbSet<sys_cau_hinh_thong_tin_language_db> sys_cau_hinh_thong_tin_languages { get; set; }
        public virtual DbSet<sys_event_en_db> sys_event_ens { get; set; }
        public virtual DbSet<sys_nang_luc_hinh_anh_db> sys_nang_luc_hinh_anh { get; set; }
        public virtual DbSet<sys_nang_luc_db> sys_nang_luc { get; set; }
        public virtual DbSet<sys_san_pham_file_upload_db> sys_san_pham_file_uploads { get; set; }
        public virtual DbSet<sys_nang_luc_file_upload_db> sys_nang_luc_file_uploads { get; set; }
        public virtual DbSet<sys_anh_san_pham_db> sys_anh_san_phams { get; set; }
        public virtual DbSet<sys_loai_san_pham_db> sys_loai_san_phams { get; set; }
        public virtual DbSet<sys_san_pham_db> sys_san_phams { get; set; }
        public virtual DbSet<sys_template_notification_db> sys_template_notifications { get; set; }
        public virtual DbSet<sys_type_notification_db> sys_type_notifications { get; set; }

        public virtual DbSet<sys_cau_hinh_thong_tin_website_db> sys_cau_hinh_thong_tin_website { get; set; }
        public virtual DbSet<sys_cau_hinh_duyet_user_db> sys_cau_hinh_duyet_users { get; set; }
        public virtual DbSet<sys_cau_hinh_duyet_su_kien_db> sys_cau_hinh_duyet_su_kiens { get; set; }


        public virtual DbSet<sys_event_nguoi_nhan_hoc_bong_db> sys_event_nguoi_nhan_hoc_bongs { get; set; }

        public virtual DbSet<sys_event_nha_tai_tro_db> sys_event_nha_tai_tros { get; set; }

        public virtual DbSet<sys_cot_moc_su_kien_db> sys_cot_moc_su_kiens { get; set; }
        public virtual DbSet<sys_cot_moc_su_kien_language_db> sys_cot_moc_su_kien_ens { get; set; }
        public virtual DbSet<sys_cau_hinh_anh_mac_dinh_db> sys_cau_hinh_anh_mac_dinhs { get; set; }

        public virtual DbSet<sys_degree_db> sys_degrees { get; set; }
        public virtual DbSet<sys_tien_te_db> sys_tien_tes { get; set; }
        public virtual DbSet<sys_user_ung_tuyen_db> sys_user_ung_tuyens { get; set; }

        public virtual DbSet<sys_nhom_hoi_dong_db> sys_nhom_hoi_dongs { get; set; }

        public virtual DbSet<sys_news_log_view_db> sys_news_log_views { get; set; }

        public virtual DbSet<sys_user_ban_be_db> sys_user_ban_bes { get; set; }




        public virtual DbSet<sys_event_khach_moi_db> sys_event_khach_mois { get; set; }
        public virtual DbSet<sys_dien_gia_db> sys_dien_gias { get; set; }
        public virtual DbSet<sys_anh_noi_bat_su_kien_db> sys_anh_noi_bat_su_kiens { get; set; }
        public virtual DbSet<sys_cau_hinh_quyen_rieng_tu_db> sys_cau_hinh_quyen_rieng_tus { get; set; }
        public virtual DbSet<sys_tu_ngu_cam_db> sys_tu_ngu_cams { get; set; }
        public virtual DbSet<sys_cuu_sinh_vien_db> sys_cuu_sinh_viens { get; set; }

        public virtual DbSet<sys_user_fileupload_db> sys_user_fileuploads { get; set; }
        public virtual DbSet<sys_log_mail_db> sys_log_mails { get; set; }
        public virtual DbSet<sys_thu_vien_hinh_anh_db> sys_thu_vien_hinh_anhs { get; set; }
        public virtual DbSet<sys_video_db> sys_videos { get; set; }
        public virtual DbSet<sys_nhom_nganh_nghe_db> sys_nhom_nganh_nghes { get; set; }
        public virtual DbSet<sys_education_user_db> sys_education_users { get; set; }
        public virtual DbSet<sys_loai_nganh_nghe_db> sys_loai_nganh_nghes { get; set; }
        public virtual DbSet<sys_vi_tri_tuyen_dung_db> sys_vi_tri_tuyen_dungs { get; set; }
        public virtual DbSet<sys_thong_tin_tuyen_dung_db> sys_thong_tin_tuyen_dungs { get; set; }

        public virtual DbSet<sys_khu_vuc_db> sys_khu_vucs { get; set; }
        public virtual DbSet<sys_work_history_user_db> sys_work_history_users { get; set; }
        public virtual DbSet<sys_certificate_user_db> sys_certificate_users { get; set; }
        public virtual DbSet<sys_success_user_db> sys_success_users { get; set; }
        public virtual DbSet<sys_experience_user_db> sys_experience_users { get; set; }
        public virtual DbSet<sys_banner_db> sys_banners { get; set; }
        public virtual DbSet<sys_contactus_db> sys_contactus { get; set; }
        public virtual DbSet<sys_loai_cau_hinh_db> sys_loai_cau_hinh_dbs { get; set; }
        public virtual DbSet<sys_event_db> sys_events { get; set; }
        public virtual DbSet<sys_event_email_db> sys_event_emails { get; set; }
        public virtual DbSet<sys_event_file_db> sys_event_files { get; set; }
        public virtual DbSet<sys_event_image_db> sys_event_images { get; set; }
        public virtual DbSet<sys_event_participate_db> sys_event_participates { get; set; }

        public virtual DbSet<sys_event_program_db> sys_event_programs { get; set; }

        public virtual DbSet<sys_event_qa_db> sys_event_qas { get; set; }
        public virtual DbSet<sys_nghi_quyet_db> sys_nghi_quyets { get; set; }
        public virtual DbSet<sys_nhom_thanh_vien_db> sys_nhom_thanh_viens { get; set; }
        public virtual DbSet<sys_thanh_vien_thuoc_nhom_db> sys_thanh_vien_thuoc_nhoms { get; set; }
        public virtual DbSet<sys_nhom_thu_vien_hinh_anh_db> sys_nhom_thu_vien_hinh_anhs { get; set; }

        public virtual DbSet<sys_cau_hinh_thong_tin_db> sys_cau_hinh_thong_tins { get; set; }
        public virtual DbSet<sys_type_mail_db> sys_type_mails { get; set; }

        public virtual DbSet<sys_user_company_db> sys_user_companys { get; set; }
        public virtual DbSet<sys_linh_vuc_db> sys_linh_vucs { get; set; }
        public virtual DbSet<sys_user_typenews_db> sys_user_typenews { get; set; }
        public virtual DbSet<sys_khoa_db> sys_khoas { get; set; }
        public virtual DbSet<sys_approval_config_db> sys_approval_configs { get; set; }
        public virtual DbSet<sys_approval_config_detail_db> sys_approval_config_details { get; set; }


        public virtual DbSet<sys_group_user_db> sys_group_users { get; set; }
        public virtual DbSet<sys_group_user_detail_db> sys_group_user_details { get; set; }
        public virtual DbSet<sys_group_user_role_db> sys_group_user_roles { get; set; }



        public virtual DbSet<sys_approval_detail_db> sys_approval_details { get; set; }
        public virtual DbSet<sys_approval_db> sys_approvals { get; set; }
        public virtual DbSet<sys_approval_step_db> sys_approval_steps { get; set; }
        public virtual DbSet<sys_department_db> sys_departments { get; set; }

        public virtual DbSet<sys_job_title_db> sys_job_titles { get; set; }
        public virtual DbSet<sys_help_db> sys_helps { get; set; }
        public virtual DbSet<sys_help_detail_db> sys_help_details { get; set; }

        public virtual DbSet<User> users { get; set; }

        public virtual DbSet<sys_template_mail_db> sys_template_mails { get; set; }

        public virtual DbSet<sys_search_db> sys_searchs { get; set; }




        public virtual DbSet<sys_company_db> sys_companys { get; set; }

        public virtual DbSet<sys_notification_db> sys_notifications { get; set; }
        public virtual DbSet<sys_group_news_db> sys_group_news { get; set; }
        public virtual DbSet<sys_type_news_db> sys_type_news { get; set; }
        public virtual DbSet<sys_news_db> sys_news { get; set; }
        public virtual DbSet<sys_news_language_db> sys_news_languages { get; set; }

        public virtual DbSet<sys_news_comment_db> sys_news_comments { get; set; }

        public virtual DbSet<sys_quyen_loi_db> sys_quyen_lois { get; set; }
        protected void systemTableBuilder(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {

            });
            modelBuilder.Query<Fn_get_sys_approval>();
            OnModelCreatingPartial(modelBuilder);
        }





    }
}
