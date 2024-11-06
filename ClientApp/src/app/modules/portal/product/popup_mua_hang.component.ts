import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import Swal from 'sweetalert2';
import { FuseAlertType } from '../../../../@fuse/components/alert';
import { debug } from 'console';


@Component({
    selector: 'popup_mua_hang',
    templateUrl: 'popup_mua_hang.html',
    styleUrls: ['./popup_mua_hang.component.scss']
})
export class popup_mua_hangComponent extends BasePopUpAddComponent {
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
    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    public loading: any = false;
    public lst_khoa: any;
    public san_pham_dat_hang: any = {};
    //public san_pham_dat_hang: any;
    constructor(public dialogRef: MatDialogRef<popup_mua_hangComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        private router: Router,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_dat_hang', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
        this.get_san_pham_dat_hang();
        this.SendUp();
    }
    get_san_pham_dat_hang(): void {
        this.http
            .post('/sys_dat_hang.ctr/get_san_pham_dat_hang/', {
                id_san_pham: this.record.db.id_san_pham,
            }
            ).subscribe(resp => {
                debugger
                this.san_pham_dat_hang = resp;
            });
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

    }
    SendUp(): void {
        this.http.post('sys_dat_hang.ctr/create_portal/', { data: this.record }).subscribe((resp) => {
            this.basedialogRef.close(this.record);
            Swal.fire('Đã gửi thành công, chúng tôi sẽ phản hồi lại sau', '', 'success').then();
        },
            (error) => {
                if (error.status == 400) {
                    this.errorModel = error.error;
                }
                // Set the alert
                this.alert = {
                    type: 'error',
                    message: 'Không hợp lệ, Vui lòng kiểm tra lại'
                };
                // Show the alert
            }
        );
    }
}
