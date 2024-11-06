/* eslint-disable @typescript-eslint/explicit-function-return-type */
import { ActivatedRouteSnapshot, Route } from '@angular/router';
import { AuthGuard } from 'app/core/auth/guards/auth.guard';
import { NoAuthGuard } from 'app/core/auth/guards/noAuth.guard';
import { LayoutComponent } from 'app/layout/layout.component';
import { InitialDataResolver } from 'app/app.resolvers';

import { PortalProfileComponent } from './profile/profile.component';

import { portal_ngay_hoi_viec_lam_indexComponent } from './portal_ngay_hoi_viec_lam/index.component';

import { PortalTypeNewsComponent } from './news/type_news.component';
import { PortalNewsComponent } from './news/news.component';
import { PortalNewsByUserComponent } from './news/news_by_user.component';
import { PortalNewsDetailComponent } from './news/news_detail.component';
import { PortalNewsByUserDetailComponent } from './news/news_by_user_detail.component';
import { homepageIndexComponent } from './homepage/index.component';
import { PortalPrivacyComponent } from './privacy/privacy.component';

import { PortalTermComponent } from './term/term.component';



import { PortalContactUsComponent } from './contact_us/contact_us.component';


import { PortalEventComponent } from './event/event.component';
import { EventDetailComponent } from './event/eventDetail.component';
import { portal_person_news_indexComponent } from './person_news/index.component';



import { portal_approved_person_news_indexComponent } from './approved_person_news/index.component';

import { portal_approved_user_indexComponent } from './approved_user/index.component';

import { PortalProfileUserComponent } from './profile_user/index.component';


import { homepageSearchComponent } from './homepage/searchpage.component';
import { person_indexComponent } from './person/index.component';
import { person_myeventComponent } from './person/myevent.component';
import { person_settingComponent } from './person/setting.component';
import { about_us_detailComponentsComponent } from './about_us/detail.component';
import { about_us_indexComponentsComponent } from './about_us/index.component';

import { person_print_previewComponent } from './person/print_preview.component';
import { portal_career_indexComponent } from './career/index.component';
import { quyen_rieng_tuComponent } from './person/quyen_rieng_tu.component';
import { portal_doi_tac_indexComponent } from './doi_tac/index.component';

// @formatter:off
// tslint:disable:max-line-length6
import { portal_product_newComponent } from './product/product_new.component';
import { portal_product_detailComponent } from './product/detail.component';
import { portal_software_detailComponent } from './product/detail_software.component';
import { portal_productComponent } from './product/index.component';

import { portal_nang_lucComponent } from './nangluc/nang_luc.component';
import { linh_vuc_detailComponent } from './linh_vuc/linh_vuc_detail.component';
import { Injectable } from '@angular/core';
import { UserService } from 'app/core/user/user.service';
import { map } from 'rxjs/operators';
import { PortalQuyDinhDoiTraHoanTienComponent } from './privacy/quy_dinh_doi_tra_hoan_tien.component';
import { PortalChinhSachVanChuyenGiaoNhanComponent } from './privacy/chinh_sach_van_chuyen_giao_nhan.component';
import { PortalChinhSachBaoVeThongTinCaNhaComponent } from './privacy/chinh_sach_bao_ve_thong_tin_ca_nhan.component .component';
import { portal_gamabookComponent } from './gamabook/index.component';
import { PaymentCartComponent } from './payment_cart/index.component';
import { SendCartComponent } from './payment_cart/send-card.component';


//Xử lý title :id
@Injectable({ providedIn: 'root' })
export class UserTitleResolver {
    constructor(private userService: UserService) { }
    resolve(route: ActivatedRouteSnapshot) {
        const userId = route.paramMap.get('id');
        return this.userService
        //.getUser(userId).pipe(map((user : IUs)))
    }
}

export const portalRoutes: Route[] = [
    // Redirect empty path to '/example'
    {
        path: 'gamabook',
        component: portal_gamabookComponent,
    },
    {
        path: 'payment-cart',
        component: PaymentCartComponent,
    },
    {
        path: 'send-cart/:id',
        component: SendCartComponent,
    },
    {
        path: 'khao-sat-sinh-vien',
        component: portal_ngay_hoi_viec_lam_indexComponent,
    },
    {
        path: 'portal_doi_tac_index',
        component: portal_doi_tac_indexComponent,
    },
    {
        path: 'portal_nang_luc',
        component: portal_nang_lucComponent,
    },
    {
        path: 'portal_software_detail/:id',
        component: portal_software_detailComponent
    },
    {
        path: 'portal_product_detail/:id',
        component: portal_product_detailComponent
    },

    {
        path: 'portal-linh-vuc-detail/:id',
        component: linh_vuc_detailComponent
    },
    {
        path: 'portal_product_new/:id',
        component: portal_product_newComponent,
    },
    {
        path: 'portal_product',
        component: portal_productComponent,
    },
    {
        path: 'person_quyen_rieng_tu',
        component: quyen_rieng_tuComponent
    },
    {
        path: 'portal_career_index',
        component: portal_career_indexComponent
    },
    {
        path: 'person_print_preview',
        component: person_print_previewComponent
    },
    {
        path: 'about_us_index',
        component: about_us_indexComponentsComponent,
    },
    {
        path: 'person_setting',
        component: person_settingComponent
    },
    {
        path: 'person_myevent',
        component: person_myeventComponent
    },
    {
        path: 'person_index',
        component: person_indexComponent
    },
    {
        path: 'homepageSearch/:id',
        component: homepageSearchComponent,

    },
    {
        path: 'eventDetail/:id',
        component: EventDetailComponent,
    },

    {
        path: 'portal-event',
        component: PortalEventComponent,
    },

    {
        path: 'portal-contact-us',
        component: PortalContactUsComponent,
    },

    {
        path: 'portal-term',
        component: PortalTermComponent
    },
    {
        path: 'portal-privacy',
        component: PortalPrivacyComponent
    },
    {
        path: 'portal_quy_dinh_doi_tra_hoan_tien',
        component: PortalQuyDinhDoiTraHoanTienComponent
    },
    {
        path: 'portal_chinh_sach_van_chuyen_giao_nhan',
        component: PortalChinhSachVanChuyenGiaoNhanComponent
    },
    {
        path: 'portal_chinh_sach_bao_ve_thong_tin_ca_nhan',
        component: PortalChinhSachBaoVeThongTinCaNhaComponent
    },
    {
        path: 'portal-news/:id',
        component: PortalNewsComponent
    },
    {
        path: 'portal-news-detail/:id',
        component: PortalNewsDetailComponent,
    },
    {
        path: 'portal-type-news/:id',
        component: PortalTypeNewsComponent,
    },
    {
        path: 'portal-news-by-user/:id',
        component: PortalNewsByUserComponent
    },
    {
        path: 'portal-news-by-user-detail/:id',
        component: PortalNewsByUserDetailComponent
    },
    {
        path: 'portal-profile',
        component: PortalProfileComponent
    },
    {
        path: 'portal-profile-user/:id',
        component: PortalProfileUserComponent
    },
    {
        path: 'portal_person_news_index',
        component: portal_person_news_indexComponent
    },
    {
        path: 'portal_approved_person_news_index',
        component: portal_approved_person_news_indexComponent
    },
    {
        path: 'homepage-index',
        component: homepageIndexComponent,
    },

    {
        path: 'portal_approved_user_index',
        component: portal_approved_user_indexComponent
    },


];
