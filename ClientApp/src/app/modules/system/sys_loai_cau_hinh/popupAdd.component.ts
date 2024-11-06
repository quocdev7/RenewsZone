import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_loai_cau_hinh_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_loai_cau_hinh_popUpAddComponent extends BasePopUpAddComponent {
    
    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/uploadimage',
    }

    public lst_cau_hinh_thong_tin_web: any;
    constructor(public dialogRef: MatDialogRef<sys_loai_cau_hinh_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_loai_cau_hinh', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.http
            .post('/sys_loai_cau_hinh.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.lst_cau_hinh_thong_tin_web = resp;
            });
    }
    
}
