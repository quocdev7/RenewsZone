import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_quan_huyen_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_tinh_thanh_model } from '../sys_tinh_thanh/sys_tinh_thanh.types';
import { HttpParams } from '@angular/common/http';


@Component({
    selector: 'sys_quan_huyen_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_quan_huyen_indexComponent extends BaseIndexDatatableComponent
{

    public list_status_del: any;
    public list_quoc_gia: any=[];
    public list_tinh_thanh: Array<sys_tinh_thanh_model>[];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_quan_huyen',
            { search: "", status_del: "1", id_quoc_gia: "",id_tinh : "" }
        )

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
    //                console.log(error);

    //            });
    //    this.rerender();
    //}
    load_quoc_gia():void{
        this.http
            .post('/sys_quoc_gia.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_quoc_gia = resp;
            });
    }
    load_tinh_thanh():void{
        this.http
            .post('/sys_tinh_thanh.ctr/getListUse/', {
                
            }
            ).subscribe(resp => {
                this.list_tinh_thanh = resp as Array<sys_tinh_thanh_model>[];
            });
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_quan_huyen_popUpAddComponent, {
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
    // exportToExcel() {
    //     this.showLoading("", "", true)
    //     const params = new HttpParams()
    //         .set('search', this.filter.search)
    //         .set('status_del', this.filter.status_del)
    //         .set('id_quoc_gia', this.filter.id_quoc_gia??"-1")
    //         .set('id_tinh', this.filter.id_tinh??"-1")
    //         ;

    //     let uri = this.controller + '.ctr/exportExcel';
    //     this.http.get(uri, { params, responseType: 'blob', observe: 'response' })
    //         .subscribe(resp => {
    //             var res;
    //             res = resp;
    //             var downloadedFile = new Blob([res.body], { type: res.body.type });
    //             const a = document.createElement('a');
    //             a.setAttribute('style', 'display:none;');
    //             document.body.appendChild(a);
    //             a.href = URL.createObjectURL(downloadedFile);
    //             a.target = '_dAblank';
    //             a.download = 'QuanHuyen.xlsx'
    //             a.click();
    //             document.body.removeChild(a);
    //             Swal.close();
    //         })
    // }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_quan_huyen_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_quan_huyen_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }



    ngOnInit(): void {
        this.baseInitData();
        this.load_quoc_gia();
        this.load_tinh_thanh();
       
    }


}


