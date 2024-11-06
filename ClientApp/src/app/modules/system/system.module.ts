import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseMasonryModule } from '@fuse/components/masonry';
import { NzProgressModule } from 'ng-zorro-antd/progress';
import { NgModule } from "@angular/core";
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { RouterModule } from "@angular/router";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { MatBadgeModule } from "@angular/material/badge";
import { DataTablesModule } from "angular-datatables";
import { MatButtonModule } from "@angular/material/button";
import { MatChipsModule } from "@angular/material/chips";
import { MatRippleModule, MAT_DATE_FORMATS } from "@angular/material/core";
import { MatExpansionModule } from "@angular/material/expansion";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSelectModule } from "@angular/material/select";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { MatDialogModule } from "@angular/material/dialog";
import { MatMenuModule } from "@angular/material/menu";
import { SweetAlert2Module } from "@sweetalert2/ngx-sweetalert2";

import { sys_approval_config_popUpAddComponent } from "./sys_approval_config/popupAdd.component";
import { sys_group_user_indexComponent } from "./sys_group_user/index.component";
import { sys_group_user_popUpAddComponent } from "./sys_group_user/popupAdd.component";
import { sys_user_indexComponent } from "./sys_user/index.component";
import { sys_user_popUpAddComponent } from "./sys_user/popupAdd.component";

import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgxMatSelectSearchModule } from "ngx-mat-select-search";

import { MatCardModule } from "@angular/material/card";

import { MatCheckboxModule } from "@angular/material/checkbox";

import { sys_department_indexComponent } from "./sys_department/index.component";
import { sys_department_popUpAddComponent } from "./sys_department/popupAdd.component";
import { sys_job_title_indexComponent } from "./sys_job_title/index.component";
import { sys_job_title_popUpAddComponent } from "./sys_job_title/popupAdd.component";

import { sys_type_mail_indexComponent } from "./sys_type_mail/index.component";
import { sys_type_mail_popUpAddComponent } from "./sys_type_mail/popupAdd.component";

import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatTreeModule } from "@angular/material/tree";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatListModule } from "@angular/material/list";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule } from "@angular-material-components/datetime-picker";
import { MatAutocompleteModule } from '@angular/material/autocomplete';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { TranslocoCoreModule } from "app/core/transloco/transloco.module";
import { commonModule } from "@fuse/components/commonComponent/common.module";
import { common_pageModule } from "@fuse/components/commonComponent/common_page.module";
import { systemsRoutes } from "./system.routing";
import { sys_approval_config_indexComponent } from "./sys_approval_config/index.component";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FusePipesModule } from "@fuse/pipes/pipes.module";
import { NzEmptyModule } from "ng-zorro-antd/empty";
import { TranslocoModule } from "@ngneat/transloco";
import { NzIconModule } from "ng-zorro-antd/icon";
import { MatDividerModule } from "@angular/material/divider";
import { MatTabsModule } from "@angular/material/tabs";
import { UploadDirective } from '@fuse/directives/upload.directive';
import { NgCircleProgressModule } from 'ng-circle-progress';

import { AutonumericModule } from '@angularfy/autonumeric';

import { CalendarModule } from 'app/calendar/calendar.module';

import { MatProgressBarModule } from '@angular/material/progress-bar';

import { SharedModule } from 'app/shared/shared.module';

import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NgApexchartsModule } from 'ng-apexcharts';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

import { AgmCoreModule } from '@agm/core';

import { sys_help_detail_indexComponent } from './sys_help_detail/index.component';
import { sys_help_indexComponent } from './sys_help/index.component';
import { sys_help_popUpAddComponent } from './sys_help/popupAdd.component';
import { sys_help_detail_popUpAddComponent } from './sys_help_detail/popupAdd.component';
import { sys_template_mail_indexComponent } from './sys_template_mail/index.component';
import { sys_template_mail_popUpAddComponent } from './sys_template_mail/popupAdd.component';

