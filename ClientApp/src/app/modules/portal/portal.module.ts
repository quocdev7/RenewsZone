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

import { portal_ngay_hoi_viec_lam_indexComponent } from './portal_ngay_hoi_viec_lam/index.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgxMatSelectSearchModule } from "ngx-mat-select-search";

import { MatCardModule } from "@angular/material/card";

import { MatCheckboxModule } from "@angular/material/checkbox";

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
import { portalRoutes } from "./portal.routing";

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


import { FuseDateRangeModule } from '@fuse/components/date-range';
import { FullCalendarModule } from '@fullcalendar/angular';
import { DATE_FORMATS } from 'app/Basecomponent/config';

import { MatCarouselModule } from '@ngmodule/material-carousel';

import { PortalTypeNewsComponent } from './news/type_news.component';
import { PortalNewsComponent } from './news/news.component';
import { PortalNewsByUserComponent } from './news/news_by_user.component';
import { PortalNewsDetailComponent } from './news/news_detail.component';
import { PortalProfileComponent } from './profile/profile.component';
import { PortalNewsByUserDetailComponent } from './news/news_by_user_detail.component';

import { homepageIndexComponent } from './homepage/index.component';


import { PortalPrivacyComponent } from './privacy/privacy.component';

import { PortalTermComponent } from './term/term.component';

//import { PortalContactUsComponent } from './contactus/contactus.component';
import { PortalContactUsComponent } from './contact_us/contact_us.component';

import { portal_news_popupAddCommentComponent } from './news/popupAddComment.component';

import { PortalEventComponent } from './event/event.component';

import { popupVeMoiComponent } from './event/popupVeMoi.component';

import { portal_homepage_popupShareComponent } from './homepage/popupShare.component';
import { EventDetailComponent } from './event/eventDetail.component';


import { popupAddCertificateComponent } from './profile/popupAddCertificate.component';

import { popupAddExperienceComponent } from './profile/popupAddExperience.component';
import { popupAddSuccessComponent } from './profile/popupAddSuccess.component';
import { popupAddWorkHistoryComponent } from './profile/popupAddWorkHistory.component';
import { popupInfoEditComponent } from './profile/popupInfoEdit.component';
import { popupSocialEditComponent } from './profile/popupSocialEdit.component';
import { popupMainImageComponent } from './profile/popupMainImage.component';
import { popupAvatarComponent } from './profile/popupAvatar.component';
import { popupAddEducationComponent } from './profile/popupAddEducation.component';
import { portal_person_news_popUpAddComponent } from './person_news/popupAdd.component';
import { portal_person_news_indexComponent } from './person_news/index.component';


//add event_news
import { portal_person_event_popUpAddComponent } from './person_event/popupAdd.component';
import { portal_person_event_indexComponent } from './person_event/index.component';
// import { portal_person_event_popUpAnhDinhKemSuKienComponent } from './person_event/popupAnhDinhKemSuKien.component';




import { portal_approved_event_popUpAddComponent } from './approved_event/popupAdd.component';
import { portal_approved_person_news_popUpAddComponent } from './approved_person_news/popupAdd.component';
import { portal_person_news_popUpViewComponent } from './person_news/popupView.component';
import { portal_approved_person_news_indexComponent } from './approved_person_news/index.component';
import { portal_approved_event_indexComponent } from './approved_event/index.component';




import { portal_approved_user_indexComponent } from './approved_user/index.component';
import { portal_approval_user_popUpAddComponent } from './approved_user/popupAdd.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { SwiperModule } from 'swiper/angular';
import { animated_digitComponent } from './homepage/animated_digit.component';
import { portal_homepage_popupViewImgComponent } from './homepage/popupViewImg.component';


import { PortalProfileUserComponent } from './profile_user/index.component';
import { popupAddUngTuyenComponent } from './profile/popupAddUngTuyen.component';

import { homepageSearchComponent } from './homepage/searchpage.component';
import { person_indexComponent } from './person/index.component';
import { popupConfirmComponent } from './event/popupConfirm.component';
import { person_myeventComponent } from './person/myevent.component';
import { person_settingComponent } from './person/setting.component';
import { about_us_detailComponentsComponent } from './about_us/detail.component';
import { about_us_indexComponentsComponent } from './about_us/index.component';
import { about_us_so_do_to_chucComponentsComponent } from './about_us/sodotochuc.component';
import { person_ban_beComponent } from './person/ban_be.component';

import { person_print_previewComponent } from './person/print_preview.component';
import { about_us_cot_mocComponentsComponent } from './about_us/cotmoc.component';
import { quyen_rieng_tuComponent } from './person/quyen_rieng_tu.component';
import { person_popupAddBanBeComponent } from './person/popupAddBanBe.component';


import { portal_product_detailComponent } from './product/detail.component';

import { portal_nang_lucComponent } from './nangluc/nang_luc.component';

