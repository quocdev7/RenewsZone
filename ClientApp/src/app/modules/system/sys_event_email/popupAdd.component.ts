import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_event_email_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_event_email_popUpAddComponent extends BasePopUpAddComponent {
    public file_logo: any;
    public Progress_logo: any = -1;
    public list_event: any;
    public dtOptions: any;
    public list_type: any;
    public list_mau_email: any;
    public list_mau_email_moi: any;
    public list_mau_email_camon: any;
    public list_trangthai_dangky: any;
    public list_eventAdd: any;

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

    constructor(public dialogRef: MatDialogRef<sys_event_email_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_event_email', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        if (this.actionEnum != 1) {

            this.http
                .post('/sys_event_email.ctr/getListItem/', {
                    id: this.record.db.id
                }
                ).subscribe(resp => {
                    this.record.list_item = resp;
                });
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
        this.http
            .post('/sys_template_mail.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_mau_email = resp;
                this.list_mau_email_moi = this.list_mau_email.filter(d => d.id_loai_email == 1);
                this.list_mau_email_camon = this.list_mau_email.filter(d => d.id_loai_email == 2);
            });
        this.list_type = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Offline" },
            { id: 2, name: "Online" }
        ]
        this.list_trangthai_dangky = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Công khai" },
            { id: 2, name: "Riêng tư" }
        ]
    }
    bindthumoi() {
        var thumoi = this.list_mau_email.filter(d => d.id == this.record.db.id_template_invite)[0];
        this.record.tieu_de_mau_thu_moi = thumoi.tieu_de;
        this.record.noi_dung_mau_thu_moi = thumoi.noi_dung;
    }
    bindthucamon() {
        var thumoi = this.list_mau_email.filter(d => d.id == this.record.db.id_template_thanks)[0];
        this.record.tieu_de_thu_cam_on = thumoi.tieu_de;
        this.record.noi_dung_thu_cam_on = thumoi.noi_dung;
    }
    chose_file_logo(fileInput: any) {

        this.file_logo = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDrop_logo(files: any) {

        this.file_logo = files;
        this.submitFile();
    }
    submitFile() {
        var formData = new FormData();

        this.Progress_logo = 0;
        for (var i = 0; i < this.file_logo.length; i++) {
            formData.append('list_file[]', this.file_logo[i]);
        }
        formData.append('list_file[]', this.file_logo);
        this.http.post('FileManager/uploadimage', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_logo = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.db.logo = item.location;
                    this.file_logo = null
                    this.Progress_logo = -1;
                }

            })
    }

}
