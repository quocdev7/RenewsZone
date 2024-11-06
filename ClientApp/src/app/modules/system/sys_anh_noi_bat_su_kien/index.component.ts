import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_anh_noi_bat_su_kien_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_anh_noi_bat_su_kien_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_anh_noi_bat_su_kien_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_status_del: any;
    public list_su_kien:any=[];
    
    constructor(http: HttpClient,
        private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_anh_noi_bat_su_kien',
            { search: "", status_del: "1" , "id_su_kien":""}
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

        this.load_su_kien();
    }
       load_su_kien(): void {
   this.http
            .post('/sys_event.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_su_kien = resp;
            });

}


    openDialogAdd(): void {
  if (this.filter.id_su_kien === "" || this.filter.id_su_kien === null) {
            Swal.fire({
                title: this._translocoService.translate('system.phaichonsukien'),
                text: "",
                icon: "warning",
            })
        }
        else {
 const dialogRef = this.dialog.open(sys_anh_noi_bat_su_kien_popUpAddComponent, {
     disableClose: true,
     width: '768px',
            data: {
                actionEnum: 1,

                db: {
                    id: 0,
id_su_kien: this.filter.id_su_kien
                },
 ten_su_kien: this.list_su_kien.filter(q => q.id == this.filter.id_su_kien)[0].name
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
        const dialogRef = this.dialog.open(sys_anh_noi_bat_su_kien_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_anh_noi_bat_su_kien_popUpAddComponent, {
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
        this.setDocTitle('Ảnh nổi bật sự kiện - Xelex'); 
    }


}

