import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';

import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_thanh_vien_thuoc_nhom_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_thanh_vien_thuoc_nhom_indexComponent extends BaseIndexDatatableComponent
{
    public list_status_del: any;
    public file: any;
    public list_nhom: any;
    public item_chose: any;
    public user_id: any;
    public errorModel: any;
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_thanh_vien_thuoc_nhom',
            { search: "", id_nhom:""}
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
            .post('/sys_nhom_thanh_vien.ctr/getListUse/', {

            }
            ).subscribe(resp => {
                this.list_nhom = resp;
            });

    }
  
    bind_data_item_chose(): void {
        this.user_id = this.item_chose.id;

    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
        this.setDocTitle('Thành viên thuộc nhóm - Xelex'); 
     }
    ngOnInit(): void {
        this.baseInitData();
    }


    deleteNew(id1): void {
        Swal.fire({
            title: this._translocoService.translate('areYouSure'),
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                    .post(this.controller + '.ctr/delete/',
                        {
                            id: id1,
                        }
                ).subscribe(resp => {

                    Swal.fire('Xóa thành công', '', 'success').then((result) => {
                        if (result.isConfirmed) {
                            this.rerender();
                        }
                    })
                      
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    addDetail(): void {
        var valid = true;
        var error = '';

        if (this.filter.id_nhom === "" || this.filter.id_nhom === null) {
            error += this._translocoService.translate('system.phaichonnhom');
            valid = false;
        }



        if (this.user_id == null || this.user_id == undefined) {
            error += this._translocoService.translate('system.phaichonnguoidung');
            valid = false;
        } else {
            if (this.listData.filter(d => d.db.user_id == this.user_id).length > 0) {
                error += this._translocoService.translate('existed');
                valid = false;
            }
        }



        if (!valid) {
            this.showMessagewarning2("", error);
            return;
        } else {
 
            Swal.fire({
                title: this._translocoService.translate('areYouSure'),
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                .post('/sys_thanh_vien_thuoc_nhom.ctr/add_nguoi_dung/', {
                    user_id: this.user_id,
                    id_nhom: this.filter.id_nhom
                }
                )

                .subscribe(resp => {
                    this.errorModel = [];
                    this.rerender();
                    Swal.fire('Lưu thành công', '', 'success')
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;

                        }
                        //this.loading = false;
                    }
                );
                }
            })

           
        }
    }
    /******/
    onFileSelected(event: any) {
        this.file = event.target.files[0];
        //this.onSubmitFile();
        //event.target.value = null;
    }
    dowloadFileMau() {
        var url = '/sys_thanh_vien_thuoc_nhom.ctr/downloadtemp';
        window.location.href = url;
    }
    onSubmitFile() {

        if (this.file == null || this.file == undefined) {

            Swal.fire('Phải chọn file import', '', 'warning')

        } else {
            var formData = new FormData();

            formData.append('file', this.file);
            this.http.post('/sys_thanh_vien_thuoc_nhom.ctr/ImportFromExcel/', formData, {
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

    }
        
}


