import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';

import { DataUrl, DOC_ORIENTATION, NgxImageCompressService, UploadResponse } from 'ngx-image-compress';
import { v4 as uuidv4 } from 'uuid';


@Component({
    selector: 'sys_cot_moc_su_kien_popupLanguage',
    templateUrl: 'popupLanguage.html',
})
export class sys_cot_moc_su_kien_popupLanguageComponent extends BasePopUpAddComponent {
    public file_image: any;
    public file_image_mobile: any;
    public Progress_image: any = -1;
    public Progress_image_mobile: any = -1;
    public list_su_kien:any=[];
   

    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig: any;

    constructor(public dialogRef: MatDialogRef<sys_cot_moc_su_kien_popupLanguageComponent>,
        http: HttpClient, _translocoService: TranslocoService,private imageCompress: NgxImageCompressService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_cot_moc_su_kien', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
      this.load_su_kien();
      this.timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,

        file_picker_callback: function (cb, value, meta) {
            var croppedImage: any = '';
            var imgResultBeforeCompression: string = "";
            var imgResultAfterCompression: string = "";
            var input = document.createElement('input');
            var controller = "sys_cot_moc_su_kien";
            var type = "editor";
            input.setAttribute('type', 'file');
            input.onchange = function () {
                var file = input.files[0];
                var reader = new FileReader();
                reader.readAsDataURL(file);

                reader.onload = function () {
                    imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 1000, 1000) // 50% ratio, 30% quality
                        .then((compressedImage: DataUrl) => {
                            imgResultAfterCompression = compressedImage;

                            const arr = imgResultAfterCompression.split(',');
                            const mime = arr[0].match(/:(.*?);/)[1];
                            const bstr = atob(arr[1]);
                            let n = bstr.length;
                            let u8arr = new Uint8Array(n);

                            while (n--) {
                                u8arr[n] = bstr.charCodeAt(n);
                            }

                            file = new File([u8arr], "image.png", { type: mime });
                            debugger
                            // FormData
                            var fd = new FormData();
                            var files = file;

                            // if(data.db.id ==null ||data.db.id ==0){
                            //     data.db.id = uuidv4()
                            //  };

                            fd.append('id', uuidv4());

                            fd.append('filetype', meta.filetype);
                            fd.append("file", files);
                            fd.append("type", type);
                            fd.append('controller', controller);
                            var filename = "";

                            // AJAX
                            var xhr;
                            xhr = new XMLHttpRequest();
                            xhr.withCredentials = false;
                            xhr.open('POST', '/FileManager/uploadimagenew');

                            xhr.onload = function () {
                                var json;
                                if (xhr.status != 200) {
                                    alert('HTTP Error: ' + xhr.status);
                                    return;
                                }
                                json = JSON.parse(xhr.responseText);
                                if (!json || typeof json.location != 'string') {
                                    alert('Invalid JSON: ' + xhr.responseText);
                                    return;
                                }
                                filename = json.location;
                                reader.onload = function (e) {
                                    cb(filename);
                                };
                                reader.readAsDataURL(file);
                            };
                            xhr.send(fd);
                            return
                        });

                };

            };

            input.click();
        },
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    };


    }

 load_su_kien(): void {
   this.http
            .post('/sys_event.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_su_kien = resp;
            });

}
    chose_file_image(fileInput: any) {

        this.file_image = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDrop_image(files: any) {

        this.file_image = files;
        this.submitFile();
    }
    submitFile() {
        var formData = new FormData();

        this.Progress_image = 0;
        for (var i = 0; i < this.file_image.length; i++) {
            formData.append('list_file[]', this.file_image[i]);
        }
        formData.append('list_file[]', this.file_image);
        this.http.post('FileManager/uploadimage', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_image = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.db.image = item.location;
                    this.file_image = null
                    this.Progress_image = -1;



                }

            })
    }


    save_language(): void {
        this.beforesave();
        this.loading = true;
        this.http
        .post(this.controller + '.ctr/edit_language/',
            {
                data: this.record,
            }
        ).subscribe(resp => {
            this.record = resp;
            this.Oldrecord = this.record;
            this.basedialogRef.close(this.record);
            Swal.fire('Lưu thành công', '', 'success');
            this.aftersave();
        },
            error => {
                if (error.status == 400) {
                    this.errorModel = error.error;
                    this.aftersavefail();
                }
                if (error.status == 403) {
                    this.basedialogRef.close();
                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                }
                this.loading = false;

            });
    }
    chose_file_image_mobile(fileInput: any) {

        this.file_image_mobile = fileInput.target.files;
        this.submitFileMobile();
        fileInput.target.value = null;
    }
    DragAndDrop_image_mobile(files: any) {

        this.file_image_mobile = files;
        this.submitFileMobile();
    }
    submitFileMobile() {
        var formData = new FormData();

        this.Progress_image = 0;
        for (var i = 0; i < this.file_image_mobile.length; i++) {
            formData.append('list_file[]', this.file_image_mobile[i]);
        }
        formData.append('list_file[]', this.file_image_mobile);
        this.http.post('FileManager/uploadimage', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_image = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.db.image_mobile = item.location;
                    this.file_image_mobile = null
                    this.Progress_image_mobile = -1;



                }

            })
    }
}
