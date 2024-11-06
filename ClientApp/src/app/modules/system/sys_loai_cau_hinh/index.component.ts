import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
;
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_loai_cau_hinh_popUpAddComponent } from './popupAdd.component';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_loai_cau_hinh_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_loai_cau_hinh_indexComponent extends BaseIndexDatatableComponent
{
   
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_loai_cau_hinh',
            { search: ""}
        )
        
    }
   
    revertStatus(model, pos): void {
       // model.db.status_del = 1;
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
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_loai_cau_hinh_popUpAddComponent, {
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
            const dialogRef = this.dialog.open(sys_loai_cau_hinh_popUpAddComponent, {
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
                const dialogRef = this.dialog.open(sys_loai_cau_hinh_popUpAddComponent, {
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
        this.setDocTitle('Loại cấu hình - Xelex');
    }
    
}


