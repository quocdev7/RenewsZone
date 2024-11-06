import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_help_detail_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_help_detail_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_help_detail_indexComponent extends BaseIndexDatatableComponent
{
    public list_status_del: any;
    public list_help: any = [];

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_help_detail',
            { search: "", status_del: "1",id_help:"" }
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
        this.http.post("/sys_help.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_help = resp;
        });
    }
    openDialogAdd(): void {
        if (this.filter.id_help === "" || this.filter.id_help === null) {
            Swal.fire({
                title: this._translocoService.translate("system.must_chose_help"),
                text: "",
                icon: "warning",
            });
            return;
        }
        const dialogRef = this.dialog.open(sys_help_detail_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                    id_help: this.filter.id_help
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
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
                    if (error.status == 403) {
                        Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                    }

                });
        this.rerender();
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_help_detail_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_help_detail_popUpAddComponent, {
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
    }


}


