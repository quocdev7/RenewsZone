import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_san_pham_popUpAddComponent } from './popupAdd.component';
import { sys_san_pham_popUpAddImageComponent } from './popupAddImage.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_san_pham_popUpLanguageComponent } from './popupLanguage.component';

@Component({
    selector: 'sys_san_pham_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_san_pham_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_status_del: any;
    public list_loai_san_pham:any=[];
    public model_ngon_ngu: any;
    
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_san_pham',
            { search: "", status_del: "1" , "id_loai":"-1"}
        )

     this.http
            .post('/sys_loai_san_pham.ctr/getListUse', {}
            ).subscribe(resp => {
                this.list_loai_san_pham = resp;
                this.list_loai_san_pham.splice(0, 0, {id:'-1',name:this._translocoService.translate('system.all')})
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
    openDialogEditlanguage(model, pos): void {
        this.http
        .post(this.controller + '.ctr/load_ngon_ngu/', {
            id:model.db.id
        }
        ).subscribe(resp => {
          this.model_ngon_ngu = resp;

          model.actionEnum = 2;
          const dialogRef = this.dialog.open(sys_san_pham_popUpLanguageComponent, {
              disableClose: true,
              width: '768px',
              data: this.model_ngon_ngu
          });
          dialogRef.afterClosed().subscribe(result => {
              if (result != undefined && result!=null)
              this.rerender();
              //this.listData[pos] = result;
          });
          
        });
 
      
        }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_san_pham_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_san_pham_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogImage(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_san_pham_popUpAddImageComponent, {
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
        const dialogRef = this.dialog.open(sys_san_pham_popUpAddComponent, {
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


