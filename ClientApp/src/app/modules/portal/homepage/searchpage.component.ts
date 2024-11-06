import { Component, Inject, OnInit,ViewEncapsulation,ViewChild } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
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


import SwiperCore, { SwiperOptions, EffectCoverflow, Pagination, Navigation ,Autoplay} from "swiper";
import { SwiperComponent } from 'swiper/angular';
import * as AOS from 'aos';

// install Swiper modules
SwiperCore.use([EffectCoverflow, Pagination, Navigation,Autoplay]);



@Component({
    selector     : 'homepage-searchpage',
    templateUrl: './searchpage.component.html',
    styleUrls: ['./searchpage.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class homepageSearchComponent implements OnInit
{
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
    public lst_tat_ca: any = [];
    public event_id: any;
    public is_login: any = false;
    public total_page:any=0;
    public total_item:any=0;
    public filter:any=  { search_key: "", type_info: "-1" ,  id_group_news:"-1",  id_type_news:"-1"}
    public group_news: any;
    public type_news: any = [{
        id: "-1",
        name: this._translocoService.translate('common.all')
    }];

    public loading:any=false;
    public page:any=0;
    public pageList:any=[];
    public lst_type_info:any=[];

    constructor(
  @Inject(DOCUMENT) private document: Document,
  private _fuseMediaWatcherService: FuseMediaWatcherService,
        private location: Location,
       private dialog: MatDialog,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient
        , public _translocoService: TranslocoService)
    {
         this.lst_type_info = [
          
            {
                id: "-1",
                name: this._translocoService.translate("system.all")
            },
            // {
            //     id: "3",
            //     name: this._translocoService.translate("system.thanh_vien")
            // },
            {
                id: "2",
                name: this._translocoService.translate("portal.event")
            },
            {
                id: "1",
                name: this._translocoService.translate("portal.news")
            },
            {
                id: "4",
                name: this._translocoService.translate("portal.product")
            },
        ]
    }

    changetab($event){
         var id=  this.lst_type_info[$event.index].id;
        if(this.filter.type_info !=id){
            
            this.filter.type_info =id;
            this.changeTypeInfo();
        }
     
    }
    ngOnInit() {
      this._authService.check().subscribe((data: any) => this.is_login = data);

        AOS.init({duration:1000 });
     this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });

       this.route.params.subscribe(params => {
           this.filter.search_key = params["id"];
           this.load_nhom();
           this.search(0);
           
        });
        document.getElementsByClassName('mat-tab-header-pagination-before')[0].remove();
        document.getElementsByClassName('mat-tab-header-pagination-after')[0].remove();
       
    
    }




    changeTypeInfo(): void {
         this.beginsearch();
    }
    changeGroupNews(): void {
        this.http
            .post('/sys_type_news.ctr/getListUseByGroup/', {
                id: this.filter.id_group_news
            }
            ).subscribe(resp => {
                this.type_news = resp;
                this.filter.id_type_news = "-1";
                this.type_news.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
                this.beginsearch();
            });
    }


    load_nhom(): void {
        this.http
            .post('/sys_group_news.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.group_news = resp;
                this.group_news.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
            });
    }

      gotoNewsDetailPage(item): void {

        if(item.type_info ==  "1"){
            this.http
            .post('/sys_news.ctr/get_title_news/', {
                id_news: item.id
            }).subscribe(resp => {
                var title_news = resp;
                const url = '/sys_post.ctr/news_detail?tieu_de=' + title_news + "&id=" + item.id + "&t=" + "1";
                window.location.href = url
            });
            // const url = '/portal-news-detail/' + item.id;
            //  this.router.navigateByUrl(url);
           
        }
         if(item.type_info ==  "2"){
            this.http
            .post('/sys_event.ctr/get_title_event/', {
                id_event: item.id
            }).subscribe(resp => {
                var title_event = resp;
                const url = '/sys_post.ctr/event_detail?tieu_de=' + title_event + "&id=" + item.id + "&t=" + "2";
                window.location.href = url
            });
            // const url = '/eventDetail/' + item.id;
            //  this.router.navigateByUrl(url);
           
        }
        if(item.type_info ==  "4"){
            this.http
            .post('/sys_san_pham.ctr/get_title_san_pham/', {
                id:  item.id
            }).subscribe(resp => {
                var title = resp;
                const url = '/sys_post.ctr/san_pham?tieu_de=' +title +"&id="+  item.id  + "&t=" + "3";
                window.location.href=url
            });

            // const url = '/portal_product_detail/' + item.id;
            //  this.router.navigateByUrl(url);
           
        }
    }

   generate_page(){
        this.pageList=[];
        for(var i=1;i<this.total_page-1;i++){
             this.pageList.push(i);
        }
   }

    beginsearch(): void {
       this.search(0);
    }
    
    transform(value: any,type:string): string {
      
        if(!this.filter.search_key) return value;
        if(type==='full'){
          const re = new RegExp("\\b("+ this.filter.search_key.toLowerCase()+"\\b)", 'igm');
          value= value.replace(re, '<span class="highlighted-text">$1</span>');
        }
        else{
          const re = new RegExp(this.filter.search_key.toLowerCase(), 'igm');
          value= value.replace(re, '<span class="highlighted-text">$&</span>');
        }
          return value;
      }
      
    search(page): void {
        if(this.total_page!=0 && (this.page >= this.total_page || this.page < 0 )){
                return
        }
        this.page=page;
        this.lst_tat_ca =[];
        this.http.post('/sys_news.ctr/search_common/', {
                                         "filter":this.filter,
                                         "page":this.page
                                        }).subscribe(resp => {
                                            var a:any =resp;
                                            this.lst_tat_ca = a.lst_news;
                                            this.total_page=a.total_page;
                                            this.total_item=a.total_item;
                                            this.generate_page()
                                        });

   
       
    }
  
   
  
     ketBan(user_id,name,pos): void {
      this.loading=true;
        Swal.fire({
            title: this._translocoService.translate('portal.ban_ket_ban_voi')+' ' + name +" ?",
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
                .post('/sys_user.ctr/invite_user/', {
                    type:"1",
                    email:"",
                    id: user_id,
                }).subscribe(resp => {
                    this.loading=false;
                    this.lst_tat_ca[pos].id_invite = resp;
                    this.lst_tat_ca[pos].status_del =3;
                 });
            }else{
                this.loading=false;
            }
        });


           
    }

    dong_y_ket_ban(id1, name,pos): void {
        this.loading=true;
        Swal.fire({
            title: this._translocoService.translate('portal.ban_ket_ban_voi')+' ' + name +" ?",
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
                .post('/sys_user.ctr/action_invite/', {
                    id: id1,
                    action: 1
                }).subscribe(resp => {
                    this.lst_tat_ca[pos].status_del =1;
                    this.loading=false;
                 });
            }else{
                this.loading=false;
            }
        });

     
           
            
          
        }
        tu_choi_ket_ban(id1, name,pos): void {
            this.loading=true;
            Swal.fire({
                title: this._translocoService.translate('portal.ban_tu_choi_ket_ban')+' ' + name +" ?",
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
                    .post('/sys_user.ctr/action_invite/', {
                        id: id1,
                        action: 2
                    }).subscribe(resp => {
                   
                        this.lst_tat_ca[pos].status_del =0;
                        this.loading=false;
                     });
                }else{
                    this.loading=false;
                }
            });
        }
   
}
