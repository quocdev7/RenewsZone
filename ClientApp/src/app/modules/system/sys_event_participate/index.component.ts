import { Component, Inject, ViewChild } from '@angular/core';

import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_participate_popUpAddComponent } from './popupAdd.component';
import { sys_event_participate_popUpNhomNguoiDungComponent } from './popupNhomNguoiDung.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_event_participate_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_participate_indexComponent extends BaseIndexDatatableComponent
{
    public list_su_kien: any;
    public list_count_su_kien: any;
    public list_trang_thai: any;
    public trang_thais: any;
    public list_hinh_thuc: any;
    public ten_hinh_thuc: any;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_event_participate',
            { search: "", trang_thai: "-1", event_id: "", hinh_thuc: "1" }
        )

        this.ten_hinh_thuc = this._translocoService.translate('system.khachmoi')

        this.list_hinh_thuc = [
            {
                id: "1",
                name: this._translocoService.translate('system.khachmoi')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.bantochuc')
            }
        ];
        this.trang_thais = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.chuaxacnhan')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.tuchoithamdu')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.xacnhan')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.denthamdu')
            }
        ];

        this.list_trang_thai = this.trang_thais;

        this.http
            .post('/sys_event.ctr/getListUse', {}
            ).subscribe(resp => {
                this.list_su_kien = resp;
            });

        this.http
            .post('/sys_event.ctr/countSuKien', {}
            ).subscribe(resp => {
                this.list_count_su_kien = resp;
            });
    }

    public moilai(id1): void {
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
                    .post(this.controller + '.ctr/moilai/',
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

    public xacnhan(id1): void {
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
                    .post(this.controller + '.ctr/xacnhan/',
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

    //revertStatus(model, pos): void {
    //    model.db.status_del = 1;
    //    this.http
    //        .post(this.controller + '.ctr/edit/',
    //            {
    //                data: model,
    //            }
    //    ).subscribe(resp => {
    //        this.rerender();
    //        },
    //            error => {
    //                if (error.status == 403) {
    //                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
    //                }

    //            });
    //    this.rerender();
    //}
    openDialogAdd(): void {
        if (this.filter.event_id === "" || this.filter.event_id === null) {
            Swal.fire({
                title: this._translocoService.translate('system.phaichonsukien'),
                text: "",
                icon: "warning",
            })
        }
        else {
            const dialogRef = this.dialog.open(sys_event_participate_popUpAddComponent, {
                disableClose: true,
                width: '850px',
                data: {
                    actionEnum: 1,
                    db: {
                        id: 0,
                        trang_thai: 1,
                        event_id: this.filter.event_id,
                        role: 1
                    },
                    ten_su_kien: this.list_su_kien.filter(q => q.id == this.filter.event_id)[0].name
                },
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result.db.id == 0) return;
                this.rerender();
            });
        }
    }

    openDialogNhomNguoiDung(): void {
        if (this.filter.event_id === "" || this.filter.event_id === null) {
            Swal.fire({
                title: this._translocoService.translate('system.phaichonsukien'),
                text: "",
                icon: "warning",
            })
        }
        else {
            const dialogRef = this.dialog.open(sys_event_participate_popUpNhomNguoiDungComponent, {
                disableClose: true,
                width: '850px',
                data: {
                    actionEnum: 1,
                    db: {
                        id: 0,
                        trang_thai: 1,
                        event_id: this.filter.event_id,
                        role: 1
                    },
                    ten_su_kien: this.list_su_kien.filter(q => q.id == this.filter.event_id)[0].name
                },
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result.db.id == 0) return;
                this.rerender();
            });
        }
    }

    openDialogEdit(model, pos): void {      
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_event_participate_popUpAddComponent, {
            disableClose: true,
           width: '100%',
           height: '100%',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_event_participate_popUpAddComponent, {
            disableClose: true,
            width: '100%',
            height: '100%',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
   
    public anhien(id1): void {
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
                    .post(this.controller + '.ctr/anhien/',
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

    ngOnInit(): void {
        this.baseInitData();
    }


}


