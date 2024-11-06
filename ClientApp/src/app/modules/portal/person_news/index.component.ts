import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { portal_person_news_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { translateDataTable } from '../../../../@fuse/components/commonComponent/VietNameseDataTable';

import { portal_person_news_popUpViewComponent } from './popupView.component';
@Component({
    selector: 'portal_person_news_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class portal_person_news_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_status_del: any;
    public group_news: any;
    public type_news: any = [{
        id: "-1",
        name: this._translocoService.translate('common.all')
    }];

    public badges:any;
    public lst_news_manager:any=[];
    
     public need_to_aprroval_user:any=false;
    public filteropen: any = false;
    public list_phamvi: any;
    public list_quyen_rieng_tu: any;
    public currentUser:any={};
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_news',
            { search: "", status_del: "3", id_group_news: "-1", id_type_news: "-1", id_phamvi: "1", tu_ngay: null, den_ngay: null, quyen_rieng_tu: "", is_hot: false }
        )
        this.loadInfo();
         this.http
            .post('/sys_user.ctr/getUserLogin/', {}
            ).subscribe(resp => {
                 this.currentUser = resp;
                if( this.currentUser.status_del==1){
                    this.load_nhom();
                    this.load_trang_thai();
                    this.load_pham_vi();
                    this.load_quyen_rieng_tu();
                }
            });


    } 
    loadInfo(): void {
        this.http
               .post('/sys_user.ctr/getNewsInfo/', {}
        ).subscribe(resp => {
        
                       this.badges =resp;
                       this.lst_news_manager =[
                        { 
                            id: "3",
                            name:"Đang chờ duyệt",
                          number:this.badges["tin_tuc_dang_cho_duyet"]
                        },
                        {   
                             id: "4",
                            name:"Không được duyệt",
                         number:this.badges["tin_tuc_khong_duoc_duyet"]
                        },
                        {    id: "2",
                            name:"Ngừng đăng",
                        number:this.badges["tin_tuc_ngung_dang"]
                        },
                        { 
                            id: "1",
                            name:"Đã đăng",
                        number:this.badges["tin_tuc_da_dang"]
                        },
                        ]    ; 
                   
        });
     
   }
    public rerender(): void {
       var that=this;
        this.dtElements.forEach((dtElement: DataTableDirective) => {
            dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
                that.loadInfo();
                dtInstance.ajax.reload(null, true);
            });
        });
        // Destroy the table first
    };
  
    load_trang_thai(): void {

        this.list_status_del = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.da_duyet')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.bi_tra_lai')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.cho_xet_duyet')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.ngung_dang')
            },  
          
        ];

    }
    load_quyen_rieng_tu(): void {
        this.list_quyen_rieng_tu = [
            { id: '-1', name: "Tất cả" },
            { id: 1, name: this._translocoService.translate('system.cong_khai') },
            { id: 4, name: this._translocoService.translate('system.nguoi_dung_chua_la_thanh_vien') },
            { id: 2, name: this._translocoService.translate('system.thanh_vien') },
            { id: 3, name: this._translocoService.translate('system.ban_be') },
        ]


    }
    load_nhom(): void {
        this.http
            .post('/sys_group_news.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.group_news = resp;
                this.group_news.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
            });

    }
    load_pham_vi(): void {

        this.list_phamvi = [
            { id: "1", name: this._translocoService.translate('system.ngay_dang') },
            //{ id: "2", name: this._translocoService.translate('system.ngay_cap_nhat') },
            //{ id: "3", name: this._translocoService.translate('system.ngay_duyet') },


        ]
    }
    filterchange() {
        if (this.filteropen == true) {
            this.filter.tu_ngay = null;
            this.filter.den_ngay = null;
            this.filter.quyen_rieng_tu = "";
            this.filteropen = false;
        } else {
            this.filter.tu_ngay = new Date();;
            this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 7);
            this.filter.den_ngay = new Date();
            this.filter.quyen_rieng_tu = "-1";
            this.filteropen = true;
        }
    }

    changeGroupNews(): void {
        this.http
            .post('/sys_type_news.ctr/getListUseByGroup/', {
                id: this.filter.id_group_news
            }
            ).subscribe(resp => {
                this.type_news = resp;
                this.filter.id_type_news = "-1";
                this.type_news.splice(0,0,{
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
                this.rerender();

            });
    }

    public delete(id1): void {
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
                        Swal.fire('Ngừng đăng thành công', '', 'success');
                        this.rerender();
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
    revertStatus(model, pos): void {
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
                model.db.status_del = 1;
                this.http
                    .post(this.controller + '.ctr/edit/',
                        {
                            data: model,
                        }
                    ).subscribe(resp => {
                        Swal.fire('Đăng bài thành công', '', 'success');
                        this.rerender();
                    },
                        error => {
                            console.log(error);

                        });
            }
        })
  
    }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(portal_person_news_popUpAddComponent, {
            disableClose: true,
            
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
        const dialogRef = this.dialog.open(portal_person_news_popUpAddComponent, {
            disableClose: true,
           
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null)
                this.rerender();
                //this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(portal_person_news_popUpAddComponent, {
            disableClose: true,
          
            
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogViewBaiDang(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(portal_person_news_popUpViewComponent, {
            disableClose: true,
           

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
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
                    .post<DataTablesResponse>(this.baseurl + '' + this.controller + '.ctr/DataHandlerPersonNews/',
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



    ngOnInit(): void {
        this.baseInitData();
    }

   
}


