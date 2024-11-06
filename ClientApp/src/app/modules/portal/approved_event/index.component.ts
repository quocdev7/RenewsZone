import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { portal_approved_event_popUpAddComponent } from './popupAdd.component';



import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'portal_approved_event_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class portal_approved_event_indexComponent extends BaseIndexDatatableComponent
{


    public trang_thais: any;
    public list_status_del: any;

    public list_type: any;

    public list_trangthai_dangky: any;


    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_approved_event',
            { search: "", trang_thai: -1, tu_ngay: null, den_ngay: null, id_hinh_thuc: "-1", is_register_event: "-1" }
        )

        this.load_trang_thai_dang_ky();
        this.load_trang_thai_su_kien();
        this.load_hinh_thuc();
        this.load_status_del();
        this.load_date();


    }
    load_date() {
        this.filter.tu_ngay = new Date();;
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 7);
        this.filter.den_ngay = new Date();
    }
    load_status_del() {
        this.list_status_del = [
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.not_use')
            }
        ];
    }
    load_trang_thai_dang_ky() {

        this.list_trangthai_dangky = [
            { id: '-1', name: "Tất cả" },
            { id: 1, name: this._translocoService.translate('system.cho_phep_dang_ky') },
            { id: 2, name: this._translocoService.translate('system.khong_cho_phep_dang_ky') },
        ]
    }
    
    load_trang_thai_su_kien() {
        this.trang_thais = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('system.dang_dien_ra')
            },
            // {
            //     id: 3,
            //     name: this._translocoService.translate('system.ket_thuc')
            // },

            {
                id: 4,
                name: this._translocoService.translate('system.sap_toi')
            },
            // {
            //     id: 2,
            //     name: this._translocoService.translate('system.huy')
            // },

        ];
        
    }

        load_hinh_thuc() {
            this.list_type = [
                { id: "-1", name: this._translocoService.translate('system.all') },
                { id: "1", name: this._translocoService.translate('system.offline') },
                { id: "2", name: this._translocoService.translate('system.online') },
                { id: "3", name: this._translocoService.translate('system.hoc_bong') },
                { id: "4", name: this._translocoService.translate('system.tai_tro') }
            ]
        }

    //approval(model, pos): void {
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

    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
            const dialogRef = this.dialog.open(portal_approved_event_popUpAddComponent, {
                disableClose: true,
                width: '768px',
                data: model
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result != undefined && result!=null) this.listData[pos] = result;
            });
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

                    Swal.fire('Duyệt thành công', '', 'success').then((result) => {
                        if (result.isConfirmed) {
                            this.rerender();
                        }
                    })
                      
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
                        .post(this.controller + '.ctr/cancel/',
                            {
                                id: id1,
                                reason: login
                            }
                        ).subscribe(resp => {
                           
                            Swal.fire('Trả về thành công', '', 'success').then((result) => {
                                if (result.isConfirmed) {
                                    this.rerender();
                                }
                            })
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

    ngOnInit(): void {
        this.baseInitData();
    }


}


