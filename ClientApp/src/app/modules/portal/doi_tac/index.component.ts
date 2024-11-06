
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { DataTableDirective } from 'angular-datatables';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
/*import { changePassComponent } from './changePass.component';*/
import { Component, Inject, OnInit, ViewEncapsulation } from '@angular/core';
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
// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation, Autoplay]);
@Component({
  selector: 'portal_doi_tac_index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})

export class portal_doi_tac_indexComponent {
  // @ViewChild('swiper', { static: false }) swiper?: SwiperComponent;
  // slideNext() {
  //   this.swiper.swiperRef.slideNext(100);
  // }
  // slidePrev() {
  //   this.swiper.swiperRef.slidePrev(100);
  // }
  public loading:any = false;
  public file: any;
  public list_status: any;
  public lst_loai_doi_tac: any;
  public lst_doi_tac: any;
  public listtype: any;
  public isScreenSmall: any = false;
  
  public activeLang: any;
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
    this.getlistDoiTac();
    

    this._translocoService.langChanges$.subscribe((activeLang) => {
      this.activeLang = activeLang;
    
    });
  }
  // getlistLoaiDoiTac(): void {
  //   this.http
  //     .post('/sys_loai_doi_tac.ctr/getListUse/', {
  //     }
  //     ).subscribe(resp => {
  //       this.lst_loai_doi_tac = resp;
  //     });
  // }
  getlistDoiTac(): void {
    this.loading = true;
    this.http
      .post('/sys_doi_tac.ctr/get_list_doi_tac/', {
      }
      ).subscribe(resp => {
        this.lst_loai_doi_tac = resp;
        console.log(this.lst_doi_tac);
        this.loading = false;
      });
  }
  setDocTitle(title: string) {
    //console.log('current title:::::' + this.titleService.getTitle());
    this.titleService.setTitle(title);
 }
  ngOnInit(): void {

    this.setDocTitle(this._translocoService.translate('NAV.sys_doi_tac') + ' - Xelex')
    AOS.init({
        duration:1000
    });
    var title = 'Xelex - Đối tác';
        var metaTag = [
            { name: 'twitter:card', content: 'summary' },
            { property: 'og:type', content: 'article' },
            { property: 'og:url', content: 'https://xelex.worldsoft.com.vn/portal_doi_tac_index' },
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
      duration:1000
  });
    this._fuseMediaWatcherService.onMediaChange$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe(({ matchingAliases }) => {
        // Check if the screen is small
        this.isScreenSmall = !matchingAliases.includes('md');
      });

  }


}




