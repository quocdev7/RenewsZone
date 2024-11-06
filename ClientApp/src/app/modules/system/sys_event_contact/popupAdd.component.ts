import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_event_conntact_popupAdd',
    templateUrl: 'popupAdd.html',   
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_event_contact_popUpAddComponent extends BasePopUpAddComponent {
    public item_chose: any;
    public dtOptions: any;
    public list_hinh_thuc: any;
    public user_info: any;
    public nguoilienhe: any;

    constructor(public dialogRef: MatDialogRef<sys_event_contact_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_event_contact', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.nguoilienhe = this._translocoService.translate('system.bantochuc');
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.user_info = null;
        if (this.actionEnum != 1) {
            this.http
                .post('/sys_event_participate.ctr/getListItem/', {
                    id: this.record.db
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
}