import { FuseDateRangeModule } from '@fuse/components/date-range';
import { FullCalendarModule } from '@fullcalendar/angular';
import { DATE_FORMATS } from 'app/Basecomponent/config';
import { doc_tailieu_view_file_onlineComponent } from './sys_user/viewfileonline.component';
import { sys_company_indexComponent } from './sys_company/index.component';
import { sys_company_popUpAddComponent } from './sys_company/popupAdd.component';

import { sys_anh_san_pham_indexComponent } from './sys_anh_san_pham/index.component';
import { sys_anh_san_pham_popUpAddComponent } from './sys_anh_san_pham/popupAdd.component';


import { sys_group_news_indexComponent } from './sys_group_news/index.component';
import { sys_group_news_popUpAddComponent } from './sys_group_news/popupAdd.component';
import { sys_type_news_indexComponent } from './sys_type_news/index.component';
import { sys_type_news_popUpAddComponent } from './sys_type_news/popupAdd.component';
import { sys_news_indexComponent } from './sys_news/index.component';
import { sys_news_popUpAddComponent } from './sys_news/popupAdd.component';
import { sys_news_popUpTinNoiBatComponent } from './sys_news/popupTinNoiBat.component';
import { sys_linh_vuc_indexComponent } from './sys_linh_vuc/index.component';
import { sys_linh_vuc_popUpAddComponent } from './sys_linh_vuc/popupAdd.component';


import { sys_nhom_thu_vien_hinh_anh_popUpAddComponent } from './sys_nhom_thu_vien_hinh_anh/popupAdd.component';
import { sys_nhom_thu_vien_hinh_anh_indexComponent } from './sys_nhom_thu_vien_hinh_anh/index.component';
import { ProfileComponent } from './profile/profile.component';
import { changePassComponent } from './sys_user/changePass.component';

import { sys_approval_news_indexComponent } from './sys_approval_news/index.component';
import { sys_approval_news_popUpAddComponent } from './sys_approval_news/popupAdd.component';

import { sys_khoa_indexComponent } from './sys_khoa/index.component';
import { sys_khoa_popUpAddComponent } from './sys_khoa/popupAdd.component';

import { sys_user_typenews_indexComponent } from './sys_user_typenews/index.component';


import { sys_loai_doi_tac_indexComponent } from './sys_loai_doi_tac/index.component';
import { sys_loai_doi_tac_popUpAddComponent } from './sys_loai_doi_tac/popupAdd.component';

import { sys_user_company_indexComponent } from './sys_user_company/index.component';

import { sys_nghi_quyet_indexComponent } from './sys_nghi_quyet/index.component';
import { sys_nghi_quyet_popUpAddComponent } from './sys_nghi_quyet/popupAdd.component';

import { sys_event_indexComponent } from './sys_event/index.component';
import { sys_event_popUpAddComponent } from './sys_event/popupAdd.component';
import { sys_event_popupFileDinhKemComponent } from './sys_event/popupFileDinhKem.component';
import { sys_event_popUpAnhDinhKemSuKienComponent } from './sys_event/popupAnhDinhKemSuKien.component';

import { sys_nhom_thanh_vien_indexComponent } from './sys_nhom_thanh_vien/index.component';
import { sys_nhom_thanh_vien_popUpAddComponent } from './sys_nhom_thanh_vien/popupAdd.component';

import { sys_thanh_vien_thuoc_nhom_indexComponent } from './sys_thanh_vien_thuoc_nhom/index.component';



import { sys_event_program_indexComponent } from './sys_event_program/index.component';

import { sys_event_program_popUpAddComponent } from './sys_event_program/popupAdd.component';
import { sys_approval_user_indexComponent } from './sys_approval_user/index.component';
import { sys_approval_user_popUpAddComponent } from './sys_approval_user/popupAdd.component';

import { sys_event_participate_indexComponent } from './sys_event_participate/index.component';
import { sys_event_participate_popUpAddComponent } from './sys_event_participate/popupAdd.component';
import { sys_event_participate_popUpNhomNguoiDungComponent } from './sys_event_participate/popupNhomNguoiDung.component';

import { sys_event_thong_bao_dang_kyComponent } from './sys_event/thongbaodangky.component';
import { sys_event_registerComponent } from './sys_event/register.component';


