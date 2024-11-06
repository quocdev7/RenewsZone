import { Component, Inject, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';

import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { Location } from '@angular/common';
import { DOCUMENT } from '@angular/common';


import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';


import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';


import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { SwiperComponent } from 'swiper/angular';
import * as AOS from 'aos';
import { Title } from '@angular/platform-browser';
import { id } from '@swimlane/ngx-charts';
import { FuseConfigService } from '@fuse/services/config';
import { AppConfig } from 'app/core/config/app.config';
import { SeoService } from '@fuse/services/seo.service';

// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);



@Component({
    selector: 'portal-news',
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PortalNewsComponent implements OnInit {
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
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public loading: any = false;

    config: AppConfig;
    public activeLang: any;
    public lst_hot_news: any = [];
    public lstTypeNews: any = [];
    public id_group_news: any;
    public group_news_name: any;
    public search_key: any;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private titleService: Title,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private location: Location,
        private seoService: SeoService,
        private _fuseConfigService: FuseConfigService,
        private dialog: MatDialog,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient
        , public _translocoService: TranslocoService,) {

    }


    ngOnInit() {
        AOS.init({
            duration: 1000
        });

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        
        var title = 'Xelex - Tin tức';
        var metaTag = [
            { name: 'twitter:card', content: 'summary' },
            { property: 'og:type', content: 'article' },
            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal-news/0a2ef7be-79c2-47e2-9ae4-270a457f391c' },
            { property: 'og:title', content: 'Xelex' },
            { property: 'og:image', content: 'assets/images/logo/worldsoft.png' },
            { property: 'og:description', content: 'Tin tức' },


        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

        this.setDocTitle(this._translocoService.translate('NAV.sys_news') + ' - Xelex')
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
            this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this.route.params.subscribe(params => {
            this.id_group_news = params["id"];
            this.loadTopics();

        });
    }


    searchFunc(): void {
        var value = this.search_key;
        if (value != "" && value != null && value != undefined) {

            const url = '/homepageSearch/' + encodeURIComponent(value);
            this.router.navigateByUrl(url);
        }

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
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }
    loadTopics(): void {
        this.loading = true;
        this.http
            .post('/sys_news.ctr/getHomePageHotNews/', {
                id: this.id_group_news
            }).subscribe(resp => {
                this.lst_hot_news = resp;
                this.http
                    .post('/sys_news.ctr/getGroupNewsInfo/', {
                        id: this.id_group_news
                    }).subscribe(resp => {
                        var data: any = resp;
                        this.lstTypeNews = data.data;
                        this.group_news_name = data.group_name;
                        if (this.id_group_news == "0a2ef7be-79c2-47e2-9ae4-270a457f391c") {
                            this.setDocTitle(this._translocoService.translate('portal.news') + ' - Xelex')
                        }
                        if (this.id_group_news == "f3ccf05d-31b3-4e38-927b-690b8d6fceb1") {
                            this.setDocTitle(this._translocoService.translate('portal.recruit') + ' - Xelex')
                        }
                        //var title = 'BKA - ' + this.group_news_name;   
                        var metaTag = [  
                            
                            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal-news/' + this.id_group_news}, 
                            { property: 'og:title', content: 'Bach Khoa Alumni' }, 
                            { property: 'og:image', content: 'https://mdbootstrap.com/img/new/textures/small/52.jpg' }, 
                            { property: 'og:description', content: this.group_news_name }, 
                
                        ]
                        
                        //this.seoService.updateTitle(title);
                        this.seoService.updateMetaTags(metaTag);
                        this.loading = false;
                    });

            });

    }
    gotoNewsDetailPage(id_news): void {
        this.http
            .post('/sys_news.ctr/get_title_news/', {
                id_news: id_news
            }).subscribe(resp => {
                var title_news = resp;
                const url = '/sys_post.ctr/news_detail?tieu_de=' + title_news + "&id=" + id_news + "&t=" + "1";
                window.location.href = url;
            });
    }
    gotoTypeNewsPage(id_type_news): void {
        debugger
        const url = '/portal-type-news/' + id_type_news;
        this.router.navigateByUrl(url);
    }
}
