
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { DataTableDirective } from 'angular-datatables';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
/*import { changePassComponent } from './changePass.component';*/
import { ChangeDetectorRef, ChangeDetectionStrategy, Component, HostListener, OnDestroy, OnInit, ViewChild, ViewEncapsulation, ElementRef, Inject, } from '@angular/core';

import { DOCUMENT, Location } from '@angular/common';
import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation, Autoplay } from "swiper";
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { SwiperComponent } from 'swiper/angular';
import * as AOS from 'aos';

import { SeoService } from '@fuse/services/seo.service';
import Swal from 'sweetalert2';
import { Title } from '@angular/platform-browser';
import { FuseAlertType } from '@fuse/components/alert';
// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);
@Component({
  selector: 'portal_gamabook',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})

export class portal_gamabookComponent implements OnInit {
  // @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
  // slideNext() {
  //   this.swiper.swiperRef.slideNext(100);
  // }
  // slidePrev() {
  //   this.swiper.swiperRef.slidePrev(100);
  // }
  public record = {
    db: {
      email: "",
      noi_dung: ""
    },
    capcha: "",
  }
  alert: { type: FuseAlertType; message: string } = {
    type: 'success',
    message: ''
  };
  public Oldrecord: any;
  public errorModel: any;
  public loading: any = false;
  public file: any;
  public list_status: any;
  public lst_loai_doi_tac: any;
  public lst_doi_tac: any;
  public listtype: any;
  public isScreenSmall: any = false;
  public activeLang: any;
  srcCaptcha: any;
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  constructor(
    @Inject(DOCUMENT) public document: Document,
    public _fuseMediaWatcherService: FuseMediaWatcherService,
    public _authService: AuthService, private seoService: SeoService,
    public location: Location,
    public router: Router,
    public activatedRoute: ActivatedRoute,
    public titleService: Title,
    public http: HttpClient, dialog: MatDialog,
    public _translocoService: TranslocoService,
    _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
    , @Inject('BASE_URL') baseUrl: string
  ) {

    //this.getlistLoaiDoiTac();
    //this.getlistDoiTac();
    this.errorModel = [];
    var d = new Date();
    var n = d.getTime();
    this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
    //this.play();
    this._translocoService.langChanges$.subscribe((activeLang) => {
      this.activeLang = activeLang;
    });
  }
  reloadCaptcha(): void {
    var d = new Date();
    var n = d.getTime();
    this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

  }
  play(): void {
    document.querySelector('button').addEventListener('click', () => {
      document.querySelector('video').play();
    });


  }
  theo_doi(): void {
    debugger
    //this.loading = true;

    this.http.post('sys_like_san_pham.ctr/theo_doi/',
      {
        data: this.record
      }
    ).subscribe(resp => {
      this.Oldrecord = resp;
      // this.basedialogRef.close(this.record);
      Swal.fire('Thành công! Kiểm tra email của bạn để xác nhận theo dõi.', '', 'success').then(
        // Navigate to the confirmation required page
        res => {
          //this.router.navigateByUrl('/confirmation-required');
          this.record.db.email = "";
        }

      );
      this.loading = false;
      this.errorModel = []
    },
      error => {
        if (error.status == 400) {
          console.log(this.errorModel)
          this.errorModel = error.error;
          debugger
          if (this.errorModel[0].value[0] == null) {
          } else if (this.errorModel[0].value[0] == "required") {
            Swal.fire(this._translocoService.translate('system.vui_long_nhap_email'), "", "warning");
          } else if (this.errorModel[0].value[0] == "system.emailKhongHopLe") {
            Swal.fire(this._translocoService.translate('system.emailKhongHopLe'), "", "warning");
          } else if (this.errorModel[0].value[0] == "system.emailTonTai") {
            Swal.fire(this._translocoService.translate('system.emailTonTai'), "", "warning");
          } else if (this.errorModel[0].value[0] == "system.capcha") {
            Swal.fire(this._translocoService.translate('system.vui_long_nhap_captcha'), "", "warning");
          } else {
            Swal.fire(this._translocoService.translate('system.capcha_khong_chinh_xac'), "", "warning");
          }


        }
        // if (error.status == 403) {
        //     this.basedialogRef.close();
        //     Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
        // }
        this.loading = false;

      }
    );
  }
  // getlistLoaiDoiTac(): void {
  //   this.http
  //     .post('/sys_loai_doi_tac.ctr/getListUse/', {
  //     }
  //     ).subscribe(resp => {
  //       this.lst_loai_doi_tac = resp;
  //     });
  // }
  // getlistDoiTac(): void {
  //   this.loading = true;
  //   this.http
  //     .post('/sys_doi_tac.ctr/get_list_doi_tac/', {
  //     }
  //     ).subscribe(resp => {
  //       this.lst_loai_doi_tac = resp;
  //       console.log(this.lst_doi_tac);
  //       this.loading = false;
  //     });
  // }
  setDocTitle(title: string) {
    //console.log('current title:::::' + this.titleService.getTitle());
    this.titleService.setTitle(title);
  }
  ngOnInit(): void {

    this.setDocTitle(this._translocoService.translate('NAV.gamabook') + ' - Xelex')
    AOS.init({
      duration: 1000
    });
    var title = 'Xelex - Gamabook';
    var metaTag = [
      { name: 'twitter:card', content: 'summary' },
      { property: 'og:type', content: 'article' },
      { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal_gamabook' },
      { property: 'og:title', content: 'Xelex' },
      { property: 'og:image', content: 'assets/images/logo/worldsoft.png' },
      { property: 'og:description', content: 'Đối tác' },


    ]
    this.seoService.updateTitle(title);
    this.seoService.updateMetaTags(metaTag);

    this._translocoService.langChanges$.subscribe((activeLang) => {
      //en
      this.activeLang = activeLang;
    });

    AOS.init({
      duration: 1000
    });
    this._fuseMediaWatcherService.onMediaChange$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe(({ matchingAliases }) => {
        // Check if the screen is small
        this.isScreenSmall = !matchingAliases.includes('md');
      });

  }


}




