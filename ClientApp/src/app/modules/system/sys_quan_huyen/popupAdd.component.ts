import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_quan_huyen_model } from './sys_quan_huyen.types';


@Component({
    selector: 'sys_quan_huyen_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_quan_huyen_popUpAddComponent extends BasePopUpAddComponent {
    public list_quoc_gia: any=[];
    public list_tinh_thanh: any=[];

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
    constructor(public dialogRef: MatDialogRef<sys_quan_huyen_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_quan_huyen', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        if (this.actionEnum != 1) {
            this.load_tinh_thanh();
        }
        this.load_quoc_gia();
        //this.load_tinh_thanh();
    } 
    load_tinh_thanh():void{
        
        this.http
            .post('/sys_tinh_thanh.ctr/getListTinhThanh/', {
                id:this.record.db.id_quoc_gia
            }
            ).subscribe(resp => {
                this.list_tinh_thanh = resp;
            });
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
