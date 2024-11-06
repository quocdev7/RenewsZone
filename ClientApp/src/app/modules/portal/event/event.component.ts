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


import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';

import Swal from 'sweetalert2';

import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';


import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { SwiperComponent } from 'swiper/angular';
import * as AOS from 'aos';
import { AppConfig } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { Title } from '@angular/platform-browser';

import { SeoService } from '@fuse/services/seo.service';

// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);



@Component({
    selector: 'portal-event',
    templateUrl: './event.component.html',
    styleUrls: ['./event.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PortalEventComponent implements OnInit {
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
    config: AppConfig;
    public loading: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public lst_tat_ca: any = [];
    public lst_events: any = [];

    public activeLang: any;
    public event_id: any;
    public is_login: any = false;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private titleService: Title,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseConfigService: FuseConfigService,
        private location: Location,   private seoService: SeoService,
        private dialog: MatDialog,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient
        , public _translocoService: TranslocoService) {

    }

    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }

    ngOnInit() {
        this._authService.check().subscribe((data: any) => this.is_login = data);

        AOS.init({
            duration: 1000
        });
        var title = 'Xelex - Sự kiện';
        var metaTag = [
            { name: 'twitter:card', content: 'summary' },
            { property: 'og:type', content: 'article' },
            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal-event' },
            { property: 'og:title', content: 'Xelex' },
            { property: 'og:image', content: 'assets/images/logo/worldsoft.png' },
            { property: 'og:description', content: 'Sự kiện' },
        ]
        this.seoService.updateTitle(title);
        this.seoService.updateMetaTags(metaTag);

        this._translocoService.langChanges$.subscribe((activeLang) => {
            this.activeLang = activeLang;

        });
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
        this.event_id = "";
        this.loadSuKien();
        this.setDocTitle(this._translocoService.translate('portal.event') + ' - Xelex')

    }
    gotoNewsEvenPage(id_event): void {
        this.http
            .post('/sys_event.ctr/get_title_event/', {
                id_event: id_event
            }).subscribe(resp => {
                var title_event = resp;
                const url = '/sys_post.ctr/event_detail?tieu_de=' + title_event + "&id=" + id_event + "&t=" + "2";
                window.location.href = url
            });

    }
    loadSuKien(): void {
        this.loading = true;
        this.http
            .post('/sys_event.ctr/getEventCurrents/', {}
            ).subscribe(resp => {
                this.lst_events = resp;

                this.http
                    .post('/sys_event.ctr/getEvents/', {
                    }
                    ).subscribe(resp => {
                        this.lst_tat_ca = resp;
                        this.loading = false;
                    });
            });



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



}
