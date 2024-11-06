import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_user_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { changePassComponent } from './changePass.component';
import { portal_approval_user_popUpAddComponent } from 'app/modules/portal/approved_user/popupAdd.component';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'sys_user_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_user_indexComponent extends BaseIndexDatatableComponent
{
    public file: any;
    public list_status_del: any;
    public listtype: any;
    constructor(http: HttpClient, dialog: MatDialog,private titleService: Title
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_user',
            { search: "", status_del: "1", type:1}
        )
        
        this.list_status_del = [
            {
                id: "1",
                name: this._translocoService.translate("system.use"),
            },
            {
                id: "2",
                name: this._translocoService.translate("system.not_use"),
            },
        ];
        this.listtype = [
            {
                id: 1,
                name: this._translocoService.translate('system.company_staff')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.khach')
            }
        ];

    }
    approval(id1): void {
        var that = this;

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
        var that = this;
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
                            Swal.fire('Trả về thành công', '', 'success').then(
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

    onFileSelected(event: any) {
        this.file = event.target.files[0];
        //this.onSubmitFile();
        //event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_user.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile() {

        if (this.file == null || this.file == undefined) {

            Swal.fire('Phải chọn file import', '', 'warning')

        } else {
            var formData = new FormData();

            formData.append('file', this.file);
            this.http.post('/sys_user.ctr/ImportFromExcel/', formData, {
                //reportProgress: true,
                //observe: 'events'
            })
                .subscribe(res => {

                    if (res == "") {
                        Swal.fire('Lưu thành công', '', 'success');
                        this.rerender();

                    } else {
                        Swal.fire(res.toString(), '', 'warning')
                    }

                })
        }

    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
    
            data: {
                actionEnum: 1,
                db: {
                    type:1,
                    id: 0,
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    changePasss(model, pos): void {
        model.type = 1;
        const dialogRef = this.dialog.open(changePassComponent, {
            width: '768px',
            disableClose: true,
            data: model
        });
    }
    revertStatus(id): void {
        this.http
            .post(this.controller + ".ctr/revert/", {
                id: id,
            })
            .subscribe(
                (resp) => {
                    this.rerender();
                },
                (error) => {
                    if (error.status == 403) {
                        Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                    }
                }
            );
        this.rerender();
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
    model.view_user = 1;
        const dialogRef = this.dialog.open(portal_approval_user_popUpAddComponent, {
            disableClose: true,
            width: '100%',
            height: '100%',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
            this.rerender();
        });
    
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }
    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Người dùng - Xelex'); 
    }


}


