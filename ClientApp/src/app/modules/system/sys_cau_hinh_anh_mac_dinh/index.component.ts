import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_cau_hinh_anh_mac_dinh_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_cau_hinh_anh_mac_dinh_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_cau_hinh_anh_mac_dinh_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_type: any;

    public list_status_del: any;
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_cau_hinh_anh_mac_dinh',
            { search: "", type: -1, status_del :"1"}
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
        this.list_type = [
            {
                id:-1,
                name: this._translocoService.translate("system.all")
            },
            {
                id: 1,
                name: this._translocoService.translate("system.thong_tin_ca_nhan")
            },
            {
                id: 2,
                name: this._translocoService.translate("system.tin_tuc")
            },
            {
                id: 3,
                name: this._translocoService.translate("system.su_kien")
            },
            {
                id: 4,
                name: this._translocoService.translate("system.tuyen_dung")
            },
            {
                id: 5,
                name: this._translocoService.translate("system.icon_fb")
            },
            {
                id: 6,
                name: this._translocoService.translate("system.icon_zl")
            },
            {
                id: 7,
                name: this._translocoService.translate("system.icon_lk")
            },
            {
                id: 8,
                name: this._translocoService.translate("system.icon_tw")
            },
            {
                id: 9,
                name: this._translocoService.translate("system.icon_bl")
            },
            {
                id: 10,
                name: this._translocoService.translate("system.avatar_khach_moi")
            },

        ]
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
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_cau_hinh_anh_mac_dinh_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_cau_hinh_anh_mac_dinh_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_cau_hinh_anh_mac_dinh_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }


    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Cấu hình ảnh mặc định - Xelex'); 
    }


}


