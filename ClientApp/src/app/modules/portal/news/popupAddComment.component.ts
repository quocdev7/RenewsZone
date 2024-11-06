import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'portal_news_popupAddComment',
    templateUrl: 'popupAddComment.html',
})
export class portal_news_popupAddCommentComponent extends BasePopUpAddComponent {
  
    
  
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 400,
        images_upload_url: '/FileManager/uploadimage',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    }
    constructor(public dialogRef: MatDialogRef<portal_news_popupAddCommentComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_news', dialogRef, dialogModal);
        this.record = {
            comment: "",
            id_news: data.id_news,
            captcha:""
        };
        this.actionEnum = data.actionEnum;
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

    }
   
    comment(): void {
        this.beforesave();
        this.loading = true;
            this.http
                .post(this.controller + '.ctr/add_comment/',
                     this.record
                ).subscribe(resp => {
                    this.basedialogRef.close({id:1});
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;
                         
                        }
                        this.loading = false;
                    }
                );
        }
   

   

}
