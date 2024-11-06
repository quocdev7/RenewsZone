import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_nha_tai_tro_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_event_nha_tai_tro_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_nha_tai_tro_indexComponent extends BaseIndexDatatableComponent {

    public list_event: any;
    public list_status_del: any;

    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_event_nha_tai_tro',
            { search: "", status_del: "1", event_id: "" }
        )
        this.http
            .post('/sys_event.ctr/getListEventUser/', {

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
                    if (error.status == 403) {
                        Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                    }

                });
        this.rerender();
    }
    openDialogAdd(): void {
        if (this.filter.event_id === "" || this.filter.event_id === null) {
            Swal.fire({
                title: this._translocoService.translate('system.phaichonsukien'),
                text: "",
                icon: "warning",
            })
        }
        else {
            const dialogRef = this.dialog.open(sys_event_nha_tai_tro_popUpAddComponent, {
                disableClose: true,
                width: '768px',
                data: {
                    actionEnum: 1,
                    db: {
                        id: 0,
                        id_su_kien: this.filter.event_id
                    },
                    ten_su_kien: this.list_event.filter(q => q.id == this.filter.event_id)[0].name
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
        const dialogRef = this.dialog.open(sys_event_nha_tai_tro_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_event_nha_tai_tro_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }

    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }

    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Nhà tài trợ - Xelex');
    }


}


