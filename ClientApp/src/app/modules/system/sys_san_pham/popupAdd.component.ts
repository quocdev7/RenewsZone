import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { DataUrl, DOC_ORIENTATION, NgxImageCompressService, UploadResponse } from 'ngx-image-compress';
import { v4 as uuidv4 } from 'uuid';

import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import Swal from 'sweetalert2';

@Component({
    selector: 'sys_san_pham_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_san_pham_popUpAddComponent extends BasePopUpAddComponent {
    public file_image: any;
    public file_image_mobile: any;
    public Progress_image: any = -1;
    public Progress_image_mobile: any = -1;
    public lst_loai_san_pham: any = [];
    public list_khuyen_mai: any = [];
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig: any;

    constructor(public dialogRef: MatDialogRef<sys_san_pham_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private imageCompress: NgxImageCompressService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_san_pham', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.http
            .post('/sys_loai_san_pham.ctr/getListUse', {}
            ).subscribe(resp => {
                this.lst_loai_san_pham = resp;
            });
        this.http
            .post('/sys_khuyen_mai.ctr/getListUse', {}
            ).subscribe(resp => {
                this.list_khuyen_mai = resp;
            });
        this.timyconfig = {
            base_url: '/tinymce'
            , suffix: '.min',
            height: 500,

            file_picker_callback: function (cb, value, meta) {
                var croppedImage: any = '';
                var imgResultBeforeCompression: string = "";
                var imgResultAfterCompression: string = "";
                var input = document.createElement('input');
                var controller = "sys_san_pham";
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
    base64ToFile(data, filename) {
        const arr = data.split(',');
        const mime = arr[0].match(/:(.*?);/)[1];
        const bstr = atob(arr[1]);
        let n = bstr.length;
        let u8arr = new Uint8Array(n);

        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }

        return new File([u8arr], filename, { type: mime });
    }
    public imgResultBeforeCompression: string = "";
    public imgResultAfterCompression: string = "";

    chose_file_image(fileInput: any) {

        this.file_image = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
            fileInput.target.value = null;
        } else {
            this.compressFile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_image(files: any) {


        this.file_image = files;
        var rule_image = 3 * 1048576;
        if (this.file_image[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
        } else {
            this.compressFile();

        }
    }
    compressFile() {
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 3000, 3000) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                    that.file_image = that.base64ToFile(
                        that.imgResultAfterCompression,
                        "image.png"
                    );
                    console.warn(
                        'Size in bytes is now:',
                        that.imageCompress.byteCount(compressedImage)
                    );
                    that.submitFile(false);
                });
        };


    }
    submitFile(is_thumbnail: boolean) {

        if (this.file_image != null && this.file_image != undefined) {
            var formData = new FormData();
            this.Progress_image = 0;
            for (var i = 0; i < this.file_image.length; i++) {
                formData.append('list_file[]', this.file_image[i]);
            }
            if (this.record.db.id == null || this.record.db.id == 0) {
                this.record.db.id = uuidv4()
            };
            if (is_thumbnail) {
                if (this.actionEnum == 2) {
                    formData.append('id', this.record.db.id + "editthumbnail");
                } else {
                    formData.append('id', this.record.db.id + "thumbnail");
                }

            } else {
                if (this.actionEnum == 2) {
                    formData.append('id', this.record.db.id + "edit");
                } else {
                    formData.append('id', this.record.db.id);
                }

            }
            formData.append('type', "web");
            formData.append('controller', this.controller.toString());
            formData.append('list_file[]', this.file_image);
            this.record.db.hinh_anh = "";
            this.http.post('FileManager/uploadimagenew', formData, {
                reportProgress: true,
                observe: 'events'
            })
                .subscribe(res => {
                    debugger
                    if (res.type == HttpEventType.UploadProgress) {

                        this.Progress_image = Math.round((res.loaded / res.total) * 100);


                    } else if (res.type === HttpEventType.Response) {
                        var item: any;
                        item = res.body;
                        if (is_thumbnail) {
                            this.record.db.thumnail = item.location + "&v=" + uuidv4();
                        } else {
                            this.record.db.hinh_anh = item.location + "&v=" + uuidv4();
                        }

                        this.file_image = null
                        this.Progress_image = -1;



                    }

                })

        } else {

        }


    }




    compressFileMobile() {
        var reader = new FileReader();
        reader.readAsDataURL(this.file_image_mobile[0]);
        var that = this;
        reader.onload = function () {
            that.imageCompress.compressFile(reader.result.toString(), 1, 50, 50, 3000, 3000) // 50% ratio, 30% quality
                .then((compressedImage: DataUrl) => {
                    that.imgResultAfterCompression = compressedImage;
                    that.file_image_mobile = that.base64ToFile(
                        that.imgResultAfterCompression,
                        "image.png"
                    );
                    console.warn(
                        'Size in bytes is now:',
                        that.imageCompress.byteCount(compressedImage)
                    );
                    that.submitFileMobile(false);
                });

        };
    }
    chose_file_image_mobile(fileInput: any) {

        this.file_image_mobile = fileInput.target.files;
        var rule_image = 3 * 1048576;
        if (this.file_image_mobile[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");
            fileInput.target.value = null;
        } else {
            this.compressFileMobile();
            fileInput.target.value = null;
        }
    }
    DragAndDrop_image_mobile(files: any) {


        this.file_image_mobile = files;
        var rule_image = 3 * 1048576;
        if (this.file_image_mobile[0].size > rule_image) {
            Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'), "", "warning");

        } else {
            this.compressFileMobile();

        }
    }
    submitFileMobile(is_thumbnail: boolean) {
        var formData = new FormData();
        this.Progress_image = 0;
        for (var i = 0; i < this.file_image_mobile.length; i++) {
            formData.append('list_file[]', this.file_image_mobile[i]);
        }
        if (this.record.db.id == null || this.record.db.id == 0) {
            this.record.db.id = uuidv4()
        };
        if (is_thumbnail) {
            if (this.actionEnum == 2) {
                formData.append('id', this.record.db.id + "editthumbnail");
            } else {
                formData.append('id', this.record.db.id + "thumbnail");
            }

        } else {
            if (this.actionEnum == 2) {
                formData.append('id', this.record.db.id + "edit");
            } else {
                formData.append('id', this.record.db.id);
            }

        }
        formData.append('type', "mobile");
        formData.append('controller', this.controller.toString());
        formData.append('list_file[]', this.file_image_mobile);
        this.record.db.hinh_anh_mobile = "";

        this.http.post('FileManager/uploadimagenew', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_image = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.db.hinh_anh_mobile = item.location;
                    this.file_image_mobile = null
                    this.Progress_image_mobile = -1;



                }

            })
    }
    // chose_file_image_mobile(fileInput: any) {

    //     this.file_image_mobile = fileInput.target.files;
    //     var rule_image = 3*1048576;
    //     if(this.file_image_mobile[0].size >  rule_image){
    //         Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'),"","warning");
    //         fileInput.target.value = null;
    //     }else{
    //         this.submitFileMobile();
    //         fileInput.target.value = null;
    //     }
    // }
    // DragAndDrop_image_mobile(files: any) {

    //     this.file_image_mobile = files;
    //     var rule_image = 3*1048576;
    //     if(this.file_image_mobile[0].size >  rule_image){
    //         Swal.fire(this._translocoService.translate('system.anh_toi_da_3mb'),"","warning");

    //     }else{
    //     this.submitFileMobile();

    //     }
    // }
    // submitFileMobile() {
    //     var formData = new FormData();

    //     this.Progress_image = 0;
    //     for (var i = 0; i < this.file_image_mobile.length; i++) {
    //         formData.append('list_file[]', this.file_image_mobile[i]);
    //     }
    //     formData.append('list_file[]', this.file_image_mobile);
    //     this.http.post('FileManager/uploadimage', formData, {
    //         reportProgress: true,
    //         observe: 'events'
    //     })
    //         .subscribe(res => {
    //             if (res.type == HttpEventType.UploadProgress) {

    //                 this.Progress_image = Math.round((res.loaded / res.total) * 100);


    //             } else if (res.type === HttpEventType.Response) {
    //                 var item: any;
    //                 item = res.body;

    //                 this.record.db.hinh_anh_mobile = item.location;
    //                 this.file_image_mobile = null
    //                 this.Progress_image_mobile = -1;



    //             }

    //         })
    // }
}
