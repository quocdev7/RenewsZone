import { Component, ViewEncapsulation, OnInit,ViewChild } from '@angular/core';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { takeUntil } from 'rxjs/operators';
import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { debug } from 'console';
import Swal from 'sweetalert2';
import { Subject } from 'rxjs';
import { FuseAlertType } from '../../../../@fuse/components/alert';
import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation ,Autoplay} from "swiper";
import { SwiperComponent } from 'swiper/angular';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as AOS from 'aos';
import { Title } from '@angular/platform-browser';
SwiperCore.use([EffectCoverflow, Pagination, Navigation,Autoplay]);
@Component({
    selector     : 'portal-contact-us',
    templateUrl: './contact_us.component.html',
    encapsulation: ViewEncapsulation.None
})

export class PortalContactUsComponent implements OnInit

{
    @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
    slideNext() {
        this.swiper.swiperRef.slideNext(100);
    }
    slidePrev() {
        this.swiper.swiperRef.slidePrev(100);
    }
    /**
     * Constructor
     */
    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    loading = false;
    showAlert: boolean = false;
    list_khoa: any;
    actionEnum: any = 1;
    
    public activeLang: any;
    public slides_web: any=[];
    public lst_banner: any=[];
    
    public isScreenSmall: any = false;
    public slides_mobile: any=[];
    private _unsubscribeAll: Subject<any> = new Subject<any>();
   

    record: any = {
        db: {
            name: "",
            content: "",
            email: "",
            phone: "",
            id_khoa: "",
        }
    }
    get currentYear(): number {
        return new Date().getFullYear();
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

    }
    public errorModel: any;
    /**
     * Constructor
     */
    constructor(
        private titleService: Title,
        private formBuilder: FormBuilder,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient, dialog: MatDialog,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        public _translocoService: TranslocoService,) {
        this.errorModel = [];      
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
        this.http
            .post('/sys_khoa.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_khoa = resp;
            });
        this.SendUp();
    }
    gobackhomepage() {
        const url = '/homepage-index';

        this.router.navigateByUrl(url);
    }
    load_banner(){

        this.http
        .post('/sys_banner.ctr/getListUseContactUs/', {
            }
        ).subscribe(resp => {
            this.lst_banner = resp;
            console.log(this.lst_banner);
        });


       
    }
    ngOnInit(): void {
        this.setDocTitle(this._translocoService.translate('portal.contactus') + ' - Xelex')
        
        AOS.init({
            duration:1000
        });
        this._translocoService.langChanges$.subscribe((activeLang) => {
            this.activeLang = activeLang;
          
        });
        this.load_banner();
    }
    gotoCompanyDetailPage(id_company): void {

        const url = '/portal-company-detail/' + id_company;
        this.router.navigateByUrl(url);
        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }
    setDocTitle(title: string) {
        //console.log('current title:::::' + this.titleService.getTitle());
        this.titleService.setTitle(title);
     }
    SendUp(): void {
        this.loading = true;
        this.http.post('sys_contactus.ctr/create/',
            {
                data: this.record
            }
        ).subscribe(
            (resp) => {
                this.loading = false;
                Swal.fire('Đã gửi thành công, chúng tôi sẽ phản hồi lại sau', '', 'success').then(
                    // Navigate to the confirmation required page
                    res => {
                        this.router.navigateByUrl('/confirmation-required');
                    }

                );
            },
            (error) => {
                if (error.status == 400) {
                    this.errorModel = error.error;

                }
                // Set the alert
                this.alert = {
                    type: 'error',
                    message: 'Không hợp lệ, Vui lòng kiểm tra lại'
                };

                // Show the alert
                this.loading = false;
            }
        );
    }
}
