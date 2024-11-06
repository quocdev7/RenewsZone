import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_email_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_event_email_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_email_indexComponent extends BaseIndexDatatableComponent {
    public send_status: any;
    public trang_thais: any;
    public list_status_del: any;
    public list_event: any;
    public event_id: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_event_email',
            { search: "", trang_thai: "-1", event_id: "" }
        )

        this.http
            .post('/sys_event.ctr/getListUse/', {
                
            }
            ).subscribe(resp => {
                this.list_event = resp;
            });
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
         // 0 la chuan bi gui, 1 thanh cong, 2 that bai
        this.send_status = [
            {
                id: "0",
                name: this._translocoService.translate('system.chuan_bi_gui')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.thanh_cong')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.that_bai')
            },
        ];
        this.trang_thais = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.dang_dien_ra')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.ket_thuc')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.sap_toi')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.huy')
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
    //                if (error.status == 403) {
    //                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
    //                }

    //            });
    //    this.rerender();
    //}
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_event_email_popUpAddComponent, {
            disableClose: true,
            width: '100%',
            height: '100%',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    event_id: this.filter.event_id
                },
                 ten_su_kien: this.list_event.filter(q => q.id == this.filter.event_id)[0].name
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_event_email_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_event_email_popUpAddComponent, {
            disableClose: true,
            width: '100%',
            height: '100%',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    ngOnInit(): void {
        this.baseInitData();
    }


}


