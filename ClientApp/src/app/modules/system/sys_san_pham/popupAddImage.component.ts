import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { DataUrl, DOC_ORIENTATION, NgxImageCompressService, UploadResponse } from 'ngx-image-compress';
import { v4 as uuidv4 } from 'uuid';

import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Upload } from 'tus-js-client';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
export interface FileStatus {
    filename: string;
    progress: number;
    hash: string;
    uuid: string;
}
@Component({
    selector: 'sys_san_pham_popupAddImage',
    templateUrl: 'popupAddImage.html',
})

export class sys_san_pham_popUpAddImageComponent extends BasePopUpAddComponent {
    public file_image: any;
    public file_image_mobile: any;
    file_compress: any;
    public Progress_image: any = -1;
    public Progress_image_mobile: any = -1;
    public list_san_pham: any = [];
    fileData: any;
    previewUrl: any = null;
    fileUploadProgress: any = -1;
    uploadedFilePath: string = null;
    constructor(public dialogRef: MatDialogRef<sys_san_pham_popUpAddImageComponent>,
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
        this.load_file();


    }

    load_file(): void {
        this.http
            .post('/sys_san_pham.ctr/getElementByIdNew/', {
                id: this.record.db.id
            }
            ).subscribe(resp => {
                var data: any;
                data = resp;
                this.record.list_file = data.list_file;
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

    save_image(): void {
        this.beforesave();
        this.loading = true;
        this.http
            .post(this.controller + '.ctr/save_image/',
                {
                    data: this.record,
                }
            ).subscribe(resp => {
                this.record = resp;
                this.Oldrecord = this.record;
                this.basedialogRef.close(this.record);
                Swal.fire('Lưu thành công', '', 'success');
                this.aftersave();
                this.load_file()
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
    //FILE
    //File
    downloadfile(id: any): string {
        var url = '/sys_anh_san_pham.ctr/download?id=' + id;
        return url;

    }


    deleteFile(id, pos: void) {
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
                //this.record.list_file.splice(pos, 1)
                this.http
                    .post(this.controller + '.ctr/delete_file/',
                        {
                            id: id,
                        }
                    ).subscribe(resp => {
                        //this.record = resp;
                        this.load_file();
                        Swal.fire('Xóa thành công', '', 'success');
                        //this.aftersave();
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
        })


    }
    private uploadStatus = new Subject<FileStatus[]>();
    uploadProgress = this.uploadStatus.asObservable();

    fileStatusArr: FileStatus[] = [];


    fileProgress(fileInput: any) {

        this.fileData = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDropProgress(files: any) {

        this.fileData = files;
        this.submitFile();
    }
    croppedImage: any = '';
    public imgResultBeforeCompression: string = "";
    public imgResultAfterCompression: string = "";



    submitFile() {


        this.loading = true;
        this.beforesave();
        this.fileUploadProgress = 0;
        if (this.record.list_file == null) this.record.list_file = [];
        for (var i = 0; i < this.fileData.length; i++) {

            var file = this.fileData[i].name.split(".");
            var duoifile = file[file.length - 1];
            if (duoifile == "xls" || duoifile == "exe") {
                Swal.fire("File " + this.fileData[i].name + " có định dạng không được upload", "", "warning");
                this.loading = false;
            }
            else {
                this.record.list_file.push({
                    db: {
                        id: 0,
                        upload_date: new Date(),
                        id_san_pham: this.record.db.id_san_pham,
                        file_name: this.fileData[i].name,
                        file_size: this.fileData[i].size,
                        file_type: this.fileData[i].type
                    },
                    newfile: true,
                    percent_complete: 0,
                });
                this.uploadFile(this.record.list_file.length - 1, this.record, this.fileData[i], this.fileData[i].name);
            }

        }
    }
    uploadFile(pos, item, file: File, filename: string) {


        const fileStatus: FileStatus = { filename, progress: 0, hash: '', uuid: '' };
        this.fileStatusArr.push(fileStatus);

        this.uploadStatus.next(this.fileStatusArr);
        const upload = new Upload(file, {
            endpoint: "/tusFiles",
            retryDelays: [0, 3000, 6000, 12000, 24000],
            chunkSize: 2000000,
            metadata: {
                fileName: filename,
                id: item.db.id,
            },
            onError: async (error) => {

                return false;
            },
            onChunkComplete: (chunkSize, bytesAccepted, bytesTotal) => {
                this.fileStatusArr.forEach(value => {
                    if (value.filename === filename) {
                        value.progress = Math.floor(bytesAccepted / bytesTotal * 100);
                        this.record.list_file[pos].percent_complete = value.progress;
                        value.uuid = upload.url.split('/').slice(-1)[0];
                    }
                });
                this.uploadStatus.next(this.fileStatusArr);
            },
            onSuccess: async () => {
                this.fileStatusArr.forEach(value => {
                    if (value.filename === filename) {
                        value.progress = 100;
                        this.record.list_file[pos].percent_complete = value.progress;
                        this.record.list_file[pos].db.file_path = "\\" + "san_pham" + "\\" + item.db.id + "\\" + value.uuid + "." + filename.split(".").pop();
                        this.record.list_file[pos].uuid = value.uuid;
                        this.record.list_file[pos].stt = pos + 1;
                        var checksync = true;

                        for (var i = 0; i < this.record.list_file.length; i++) {
                            if (this.record.list_file[i].newfile == true) {
                                if (this.record.list_file[i].percent_complete != 100) {
                                    checksync = false;
                                }
                            }
                        }
                        if (checksync == true) this.loading = false;



                    }
                });
                this.uploadStatus.next(this.fileStatusArr);
                return true;
            }
        });
        upload.start();
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

    // submitFile() {
    //     this.loading = true;
    //     this.beforesave();
    //     this.fileUploadProgress = 0;
    //     if (this.record.list_file == null) this.record.list_file = [];
    //     for (var i = 0; i < this.fileData.length; i++) {

    //         var file = this.fileData[i].name.split(".");
    //         var duoifile = file[file.length - 1];
    //         if (duoifile == "xls" || duoifile == "exe") {
    //             Swal.fire("File " + this.fileData[i].name + " có định dạng không được upload", "", "warning");
    //             this.loading = false;
    //         }
    //         else {
    //             this.record.list_file.push({
    //                 db: {
    //                     id: 0,
    //                     upload_date: new Date(),
    //                     id_san_pham: this.record.db.id_san_pham,
    //                     file_name: this.fileData[i].name,
    //                     file_size: this.fileData[i].size,
    //                     file_type: this.fileData[i].type
    //                 },
    //                 newfile: true,
    //                 percent_complete: 0,
    //             });
    //             this.uploadFile(this.record.list_file.length - 1, this.record, this.fileData[i], this.fileData[i].name);
    //         }

    //     }
    // }
    // deleteFile(pos: void) {
    //     this.record.list_file.splice(pos, 1)
    // }
    // uploadFile(pos, item, file: File, filename: string) {
    //     const fileStatus: FileStatus = { filename, progress: 0, hash: '', uuid: '' };
    //     this.fileStatusArr.push(fileStatus);
    //     
    //     this.uploadStatus.next(this.fileStatusArr);
    //     const upload = new Upload(file, {
    //         endpoint: "/tusFiles",
    //         retryDelays: [0, 3000, 6000, 12000, 24000],
    //         chunkSize: 2000000,
    //         metadata: {
    //             fileName: filename,
    //             id: item.db.id,
    //         },
    //         onError: async (error) => {
    //             console.log(error);
    //             return false;
    //         },
    //         onChunkComplete: (chunkSize, bytesAccepted, bytesTotal) => {
    //             this.fileStatusArr.forEach(value => {
    //                 if (value.filename === filename) {
    //                     value.progress = Math.floor(bytesAccepted / bytesTotal * 100);
    //                     this.record.list_file[pos].percent_complete = value.progress;
    //                     value.uuid = upload.url.split('/').slice(-1)[0];
    //                 }
    //             });
    //             this.uploadStatus.next(this.fileStatusArr);
    //         },
    //         onSuccess: async () => {
    //             this.fileStatusArr.forEach(value => {
    //                 if (value.filename === filename) {
    //                     value.progress = 100;
    //                     this.record.list_file[pos].percent_complete = value.progress;
    //                     this.record.list_file[pos].db.file_path =  "\\"+"san_pham"+ "\\"+ item.db.id + "\\" + value.uuid + "." + filename.split(".").pop();
    //                     this.record.list_file[pos].uuid = value.uuid;
    //                     this.record.list_file[pos].stt =pos+1;
    //                     var checksync = true;
    //                     for (var i = 0; i < this.record.list_file.length; i++) {
    //                         if (this.record.list_file[i].percent_complete != 100) {
    //                             checksync = false;
    //                         }
    //                     }
    //                     if (checksync == true) this.loading = false;



    //                 }
    //             });
    //             this.uploadStatus.next(this.fileStatusArr);
    //             return true;
    //         }
    //     });
    //     upload.start();
    // }
    download() {
        var valid = true;
        var error = '';
        if (this.record.list_file.length == 0) {
            error += this._translocoService.translate('system.phai_chon_file') + '<br>';
            valid = false;
        }

        if (!valid) {
            this.showMessagewarning(error);
            return;
        }
        var list_id = "";
        for (var i = 0; i < this.record.list_file.length; i++) {


            var model = this.record.list_file[i];
            if (model.isCheck == true) {
                list_id = list_id + model.db.id + ",";

            }
        }

        var url = '/doc_tailieu.ctr/download?id=' + list_id.substring(0, list_id.length - 1);
        window.location.href = url;

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

    getFontAwesomeIconFromMIME(mimeType) {
        // List of official MIME Types: http://www.iana.org/assignments/media-types/media-types.xhtml
        var icon_classes = {
            // Media
            "image/jpeg": "assets/icon_file_type/jpg.png",
            "image/png": "assets/icon_file_type/png.png",
            // Documents
            "application/pdf": "assets/icon_file_type/pdf.png",
            "application/msword": "assets/icon_file_type/doc.png",
            "application/vnd.ms-word": "assets/icon_file_type/doc.png",
            "application/vnd.oasis.opendocument.text": "assets/icon_file_type/doc.png",
            "application/vnd.openxmlformats-officedocument.wordprocessingml":
                "assets/icon_file_type/doc.png",
            "application/vnd.ms-excel": 'assets/icon_file_type/excel.png',
            "application/vnd.openxmlformats-officedocument.spreadsheetml":
                'assets/icon_file_type/excel.png',
            "application/vnd.oasis.opendocument.spreadsheet": "assets/icon_file_type/excel.png",
            "application/vnd.ms-powerpoint": "assets/icon_file_type/ppt.png",
            "application/vnd.openxmlformats-officedocument.presentationml":
                "assets/icon_file_type/ppt.png",

            "application/vnd.oasis.opendocument.presentation": "assets/icon_file_type/ppt.png",
            "text/plain": "assets/icon_file_type/txt.png",
            "text/html": "assets/icon_file_type/html.png",
            "application/json": "assets/icon_file_type/json-file.png",
            // Archives
            "application/gzip": "assets/icon_file_type/zip.png",
            "application/x-zip-compressed": "assets/icon_file_type/zip.png",
            "application/octet-stream": "assets/icon_file_type/zip-1.png",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation": "assets/icon_file_type/ppt.png",
        };

        for (var key in icon_classes) {
            if (icon_classes.hasOwnProperty(key)) {
                if (mimeType.search(key) === 0) {
                    // Found it
                    return icon_classes[key];
                }
            } else {
                return "assets/icon_file_type/file.png";
            }
        }
    }
}
