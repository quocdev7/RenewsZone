import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_cot_moc_su_kien_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_cot_moc_su_kien_popupLanguageComponent } from './popupLanguage.component';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_cot_moc_su_kien_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_cot_moc_su_kien_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_status_del: any;
    public list_su_kien:any=[];
    public model_ngon_ngu:any=[]
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_cot_moc_su_kien',
            { search: "", status_del: "1" }
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

 const dialogRef = this.dialog.open(sys_cot_moc_su_kien_popUpAddComponent, {
     disableClose: true,
     width: '768px',
            data: {
                actionEnum: 1,

                db: {
                    id: 0,
                },

            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });

       
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_cot_moc_su_kien_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogEdit_language(model, pos): void {
        this.http
        .post('/sys_cot_moc_su_kien.ctr/load_ngon_ngu/', {
            id_cot_moc : model.db.id
        }
        ).subscribe(resp =>{
            this.model_ngon_ngu = resp;
            model.actionEnum = 2;
            const dialogRef = this.dialog.open(sys_cot_moc_su_kien_popupLanguageComponent, {
                disableClose: true,
                width: '768px',
                data: this.model_ngon_ngu
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result != undefined && result!=null) this.listData[pos] = result;
            });
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_cot_moc_su_kien_popUpAddComponent, {
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
        this.setDocTitle('Cột mốc sự kiện - Xelex'); 
    }


}


