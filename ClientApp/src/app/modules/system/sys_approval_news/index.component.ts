import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_approval_news_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_approval_news_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_approval_news_indexComponent extends BaseIndexDatatableComponent
{
    public record: any;
    public group_news: any;
    public type_news: any;
    public list_status: any;
    public list_count_tin_tuc: any;
    public data: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string,
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_approval_news',
            { search: "", status_del: "3" , id_group_news : "", id_type_news: ""}
        )

        this.list_status = [
            {
                id: "3",
                name: this._translocoService.translate("common.waiting_approval"),
            },
        ];

        this.http
            .post('/sys_group_news.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.group_news = resp;
            });

        this.http
            .post('/sys_type_news.ctr/getListUseByGroupNew/', {
                id: this.filter.id_group_news
            }
        ).subscribe(resp => {
            this.list_count_tin_tuc = resp;
            });
    }

    changeGroupNews(): void {
        this.http
            .post('/sys_type_news.ctr/getListUseByGroupNew/', {
                id: this.filter.id_group_news
            }
        ).subscribe(resp => {
            this.type_news = resp;
        });
    }

    filtertintuc(trangThaiThongBao) {
        this.filter.loai_thong_bao = trangThaiThongBao;
        this.rerenderfilter();
    }

    approval(id1): void {
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
                        this.rerender();
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
    reject(id1): void {
        Swal.fire({
            title: "Lý do trả về",
            text: "",
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            preConfirm: (login) => {

                if (login != '') {
                    this.http
                        .post(this.controller + '.ctr/reject/',
                            {
                                id: id1,
                                reason: login
                            }
                        ).subscribe(resp => {
                            this.rerender();
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
    revertStatus(model, pos): void {
        model.db.status_del = 1;
        this.http
            .post(this.controller + '.ctr/edit/',
                {
                    data: model,
                }
        ).subscribe(resp => {
            this.rerender();
            },
                error => {
                    console.log(error);

                });
        this.rerender();
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_approval_news_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_approval_news_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_approval_news_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.rerender();
        });
    }
   


    ngOnInit(): void {
        this.baseInitData();
    }


}


