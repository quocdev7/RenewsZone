import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_program_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_event_program_popUpLanguageComponent } from './popupLanguage.component';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_event_program_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_program_indexComponent extends BaseIndexDatatableComponent
{
    public model_ngon_ngu: any;
    public list_status_del: any;

    public list_su_kien: any;

    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_event_program',
            {
                search: "", status_del: "1",
                event_id: ""            }
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
            .post('/sys_event.ctr/getListEventUser/', {}
            ).subscribe(resp => {
                this.list_su_kien = resp;
            });
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
            const dialogRef = this.dialog.open(sys_event_program_popUpAddComponent, {
                disableClose: true,

              
                data: {
                    actionEnum: 1,
                    db: {
                        id: 0,
                        trang_thai: 1,
                        event_id: this.filter.event_id
                    },
                    ten_su_kien: this.list_su_kien.filter(q => q.id == this.filter.event_id)[0].name
                },
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result.db.id == 0) return;
                this.rerender();
            });
        }
       
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
        const dialogRef = this.dialog.open(sys_event_program_popUpAddComponent, {
            disableClose: true,
          
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogEdit_language(model, pos): void {
        this.http
        .post('/sys_event_program.ctr/load_ngon_ngu/', {
            id_event_program : model.db.id
        }
        ).subscribe(resp =>{
            this.model_ngon_ngu = resp;
            model.actionEnum = 2;
            const dialogRef = this.dialog.open(sys_event_program_popUpLanguageComponent, {
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
        const dialogRef = this.dialog.open(sys_event_program_popUpAddComponent, {
            disableClose: true,
           
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
        this.setDocTitle('Chương trình sự kiện - Xelex'); 
    }


}