import { portal_gamabookComponent } from './gamabook/index.component';
import { portal_productComponent } from './product/index.component';
import { portal_product_newComponent } from './product/product_new.component';
import { popup_mua_hangComponent } from './product/popup_mua_hang.component';
import { linh_vuc_detailComponent } from './linh_vuc/linh_vuc_detail.component';
import { portal_nang_luc_popupViewImgComponent } from './nangluc/popupViewImg.component';
import { portal_software_detailComponent } from './product/detail_software.component';
import { portal_doi_tac_indexComponent } from './doi_tac/index.component';
import { portal_event_popupViewImgComponent } from './event/popupViewImg.component';


import { scan_checkinComponent } from './event/scan_checkin.component';
import { user_viewComponent } from './event/user_view.component';
import { progress_spinnerComponent } from 'app/layout/common/loading/index.component';
import { update_navigationService } from '@fuse/services/update_navigation.service';
import { PortalQuyDinhDoiTraHoanTienComponent } from './privacy/quy_dinh_doi_tra_hoan_tien.component';
import { PortalChinhSachVanChuyenGiaoNhanComponent } from './privacy/chinh_sach_van_chuyen_giao_nhan.component';
import { PortalChinhSachBaoVeThongTinCaNhaComponent } from './privacy/chinh_sach_bao_ve_thong_tin_ca_nhan.component .component';
import { PaymentCartComponent } from './payment_cart/index.component';
import { MatRadioModule } from '@angular/material/radio';
import { SendCartComponent } from './payment_cart/send-card.component';
@NgModule({
    providers: [
        update_navigationService,
        { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },

    ],
    declarations: [
        SendCartComponent,
        PaymentCartComponent,
        portal_gamabookComponent,
        PortalChinhSachBaoVeThongTinCaNhaComponent,
        PortalChinhSachVanChuyenGiaoNhanComponent,
        PortalQuyDinhDoiTraHoanTienComponent,
        popup_mua_hangComponent,
        portal_ngay_hoi_viec_lam_indexComponent,
        scan_checkinComponent,
        progress_spinnerComponent,
        user_viewComponent,
        portal_doi_tac_indexComponent,
        portal_nang_luc_popupViewImgComponent,
        // portal_person_event_popUpAnhDinhKemSuKienComponent,
        portal_nang_lucComponent,
        linh_vuc_detailComponent,
        portal_product_newComponent,
        portal_product_detailComponent,
        portal_software_detailComponent,
        portal_productComponent,
        portal_person_event_indexComponent,
        portal_person_event_popUpAddComponent,
        portal_approved_event_popUpAddComponent,
        portal_approved_event_indexComponent,
        person_popupAddBanBeComponent,
        quyen_rieng_tuComponent,
        person_print_previewComponent,
        person_ban_beComponent,
        about_us_cot_mocComponentsComponent,
        about_us_so_do_to_chucComponentsComponent,
        about_us_indexComponentsComponent,
        about_us_detailComponentsComponent,
        person_settingComponent,
        popupConfirmComponent,
        person_indexComponent,
        person_myeventComponent,
        homepageSearchComponent,
        popupAddUngTuyenComponent,
        PortalProfileUserComponent,
        portal_person_news_popUpViewComponent,
        animated_digitComponent,
        portal_homepage_popupViewImgComponent,
        portal_approval_user_popUpAddComponent,
        portal_approved_user_indexComponent,
        portal_approved_person_news_popUpAddComponent,
        portal_approved_person_news_indexComponent,
        popupAddEducationComponent,
        popupAvatarComponent,
        popupMainImageComponent,
        portal_event_popupViewImgComponent,
        popupAddCertificateComponent,
        popupAddExperienceComponent,
        popupAddSuccessComponent,
        popupAddWorkHistoryComponent,
        portal_homepage_popupShareComponent,
        popupVeMoiComponent,
        popupInfoEditComponent,
        popupSocialEditComponent,
        PortalEventComponent,
        EventDetailComponent,
        portal_news_popupAddCommentComponent,
        PortalContactUsComponent,

        PortalTermComponent,
        PortalPrivacyComponent,

        PortalTypeNewsComponent,
        PortalNewsDetailComponent,
        PortalNewsComponent,

        PortalProfileComponent,
        //UploadDirective,
        PortalNewsByUserComponent,
        PortalNewsByUserDetailComponent,
        homepageIndexComponent,
        portal_person_news_popUpAddComponent,
        portal_person_news_indexComponent,
    ],
    imports: [
        RouterModule.forChild(portalRoutes),
        MatProgressSpinnerModule,
        ImageCropperModule,
        FullCalendarModule,
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
        DragDropModule,
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
        MatRadioModule,
        NgCircleProgressModule.forRoot({
            // set defaults here
            radius: 100,
            outerStrokeWidth: 16,
            innerStrokeWidth: 8,
            outerStrokeColor: "#78C000",
            innerStrokeColor: "#C7E596",
            animationDuration: 300,

        }),
        MatCarouselModule.forRoot(),
        ReactiveFormsModule,
        NgxMatTimepickerModule,
        MatDividerModule,
        NzProgressModule,
        MatSidenavModule,
        FuseMasonryModule,
        EditorModule,
        CalendarModule,
        SwiperModule
    ],
    exports: [],
})
export class PortalModule { }
