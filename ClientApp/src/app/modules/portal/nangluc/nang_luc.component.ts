import { Component, ViewEncapsulation, OnInit, Inject, ViewChild ,ChangeDetectorRef } from '@angular/core';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { takeUntil } from 'rxjs/operators';
import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { Subject } from 'rxjs';
import { DOCUMENT } from '@angular/common';
import Swal from 'sweetalert2';
import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation ,Autoplay} from "swiper";
import { SwiperComponent } from 'swiper/angular';
import  {portal_nang_luc_popupViewImgComponent } from './popupViewImg.component';


import * as AOS from 'aos';
import { Title } from '@angular/platform-browser';

// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation,Autoplay]);
@Component({
    selector     : 'portal_nang_luc',
    templateUrl: './nang_luc.component.html',
    styleUrls: ['./nang_luc.component.scss'],
    encapsulation  : ViewEncapsulation.None,
})

export class portal_nang_lucComponent implements OnInit 
{
    
    @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;

    slideNext() {
        this.swiper.swiperRef.slideNext(100);
    }
    slidePrev() {
        this.swiper.swiperRef.slidePrev(100);
    }
    public loading:any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    
    public activeLang: any;
    public lst_hinh_anh:any;
    public lst_nang_luc: any;
    public lst_nang_luc_hinh_anh: any;
    public isScreenSmall: any = false;
    pagination = {
        clickable: true,
    };
   
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private titleService: Title,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient,private dialog: MatDialog
        , public _translocoService: TranslocoService,
        private _changeDetectorRef: ChangeDetectorRef,
        ) {

    }
    
   
    loadNangLuc(): void {
        this.loading = true;
        this.http
        .post('/sys_nang_luc.ctr/getListUse/', {}
        ).subscribe(resp => {
            this.lst_nang_luc = resp;       
            this._changeDetectorRef.markForCheck();
            this.loadNangLucHinhAnh();
        });
    }

    loadNangLucHinhAnh(): void {
        this.http
               .post('/sys_nang_luc_hinh_anh.ctr/getListUse/', {}
        ).subscribe(resp => {
                
                   this.lst_nang_luc_hinh_anh = resp;
                   this._changeDetectorRef.markForCheck();
                   this.loading = false;
               });
       }
       
    openViewImg(model): void {
        const dialogRef = this.dialog.open(portal_nang_luc_popupViewImgComponent, {
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {});
    }  

    setDocTitle(title: string) {
        //console.log('current title:::::' + this.titleService.getTitle());
        this.titleService.setTitle(title);
     }

    ngOnInit(): void {
        AOS.init({
            duration:1000
        });
        this.setDocTitle(this._translocoService.translate('portal.dich_vu') + ' - Xelex')


        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        
       
        this._fuseMediaWatcherService.onMediaChange$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe(({ matchingAliases }) => {
            // Check if the screen is small
            this.isScreenSmall = !matchingAliases.includes('md');
        });
        this.loadNangLuc();
        
    }
}
