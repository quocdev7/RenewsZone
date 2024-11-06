import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_popUpAddComponent } from './popupAdd.component';
import { sys_event_popupFileDinhKemComponent } from './popupFileDinhKem.component';
import { sys_event_popUpAnhDinhKemSuKienComponent } from './popupAnhDinhKemSuKien.component';

import { sys_event_popUpLanguageComponent } from './popupLanguage.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_event_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_indexComponent extends BaseIndexDatatableComponent
{
   

    public trang_thais: any;
    public list_status_del: any;
    public model_ngon_ngu: any;
    public list_type: any;

    public list_trangthai_dangky: any;

    
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_event',
            { search: "", trang_thai: -1, tu_ngay: null, den_ngay: null, id_hinh_thuc: "-1", is_register_event: "-1" }
        )
       
        this.load_trang_thai_dang_ky();
        this.load_trang_thai_su_kien();
        this.load_hinh_thuc();
        this.load_status_del();
        this.load_date();

   
    }
    load_date() {
        this.filter.tu_ngay = new Date();;
        this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 7);
        this.filter.den_ngay = new Date();
    }
    load_status_del() {
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
    load_trang_thai_dang_ky() {

        this.list_trangthai_dangky = [
            { id: '-1', name: "Tất cả" },
            { id: 1, name: this._translocoService.translate('system.cho_phep_dang_ky') },
            { id: 2, name: this._translocoService.translate('system.khong_cho_phep_dang_ky') },
        ]
    }
    load_trang_thai_su_kien() {
        this.trang_thais = [
            {
                id: -1,
                name: this._translocoService.translate('system.all')
            },
            {
                id: 1,
                name: this._translocoService.translate('system.dang_dien_ra')
            },
            {
                id: 3,
                name: this._translocoService.translate('system.ket_thuc')
            },
          
            {
                id: 4,
                name: this._translocoService.translate('system.sap_toi')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.huy')
            },
           
        ];
    }
  
        load_hinh_thuc() {
            this.list_type = [
                { id: "-1", name: this._translocoService.translate('system.all') },
                { id: "1", name: this._translocoService.translate('system.offline') },
                { id: "2", name: this._translocoService.translate('system.online') },
                { id: "3", name: this._translocoService.translate('system.hoc_bong') },
                { id: "4", name: this._translocoService.translate('system.tai_tro') }
            ]
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
    checkin(id): void {
        window.open("/sys_scan.ctr/index/?id=" + id, '_blank');
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_event_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_event_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogEditEN(model, pos): void {
        this.http
        .post('/sys_event.ctr/load_ngon_ngu/', {
            id_event:model.db.id
        }
        ).subscribe(resp =>{
            this.model_ngon_ngu = resp;
            model.actionEnum = 2;
            const dialogRef = this.dialog.open(sys_event_popUpLanguageComponent, {
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
        const dialogRef = this.dialog.open(sys_event_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    
    openDialogAnhDinhKemSuKien(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_event_popUpAnhDinhKemSuKienComponent, {
            disableClose: true,
          
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogFileDinhKemSuKien(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_event_popupFileDinhKemComponent, {
            disableClose: true,
           
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
        this.setDocTitle('Sự kiện - Xelex'); 
    }


}


