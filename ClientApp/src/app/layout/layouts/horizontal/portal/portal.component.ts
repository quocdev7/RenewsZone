import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation,HostListener } from '@angular/core';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { FuseNavigationItem, FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';
import { InitialData } from 'app/app.types';
import { AuthService } from '../../../../core/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { NgIf } from '@angular/common';
import { Title } from '@angular/platform-browser';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';


@Component({
    selector     : 'portal-layout',
    templateUrl  : './portal.component.html',
     styleUrls: ['./portal.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class portalLayoutComponent implements OnInit, OnDestroy
{
    private _compactNavigation: FuseNavigationItem[] ;
    private _defaultNavigation: FuseNavigationItem[] ;
    private _futuristicNavigation: FuseNavigationItem[] ;
    private _horizontalNavigation: FuseNavigationItem[] ;
    isScreenSmall: boolean;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public user: any = {
        type:0
    };

    
    public activeLang: any;

    windowScrolledHeader: boolean;

    windowScrolled: boolean;
    public is_login: any=false;
    public list_group_new: any;
    public list_khoa: any;
    public navigation: any = {};
    /**
     * Constructor
     */
    constructor(
        private _authService: AuthService,
        private changeDetectorRef: ChangeDetectorRef,
        private _activatedRoute: ActivatedRoute,
        private _router: Router,
        private http: HttpClient,
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _fuseNavigationService: FuseNavigationService,
         public _translocoService: TranslocoService,
    )
    {

      
      
    }
 
      @HostListener("window:scroll", [])
      onWindowScroll(): void  {
          if (   (window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop) >= 50) {
            this.windowScrolledHeader = true;
              this.windowScrolled = true;
          } 
         else {
              this.windowScrolled = false;
               this.windowScrolledHeader = false;
          }


          
      }
      scrollToTop(): void  {
          (function smoothscroll() {
              var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
              if (currentScroll > 0) {
                  window.requestAnimationFrame(smoothscroll);
                  window.scrollTo(0, currentScroll - (currentScroll / 8));
              }
          })();
      }
        

    loadmenu(): void  {
        if(this.is_login==true){
            this._defaultNavigation = 
            [
                {
                    link:"/homepage-index",
                    id: 'homepage',
                    module: 'system',
                    title: 'portal.homepage',
                    translate: 'portal.homepage',
                    type: 'basic',
                        //icon_img: "/assets/images/portal/home.png",
                }, 
                {
                    id: 'aboutus',
                    module: 'system',
                    title: 'portal.gioithieu',
                    translate: 'portal.gioithieu',
                    link: "/about_us_index",
                    type: 'basic',
                // icon_img: "/assets/images/portal/Ve_chung_toi.png",
                
                },
            ]
        }else{
            this._defaultNavigation = [{
                link:"/homepage-index",
                id: 'homepage',
                module: 'system',
                title: 'portal.homepage',
                translate: 'portal.homepage',
                type: 'basic',
                //icon_img: "/assets/images/portal/home.png",
            },
            {
                id: 'aboutus',
                module: 'system',
                title: 'portal.gioithieu',
                translate: 'portal.gioithieu',
            link: "/about_us_index",
                type: 'basic',
            // icon_img: "/assets/images/portal/Ve_chung_toi.png",
            
            }]
        }
        var đinh_gia_thu_cu: FuseNavigationItem = {
            id: 'faculty',
             module: 'system',
            title: 'Định giá - Thu cũ',
            translate: 'Định giá - Thu cũ',
            type: 'basic',
       //  icon_img: "/assets/images/portal/Lien_he.png",
            link: "dinh-gia-thu-cu"

         };
         var hang_cu: FuseNavigationItem = {
            id: 'faculty',
             module: 'system',
            title: 'Sản phẩm',
            translate: 'portal.product',
            type: 'basic',
       //  icon_img: "/assets/images/portal/Lien_he.png",
            link: "hang-cu"

         };
         var lien_he: FuseNavigationItem = {
            id: 'faculty',
            module: 'system',
            title: 'portal.contactus',
            translate: 'portal.contactus',
            type: 'basic',
          //  icon_img: "/assets/images/portal/Lien_he.png",
            link: "lien-he"

        }
        this._defaultNavigation.push(đinh_gia_thu_cu);   
        this._defaultNavigation.push(hang_cu);   
        this._defaultNavigation.push(lien_he);   
        
        this._compactNavigation = [...this._defaultNavigation];
        this._futuristicNavigation = [...this._defaultNavigation];
        this._horizontalNavigation = [...this._defaultNavigation];
        this.navigation =
        {
            compact: this._compactNavigation,
            default: this._defaultNavigation,
            futuristic: this._futuristicNavigation,
            horizontal: this._horizontalNavigation
        };
   
            //     var san_pham: FuseNavigationItem = {
            //         id: 'faculty',
            //          module: 'system',
            //         title: 'Sản phẩm',
            //         translate: 'portal.product',
            //         type: 'basic',
            //    //  icon_img: "/assets/images/portal/Lien_he.png",
            //         link: "portal_product"

            //      };
            //      var gamabook: FuseNavigationItem = {
            //         id: 'faculty',
            //          module: 'system',
            //         title: 'GamaBook',
            //         translate: 'portal.gamabook',
            //         type: 'basic',
            //    //  icon_img: "/assets/images/portal/Lien_he.png",
            //         link: "gamabook"

            //      };
            //      var dich_vu: FuseNavigationItem = {
            //         id: 'faculty',
            //          module: 'system',
            //         title: 'Sản phẩm',
            //         translate: 'portal.dich_vu',
            //         type: 'basic',
            //    //  icon_img: "/assets/images/portal/Lien_he.png",
            //         link: "portal_nang_luc"

            //      };
            //      var doi_tac: FuseNavigationItem = {
            //         id: 'faculty',
            //          module: 'system',
            //         title: 'Đối tác',
            //         translate: 'portal.doi_tac',
            //         type: 'basic',
            //    //  icon_img: "/assets/images/portal/Lien_he.png",
            //         link: "portal_doi_tac_index"

            //      };
            //      var nang_luc: FuseNavigationItem = {
            //         id: 'faculty',
            //          module: 'system',
            //         title: 'Năng lực',
            //         translate: 'portal.nang_luc',
            //         type: 'basic',
            //    //  icon_img: "/assets/images/portal/Lien_he.png",
            //         link: "portal_nang_luc"

            //      };
            //      var event: FuseNavigationItem = {
            //          id: 'faculty',
            //          module: 'system',
            //          title: 'portal.event',
            //          translate: 'portal.event',
            //          type: 'basic',
            //        //  icon_img: "/assets/images/portal/Su_kien.png",
            //          link: "portal-event"
                    
            //     }   
               
                
            



    // this.http
    //     .post('/sys_group_news.ctr/getListUseDetail', {}
    // ).subscribe(resp_news => {
    //     this.list_group_new = resp_news
    //         this.http
    //             .post('/sys_khoa.ctr/getListUse', {}
    //         ).subscribe(resp_khoa => {
    //             this.list_khoa = resp_khoa;

    //             //this._defaultNavigation.push(nang_luc);   
                
    //             for (var i = 0; i < this.list_group_new.length; i++){

                    
                    
    //                     //if loai tin tuc thuoc nhom ho tro
    //                     if(this.list_group_new[i].list_types.length>0 && this.list_group_new[i].id =="eb20e2d0-e2bc-4453-a38d-830cbc4261c3"){
                           
                          
    //                         var group: FuseNavigationItem =
    //                         {
    //                             id: this.list_group_new[i].id,
    //                             module: 'system',
    //                             title: this.list_group_new[i].name,
    //                             translate: this.list_group_new[i].name,
    //                             type: 'group',
    //                           //  icon_img: this.list_group_new[i].image,
    //                              link: "/portal-news/"+ this.list_group_new[i].id,
    //                              children:[]
    //                         };
                           
    
                       
    //                         for (var j = 0; j < this.list_group_new[i].list_types.length; j++) {                               
    //                             var type: FuseNavigationItem ={
    //                                 id : this.list_group_new[i].list_types[j].id,
    //                                 module: 'system',
    //                                 title: this.list_group_new[i].list_types[j].name,
    //                                 link: '/portal-type-news/' + this.list_group_new[i].list_types[j].id,
    //                                 translate:  '',
    //                                 //icon_img: this.list_group_new[i].list_types[j].image,
    //                                 type: 'basic',
    //                             }

    //                             var title =  "" ;
    //                             if(this.activeLang == 'vi'){
    //                               title= this.list_group_new[i].list_types[j].name;
    //                             }
    //                             if(this.activeLang == 'en'){
    //                               title= this.list_group_new[i].list_types[j].name_en;
    //                             }
    //                             type.title = title;
    //                             group.children.push(type)          
                            
                     

    //                        }
    //                        this._defaultNavigation.push(event);

                           
    //                         group.translate="portal.support"
                   

    //                        this._defaultNavigation.push(group);
    //                     }else{

    //                         if(this.list_group_new[i].id !="7282a98e-54bf-4c70-8857-a9b96facb6ec"){
    //                             var group: FuseNavigationItem =
    //                             {
    //                                 id: this.list_group_new[i].id,
    //                                 module: 'system',
    //                                 title: this.list_group_new[i].name,
    //                                 translate: this.list_group_new[i].name,
    //                                 type: 'basic',
    //                               //  icon_img: this.list_group_new[i].image,
    //                                  link: "/portal-news/"+ this.list_group_new[i].id,
    //                                  children:[]
    //                             };

                            
    //                             if(this.list_group_new[i].id =="0a2ef7be-79c2-47e2-9ae4-270a457f391c"){
    //                                 group.translate="portal.news"
    //                             }
    //                             if(this.list_group_new[i].id =="f3ccf05d-31b3-4e38-927b-690b8d6fceb1"){
    //                                 group.translate="portal.recruit"
    //                             }

    //                             this._defaultNavigation.push(group);
    //                         }
                           
    //                     }
                       
                     
                           
                           
    //              };
                
           
    //             //this._defaultNavigation.push(item_type_new);
             
             
    //             //this._defaultNavigation.push(tin_tuc);
              
    //             //this._defaultNavigation.push(doi_tac);
    //             //this._defaultNavigation.push(career);
    //             //this._defaultNavigation.push(ho_tro);
    //             //this._defaultNavigation.push(alumni_conect);
              
    //            // this._defaultNavigation.push(contact_us);

                
                 
             
    //             });
    //     });

}


    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for current year
     */
    get currentYear(): number
    {
        return new Date().getFullYear();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
      

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this._authService.check().subscribe((data: any) => {
            this.is_login = data
            this.loadmenu();
        });
        this._authService.getUser().subscribe((data: any) => {
            if (data != undefined)  this.user = data;
         
        });
      

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({matchingAliases}) => {

                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
           
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Toggle navigation
     *
     * @param name
     */
    toggleNavigation(name: string): void
    {
        // Get the navigation
        const navigation = this._fuseNavigationService.getComponent<FuseVerticalNavigationComponent>(name);

        if ( navigation )
        {
            // Toggle the opened status
            navigation.toggle();
        }
    }
}
