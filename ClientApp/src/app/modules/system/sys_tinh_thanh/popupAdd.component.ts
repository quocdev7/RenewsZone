import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_tinh_thanh_model } from './sys_tinh_thanh.types';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';


@Component({
    selector: 'sys_tinh_thanh_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_tinh_thanh_popUpAddComponent extends BasePopUpAddComponent {
    public list_quoc_gia: any=[];
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/uploadimage',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    }
    constructor(public dialogRef: MatDialogRef<sys_tinh_thanh_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_tinh_thanh', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.load_quoc_gia();
    }
    
    load_quoc_gia():void{
        
        this.http
            .post('/sys_quoc_gia.ctr/getListUse/', {
            }
            ).subscribe(resp => {
                this.list_quoc_gia = resp;
            });
     }

}


