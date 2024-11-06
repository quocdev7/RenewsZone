import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_cau_hinh_duyet_user_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_cau_hinh_duyet_user_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_cau_hinh_duyet_user_indexComponent extends BaseIndexDatatableComponent
{
  
    public list_hinh_thuc: any;
    public list_khoa: any;
    

    constructor(http: HttpClient, dialog: MatDialog, private titleService: Title
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_cau_hinh_duyet_user',
            { search: "", id_khoa :"",id_hinh_thuc:"-1" }
        )
    
        this.load_hinh_thuc();
        this.load_khoa();
    }
    load_hinh_thuc(): void {
        this.list_hinh_thuc = [
            {
                id: 1,
                name: this._translocoService.translate("portal.student")
            },
            {
                id: 2,
                name: this._translocoService.translate("portal.alumni")
            },
            {
                id: 3,
                name: this._translocoService.translate("portal.teacher")
            },
            {
                id: 4,
                name: this._translocoService.translate("portal.CBCNV")
            },
            {
                id: 5,
                name: this._translocoService.translate("portal.retire")
            },
        ]
    }
    load_khoa(): void {
        var all= {id:"-1" , name:this._translocoService.translate("common.all")};
        this.http
            .post('/sys_khoa.ctr/getListUse/', {

            }
            ).subscribe(resp => {
                this.list_khoa = resp;
              
                this.list_khoa.splice(0, 0,all );
            });
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_cau_hinh_duyet_user_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_cau_hinh_duyet_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) 
            //this.listData[pos] = result;
            this.rerender();
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_cau_hinh_duyet_user_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) 
            //this.listData[pos] = result;
            this.rerender();
        });
    }
   
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }


    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Duyệt người dùng - Xelex'); 
    }


}


