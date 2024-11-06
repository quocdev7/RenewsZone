import { compactNavigation, defaultNavigation, futuristicNavigation, horizontalNavigation } from 'app/mock-api/common/navigation/data';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { InitialData } from 'app/app.types';
import { cloneDeep } from 'lodash-es';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMockApiService } from '@fuse/lib/mock-api';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { of } from 'rxjs';
import { AuthService } from './core/auth/auth.service';
import { AngularFirestore } from '@angular/fire/firestore';
import { UserService } from './core/user/user.service';


@Injectable({
    providedIn: 'root'
})
export class InitialDataResolver implements Resolve<any> {
    public menu: any;
    private _defaultNavigation: FuseNavigationItem[] = [];
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient,
        private _fuseMockApiService: FuseMockApiService,
        public _translocoService: TranslocoService,
        private db: AngularFirestore,
        private _authService: AuthService) {

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Use this resolver to resolve initial mock-api for the application
     *
     * @param route
     * @param state
     */

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<InitialData> {
        // Fork join multiple API endpoint calls to wait all of them to finish
        return forkJoin([
            this._httpClient.get<any>('api/common/messages'),
            this._httpClient.post('/sys_home.ctr/getModule/', {}).pipe(
                switchMap((resp: any) => {
                    this.menu = resp;
                    this._defaultNavigation = [];
                    var menu_user_managerment = this.menu.filter(d => this.checkInclueFn(d.menu.id, ['sys_cau_hinh_duyet_user', 'sys_user', 'sys_group_user', 'sys_approval_user', 'sys_nhom_thanh_vien', 'sys_thanh_vien_thuoc_nhom']));
                    var menu_id = [];
                    if (menu_user_managerment.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "user_managerment",
                            module: 'system',
                            title: "",
                            translate: "user_managerment",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_user_managerment.length; i++) {
                            menu_id.push(menu_user_managerment[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_user_managerment[i].menu.id,
                                    badge: menu_user_managerment[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_user_managerment[i].menu.url,
                                    translate: menu_user_managerment[i].menu.translate,
                                    icon: menu_user_managerment[i].menu.icon,
                                    type: 'basic',

                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }


                    var menu_san_pham_managerment = this.menu.filter(d => this.checkInclueFn(d.menu.id, ['sys_loai_san_pham', 'sys_san_pham', 'sys_khuyen_mai', 'sys_dat_hang', 'sys_like_san_pham']));

                    if (menu_san_pham_managerment.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "san_pham_managerment",
                            module: 'system',
                            title: "",
                            translate: "san_pham_managerment",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_san_pham_managerment.length; i++) {
                            menu_id.push(menu_san_pham_managerment[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_san_pham_managerment[i].menu.id,
                                    badge: menu_san_pham_managerment[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_san_pham_managerment[i].menu.url,
                                    translate: menu_san_pham_managerment[i].menu.translate,
                                    icon: menu_san_pham_managerment[i].menu.icon,
                                    type: 'basic',

                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }



                    var menu_news_managerment = this.menu.filter(d => this.checkInclueFn(d.menu.id, ['sys_tu_ngu_cam', 'sys_group_news', 'sys_type_news', 'sys_news', 'sys_approval_news', 'sys_user_typenews']));

                    if (menu_news_managerment.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "news_managerment",
                            module: 'system',
                            title: "",
                            translate: "news_managerment",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_news_managerment.length; i++) {
                            menu_id.push(menu_news_managerment[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_news_managerment[i].menu.id,
                                    badge: menu_news_managerment[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_news_managerment[i].menu.url,
                                    translate: menu_news_managerment[i].menu.translate,
                                    icon: menu_news_managerment[i].menu.icon,
                                    type: 'basic',

                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }



                    var menu_event_managerment = this.menu.filter(d => this.checkInclueFn(d.menu.id, ['sys_cau_hinh_duyet_su_kien', 'sys_hoc_bong', 'sys_nguoi_nhan_tai_tro', 'sys_event_khach_moi', 'sys_event', 'sys_event_program',
                        'sys_event_contact', 'sys_event_participate',
                        'sys_event_qa', 'sys_anh_noi_bat_su_kien', 'sys_dien_gia']));

                    if (menu_event_managerment.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "event_managerment",
                            module: 'system',
                            title: "",
                            translate: "event_managerment",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_event_managerment.length; i++) {
                            menu_id.push(menu_event_managerment[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_event_managerment[i].menu.id,
                                    badge: menu_event_managerment[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_event_managerment[i].menu.url,
                                    translate: menu_event_managerment[i].menu.translate,
                                    icon: menu_event_managerment[i].menu.icon,
                                    type: 'basic',

                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }

                    var menu_home_page = this.menu.filter(d => this.checkInclueFn(d.menu.id, ['sys_video', 'sys_nhom_thu_vien_hinh_anh', 'sys_thu_vien_hinh_anh',
                        'sys_banner', 'sys_quyen_loi']));

                    if (menu_home_page.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "menu_home_page",
                            module: 'system',
                            title: "",
                            translate: "portal.homepage",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_home_page.length; i++) {
                            menu_id.push(menu_home_page[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_home_page[i].menu.id,
                                    badge: menu_home_page[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_home_page[i].menu.url,
                                    translate: menu_home_page[i].menu.translate,
                                    icon: menu_home_page[i].menu.icon,
                                    type: 'basic',

                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }


                    var menu_about_us = this.menu.filter(d => this.checkInclueFn(d.menu.id, ['sys_nhom_hoi_dong', 'sys_cuu_sinh_vien', 'sys_cot_moc_su_kien', 'sys_nghi_quyet', 'sys_cau_hinh_thong_tin']));

                    if (menu_about_us.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "menu_about_us",
                            module: 'system',
                            title: "",
                            translate: "portal.aboutus",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_about_us.length; i++) {
                            menu_id.push(menu_about_us[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_about_us[i].menu.id,
                                    badge: menu_about_us[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_about_us[i].menu.url,
                                    translate: menu_about_us[i].menu.translate,
                                    icon: menu_about_us[i].menu.icon,
                                    type: 'basic',

                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }


                    var menu_other = this.menu.filter(d => !this.checkInclueFn(d.menu.id, menu_id));
                    var menu_id = [];
                    if (menu_other.length > 0) {
                        var item_menu: FuseNavigationItem =
                        {
                            id: "other_menu",
                            module: 'system',
                            title: "",
                            translate: "system_category",
                            type: 'group',
                            icon: "apps",
                            children: []
                        };
                        for (let i = 0; i < menu_other.length; i++) {
                            menu_id.push(menu_other[i].menu.id);
                            item_menu.children.push(
                                {
                                    id: menu_other[i].menu.id,
                                    badge: menu_other[i].menu.badge_approval,
                                    module: 'system',
                                    title: '',
                                    link: menu_other[i].menu.url,
                                    translate: menu_other[i].menu.translate,
                                    icon: menu_other[i].menu.icon,
                                    type: 'basic',
                                });

                        }
                        this._defaultNavigation.push(item_menu);
                    }
                    console.log(this._defaultNavigation)
                    // Return the response
                    resp =
                    {
                        compact: this._defaultNavigation,
                        default: this._defaultNavigation,
                        futuristic: this._defaultNavigation,
                        horizontal: this._defaultNavigation
                    };
                    return of(resp);
                })
            ),
            this._httpClient.get<any>('api/common/notifications'),
            this._httpClient.get<any>('api/common/shortcuts'),
            this._httpClient.post<any>('sys_home.ctr/checkLogin', { accessToken: this._authService.accessToken }).pipe(
                switchMap((resp: any) => {
                    resp = JSON.parse(sessionStorage.getItem('user'));
                    return of(resp);
                }))
        ]).pipe(
            map(([messages, navigation, notifications, shortcuts, user]) => ({
                messages,
                navigation: {
                    compact: navigation.compact,
                    default: navigation.default,
                    futuristic: navigation.futuristic,
                    horizontal: navigation.horizontal
                },
                notifications,
                shortcuts,
                user
            })
            )
        );
    }
    // getMessageFromFireBase(): Observable<IMessage[]>{
    //     let id_user  = JSON.parse( sessionStorage.getItem('user')).id??"" as string
    //     let domain = window.location.host;
    //     console.log(domain,id_user)
    //     return this.db.collection<IMessage>('/notification', ref => ref.where('domain','==', domain).where('user_id_receive','==',id_user).orderBy('send_time_index', 'desc')).get().pipe(
    //         map(querySnapshot => querySnapshot.docs.map(doc => {console.log(doc.data()); return doc.data()}))
    //      );
    // }
    checkInclueFn(listInclude: any, stringValue: any): boolean {
        let isInclude = false;
        stringValue.every(element => {
            if (listInclude === element) {
                isInclude = true;
                return false;
            }
            else return true
        });
        return isInclude
    }
}