import { sys_event_contact_indexComponent } from './sys_event_contact/index.component';
import { sys_event_contact_popUpAddComponent } from './sys_event_contact/popupAdd.component';

import { sys_cau_hinh_thong_tin_indexComponent } from './sys_cau_hinh_thong_tin/index.component';
import { sys_cau_hinh_thong_tin_popUpAddComponent } from './sys_cau_hinh_thong_tin/popupAdd.component';

import { sys_event_qa_indexComponent } from './sys_event_qa/index.component';
import { sys_event_qa_popUpAddComponent } from './sys_event_qa/popupAdd.component';

import { sys_event_email_indexComponent } from './sys_event_email/index.component';
import { sys_event_email_popUpAddComponent } from './sys_event_email/popupAdd.component';

import { sys_banner_indexComponent } from './sys_banner/index.component';
import { sys_banner_popUpAddComponent } from './sys_banner/popupAdd.component';

import { sys_contactus_indexComponent } from './sys_contactus/index.component';
import { sys_contactus_popUpAddComponent } from './sys_contactus/popupAdd.component';


import { sys_loai_nganh_nghe_indexComponent } from './sys_loai_nganh_nghe/index.component';
import { sys_loai_nganh_nghe_popUpAddComponent } from './sys_loai_nganh_nghe/popupAdd.component';

import { sys_nhom_nganh_nghe_indexComponent } from './sys_nhom_nganh_nghe/index.component';
import { sys_nhom_nganh_nghe_popUpAddComponent } from './sys_nhom_nganh_nghe/popupAdd.component';

import { sys_khu_vuc_indexComponent } from './sys_khu_vuc/index.component';
import { sys_khu_vuc_popUpAddComponent } from './sys_khu_vuc/popupAdd.component';

import { sys_thong_tin_tuyen_dung_indexComponent } from './sys_thong_tin_tuyen_dung/index.component';
import { sys_thong_tin_tuyen_dung_popUpAddComponent } from './sys_thong_tin_tuyen_dung/popupAdd.component';
import { sys_vi_tri_tuyen_dung_indexComponent } from './sys_vi_tri_tuyen_dung/index.component';
import { sys_vi_tri_tuyen_dung_popUpAddComponent } from './sys_vi_tri_tuyen_dung/popupAdd.component';


import { sys_video_indexComponent } from './sys_video/index.component';
import { sys_video_popUpAddComponent } from './sys_video/popupAdd.component';

import { sys_thu_vien_hinh_anh_indexComponent } from './sys_thu_vien_hinh_anh/index.component';
import { sys_thu_vien_hinh_anh_popUpAddComponent } from './sys_thu_vien_hinh_anh/popupAdd.component';

import { sys_cuu_sinh_vien_popUpAddComponent } from './sys_cuu_sinh_vien/popupAdd.component';
import { sys_cuu_sinh_vien_indexComponent } from "./sys_cuu_sinh_vien/index.component";
import { sys_tu_ngu_cam_indexComponent } from './sys_tu_ngu_cam/index.component';
import { sys_tu_ngu_cam_popUpAddComponent } from './sys_tu_ngu_cam/popupAdd.component';

import { sys_anh_noi_bat_su_kien_indexComponent } from './sys_anh_noi_bat_su_kien/index.component';
import { sys_anh_noi_bat_su_kien_popUpAddComponent } from './sys_anh_noi_bat_su_kien/popupAdd.component';

import { sys_dien_gia_indexComponent } from './sys_dien_gia/index.component';
import { sys_dien_gia_popUpAddComponent } from './sys_dien_gia/popupAdd.component';

import { sys_event_khach_moi_indexComponent } from './sys_event_khach_moi/index.component';
import { sys_event_khach_moi_popUpAddComponent } from './sys_event_khach_moi/popupAdd.component';


import { sys_user_typenews_popUpAddComponent } from './sys_user_typenews/popupAdd.component';

import { sys_tien_te_indexComponent } from './sys_tien_te/index.component';
import { sys_tien_te_popUpAddComponent } from './sys_tien_te/popupAdd.component';

