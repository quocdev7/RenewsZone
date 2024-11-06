import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';
import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
@Component({
    selector: 'sys_news_popupComment',
    templateUrl: 'popupComment.html',
})
export class sys_news_popUpCommentComponent extends BasePopUpAddComponent {
   
    public filter: any;
    public currentIndex: number;
    public listData: any;
    public dtOptions: any;
    public table: any;
    constructor(public dialogRef: MatDialogRef<sys_news_popUpCommentComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_news', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
      
        this.filter = {search:"",id_news:""};

        this.filter.id_news=this.record.db.id;
        
    }
    
    hien_binh_luan(id1): void {
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
                    .post(this.controller + '.ctr/hien_binh_luan/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.baseInitData();
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
    an_binh_luan(id1): void {
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
                    .post(this.controller + '.ctr/an_binh_luan/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.baseInitData();
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
    
     save(): void {
      
        this.loading = true;
     
            this.http
                .post(this.controller + '.ctr/update_vi_tri_tin_noi_bat/',
                    {
                       
                    }
                ).subscribe(resp => {
                   Swal.fire('Lưu thành công', '', 'success');
                    this.basedialogRef.close();
                  
                  
                },
                   
                );
     
    }

  

    
    public baseInitData(): void {
      
        const that = this;
        this.dtOptions = {
            language: translateDataTable,
            scrollY: '50vh',
            scrollCollapse: true,
            retrieve: true,
            scrollX: true,
            fixedHeader: true,
            ordering: false,
            serverSide: true,
            processing: true,
            lengthMenu: [50, 75, 100],
            "drawCallback": function (settings) {
                var api = this.api();
                that.table = api;
                setTimeout(function () { api.columns.adjust(); }, 300);
                $('tbody').on('click', 'tr', function () {
                    if ($(this).hasClass('selected')) {
                        $(this).removeClass('selected');
                    }
                    else {
                        $('tr.selected').removeClass('selected');
                        $(this).addClass('selected');
                    }
                });

            },
            responsive: {
                details: {
                    renderer: function (api, rowIdx, columns) {
                        setTimeout(function () {
                            api.columns.adjust();
                        }, 300);
                    }
                }
            },
            "searching": false,
            ajax: (data, callback, settings) => {
                this.pageLoading = true;
                this.http
                    .post<DataTablesResponse>(this.baseurl + 'sys_news.ctr/DataHandlerComment/',
                        {
                            param1: data,
                            data: this.filter
                        }
                    ).subscribe(resp => {
                        that.listData = resp.data;
                        this.pageLoading = false;
                        that.currentIndex = resp.start;
                        callback({
                            recordsTotal: resp.recordsTotal,
                            recordsFiltered: resp.recordsFiltered,
                            data: []
                        });
                    });
            },

        };


    }

}
