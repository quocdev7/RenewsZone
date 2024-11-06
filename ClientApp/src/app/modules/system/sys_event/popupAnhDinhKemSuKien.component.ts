import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import Swal from 'sweetalert2';

@Component({
    selector: 'sys_event_popUpAnhDinhKemSuKien',
    templateUrl: 'popupAnhDinhKemSuKien.component.html',
    styleUrls: ['./popupAnhDinhKemSuKien.component.scss']
})
export class sys_event_popUpAnhDinhKemSuKienComponent extends BasePopUpAddComponent {
    public item_chose: any;
    public dtOptions: any;

    public list_anh: any = [];



    fileData: any;
    previewUrl: any = null;
    fileUploadProgress: any = -1;
    uploadedFilePath: string = null;


    constructor(public dialogRef: MatDialogRef<sys_event_popUpAnhDinhKemSuKienComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_event', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }

        this.reload();

    }
    public reload(): void {
        this.http
            .post('/sys_event.ctr/get_list_image/', {
                event_id: this.record.db.id,

            }
            ).subscribe(resp => {
                this.list_anh = resp;
            });
    }
    saveFileDinhKemEvent(): void {
        //this.loading = true;

        if (this.list_anh.length == 0) {
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
                        .post('sys_event.ctr/create_listimage/',
                            {

                                data: this.list_anh,
                                event_id: this.record.db.id
                            }
                        ).subscribe(resp => {
                            this.errorModel = [];
                            this.basedialogRef.close(this.record);
                            Swal.fire('Lưu thành công', '', 'success')
                        },
                            error => {
                                if (error.status == 400) {
                                    this.errorModel = error.error;

                                }
                                //this.loading = false;
                            }
                        );
                }
            })
        }


    }


    fileProgress(fileInput: any) {

        this.fileData = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDropProgress(files: any) {

        this.fileData = files;
        this.submitFile();
    }
    submitFile() {
        this.beforesave();
        this.fileUploadProgress = 0;
        var formData = new FormData();
        for (var i = 0; i < this.fileData.length; i++) {
            formData.append('list_anh[]', this.fileData[i]);
        }
        var modelsubmit = {
            db: null,
            list_image: null,
        };
        modelsubmit.db = this.record.db;
        modelsubmit.list_image = this.list_anh;
        formData.append('model', JSON.stringify(modelsubmit));
        this.aftersavefail();
        this.http.post('sys_event.ctr/upload_file_image', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {
                    this.fileUploadProgress = Math.round((res.loaded / res.total) * 100);

                } else if (res.type === HttpEventType.Response) {
                    this.fileData = [];
                    this.fileUploadProgress = -1;
                    var item: any;
                    item = res.body;
                    this.list_anh = item.list_image;

                }

            })
    }
    formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }
    remove_file(id): void {


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
                    .post('sys_event.ctr/delete_image/',
                        {

                            id: id
                        }
                    ).subscribe(resp => {
                        this.errorModel = [];
                        this.reload();
                        Swal.fire('Xoá thành công', '', 'success')
                    },
                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;

                            }
                            //this.loading = false;
                        }
                    );
            }
        })



    }
}