import { sys_degree_indexComponent } from './sys_degree/index.component';
import { sys_degree_popUpAddComponent } from './sys_degree/popupAdd.component';
import { sys_quyen_loi_indexComponent } from './sys_quyen_loi/index.component';
import { sys_quyen_loi_popUpAddComponent } from './sys_quyen_loi/popupAdd.component';
import { sys_nhom_hoi_dong_indexComponent } from './sys_nhom_hoi_dong/index.component';
import { sys_nhom_hoi_dong_popUpAddComponent } from './sys_nhom_hoi_dong/popupAdd.component';

import { sys_cau_hinh_anh_mac_dinh_indexComponent } from './sys_cau_hinh_anh_mac_dinh/index.component';
import { sys_cau_hinh_anh_mac_dinh_popUpAddComponent } from './sys_cau_hinh_anh_mac_dinh/popupAdd.component';

import { sys_cot_moc_su_kien_indexComponent } from './sys_cot_moc_su_kien/index.component';
import { sys_cot_moc_su_kien_popUpAddComponent } from './sys_cot_moc_su_kien/popupAdd.component';
import { sys_cot_moc_su_kien_popupLanguageComponent } from './sys_cot_moc_su_kien/popupLanguage.component';
import { sys_event_nguoi_nhan_hoc_bong_indexComponent } from './sys_event_nguoi_nhan_hoc_bong/index.component';
import { sys_event_nguoi_nhan_hoc_bong_popUpAddComponent } from './sys_event_nguoi_nhan_hoc_bong/popupAdd.component';
import { sys_event_nha_tai_tro_indexComponent } from './sys_event_nha_tai_tro/index.component';
import { sys_event_nha_tai_tro_popUpAddComponent } from './sys_event_nha_tai_tro/popupAdd.component';

import { sys_cau_hinh_duyet_user_indexComponent } from "./sys_cau_hinh_duyet_user/index.component";
import { sys_cau_hinh_duyet_user_popUpAddComponent } from "./sys_cau_hinh_duyet_user/popupAdd.component";

import { sys_cau_hinh_duyet_su_kien_indexComponent } from './sys_cau_hinh_duyet_su_kien/index.component';
import { sys_cau_hinh_duyet_su_kien_popUpAddComponent } from './sys_cau_hinh_duyet_su_kien/popupAdd.component';

import { sys_cau_hinh_thong_tin_website_indexComponent } from './sys_cau_hinh_thong_tin_website/index.component';
import { sys_cau_hinh_thong_tin_website_popUpAddComponent } from './sys_cau_hinh_thong_tin_website/popupAdd.component';

import { sys_template_notification_indexComponent } from "./sys_template_notification/index.component";
import { sys_template_notification_popUpAddComponent } from "./sys_template_notification/popupAdd.component";
import { sys_type_notification_indexComponent } from "./sys_type_notification/index.component";
import { sys_type_notification_popUpAddComponent } from "./sys_type_notification/popupAdd.component";
import { sys_news_popUpCommentComponent } from './sys_news/popupComment.component';

import { sys_loai_cau_hinh_indexComponent } from "./sys_loai_cau_hinh/index.component";
import { sys_loai_cau_hinh_popUpAddComponent } from "./sys_loai_cau_hinh/popupAdd.component";


import { sys_san_pham_indexComponent } from './sys_san_pham/index.component';
import { sys_san_pham_popUpAddComponent } from './sys_san_pham/popupAdd.component';
import { sys_dat_hang_indexComponent } from './sys_dat_hang/index.component';
import { sys_dat_hang_popUpAddComponent } from './sys_dat_hang/popupAdd.component';

import { sys_loai_san_pham_indexComponent } from './sys_loai_san_pham/index.component';
import { sys_loai_san_pham_popUpAddComponent } from './sys_loai_san_pham/popupAdd.component';

import { sys_nang_luc_indexComponent } from './sys_nang_luc/index.component';
import { sys_nang_luc_popUpAddComponent } from './sys_nang_luc/popupAdd.component';

