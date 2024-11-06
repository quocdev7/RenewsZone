import { Component, ViewEncapsulation, OnInit, Inject, ViewChild } from '@angular/core';
import { TranslocoService, AvailableLangs } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { concatMap, takeUntil } from 'rxjs/operators';
import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { Subject } from 'rxjs';
import { DOCUMENT } from '@angular/common';
import { portal_homepage_popupShareComponent } from './popupShare.component';
import Swal from 'sweetalert2';
import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { SwiperComponent } from 'swiper/angular';
import { portal_homepage_popupViewImgComponent } from './popupViewImg.component';


import { SeoService } from '@fuse/services/seo.service';
import * as AOS from 'aos';
import { Title } from '@angular/platform-browser';

// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);

// install Swiper modules

@Component({
    selector: 'homepage-index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class homepageIndexComponent implements OnInit {

    @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;

    slideNext() {
        this.swiper.swiperRef.slideNext(100);
    }
    slidePrev() {
        this.swiper.swiperRef.slidePrev(100);
    }
    public loading: any = false;

    public activeLang: any;
    public show_tuyen_dung: any = false;
    public lst_events: any = [];
    public lst_tuyen_dung: any = [];
    public link_video: any = [];
    public lst_card: any = [];
    public lst_cuu_sinh_vien: any = [];
    public lst_video: any = [];
    public lst_hinh_anh: any = [];
    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public lst_group_type_news: any = [];
    public lst_group_type_news_detail: any = [];
    public is_login: any = false;
    public selectedFilter: any;
    public slides_web: any = [];
    public slides_mobile: any = [];
    public lst_banner: any = [];
    public lst_hot_news: any = [];
    public stats: any = {
        count_event: 0,
        count_post: 0,
        count_people: 0,
    };
    pagination = {
        clickable: true,
    };


    public lst_san_pham: any = [];
    public lst_linh_vuc: any = [];
    public id_group_news: any;


    public products: any = [];

    constructor(
        @Inject(DOCUMENT) private document: Document,
        private titleService: Title,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService, private seoService: SeoService,

        public http: HttpClient, private dialog: MatDialog
        , public _translocoService: TranslocoService,) {
        this.id_group_news = "";




        this._authService.check().subscribe((data: any) => this.is_login = data);
        



    }


    thamgia(record): void {
        if (this.is_login == false) {
            const url = '/sign-in'
            this.router.navigateByUrl(url);
        }
        else {
            Swal.fire({
                title: this._translocoService.translate('portal.paticipate_event'),
                text: this._translocoService.translate('areYouSure'),
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                        .post('/sys_event.ctr/update_status_event/', {
                            event_id: record.db.id,
                            status: 3
                        }
                        ).subscribe(resp => {
                            record.check_in_status = 3;
                            Swal.fire("Đăng ký tham gia thành công", '', 'success');
                        });
                }
            })
        }


    }
    gotoNewsEvenPage(id_event): void {
        // const url = '/eventDetail/' + id_event;
        // this.router.navigateByUrl(url);
        this.http
            .post('/sys_event.ctr/get_title_event/', {
                id_event: id_event
            }).subscribe(resp => {
                var title_event = resp;
                const url = '/sys_post.ctr/event_detail?tieu_de=' + title_event + "&id=" + id_event + "&t=" + "2";
                window.location.href = url
            });
    }

    openShareNews(id): void {
        let url = 'https://' + this.document.location.hostname + '/portal-news-detail/' + id;
        var that = this;
        const dialogRef = this.dialog.open(portal_homepage_popupShareComponent, {
            disableClose: true,

            data: {
                link: url
            },
        });
        dialogRef.afterClosed().subscribe(result => { });
    }
    openViewImg(id, title): void {
        const dialogRef = this.dialog.open(portal_homepage_popupViewImgComponent, {

            data: {
                id_group: id,
                title: title,
            },
        });
        dialogRef.afterClosed().subscribe(result => { });
    }
    openVideo(item) {
        this.link_video = item.link;
    }
    load_san_pham() {
        this.http
            .post('/sys_san_pham.ctr/getListUse/', {}
            ).subscribe(resp => {

                this.lst_san_pham = resp;
                this.lst_san_pham = this.lst_san_pham.filter(q => q.ma_loai == "MTB");
                this.load_linh_vuc();
                console.log("san pham");
            });
    }
    load_tuyen_dung() {
        this.http
            .post('/sys_news.ctr/getNewsTuyenDung/', {
                id: 'f3ccf05d-31b3-4e38-927b-690b8d6fceb1'
            }
            ).subscribe(resp => {
                this.lst_tuyen_dung = resp;
                console.log("tuyen dung");
                this.loading = false;
            });

    }
    load_hinh_anh() {
        this.http
            .post('/sys_nhom_thu_vien_hinh_anh.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.lst_hinh_anh = resp;
                this.load_tuyen_dung();
                console.log("hinh anh");
            });
    }

    load_video() {
        this.http
            .post('/sys_video.ctr/getListUse/', {}
            ).subscribe(resp => {

                this.lst_video = resp;
                this.link_video = this.lst_video[0].link;
                this.load_hinh_anh();
                console.log("video");
            });
    }

    load_linh_vuc() {
        this.http
            .post('/sys_linh_vuc.ctr/getListUse/', {}
            ).subscribe(resp => {

                this.lst_linh_vuc = resp;
                this.load_video();
                console.log("linh vuc");
            });
    }
    load_banner() {
        this.loading = true;
        this.http
            .post('/sys_banner.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.lst_banner = resp;
                this.load_hot_new();
                console.log("banner");
            });
    }
    load_hot_new() {
        this.http
            .post('/sys_news.ctr/getHomePageHotNews/', {
                id: '0a2ef7be-79c2-47e2-9ae4-270a457f391c'
            }
            ).subscribe(resp => {
                this.lst_hot_news = resp;
                this.load_su_kien();
                console.log("hot new")
            })
    }
    wait(ms: any) {
        return new Promise(r => setTimeout(r, ms))
    }
    load_su_kien() {
        this.http
            .post('/sys_event.ctr/getEventCurrents/', {}
            ).subscribe(resp => {
                this.lst_events = resp;
                this.load_san_pham();
                console.log("su kien")
            })
    }
    load_cuu_sinh_vien() {
        this.http
            .post('/sys_cuu_sinh_vien.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.lst_cuu_sinh_vien = resp;

                console.log("cuu sinh vien")
            });

    }
    load_list_product() {
        debugger
        this.products = [
            { name: 'Iphone 5s Gold 32 Gb 2013', price: 451, image: 'assets/images/small/products-01.png', offer: '- %20' },
            { name: 'Iphone 5s Gold 32 Gb 2013', price: 451, image: 'assets/images/small/products-02.png' },
            { name: 'Iphone 5s Gold 32 Gb 2013', price: 451, image: 'assets/images/small/products-03.png', offer: 'New' },
            { name: 'Iphone 5s Gold 32 Gb 2013', price: 451, image: 'assets/images/small/products-04.png' },
          ];

    }
    ngOnInit() {

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });


        this.setDocTitle(this._translocoService.translate('portal.homepage') + ' - Xelex')
        AOS.init({
            duration: 1000
        });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        var title = 'Xelex - ' + this._translocoService.translate('portal.trang_chu');
        var metaTag = [
            { name: 'twitter:card', content: 'summary' },
            { property: 'og:type', content: 'article' },
            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/homepage-index' },
            { property: 'og:title', content: 'Xelex' },
            { property: 'og:image', content: 'assets/images/logo/worldsoft.png' },
            { property: 'og:description', content: 'Trang chủ' },
        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);


        //this.load_hot_new();
        this.load_list_product();
        this.load_banner();
        //this.load_su_kien();
        //this.load_san_pham();
        //this.load_linh_vuc();
        //this.load_hinh_anh();
        //this.load_video();
        //this.load_tuyen_dung();
    }


    goto_san_pham_page(id_san_pham): void {
        this.http
            .post('/sys_san_pham.ctr/get_title_san_pham/', {
                id: id_san_pham
            }).subscribe(resp => {
                var title = resp;
                const url = '/sys_post.ctr/san_pham?tieu_de=' + title + "&id=" + id_san_pham + "&t=" + "3";
                window.location.href = url
            });
        // const url = '/portal_product_detail/' + id_san_pham;
        // this.router.navigateByUrl(url);
        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }

    goto_linh_vuc_page(id_linh_vuc): void {

        const url = '/portal-linh-vuc-detail/' + id_linh_vuc;
        this.router.navigateByUrl(url);

        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }


    gotoNewsDetailPage(id_news): void {
        this.http
            .post('/sys_news.ctr/get_title_news/', {
                id_news: id_news
            }).subscribe(resp => {
                var title_news = resp;
                const url = '/sys_post.ctr/news_detail?tieu_de=' + title_news + "&id=" + id_news + "&t=" + "1";
                window.location.href = url
            });
        // const url = '/portal-news-detail/' + id_news;
        // this.router.navigateByUrl(url);
        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }
    gotoEventDetailPage(id_event): void {
        this.http
            .post('/sys_event.ctr/get_title_event/', {
                id_event: id_event
            }).subscribe(resp => {
                var title_event = resp;
                const url = '/sys_post.ctr/event_detail?tieu_de=' + title_event + "&id=" + id_event + "&t=" + "2";
                window.location.href = url
            });
        // const url = '/portal-event-detail/' + id_event;
        // this.router.navigateByUrl(url);
        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }
    gotoGroupNewsPage(id_type_news): void {


        const url = '/portal-news/' + id_type_news;
        this.router.navigateByUrl(url);
    }
    gotoTypeNewsPage(id_type_news): void {


        const url = '/portal-type-news/' + id_type_news;
        this.router.navigateByUrl(url);
    }
    setDocTitle(title: string) {
        console.log('current title:::::' + this.titleService.getTitle());
        this.titleService.setTitle(title);
    }
}
