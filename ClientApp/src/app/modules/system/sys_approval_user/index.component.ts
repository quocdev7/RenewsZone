import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_approval_user_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';
/*import { changePassComponent } from './changePass.component';*/


@Component({
    selector: 'sys_approval_user_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_approval_user_indexComponent extends BaseIndexDatatableComponent
{
   public file: any;
    public list_status: any;
    public listtype: any;
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_approval_user',
            { search: "", status_del: 4 }
        )
        this.list_status = [
             {
                id:4,
                 name: this._translocoService.translate('system.cho_xet_duyet')
            },
            {
                id: 5,
                name: this._translocoService.translate('system.khong_duyet')
            },
        ];

    }

      approval(id1): void {
var that=this;

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
                this.http
                    .post(this.controller + '.ctr/approval/',
                        {
                            id: id1,
                        }
                ).subscribe(resp => {

                        Swal.fire('Duyệt thành công', '', 'success').then(
                        // Navigate to the confirmation required page
                        res => {
                             that.rerender();
                        });
                       
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    
 cancel(id1): void {
var that=this;
        Swal.fire({
            title: "Lý do không xác minh",
            text: "",
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            preConfirm: (login) => {

                if (login != '') {
                    this.http
                        .post(this.controller + '.ctr/cancel/',
                            {
                                id: id1,
                                reason: login
                            }
                        ).subscribe(resp => {
                              Swal.fire('Duyệt thành công', '', 'success').then(
                                // Navigate to the confirmation required page
                                res => {
                                     that.rerender();
                                });

                            
                        },
                            error => {
                                if (error.status == 403) {
                                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                                }


                            }
                        );
                } else {
                    Swal.showValidationMessage("Vui lòng nhập lý do");
                }
            },
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no'),
        }).then((result) => {

        })

    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }

    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Duyệt thành viên - Xelex'); 
    }


   
}