import { sys_nang_luc_hinh_anh_indexComponent } from './sys_nang_luc_hinh_anh/index.component';
import { sys_nang_luc_hinh_anh_popUpAddComponent } from './sys_nang_luc_hinh_anh/popupAdd.component';
import { sys_event_popUpLanguageComponent } from './sys_event/popupLanguage.component';
import { sys_san_pham_popUpLanguageComponent } from './sys_san_pham/popupLanguage.component';
import { sys_san_pham_popUpAddImageComponent } from './sys_san_pham/popupAddImage.component';
import { sys_news_popUpLanguageComponent } from './sys_news/popupLanguage.component';
import { sys_loai_doi_tac_popUpLanguageComponent } from './sys_loai_doi_tac/popupLanguage.component';

import { sys_doi_tac_popUpLanguageComponent } from './sys_doi_tac/popupLanguage.component';
import { sys_doi_tac_indexComponent } from './sys_doi_tac/index.component';
import { sys_doi_tac_popUpAddComponent } from './sys_doi_tac/popupAdd.component';

import { sys_event_qa_popUpLanguageComponent } from './sys_event_qa/popupLanguage.component';
import { sys_event_program_popUpLanguageComponent } from './sys_event_program/popupLanguage.component';
import { sys_dien_gia_popUpLanguageComponent } from './sys_dien_gia/popupLanguage.component';

