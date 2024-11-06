import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_anh_san_pham_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_anh_san_pham_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_anh_san_pham_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_status_del: any;
    public list_san_pham:any;
    
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_anh_san_pham',
            { search: "", status_del: "1" , "id_san_pham":""}
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

        this.http
            .post('/sys_san_pham.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_san_pham = resp;
            });
    }


    openDialogAdd(): void {
        
  if (this.filter.id_san_pham === "" || this.filter.id_san_pham === null) {
            Swal.fire({
                title: this._translocoService.translate('system.phai_chon_san_pham'),
                text: "",
                icon: "warning",
            })
        }
        else {
 const dialogRef = this.dialog.open(sys_anh_san_pham_popUpAddComponent, {
     disableClose: true,
     width: '768px',
            data: {
                actionEnum: 1,

                db: {
                    id: 0,
id_san_pham: this.filter.id_san_pham
                },
 ten_san_pham: this.list_san_pham.filter(q => q.id == this.filter.id_san_pham)[0].name
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
        const dialogRef = this.dialog.open(sys_anh_san_pham_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_anh_san_pham_popUpAddComponent, {
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


