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
@Component({
    selector: 'person_quyen_rieng_tu',
    templateUrl: './quyen_rieng_tu.component.html',
    styleUrls: ['./quyen_rieng_tu.component.scss']
})

export class quyen_rieng_tuComponent  
{
   
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public quyen_rieng_tu: any=null;
    public quyen_loi: any = {};
   public loading:any=false;
   public is_change:any=false;
    
   public lst_quyen:any=[];
    
    constructor(public http: HttpClient, dialog: MatDialog,
    private router: Router,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        public _translocoService: TranslocoService
        ,public  _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
            this.lst_quyen = [
                {
                    id: 1,
                    name: this._translocoService.translate("system.cong_khai")
                },
                {
                    id: 2,
                    name: this._translocoService.translate("system.ban_be")
                },
                {
                    id: 3,
                    name: this._translocoService.translate("system.chi_minh_toi")
                },
    
            ]
           
        }
       
  
        change_quyen_rieng_tu(setting): void {
            this.is_change=true;
        }
    
         
        save_quyen_rieng_tu(): void {
            this.loading = true;
            Swal.fire({
                title: this._translocoService.translate('areYouSure'),
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
    
                    this.http.post("/sys_user.ctr/createQuyenRiengTu", {
                        data: this.quyen_rieng_tu
                    }).subscribe((resp) => {
                        this.load_quyen_rieng_tu();
                        Swal.fire('Lưu thành công', '', 'success');
                    })
                }else{
                    this.loading = false;
                }
            })
        }
        load_quyen_rieng_tu(): void {
            this.http.post("/sys_user.ctr/getQuyenRiengTu", {
            }).subscribe((resp) => {
                this.is_change = false;
                this.loading =false;
                this.quyen_rieng_tu = resp;
                if(this.quyen_rieng_tu.status_user==1){
                   
                } else {
                    this.http
                    .post('/sys_quyen_loi.ctr/getListUse/', {}
                    ).subscribe(resp => {
                    
                                this.quyen_loi = resp;
                    });
                }
            })
    
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
            this.load_quyen_rieng_tu();
    }

    
    gotopersonnews(): void {
        this.router.navigate(["/portal_person_news_index/", { } ]);

    }
    gotopersonevent(): void {
        this.router.navigate(["/person_myevent/", { } ]);

    }
    gotoAbout(): void {
        this.router.navigate(["/portal-association-member/", { } ]);

    }

    gotoprintpreview(): void {
        this.router.navigate(["portal-profile-user/"+this.quyen_rieng_tu.db.Id, { } ]);

    }


}


