import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { error } from 'jquery';


@Component({
    selector: 'sys_event_participate_popupNhomNguoiDung',
    templateUrl: 'popupNhomNguoiDung.html',
    /*styleUrls: ['./popupNhomNguoiDung.component.scss']*/
})
export class sys_event_participate_popUpNhomNguoiDungComponent extends BasePopUpAddComponent {
    public item_chose: any;
    public dtOptions: any;
    public list_hinh_thuc: any;
    public user_info: any;
    public khachmoi: any;

    public additemrole: any;
    public searchRole: any = "";
    public listRoleFilter: any;
    public list_role: any;
    public list_filter: any = [];


    constructor(public dialogRef: MatDialogRef<sys_event_participate_popUpNhomNguoiDungComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_event_participate', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.khachmoi = this._translocoService.translate('system.khachmoi');
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }

        if (this.actionEnum != 1) {
            this.http
                .post('/sys_event_participate.ctr/getListItem/', {
                    id: this.record.db.id
                }).subscribe(resp => {
                    this.record.list_item = resp;
                });
        }
       
        //this.http
        //    .post('/sys_event.ctr/getListItem/', {
        //        id: this.record.db.id
        //    }
        //    ).subscribe(resp => {
        //        this.record.list_item = resp;
        //    });

        this.list_hinh_thuc = [
            { id: 1, name: this._translocoService.translate('system.khachmoi') },
            { id: 2, name: this._translocoService.translate('system.bantochuc') }
        ]

        if (this.actionEnum != 2) {
            this.dialogRef.keydownEvents().subscribe(event => {
                if (event.key === "Escape") {
                    this.dialogRef.close();
                }
            })
        }

        this.dtOptions = {
            language: {
                zeroRecords: "",
                infoEmpty: "",
                info: "",
                search: this._translocoService.translate('search') 
            },
            ordering: false,
            "paging": false,
        }
        this.loadNhomNguoiDung();
    }

    public loadNhomNguoiDung(): void {
        this.http
            .post('/sys_thanh_vien_thuoc_nhom.ctr/getListNhomNguoiDung/', {
                event_id: this.record.db.event_id
            }).subscribe(resp => {
                this.list_role = resp
            })
    }

    aftersavefail(){
            //this.rerender();
    }

    bind_data_item_chose(): void {
        this.record.db.user_id = this.item_chose.id;
        this.http
            .post('/sys_user.ctr/getListUseInfo/', {
                user_id: this.item_chose.id
            }).subscribe(resp => {
                this.user_info = resp;
                this.record.position = this.user_info.db.position;
                this.record.avatar_link = this.user_info.db.avatar_link;
                this.record.ten_cong_ty = this.user_info.ten_cong_ty;
                this.record.school_year = this.user_info.db.school_year;
                this.record.faculty = this.user_info.ten_khoa;
                this.record.ten_quoc_gia = this.user_info.ten_quoc_gia;
            })
    }

    resetAddItemRole(): void {

    }

    resetlistRole(): void {
        this.updateAllComplete();
    }
    allComplete: boolean = false;

    updateAllComplete() {
        this.allComplete = this.list_role != null && this.list_role.every(t => t.completed);
    }

    someComplete(): boolean {
        if (this.list_role == null) {
            return false;
        }
        return this.list_role.filter(t => t.completed).length > 0 && !this.allComplete;
    }

    setAll(completed: boolean) {
        this.allComplete = completed;
        if (this.list_role == null) {
            return;
        }
        this.list_role.forEach(t => t.completed = completed);
    }

    public beforesave(): void {
        this.list_filter = [];
        if (this.list_role != undefined) {
            for (var i = 0; i < this.list_role.length; i++) {
                var d = this.list_role[i];

                if (this.list_role[i].completed == true) {
                    this.list_filter.push(d.id);
                } else {

                }
            }
        }
    }

    saveNhomNguoiDung(): void {
        this.beforesave();
        if (this.list_role.length == 0) {
            Swal.fire({
                title: this._translocoService.translate('khongcodongnao'),
                text: "",
                icon: 'warning',
            })
        }
        else {
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
                        .post('sys_event_participate.ctr/create_group_event_participate/',
                            {
                                data: this.record,
                                list_filter: this.list_filter
                            }
                        ).subscribe(resp => {
                            this.errorModel = [];
                            this.record = resp;
                            this.basedialogRef.close(this.record);
                            Swal.fire('Lưu thành công', '', 'success')
                        },
                            error => {
                                if (error.status == 400) {
                                    this.errorModel = error.error;
                                }
                            }
                        );
                }
            })
        }
    }
}
