import { ChangeDetectionStrategy, ChangeDetectorRef, Component, HostListener, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { Subject, Observable } from 'rxjs';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import * as AOS from 'aos';
import { HttpClient } from '@angular/common/http';
import { Title } from '@angular/platform-browser';
@Component({
    selector       : 'about_us_index',
    templateUrl    : './index.component.html',
    styleUrls      : ['./index.component.scss'],
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class about_us_indexComponentsComponent implements OnInit, OnDestroy
{
 
    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    
    selectedPanel: string;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public windowScrolled:any=false;
    public isScreenSmall: any = false;
    public currentUser: any=null;
    public panels_temp:any=[];
    public panels:any=[];
    
    public activeLang: any;
    public loading:any = false;
    public type = '';
    public panel_ve_chung_tois:any=[];
    public panel_gioi_thieus:any=[];
    /**
     * Constructor
     */
    constructor(  private  _translocoService: TranslocoService,
        private titleService : Title,
        private _changeDetectorRef: ChangeDetectorRef,
        public http: HttpClient, 
        private _fuseMediaWatcherService: FuseMediaWatcherService
    )
    {
       
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

   
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }
    load_panel() {
        this.loading = true;
        // this.panels_temp = [
        //     {
        //         id         : '1',
        //         type       : 1,
        //         //icon       : 'account_circle',
        //         title      : this._translocoService.translate("portal.hanh_trinh_xelex"),
        //         description: ''
        //     },
        //     {
        //         id         : '2',
        //         type       : 1,
        //        // icon       : 'edit',
        //         title      : this._translocoService.translate("portal.gia_tri_cot_loi"),
        //         description: ''
        //     },
        //     {
        //         id         : '3',
        //         type       : 1,
        //      //   icon       : 'calendar_today',
        //         title      : this._translocoService.translate("portal.tam_nhin_su_menh"),
        //         description: ''
        //     },
        //     {
        //         id         : '4',
        //         type       : 4,
        //        // icon       : 'calendar_today',
        //         title      : this._translocoService.translate("portal.chung_nhan_nang_luc"),
        //         description: ''
        //     },
        // ];
        this.http
        .post('/sys_cau_hinh_thong_tin.ctr/getListUseNew/', {}
        ).subscribe(resp => {
        
            let result: any= resp;
            var gioi_thieus: any = [];
            var panels_cot_moc: any ;
            for(var i=0;i<result.length;i++){
                this.panels_temp.push({
                    id         : result[i].id,
                    type       : 1,
                   // icon       : 'calendar_today',
                    title      : result[i].name,
                    title_en      : result[i].name_en,
                    noi_dung      : result[i].noi_dung,
                    noi_dung_mobile      : result[i].noi_dung_mobile,
                    noi_dung_en      : result[i].noi_dung_en,
                    noi_dung_mobile_en      : result[i].noi_dung_mobile_en,
                    description: ''
                    
                })

                gioi_thieus=this.panels_temp;
                

              
                
            }
            panels_cot_moc = 
                {
                    id         : '1',
                    type       : 1,
                    //icon       : 'account_circle',
                    title      : "Cột mốc phát triển",
                    title_en   : "Development milestones",
                    description: ''
                }
                //gioi_thieus.push(panels_cot_moc);
                gioi_thieus.splice(2,0,panels_cot_moc);


            this.panel_gioi_thieus = gioi_thieus;
            this.selectedPanel = this.panel_gioi_thieus[0].id;
            this.http
            .post('/sys_nhom_hoi_dong.ctr/getListUse/', {}
            ).subscribe(resp => {
               
                let result: any= resp;
                var ve_chung_tois :any;
                var panels_temp_chung_tois:any =[];
                for(var i=0;i<result.length;i++){
                    panels_temp_chung_tois.push({
                        id         : result[i].id,
                        type       : 2,
                       // icon       : 'calendar_today',
                        title      : result[i].name,
                        title_en      : result[i].name_en,
                        noi_dung      : result[i].noi_dung,
                        noi_dung_mobile      : result[i].noi_dung_mobile,
                        noi_dung_en      : result[i].noi_dung_en,
                        noi_dung_mobile_en      : result[i].noi_dung_mobile_en,
                        description: ''
                        
                    })
    
                    ve_chung_tois=panels_temp_chung_tois;
                    
                }
                this.panel_ve_chung_tois = ve_chung_tois;
                this.panels = this.panel_gioi_thieus.concat(this.panel_ve_chung_tois);
                console.log(this.panels)
                
            });
           
        });
        this.loading = false;
     }
    ngOnInit(): void {
        AOS.init({
            duration:1000
        });
        this._fuseMediaWatcherService.onMediaChange$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe(({ matchingAliases }) => {
            this.isScreenSmall = !matchingAliases.includes('md');
            if ( matchingAliases.includes('md') )
            {
                this.drawerMode = 'side';
                this.drawerOpened = true;
            }
            else
            {
                this.drawerMode = 'over';
                this.drawerOpened = false;
            }

            // Mark for check
            this._changeDetectorRef.markForCheck();});
    
        

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this.setDocTitle(this._translocoService.translate('portal.gioithieu')  + ' - Xelex')

      this.load_panel(); 
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
    scrollToTop(): void  {
        (function smoothscroll() {
            var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
            if (currentScroll > 0) {
                window.requestAnimationFrame(smoothscroll);
                window.scrollTo(0, currentScroll - (currentScroll / 8));
            }
        })();
    }
      
    /**
     * Navigate to the panel
     *
     * @param panel
     */
     goToPanel(panel: string): void
     {
         this.selectedPanel = panel;
 
         // Close the drawer on 'over' mode
         if ( this.drawerMode === 'over' )
         {
             this.drawer.close();
         }
     }
     @HostListener("window:scroll", [])
     onWindowScroll(): void  {
         if (   (window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop) >= 50) {
          
             this.windowScrolled = true;
         } 
        else {
             this.windowScrolled = false;
           
         }


         
     }

    

     /**
      * Get the details of the panel
      *
      * @param id
      */
     getPanelInfo(id: string): any
     {
        
         return this.panels.find(panel => panel.id === id);
     }
 
     /**
      * Track by function for ngFor loops
      *
      * @param index
      * @param item
      */
     trackByFn(index: number, item: any): any
     {
         return item.type || index;
     }
    
}