import { sys_like_san_pham_indexComponent } from './sys_like_san_pham/index.component';
import { sys_tinh_thanh_indexComponent } from './sys_tinh_thanh/index.component';
import { sys_tinh_thanh_popUpAddComponent } from './sys_tinh_thanh/popupAdd.component';
import { sys_quan_huyen_indexComponent } from './sys_quan_huyen/index.component';
import { sys_quan_huyen_popUpAddComponent } from './sys_quan_huyen/popupAdd.component';
import { MatRadioModule } from '@angular/material/radio';
import { sys_khuyen_mai_indexComponent } from './sys_khuyen_mai/index.component';
import { sys_khuyen_mai_popUpAddComponent } from './sys_khuyen_mai/popupAdd.component';
import { sys_dat_hang_popUpAddProductComponent } from './sys_dat_hang/popupAddProduct.component';
@NgModule({
    providers: [
        { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },


    ],

    declarations: [
        sys_dat_hang_popUpAddProductComponent,
        sys_khuyen_mai_popUpAddComponent,
        sys_khuyen_mai_indexComponent,
        sys_quan_huyen_popUpAddComponent,
        sys_quan_huyen_indexComponent,
        sys_tinh_thanh_popUpAddComponent,
        sys_tinh_thanh_indexComponent,
        sys_like_san_pham_indexComponent,
        sys_dat_hang_indexComponent,
        sys_dat_hang_popUpAddComponent,
        sys_cot_moc_su_kien_popupLanguageComponent,
        sys_dien_gia_popUpLanguageComponent,
        sys_event_program_popUpLanguageComponent,
        sys_event_qa_popUpLanguageComponent,
        sys_san_pham_popUpLanguageComponent,
        sys_doi_tac_indexComponent,
        sys_doi_tac_popUpAddComponent,
        sys_doi_tac_popUpLanguageComponent,
        sys_loai_doi_tac_indexComponent,
        sys_loai_doi_tac_popUpAddComponent,
        sys_loai_doi_tac_popUpLanguageComponent,
        sys_news_popUpLanguageComponent,
        sys_san_pham_popUpAddImageComponent,
        sys_event_popUpLanguageComponent,
        sys_nang_luc_hinh_anh_indexComponent,
        sys_nang_luc_hinh_anh_popUpAddComponent,
        sys_nang_luc_indexComponent,
        sys_nang_luc_popUpAddComponent,
        sys_anh_san_pham_indexComponent,
        sys_anh_san_pham_popUpAddComponent,
        sys_loai_san_pham_indexComponent,
        sys_loai_san_pham_popUpAddComponent,
        sys_san_pham_indexComponent,
        sys_san_pham_popUpAddComponent,

        sys_linh_vuc_indexComponent,
        sys_linh_vuc_popUpAddComponent,

        sys_loai_cau_hinh_popUpAddComponent,
        sys_loai_cau_hinh_indexComponent,
        sys_template_notification_popUpAddComponent,
        sys_template_notification_indexComponent,
        sys_type_notification_indexComponent,
        sys_type_notification_popUpAddComponent,


        sys_news_popUpCommentComponent,
        sys_type_notification_indexComponent,
        sys_type_notification_popUpAddComponent,

        sys_cau_hinh_thong_tin_website_indexComponent,
        sys_cau_hinh_thong_tin_website_popUpAddComponent,
        sys_cau_hinh_duyet_user_indexComponent,
        sys_cau_hinh_duyet_user_popUpAddComponent,
        sys_cau_hinh_duyet_su_kien_indexComponent,
        sys_cau_hinh_duyet_su_kien_popUpAddComponent,
        sys_event_nguoi_nhan_hoc_bong_indexComponent,
        sys_event_nguoi_nhan_hoc_bong_popUpAddComponent,
        sys_event_nha_tai_tro_indexComponent,
        sys_event_nha_tai_tro_popUpAddComponent,
        sys_cot_moc_su_kien_indexComponent,
        sys_cot_moc_su_kien_popUpAddComponent,
        sys_cau_hinh_anh_mac_dinh_indexComponent,
        sys_cau_hinh_anh_mac_dinh_popUpAddComponent,
        sys_nhom_hoi_dong_indexComponent,
        sys_nhom_hoi_dong_popUpAddComponent,
        sys_quyen_loi_indexComponent,
        sys_quyen_loi_popUpAddComponent,
        sys_news_popUpTinNoiBatComponent,
        sys_degree_indexComponent,
        sys_degree_popUpAddComponent,
        sys_user_typenews_popUpAddComponent,
        sys_tien_te_indexComponent,
        sys_tien_te_popUpAddComponent,
        sys_event_khach_moi_indexComponent,
        sys_event_khach_moi_popUpAddComponent,
        sys_dien_gia_indexComponent,
        sys_dien_gia_popUpAddComponent,
        sys_anh_noi_bat_su_kien_indexComponent,
        sys_anh_noi_bat_su_kien_popUpAddComponent,
        sys_tu_ngu_cam_indexComponent,
        sys_tu_ngu_cam_popUpAddComponent,
        sys_nhom_thu_vien_hinh_anh_popUpAddComponent,
        sys_nhom_thu_vien_hinh_anh_indexComponent,
        sys_cuu_sinh_vien_indexComponent,
        sys_cuu_sinh_vien_popUpAddComponent,
        sys_video_indexComponent,
        sys_video_popUpAddComponent,
        sys_thu_vien_hinh_anh_indexComponent,
        sys_thu_vien_hinh_anh_popUpAddComponent,
        sys_loai_nganh_nghe_indexComponent,
        sys_loai_nganh_nghe_popUpAddComponent,
        sys_nhom_nganh_nghe_indexComponent,
        sys_nhom_nganh_nghe_popUpAddComponent,
        sys_khu_vuc_indexComponent,
        sys_khu_vuc_popUpAddComponent,
        sys_thong_tin_tuyen_dung_indexComponent,
        sys_thong_tin_tuyen_dung_popUpAddComponent,
        sys_vi_tri_tuyen_dung_indexComponent,
        sys_vi_tri_tuyen_dung_popUpAddComponent,

        sys_event_thong_bao_dang_kyComponent,
        sys_event_registerComponent,
        sys_event_popupFileDinhKemComponent,
        sys_event_email_indexComponent,
        sys_event_email_popUpAddComponent,
        sys_event_contact_indexComponent,
        sys_event_contact_popUpAddComponent,
        sys_event_popUpAnhDinhKemSuKienComponent,
        sys_event_participate_indexComponent,
        sys_event_participate_popUpAddComponent,
        sys_event_participate_popUpNhomNguoiDungComponent,

        sys_contactus_popUpAddComponent,
        sys_contactus_indexComponent,
        sys_banner_indexComponent,
        sys_banner_popUpAddComponent,
        sys_event_qa_indexComponent,
        sys_event_qa_popUpAddComponent,
        sys_cau_hinh_thong_tin_indexComponent,
        sys_cau_hinh_thong_tin_popUpAddComponent,
        sys_type_mail_indexComponent,
        sys_type_mail_popUpAddComponent,
        sys_nghi_quyet_indexComponent,
        sys_nghi_quyet_popUpAddComponent,
        sys_event_indexComponent,
        sys_event_popUpAddComponent,
        sys_event_program_indexComponent,
        sys_event_program_popUpAddComponent,

        sys_nhom_thanh_vien_indexComponent,
        sys_nhom_thanh_vien_popUpAddComponent,
        sys_thanh_vien_thuoc_nhom_indexComponent,

        sys_user_typenews_indexComponent,
        sys_user_company_indexComponent,

        sys_khoa_indexComponent,
        sys_khoa_popUpAddComponent,

        sys_approval_news_indexComponent,
        sys_approval_news_popUpAddComponent,

        doc_tailieu_view_file_onlineComponent,
        UploadDirective,
        changePassComponent,
        sys_approval_config_indexComponent,
        sys_approval_config_popUpAddComponent,
        sys_group_user_indexComponent,
        sys_group_user_popUpAddComponent,
        sys_user_indexComponent,
        sys_user_popUpAddComponent,

        sys_department_indexComponent,
        sys_department_popUpAddComponent,
        sys_job_title_indexComponent,
        sys_job_title_popUpAddComponent,

        sys_help_detail_indexComponent,
        sys_help_indexComponent,
        sys_help_popUpAddComponent,
        sys_help_detail_popUpAddComponent,
        sys_approval_user_indexComponent,
        sys_approval_user_popUpAddComponent,

        sys_template_mail_indexComponent,
        sys_template_mail_popUpAddComponent,
        sys_company_indexComponent,
        sys_company_popUpAddComponent,

        sys_group_news_indexComponent,
        sys_group_news_popUpAddComponent,
        sys_type_news_indexComponent,
        sys_type_news_popUpAddComponent,
        sys_news_indexComponent,
        sys_news_popUpAddComponent,
        ProfileComponent
    ],
    imports: [
        RouterModule.forChild(systemsRoutes),
        FullCalendarModule,
        MatRadioModule,
        FuseDateRangeModule,
        SweetAlert2Module,
        MatButtonModule,
        MatChipsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatPaginatorModule,
        MatTreeModule,
        MatRippleModule,
        MatSelectModule,
        MatBadgeModule,
        MatCheckboxModule,
        MatSortModule,
        MatSnackBarModule,
        MatTabsModule,
        MatDialogModule,
        DataTablesModule,
        MatCardModule,
        MatDatepickerModule,
        NgxChartsModule,
        NgxMatSelectSearchModule,
        MatAutocompleteModule,
        MatTooltipModule,
        commonModule,
        common_pageModule,
        EditorModule,
        MatListModule,
        MatSlideToggleModule,
        TranslocoCoreModule,
        CommonModule,
        FormsModule,
        FusePipesModule,
        NzEmptyModule,
        MatButtonToggleModule,
        TranslocoModule,
        NzIconModule,
        MatProgressBarModule,
        SharedModule,
        NgApexchartsModule,
        NgCircleProgressModule.forRoot({
            // set defaults here
            radius: 100,
            outerStrokeWidth: 16,
            innerStrokeWidth: 8,
            outerStrokeColor: "#78C000",
            innerStrokeColor: "#C7E596",
            animationDuration: 300,

        }),
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyA1T28g5sfWOnkimOZBJutBACuO91CC4o0',
            libraries: ['places']
        }),
        ReactiveFormsModule,
        NgxMatTimepickerModule,
        MatDividerModule,
        NzProgressModule,
        MatSidenavModule,
        FuseMasonryModule,
        EditorModule,
        CalendarModule,
        DragDropModule
    ],
    exports: [],
})
export class SystemModule { }
