import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_nguoi_nhan_hoc_bong_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_event_nguoi_nhan_hoc_bong_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_nguoi_nhan_hoc_bong_indexComponent extends BaseIndexDatatableComponent {

    public list_event: any;
    public list_status_del: any;
    public file: any;
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_event_nguoi_nhan_hoc_bong',
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
    onFileSelected(event: any) {
        this.file = event.target.files[0];
        //this.onSubmitFile();
        //event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_event_nguoi_nhan_hoc_bong.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile(event: any) {

        if (this.file == null || this.file == undefined) {

            Swal.fire('Phải chọn file import', '', 'warning')

            event.target.value = null;


        } else {
            if (this.filter.event_id == null || this.filter.event_id == undefined || this.filter.event_id == "") {
                Swal.fire('Phải chọn sự kiện', '', 'warning')


            } else {
         
                var formData = new FormData();
                formData.append('file', this.file);
                formData.append('id_su_kien', this.filter.event_id);

                this.http.post('/sys_event_nguoi_nhan_hoc_bong.ctr/ImportFromExcel/', formData, {
                    //reportProgress: true,
                    //observe: 'events'
                })
                    .subscribe(res => {

                        if (res == "") {
                            Swal.fire('Lưu thành công', '', 'success');
                            this.rerender();

                        } else {
                            Swal.fire(res.toString(), '', 'warning')
                        }

                    })
            }
            event.target.value = null;
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
            const dialogRef = this.dialog.open(sys_event_nguoi_nhan_hoc_bong_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_event_nguoi_nhan_hoc_bong_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_event_nguoi_nhan_hoc_bong_popUpAddComponent, {
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
        this.setDocTitle('Người nhận học bổng - Xelex'); 
    }


}


