import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_news_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { portal_person_news_popUpViewComponent } from 'app/modules/portal/person_news/popupView.component';
import { sys_news_popUpTinNoiBatComponent } from './popupTinNoiBat.component';
import { sys_news_popUpCommentComponent } from './popupComment.component';
import { sys_news_popUpLanguageComponent } from './popupLanguage.component';
import { Title } from '@angular/platform-browser';



@Component({
    selector: 'sys_news_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_news_indexComponent extends BaseIndexDatatableComponent
{
   

    public list_status_del: any;
    public group_news: any;
    public type_news: any = [{
        id: "-1",
        name: this._translocoService.translate('common.all')
    }];
    public filteropen: any = false;
    public list_phamvi: any;
    public list_quyen_rieng_tu: any;
    public model_ngon_ngu: any;
    
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_news',
          
            { search: "", status_del: "-1", id_group_news: "-1", id_type_news: "-1", id_phamvi: "1", tu_ngay: null, den_ngay: null, quyen_rieng_tu: "", is_hot: false }
        )

        this.load_nhom();
        this.load_trang_thai();
        this.load_pham_vi();
        this.load_quyen_rieng_tu();

        

     
    }
    onChangeDemo(ob) {
        console.log("checked: " + ob);
        this.rerender();
    } 
    save_tin_tuc_noi_bat(record,type) {
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
                    .post(this.controller + '.ctr/save_tin_tuc_noi_bat/', 
                        {
                            id: record.db.id,
                            id_group_news: record.db.id_group_news,
                            type:type
                        }
                    ).subscribe(resp => {
                        Swal.fire('system.cap_nhat_thanh_cong', '', 'success');
                        this.rerender();
                    },
                        error => {
                            if (error.status == 400) {
                                Swal.fire(this._translocoService.translate('system.toi_da_5_tin_noi_bat'), '', 'warning');
                            }
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

       
    }
    approval(id1): void {
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
                    .post(this.controller + '.ctr/approval/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        Swal.fire(this._translocoService.translate('system.duyet_thanh_cong'), '', 'success');
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
    cancel(id1): void {
        Swal.fire({
            title: "Lý do trả về",
            text: "",
            input: 'text',
            inputAttributes: {
                autocapitalize: 'off'
            },
            preConfirm: (login) => {

                if (login != '') {
                    this.http
                        .post(this.controller + '.ctr/cancel/',
                            {
                                id: id1,
                                reason: login
                            }
                        ).subscribe(resp => {
                            Swal.fire(this._translocoService.translate('system.tra_ve_thanh_cong'), '', 'success');
                            this.rerender();
                        },
                            error => {
                                if (error.status == 403) {
                                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                                }


                            }
                        );
                } else {
                    Swal.showValidationMessage("system.nhap_ly_do");
                }
            },
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no'),
        }).then((result) => {
           
        })

    }
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
                id: "2",
                name: this._translocoService.translate('system.ngung_dang')
            },
            {
                id: "3",
                name: this._translocoService.translate('system.cho_xet_duyet')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.bi_tra_lai')
            }
        ];

    }
    load_quyen_rieng_tu(): void {
        this.list_quyen_rieng_tu = [
            { id: '-1', name: "common.all" },
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
            { id: "2", name: this._translocoService.translate('system.ngay_cap_nhat') },
            { id: "3", name: this._translocoService.translate('system.ngay_duyet') },
           

        ]
    }
    filterchange() {
        if (this.filteropen == true) {
            this.filter.tu_ngay = null;
            this.filter.den_ngay = null;
            this.filteropen = false;
            this.baseInitData();
        } else {
            this.filter.tu_ngay = new Date();;
            this.filter.tu_ngay.setDate(this.filter.tu_ngay.getDate() - 7);
            this.filter.den_ngay = new Date();
            this.filteropen = true;
            this.baseInitData();
        }
    }
    openDialogTinNoiBat(): void {
        if (this.filter.id_group_news === "-1" ) {
            Swal.fire({
                title: this._translocoService.translate('system.phaichonnhomtintuc'),
                text: "",
                icon: "warning",
            })
        }else{
            const dialogRef = this.dialog.open(sys_news_popUpTinNoiBatComponent, {
                disableClose: true,
                width: '768px',
                data: {
                    actionEnum: 1,
                    db: {
                        id: 0,
                        trang_thai: 1,
                        id_group_news: this.filter.id_group_news
                    },
                    ten_nhom: this.group_news.filter(q => q.id == this.filter.id_group_news)[0].name
                },
            });
            dialogRef.afterClosed().subscribe(result => {           
                this.rerender();
            });
        }
       
    }

    changeGroupNews(): void {
        this.http
            .post('/sys_type_news.ctr/getListUseByGroupNew/', {
                id: this.filter.id_group_news
            }
            ).subscribe(resp => {
                this.type_news = resp;
                this.filter.id_type_news = "-1";
                this.type_news.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
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
                    console.log(error);

                });
        this.rerender();
    }
    openDialogAdd(): void {
    
        const dialogRef = this.dialog.open(sys_news_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_news_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null)
            this.rerender();
            //this.listData[pos] = result;
        });
    }

  
    openDialogEditlanguage(model, pos): void {

      
        this.http
        .post('/sys_news.ctr/load_ngon_ngu/', {
            id_news:model.db.id
        }
        ).subscribe(resp => {
          this.model_ngon_ngu = resp;

          model.actionEnum = 2;
          const dialogRef = this.dialog.open(sys_news_popUpLanguageComponent, {
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
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_news_popUpAddComponent, {
            disableClose: true,
            width: '768px',
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
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }

    openDialogComment(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_news_popUpCommentComponent, {
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
        this.setDocTitle('Tin tức - Xelex'); 
    }


}


