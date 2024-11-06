import { Component, Inject, ViewChild } from '@angular/core';


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
import { NgxCaptureService } from 'ngx-capture';
import { tap } from 'rxjs/operators';
@Component({
    selector: 'person_print_preview',
    templateUrl: './print_preview.component.html',
    styleUrls: ['./print_preview.component.scss']
})

export class person_print_previewComponent extends BaseIndexDatatableComponent
{
   
    @ViewChild('screen', { static: true }) screen: any;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public currentUser: any=null;
    public quyen_loi: any = {};
    public list_user_education: any = [];
    public list_user_success: any = [];
    public list_user_certificate: any = [];
    public list_user_work_history: any = [];
    public list_user_experience: any = [];
    public lst_users_manager:any=[];
    public lst_news_manager:any=[];
    public lst_event_manager:any=[];
    
    public is_show_hide_education: any = true;
    public is_show_hide_success: any = true;
    public is_show_hide_certificate: any = true;
    public is_show_hide_work_history: any = true;
    public is_show_hide_experience: any = true;
    public is_show_hide_social: any = true;


    public lst_events:any=[];
    
    constructor(http: HttpClient, dialog: MatDialog,
    private router: Router,
        public captureService: NgxCaptureService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
         _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_news',
            { search: "", status_del: "-1", id_group_news: "-1", id_type_news: "-1", id_phamvi: "1", tu_ngay: null, den_ngay: null, quyen_rieng_tu: "", is_hot: false }
        )
    }
    loadUser(): void {
             this.http
                    .post('/sys_user.ctr/getUserInfo/', {}
             ).subscribe(resp => {
             
                        this.currentUser = resp;
                        if(this.currentUser.db.status_del==1){
                            this.lst_users_manager =[
                                // { name:"Thông báo",
                                // number: 1,
                                // },
                                // { name:"Tin nhắn",
                                // number:3},
                             ];
                             this.loadEducation();  
                             this.loadSuccess(); 
                             this.loadCertificate(); 
                             this.loadWorkHistory(); 
                             this.loadExperience();  

                             this.currentUser.show_ket_noi_voi_toi = 
                    (this.currentUser.db.website_link!=null && this.currentUser.db.website_link!='') ||
                    (this.currentUser.db.linkedin_link!=null && this.currentUser.db.linkedin_link!='') ||
                    (this.currentUser.db.facebook_link!=null && this.currentUser.db.facebook_link!='') ||
                    (this.currentUser.db.twitter_link!=null && this.currentUser.db.twitter_link!='') ||
                    (this.currentUser.db.youtube_link!=null && this.currentUser.db.youtube_link!='') ||
                    (this.currentUser.db.instagram_link!=null && this.currentUser.db.instagram_link!='') 
                    ;
                    
                        } else {
                            this.http
                            .post('/sys_quyen_loi.ctr/getListUse/', {}
                            ).subscribe(resp => {
                            
                                        this.quyen_loi = resp;
                            });
                        }
             });
          
        }
        loadEducation(): void {
            this.http.post("/sys_user.ctr/getEducationUser/", {}).subscribe((resp) => {
                this.list_user_education = resp;
            });
        }
      
        loadSuccess(): void {
            this.http.post("/sys_user.ctr/getSuccessUser/", {}).subscribe((resp) => {
                this.list_user_success = resp;
            });
        }
        loadCertificate(): void {
    
            this.http.post("/sys_user.ctr/getCertificateUser/", {}).subscribe((resp) => {
                this.list_user_certificate = resp;
            });
        }
        loadWorkHistory(): void {
    
            this.http.post("/sys_user.ctr/getWorkHistoryUser/", {}).subscribe((resp) => {
                this.list_user_work_history = resp;
            });
        }
        loadExperience(): void {
            this.http.post("/sys_user.ctr/getExperienceUser/", {}).subscribe((resp) => {
                this.list_user_experience = resp;
            });
        }
        showHideEducation(): void {
            this.is_show_hide_education = !this.is_show_hide_education;
        }
        
        showHideSuccess(): void {
            this.is_show_hide_success = !this.is_show_hide_success;
        }
        showHideCertificate(): void {
            this.is_show_hide_certificate = !this.is_show_hide_certificate;
    
        }
        showHideWorkHistory(): void {
            this.is_show_hide_work_history = !this.is_show_hide_work_history;
        }
        showHideExperience(): void {
            this.is_show_hide_experience = !this.is_show_hide_experience;
        }
        showHideSocial(): void {
            this.is_show_hide_social = !this.is_show_hide_social;
        }



    ngOnInit(): void {
        AOS.init({
            duration:1000
        });
     this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
            this.loadUser();
    }

    
    print() {
        this.captureService.getImage(this.screen.nativeElement, true)
            .pipe(
                tap(img => {
                    let base64String = img;
                    this.debugBase64(base64String);
                })
            ).subscribe();
    }


    debugBase64(base64URL) {
        var popupWin: any;
        // console.log(printContents);
        popupWin = window.open("", "_blank", "top=0,left=0,height=100%,width=auto");
        popupWin.document.open();
        var printcontent = base64URL;
        popupWin.document.write(`
      <html>
        <head>
          <title>Reporte</title>
          <meta name="viewport" content="width=10000, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
        <script>
        window.onafterprint = function(){
            window.close();
        }
        </script>
          <style>
            .salto_pagina_despues{
              page-break-after:always;
            }
            
            .salto_pagina_anterior{
              page-break-before:always;
            }
            @page  
            { 
                size: auto;   /* auto is the initial value */ 

                /* this affects the margin in the printer settings */ 
                margin: 25mm 15mm 25mm 15mm;  
            } 
             .content {
              height: 100vh;
              width: 100%;
              display: flex;
              flex-direction: column;
            }

            .img-content {
              flex: 1;
              display: flex;
              justify-content: center;
              align-items: center;
            }

            .observation {
              height: 150px;
              overflow: hidden;
              overflow-y: auto;
            }
          </style>
        </head>
        <body onload="window.print();">
         <div class="salto_pagina_anterior content">
            
            <div class="img-content">
              <img src="${printcontent}">
            </div>
          </div>
          
        </body>
      </html>`);
        /* window.close(); */
        popupWin.document.close();
    }


}


