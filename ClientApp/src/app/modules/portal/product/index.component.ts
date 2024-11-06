import { Component, Inject, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';

import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { Location } from '@angular/common';
import { DOCUMENT } from '@angular/common';


import { SeoService } from '@fuse/services/seo.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';



import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';


import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { SwiperComponent } from 'swiper/angular';
import * as AOS from 'aos';
import { Title } from '@angular/platform-browser';

// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);



@Component({
    selector: 'portal_product',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class portal_productComponent implements OnInit {
    /**
     * Constructor
     */
    @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
    slideNext() {
        this.swiper.swiperRef.slideNext(100);
    }
    slidePrev() {
        this.swiper.swiperRef.slidePrev(100);
    }
    public loading:any = false;
    
    public activeLang: any;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public list_san_pham: any = [];
    public list_mtb: any = [];
    public list_pm: any = [];
    public record: any = null;
    public id_san_pham: any;
    public id_group_news: any;
    public search_key: any;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private titleService: Title,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private location: Location,
        private dialog: MatDialog,        private seoService: SeoService,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient
        , public _translocoService: TranslocoService,) {

    }

    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }

    ngOnInit() {
        AOS.init({
            duration: 1000
        });
        var title = 'Xelex - Sản phẩm';
        var metaTag = [
            { name: 'twitter:card', content: 'summary' },
            { property: 'og:type', content: 'article' },
            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal_product' },
            { property: 'og:title', content: 'Xelex' },
            { property: 'og:image', content: 'assets/images/logo/worldsoft.png' },
            { property: 'og:description', content: 'Sản phẩm' },


        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => {
            this.id_group_news = params["id"];
            this.loadSanPham();
        });
        this.setDocTitle(this._translocoService.translate('portal.product') + ' - Xelex')
    }


    loadSanPham(): void {
        this.loading = true;
        this.http
            .post('/sys_san_pham.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_san_pham = resp;
                this.list_mtb = this.list_san_pham.filter(q => q.ma_loai == "MTB");
                this.list_pm = this.list_san_pham.filter(q => q.ma_loai == "PM");
                this.loading = false;
            });

    }
    // gotoProductDetail(id_san_pham): void {
    //     const url = '/portal_product_detail/' + id_san_pham;
    //     this.router.navigateByUrl(url);
    // }
    gotoProductDetail(id_san_pham): void {
        this.http
            .post('/sys_san_pham.ctr/get_title_san_pham/', {
                id: id_san_pham
            }).subscribe(resp => {
                var title = resp;
                const url = '/sys_post.ctr/san_pham?tieu_de=' +title +"&id="+ id_san_pham + "&t=" + "3" ;
                window.location.href=url
            });

    }
    gotoSoftWareDetail(id_san_pham): void {
        const url = '/portal_software_detail/' + id_san_pham;
        this.router.navigateByUrl(url);
    }
    // checkinpage(): void {

    //     this.loading = true;

    //     this.http
    //     .post('/sys_scan_checkin.ctr/phoneemail/', {
    //         id: this.id_event,
    //         ph:this.search
    //                 }
    //             ).subscribe(resp => {
    //                 var data: any;
    //                 data = resp;
    //                 this.eventinfo = data.eventinfo;
    //                 this.userinfo = data.userinfo;
    //                 const url = '/user_view/' + this.eventinfo.id +"/" + this.userinfo.id;
    //                 this.router.navigateByUrl(url);
    //             },
    //                 error => {
    //                     if (error.status == 400) {
    //                         this.errorModel = error.error;

    //                     }
    //                     if (error.status == 403) {

    //                         Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
    //                     }
    //                     this.loading = false;

    //                 }
    //             );


    // }
}
