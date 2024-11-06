import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { DOCUMENT, Location } from '@angular/common';

import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, ParamMap, NavigationStart } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import { portal_news_popupAddCommentComponent } from './popupAddComment.component';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import * as AOS from 'aos';
import { SeoService } from '@fuse/services/seo.service';
import { FuseConfigService } from '@fuse/services/config';


import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
import Swal from 'sweetalert2';
import { Title } from '@angular/platform-browser';
import { AppConfig } from 'app/core/config/app.config';

import { Clipboard } from "@angular/cdk/clipboard";

@Component({
    selector: 'portal-news-detail',
    templateUrl: './news_detail_new.component.html',
    styleUrls: ['./news_detail.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PortalNewsDetailComponent implements OnInit {
    /**
     * Constructor
     */

    public img = '/assets/images/portal/share_2.png';
    public activeLang: any;
    public icons: any;
    public title_news: any;
    shares: any;
    link: any;
    config: AppConfig;
    public id_news: any;
    public loading: any = false;
    public news: any = {};
    public lst_comment: any = [];
    public lst_news_by_user: any;
    public type_news_name: any;
    public group_news_name: any;
    public expandableCard02: any = {
    };
    public id_group_news: any = [];
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public is_login: any = false;
    public lst_type_news: any = [];
    public user: any = [];

    public type_news: any = [];
    public group_news: any = [];
    public status_del: any;
    news_tieu_de: any;
    public lst_hot_news: any = [];
    public copied: any = false;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private titleService: Title,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _authService: AuthService,
        private location: Location,
        public clipboard: Clipboard,
        private seoService: SeoService,
        private _fuseConfigService: FuseConfigService,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, public dialog: MatDialog
        , public _translocoService: TranslocoService,
        private activatedRoute: ActivatedRoute
    ) {
        this.expandableCard02.expanded = true;
    }
    copy(): void {
        this.http
            .post('/sys_news.ctr/get_title_news/', {
                id_news: this.id_news
            }).subscribe(resp => {
                this.title_news = resp;
                this.link = 'https://' + this.document.location.hostname + '/sys_post.ctr/news_detail?tieu_de=' + this.title_news + "&id=" + this.id_news + "&t=" + "1";
                this.clipboard.copy(this.link);
                this.copied = true;
            });
    }
    goToProfileUser(user_id): void {

        const url = '/portal-profile-user/' + user_id;
        this.router.navigateByUrl(url);
        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }

    goback(): void {

        const url = '/portal-type-news/' + this.news.db.id_type_news;
        this.location.back();


    }
    setDocTitle(title: string) {
        //console.log('current title:::::' + this.titleService.getTitle());
        this.titleService.setTitle(title);
    }
    loadNews(): void {
        this.loading = true;
        this.http
            .post('/sys_news.ctr/getNews/', {
                id_news: this.id_news
            }
            ).subscribe(resp => {
                var data: any = resp;
                this.news = data.data;
                this.lst_news_by_user = data.lst_news_by_user;
                this.type_news = data.type_news;
                this.group_news = data.group_news;

                this.id_group_news = data.id_group_news;
                this.http
                    .post('/sys_type_news.ctr/getInfoByTypeNews/', {
                        id: this.news.db.id_type_news
                    }
                    ).subscribe(resp => {
                        var data: any = resp;
                        this.lst_type_news = data.lst_type;
                        var title = "";
                        if (this.activeLang == 'vi') {
                            title = this.news.db.tieu_de + ' - Xelex';
                        } else {
                            title = this.news.tieu_de_language + ' - Xelex';
                        }
                        var metaTag = [

                            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal-news/' + this.id_group_news },
                            { property: 'og:title', content: 'Xelex' },
                            { property: 'og:image', content: this.news.db.hinh_anh },
                            { property: 'og:description', content: this.news.db.tieu_de },
                        ]
                        this.seoService.updateTitle(title);
                        this.seoService.updateMetaTags(metaTag);
                        this.setDocTitle(title);
                        this.title_news = this.news.db.tieu_de
                        this.loading = false;
                    });

            });
        this.getcomment();
        this.load_icon();
        this.get_list_hot_new();
    }

    gotoTypeNewsPage(id_type_news): void {


        const url = '/portal-type-news/' + id_type_news;
        this.router.navigateByUrl(url);
    }
    getcomment() {
        this.http
            .post('/sys_news.ctr/get_comment/', {
                id_news: this.id_news
            }
            ).subscribe(resp => {
                var data: any = resp;
                this.lst_comment = data;
                this.news.db.comment_count = this.lst_comment.length;
            });
    }
    opendialog: any = false;

    checklogin() {
        if (!this.is_login) {
            this.router.navigateByUrl("/sign-in");
        } else {
            if (this.user.status_del != 1) {

                this.goToInfoUser();
            } else {
                if (this.opendialog == true) return;
                this.opendialog = true;
                this.openDialogAddComment();
            }

        }
    }
    goToInfoUser(): void {
        //this.loading = true;
        Swal.fire({
            title: this._translocoService.translate('portal.ban_phai_la_thanh_vien_moi_duoc_binh_luan'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('portal.dang_ky_thanh_vien'),
            cancelButtonText: this._translocoService.translate('close')
        }).then((result) => {
            if (result.value) {
                this.router.navigateByUrl("/person_setting");
            } else {

            }
        })
    }
    gotoNewsDetailPage(id_news): void {
        this.http
            .post('/sys_news.ctr/get_title_news/', {
                id_news: id_news
            }).subscribe(resp => {
                this.title_news = resp;
                const url = '/sys_post.ctr/news_detail?tieu_de=' + this.title_news + "&id=" + id_news + "&t=" + "1";
                window.location.href = url
            });
        // const url = '/portal-news-detail/' + id_news;
        // this.router.navigateByUrl(url);
    }
    gotoUserDetail(id_user): void {
        const url = '/portal-news-by-user-detail/' + id_user;
        this.router.navigateByUrl(url);
    }
    openDialogAddComment(): void {
        var that = this;
        const dialogRef = this.dialog.open(portal_news_popupAddCommentComponent, {
            disableClose: true,

            data: {
                id_news: that.id_news,
                id: 0
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            that.opendialog = false;
            if (result.id == 0) return;
            that.getcomment()
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
    load_icon(): void {
        this.http
            .post('/sys_news.ctr/load_icon/', {
            }
            ).subscribe(resp => {
                this.icons = resp;
                this.link = 'https://' + this.document.location.hostname + '/sys_post.ctr/index?id=' + this.id_news;
                this.shares = [
                    {
                        "tooltip": "Chia sẻ trên Facebook",
                        "link": "https://www.facebook.com/sharer/sharer.php?u=" + encodeURIComponent(this.link),
                        "image": this.icons.icon_facebook,
                    },
                    {
                        "tooltip": "Chia sẻ trên Linkedin",
                        "link": "https://www.linkedin.com/sharing/share-offsite/?url=" + encodeURIComponent(this.link),
                        "image": this.icons.icon_linkedin,
                    },
                    {
                        "tooltip": "Chia sẻ trên Twitter",
                        "link": "https://twitter.com/intent/tweet?source=tweetbutton&text=" + encodeURIComponent(this.link),
                        "image": this.icons.icon_twitter,
                    },
                ]
            });
    }
    get_list_hot_new(): void {
        this.http
            .post('/sys_news.ctr/getHomePageHotNews/', {
                id: '0a2ef7be-79c2-47e2-9ae4-270a457f391c'
            }
            ).subscribe(resp => {
                debugger
                this.lst_hot_news = resp;
            });
    }
    ngOnInit() {
        AOS.init({
            duration: 1000
        });


        this.router.events.subscribe((val) => {
            if (val instanceof NavigationStart) {

                if (val.url.includes('/sys_post.ctr/news_detail?tieu_de=' + this.title_news + "&id=" + this.id_news + "&t=" + "1")) {

                } else {
                    this.router.navigate(['/sys_post.ctr/news_detail?tieu_de='+ this.title_news + "&id=" + this.id_news + "&t=" + "1"]);
                    window.location.href = val.url;

                }


            }


        });
        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => {
            this.id_news = params["id"];
            this.loadNews();
        });

    }

}
