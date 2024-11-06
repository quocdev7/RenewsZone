import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
import { DOCUMENT, Location } from '@angular/common';

import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, ParamMap, NavigationStart } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';

import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import * as AOS from 'aos';

import { SeoService } from '@fuse/services/seo.service';

import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
import Swal from 'sweetalert2';
import { Title } from '@angular/platform-browser';
import { FuseConfigService } from '@fuse/services/config';
import { AppConfig } from 'app/core/config/app.config';
import { popup_mua_hangComponent } from './popup_mua_hang.component';


@Component({
    selector: 'portal_product_detail',
    templateUrl: './detail.component.html',
    styleUrls: ['./detail.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class portal_product_detailComponent implements OnInit {
    /**
     * Constructor
     */
    config: AppConfig;
    public hinh_anh: any;

    public activeLang: any;
    public id_san_pham: any;
    public id_loai_san_pham: any;
    public san_pham: any = {};
    public list_mtb: any = {};
    public lst_hinh_anh: any = {};
    public list_mtb_tuong_tu: any = {};
    public openTab: any;
    public lst_san_pham_tuong_tu: any = {};
    public lst_san_pham_quan_tam: any = {};
    public user: any;

    public expandableCard02: any = {
    };
    public loading: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public is_login: any = false;
    public status_del: any;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private _fuseConfigService: FuseConfigService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _authService: AuthService,
        private location: Location, private seoService: SeoService,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, public dialog: MatDialog
        , public _translocoService: TranslocoService,
        private activatedRoute: ActivatedRoute,
        private titleService: Title,
    ) {
        this.expandableCard02.expanded = true;
    }


    gotoProductDetail(id_san_pham): void {
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
    }

    goback(): void {

        const url = '/portal-loai-san-pham/' + this.id_san_pham;
        this.location.back();


    }
    setDocTitle(title: string) {
        //console.log('current title:::::' + this.titleService.getTitle());
        this.titleService.setTitle(title);
    }

    load_anh_san_pham(): void {
        this.loading = true;
        this.http
            .post('/sys_san_pham.ctr/get_list_hinh_anh/', {
                id_san_pham: this.id_san_pham
            }
            ).subscribe(resp => {
                this.lst_hinh_anh = resp;
                this.loading = false;
            });
    }
    list_product_card: any = []
    setlocalStorage() {
        localStorage.setItem(
            'list_product_card',
            JSON.stringify(this.list_product_card)
        );
    }

    resetlocalStorage() {
        localStorage.removeItem('list_product_card');
    }
    getlocalStorage() {
        return JSON.parse(localStorage.getItem('list_product_card'));
    }
    add_product_storage() {
        debugger
        let list = this.getlocalStorage()
        if (list != null && list != undefined)
            this.list_product_card = list
        var check = this.list_product_card.find(q => q.db.id == this.san_pham.db.id)
        if (check == null || check == undefined) {
            this.san_pham.so_luong = 1;
            this.san_pham.is_showKhuyenMai = false;
            this.san_pham.so_tien = this.san_pham.so_luong * this.san_pham.db.so_tien;
            this.list_product_card.push(this.san_pham)
        } else {
            this.san_pham.so_luong = check.so_luong + 1;
            this.san_pham.so_tien = this.san_pham.so_luong * this.san_pham.db.so_tien;
            this.list_product_card = this.list_product_card.filter(q => q.db.id != this.san_pham.db.id)
            this.list_product_card.push(this.san_pham)
        }
        this.setlocalStorage()
    }
    popup_mua_hang(): void {
        //payment-cart
        this.add_product_storage();
        const url = '/payment-cart';
        this.router.navigateByUrl(url);
    }
    rerender(): void {
    }
    toggleTabs($tabNumber: number) {
        this.openTab = $tabNumber;
    }
    load_san_pham(): void {
        this.loading = true;
        this.http
            .post('/sys_san_pham.ctr/get_san_pham/', {
                id_san_pham: this.id_san_pham
            }
            ).subscribe(resp => {

                this.san_pham = resp;
                this.hinh_anh = this.san_pham.db.hinh_anh;

                var title = "";
                if (this.activeLang == 'vi') {
                    title = this.san_pham.db.ten_san_pham + ' - Xelex';
                } else {
                    title = this.san_pham.ten_san_pham_language + ' - Xelex';
                }

                var metaTag = [

                    { property: 'og:url', content: 'https://bka.edu.vn/sys_post.ctr/index?id=' + this.san_pham.db.id },
                    { property: 'og:title', content: 'Bach Khoa Alumni' },
                    { property: 'og:image', content: this.hinh_anh },
                    { property: 'og:description', content: this.san_pham.db.ten_san_pham },

                ]
                this.seoService.updateTitle(title);
                this.seoService.updateMetaTags(metaTag);


                //this.setDocTitle(title); 
                this.loading = false;
            });


    }
    changeImage(hinh_anh): void {
        this.hinh_anh = hinh_anh;


    }
    goto_loai_san_pham(id_loai_san_pham): void {

        const url = '/portal-product-category/' + id_loai_san_pham;
        this.router.navigateByUrl(url);
    }

    opendialog: any = false;


    load_san_pham_tuong_tu() {
        this.loading = true;
        this.http
            .post('/sys_san_pham.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.lst_san_pham_tuong_tu = resp;
                this.list_mtb_tuong_tu = this.lst_san_pham_tuong_tu.filter(q => q.ma_loai == "MTB");
                this.loading = false;
            });
    }

    load_san_pham_quan_tam() {
        this.loading = true;
        this.http
            .post('/sys_san_pham.ctr/getListSanPhamQuanTam/', {
                id_san_pham: this.id_san_pham
            }
            ).subscribe(resp => {
                this.lst_san_pham_quan_tam = resp;
                this.list_mtb = this.lst_san_pham_quan_tam.filter(q => q.id_loai == "bc76bfb6-d891-43ce-ad59-c3d07804c692");
                this.loading = false;
            });
    }
    ngOnInit() {
        AOS.init({
            duration: 1000
        });

        this.router.events.subscribe((val) => {
            if (val instanceof NavigationStart) {

                if (val.url.includes("/sys_post.ctr/index?id=" + this.id_san_pham)) {

                } else {
                    this.router.navigate(['/sys_post.ctr/index?id=' + this.id_san_pham]);
                    window.location.href = val.url;
                }
            }
        });
        this._fuseConfigService.config$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((config: AppConfig) => {
                // Store the config
                this.config = config;
            });
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
        this.route.params.subscribe(params => {
            this.id_san_pham = params["id"];
            this.id_loai_san_pham = params["id"];
            this.load_san_pham();
            this.load_san_pham_quan_tam();
            this.load_san_pham_tuong_tu();
            this.load_anh_san_pham();

        });
        this.toggleTabs(1);
    }
}
