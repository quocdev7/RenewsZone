import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_event_khach_moi_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { Router,ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_event_khach_moi_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_event_khach_moi_indexComponent extends BaseIndexDatatableComponent
{
   

    public lst_check_in_status: any;
    public trang_thais:any;
    public list_event: any;
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,private router: Router
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_event_khach_moi',
            { search: "",event_id:"", check_in_status: "-1" ,trang_thai:"-1"}
        )
         this.http
         .post('/sys_event.ctr/getListEventUser/', {
            
         }
         ).subscribe(resp => {
            this.list_event = resp;
         });

        this.load_trang_thai_nguoi_dung();
        //this.load_trang_thai_su_kien();
    }
    checkin(id_event): void {
        const url = '/scan_checkin/' + id_event;
        this.router.navigateByUrl(url);
    }
    load_su_kien_theo_trang_thai(): void {
        this.http
            .post('/sys_event.ctr/get_su_kien_theo_trang_thai/', {
                id: this.filter.trang_thai
            }
            ).subscribe(resp => {
                this.list_event = resp;
                this.filter.id_type_news = "-1";
                this.list_event.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
                this.rerender();
            });
    }

    load_trang_thai_nguoi_dung() {

        this.lst_check_in_status = [
            {
                id: "-1",
                name: this._translocoService.translate('system.all')
            },
            {
                id: "1",
                name: this._translocoService.translate('system.moi_tham_gia_su_kien')
            },
            {
                id: "6",
                name: this._translocoService.translate('system.dang_ky_tham_gia_su_kien')
            },
            
            {
                id: "3",
                name: this._translocoService.translate('system.se_tham_gia_su_kien')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.tuchoithamdu')
            },
            {
                id: "5",
                name: this._translocoService.translate('system.khong_du_dieu_kien_tham_du')
            },
            {
                id: "4",
                name: this._translocoService.translate('system.denthamdu')
            },
        ];
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
            // {
            //     id: 2,
            //     name: this._translocoService.translate('system.huy')
            // },
           
        ];
    }
    revertStatus(id, status): void {
        if (status == 3) {
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
                    .post(this.controller + '.ctr/update_status/',
                        {
                            ly_do: "",
                            status: status,
                            id: id
                        }
                    ).subscribe(resp => {
                        Swal.fire("Đã tham gia sự kiện", "", "success").then(r => {this.rerender();});
                        
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }
                        });
                }
               
            })
            
        
        }else{
            Swal.fire({
                title: "Lý do không đủ điều kiện tham dự",
                input: 'text',
                inputAttributes: {
                    autocapitalize: 'off'
                },
                showCancelButton: true,
                cancelButtonText: this._translocoService.translate('close'),
                confirmButtonText: this._translocoService.translate('common.confirm'),
                showLoaderOnConfirm: true,
                inputValidator: (value) => {
                    if (!value) {
                        return this._translocoService.translate('common.must_input_reason')
                    }
                },
                allowOutsideClick: () => false,
            }).then((result) => {
                if (result.value) {
                    this.http
               .post(this.controller + '.ctr/update_status/',
                   {
                       ly_do:result.value,
                    status: status,
                    id :id
                   }
                    ).subscribe(resp => {
                        Swal.fire("Cập nhật thành công", "", "success").then(r => {this.rerender();});
                            
                        },
                            error => {
                                if (error.status == 403) {
                                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                                }
                            });
                            }
                
                })
        
           
        }
      
      
     
    }

    gui_thu_cam_on(): void {
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
                    .post(this.controller + '.ctr/gui_thu_cam_on/',
                        {
                            id: this.filter.event_id,
                        }
                ).subscribe(resp => {

                        var data =resp;
                        if(data == "1"){
                            Swal.fire('Chưa có khách mời trong sự kiện này', '', 'warning')
                           
                        }else if(data=="2"){
                            Swal.fire('', '<span class="swal2-title">Sự kiện chưa kết thúc.<br/> Bạn chưa thể gửi thư cảm ơn.</span>', 'warning')
                        }else{
                            Swal.fire('Gửi mail thành công', '', 'success').then(
                                // Navigate to the confirmation required page
                                res => {
                                     this.rerender();
                                });
                        }
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
    openDialogAdd(): void {
        if (this.filter.event_id === "" || this.filter.event_id === null) {
            Swal.fire({
                title: this._translocoService.translate('system.phaichonsukien'),
                text: "",
                icon: "warning",
            })
        } 
        else {
            const dialogRef = this.dialog.open(sys_event_khach_moi_popUpAddComponent, {
                disableClose: true,
                
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
        const dialogRef = this.dialog.open(sys_event_khach_moi_popUpAddComponent, {
            disableClose: true,
            
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_event_khach_moi_popUpAddComponent, {
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
        this.setDocTitle('Khách mời - Xelex'); 
    }


}


