import { ChangeDetectorRef, Component, Inject, OnDestroy, OnInit, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { translateDataTable } from '../../../../@fuse/components/commonComponent/VietNameseDataTable';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import * as AOS from 'aos';
import { MatDrawer } from '@angular/material/sidenav';
@Component({
    selector: 'person_setting',
    templateUrl: './setting.component.html',
    styleUrls: ['./setting.component.scss']
})

export class person_settingComponent implements OnInit, OnDestroy
{
   
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public currentUser: any=null;
    
    public panels:any=[];
    public quyen_loi:any=[];
    
    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    selectedPanel: string = 'account';
  
    
    constructor(private http: HttpClient, dialog: MatDialog,
    private router: Router,
         private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private  _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        ) {
      
    }
    loadUser(): void {
             this.http
                    .post('/sys_user.ctr/getUserLogin/', {}
             ).subscribe(resp => {
             
            this.currentUser = resp;
            if(this.currentUser.status_del!=1){
                this.http
                .post('/sys_quyen_loi.ctr/getListUse/', {}
                ).subscribe(resp => {
                
                            this.quyen_loi = resp;
                       
                });

            } else{
                this.panels = [
                    {
                        id         : 'account',
                        icon       : 'account_circle',
                        title      : this._translocoService.translate("person_info"),
                        // description: 'Quản lý thông tin cá nhân, kết nối với mọi người'
                    },
                    {
                        id         : 'person_ban_be',
                        icon       : 'contacts',
                        title      : this._translocoService.translate("portal.ban_be"),
                        description: ''
                    },
                    {
                        id         : 'portal_person_news_index',
                        icon       : 'edit',
                        title      : this._translocoService.translate("system.bai_viet"),
                        // description: 'Quản lý bài viết cá nhân'
                    },
                    
                   
                    // {
                    //     id         : 'gioi_thieu_bka',
                    //     icon       : 'info',
                    //     title      : this._translocoService.translate("portal.gioi_thieu_bka"),
                    //     description: ''
                    // },
                    
                ];
                if(this.currentUser.check_aprroval_news){
                    this.panels.push({
                        id         : 'portal_approval_new_index',
                        icon       : 'edit',
                        title      : this._translocoService.translate("portal.duyet_bai_viet"),
                        description: ''
                    })
                    
                }

                if(this.currentUser.check_aprroval_user){
                    this.panels.push({
                        id         : 'portal_approval_user_index',
                        icon       : 'edit',
                        title      : this._translocoService.translate("portal.duyet_user"),
                        // description: 'Duyệt người dùng thuộc hình thức và khoa mà bạn được phân quyền'
                    })
                    
                }
                if(this.currentUser.check_aprroved_event){
                    this.panels.push({
                        id         : 'portal_approved_event_index',
                        icon       : 'edit',
                        title      : this._translocoService.translate("portal.duyet_event"),
                        // description: 'Duyệt sự kiện thuộc hình thức và khoa bạn được phân quyền'
                    })
                    
                }
                //this.panels.push( {
                //    id         : 'portal_person_event_index',
                //    icon       : 'calendar_today',
                //    title      : this._translocoService.translate("portal.tao_su_kien"),
                //    // description: 'Tạo sự kiện'
                //})
                this.panels.push({
                    id         : 'portal_approved_event_index',
                    icon       : 'edit',
                    title      : this._translocoService.translate("portal.duyet_event"),
                    // description: 'Duyệt sự kiện thuộc hình thức và khoa bạn được phân quyền'
                })

                
              
                
                this.panels.push( {
                    id         : 'person_myevent',
                    icon       : 'calendar_today',
                    title      : this._translocoService.translate("event_managerment"),
                    // description: 'Quản lý những sự kiện đã đăng ký, được mời, tham dự'
                })

  
                this.panels.push( {
                    id         : 'person_quyen_rieng_tu',
                    icon       : 'settings',
                    title      : this._translocoService.translate("system.quyen_rieng_tu"),
                    description: ''
                })
                this._changeDetectorRef.markForCheck();
            }
             });
          
        }
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
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
                this._changeDetectorRef.markForCheck();

        });
            this.loadUser();
           


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
         if(panel=="gioi_thieu_bka"){
            const url = '/about_us_index';
            this.router.navigateByUrl(url);
            return;
         }
         // Close the drawer on 'over' mode
         if ( this.drawerMode === 'over' )
         {
             this.drawer.close();
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
         return item.id || index;
     }
    

}


