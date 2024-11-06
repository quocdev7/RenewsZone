

import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';



import { FuseNavigationService } from '@fuse/components/navigation';


import { Component, ViewEncapsulation, OnInit, Inject, HostListener } from '@angular/core';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { Router, ActivatedRoute, NavigationStart } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';

import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import Swal from 'sweetalert2';
import { debug } from 'console';
import { TranslocoModule } from "@ngneat/transloco";
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { popupVeMoiComponent } from './popupVeMoi.component';

import { DOCUMENT } from '@angular/common';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import * as AOS from 'aos';
import { popupConfirmComponent } from './popupConfirm.component';
import { scan_checkinComponent } from './scan_checkin.component';

import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { SwiperComponent } from 'swiper/angular';
import { AppConfig } from 'app/core/config/app.config';
import { FuseConfigService } from '@fuse/services/config';
import { ThisReceiver } from '@angular/compiler';

import { portal_event_popupViewImgComponent } from './popupViewImg.component';

import { style } from '@angular/animations';
import { Title } from '@angular/platform-browser';
import { SeoService } from '@fuse/services/seo.service';


// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);

@Component({
    selector: 'event_detail',
    templateUrl: './eventDetail.component.html',
    styleUrls: ['./eventDetail.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class EventDetailComponent {
    /**
     * Constructor
     */

    link: any;
    shares: any;
    public currentUser: any = null;
    public loading: any = false;
    config: AppConfig;
    public lstTypeNews: any;
    public id_group_new: any;
    public group_type_new_name: any;
    public lst_events: any = [];
    public id_event: any;
    public is_login: any = false;
    public record: any = null;
    public list_dien_gia: any = [];
    public list_event_proram: any = [];
    public list_event_proram_file: any = [];
    public list_event_qa: any = [];
    public list_event_contact: any = [];
    public list_event_rating: any = [];
    public list_event_image: any = [];
    public list_event_nguoi_nhan_hoc_bong: any = [];
    public list_event_nha_tai_tro: any = [];
    public lst_hot_events: any = [];
    public event_id: any;
    public lst_hinh_anh: any = [];
    public list_group_event_proram: any = [];
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public openTab: any;
    public status_del: any;
    public title_event: any;

    public activeLang: any;
    public is_show_hide_success: any = true;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        public http: HttpClient, public _translocoService: TranslocoService,
        private titleService: Title,
        private _fuseConfigService: FuseConfigService,
        private dialog: MatDialog, private seoService: SeoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _authService: AuthService,
        _fuseNavigationService: FuseNavigationService, private route: ActivatedRoute, private router: Router,
        public dialogModal: MatDialog) {

        this.status_del = -1;
        this.openTab = 1;


    }
    showHideSuccess(item): void {
        if (item.is_show == undefined) item.is_show = true;
        else {
            item.is_show = !item.is_show;
        }

    }
    toggleTabs($tabNumber: number) {
        this.openTab = $tabNumber;
    }

    gotologin(): void {
        const url = '/sign-in'
        this.router.navigateByUrl(url);
    }

    openViewImg(item): void {
        const dialogRef = this.dialog.open(portal_event_popupViewImgComponent, {
            data: {
                id: item.db.id,
                name: item.db.name,
            },
        });
        dialogRef.afterClosed().subscribe(result => { });
    }




    loadAnhNoiBat(): void {
        this.http
            .post('/sys_event.ctr/get_list_hinh_anh/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.lst_hinh_anh = resp;
                console.log("8");
                this.loadEventProgramGroup();
            });
    }


    loadEventProgram(): void {

        this.http
            .post('/sys_event.ctr/get_list_event_program/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.list_event_proram = resp;
                console.log("2");
                this.loadEventProgramFile();
                if (this.list_event_proram.length > 0) {
                    this.list_dien_gia = this.list_event_proram.filter(q => q.anh_dai_dien != null)

                }

            });
    }

    loadEventProgramGroup(): void {
        this.http
            .post('/sys_event.ctr/get_list_group_event_program/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.list_group_event_proram = resp;
                console.log("9");
                this.loading = false;
            });
    }
    loadEventImage(): void {
        this.http
            .post('/sys_event.ctr/get_list_image/', {
                event_id: this.record.db.id,

            }
            ).subscribe(resp => {
                this.list_event_image = resp;
                console.log("7");
                this.loadAnhNoiBat();
            });
    }
    loadEventProgramFile(): void {
        this.http
            .post('/sys_event.ctr/get_list_file/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.list_event_proram_file = resp;
                console.log("3");
                this.loadEventQA();
            });
    }
    loadEventNguoiNhanHocBong(): void {
        this.http
            .post('/sys_event.ctr/getNguoiNhanHocBongEvent/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.list_event_nguoi_nhan_hoc_bong = resp;
                console.log("5");
            });

    }

    loadEventNhaTaiTro(): void {
        this.http
            .post('/sys_event.ctr/getNhaTaiTroEvent/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.list_event_nha_tai_tro = resp;
                console.log("6");
            });
    }

    loadEventQA(): void {
        this.http
            .post('/sys_event.ctr/getQAEvent/', {
                event_id: this.record.db.id
            }
            ).subscribe(resp => {
                this.list_event_qa = resp;
                console.log("4");
                this.loadEventImage();
            });
    }

    // loadEventRating(): void {
    //     this.http
    //         .post('/sys_event.ctr/get_list_danh_gia/', {
    //             event_id: this.record.db.id
    //         }
    //         ).subscribe(resp => {
    //             this.list_event_rating = resp;
    //         });
    // }
    download(file_path): void {

    }
    tuchoi(id): void {
        Swal.fire({
            title: this._translocoService.translate('areYouSure'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                    .post('/sys_event.ctr/register_event/', {
                        event_id: id,
                    }
                    ).subscribe(resp => {
                        //this.lst_sukienthamgia = resp;
                        //this.loadSuKien();
                        Swal.fire("success", '', 'success');
                    });
            }
        })


    }

    showIcon(index: number, rate) {
        if ((rate / 2) >= index + 1) {
            return 'star';
        } else {
            return 'star_border';
        }
    }

    checkin(id_event): void {
        const url = '/scan_checkin/' + id_event;
        this.router.navigateByUrl(url);
    }



    thamgia(id): void {
        var that = this;
        const dialogRef = this.dialog.open(popupConfirmComponent, {

            disableClose: true,

            data: this.record
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.cho_phep_dang_ky) {
                that.http
                    .post('/sys_event.ctr/getDetailEvent/', {
                        id: that.id_event
                    }
                    ).subscribe(resp => {
                        that.record = resp;
                    });
            }
        });
    }

    openDialogVeMoi(model): void {
        model.actionEnum = 3;

        const dialogRef = this.dialogModal.open(popupVeMoiComponent, {
            disableClose: true,

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {

        });
    }
    gotoNewsEvenPage(id_event): void {
        this.http
            .post('/sys_event.ctr/get_title_event/', {
                id_event: id_event
            }).subscribe(resp => {
                this.title_event = resp;
                const url = '/sys_post.ctr/event_detail?tieu_de=' + this.title_event + "&id=" + id_event + "&t=" + "2";
                window.location.href = url
            });
        // const url = '/eventDetail/' + id_event;
        // this.router.navigateByUrl(url);
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }
    get_title_event(): void {
        this.http
            .post('/sys_event.ctr/get_title_event/', {
                id_event: this.id_event
            }).subscribe(resp => {
                this.title_event = resp;
            });

    }
    ngOnInit() {

        this._authService.check().subscribe((data: any) => this.is_login = data);
        AOS.init({
            duration: 1000
        });



        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this._authService.check().subscribe((data: any) => this.is_login = data);
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
        this.route.params.subscribe(params => {
            this.id_event = params["id"];
            this.loading = true;
            this.http
                .post('/sys_event.ctr/getDetailEvent/', {
                    id: this.id_event
                }
                ).subscribe(resp => {
                    this.record = resp;
                    this.loadEventProgram();
                    console.log("1");
                    // this.loadEventProgramFile();
                    // this.loadEventQA();
                    this.loadEventNguoiNhanHocBong();
                    this.loadEventNhaTaiTro();
                    // this.loadEventImage();
                    // this.loadAnhNoiBat();
                    // this.loadEventProgramGroup()

                    this.link = 'https://' + this.document.location.hostname + '/eventDetail/' + this.record.db.id;

                    this.shares = [
                        {
                            "link": "https://www.facebook.com/sharer/sharer.php?u=" + encodeURI(this.link),
                            "image": "/assets/images/portal/facebook.png",
                        },
                        {
                            "link": "https://www.linkedin.com/sharing/share-offsite/?url=" + encodeURI(this.link),
                            "image": "/assets/images/portal/linkedin.png",
                        },

                    ]
                    this.setDocTitle('Xelex - ' + this._translocoService.translate('portal.event_detail'))
                });
                
                this.get_title_event();
        });
        this.router.events.subscribe((val) => {
            if (val instanceof NavigationStart) {

                if (val.url.includes("/sys_post.ctr/event_detail?tieu_de=" + this.title_event + "&id=" + this.id_event + "&t=" + "2")) {

                } else {
                    this.router.navigate(['/sys_post.ctr/index?id=' + this.id_event]);
                    window.location.href = val.url;

                }
            }
        });
    }
}
