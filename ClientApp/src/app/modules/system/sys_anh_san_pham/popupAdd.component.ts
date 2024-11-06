import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


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
    selector: 'sys_anh_san_pham_popupAdd',
    templateUrl: 'popupAdd.html',
})

export class sys_anh_san_pham_popUpAddComponent extends BasePopUpAddComponent {
    public file_image: any;
    public file_image_mobile: any;
    public Progress_image: any = -1;
    public Progress_image_mobile: any = -1;
    public list_san_pham: any = [];
    fileData: any;
    previewUrl: any = null;
    fileUploadProgress: any = -1;
    uploadedFilePath: string = null;
    constructor(public dialogRef: MatDialogRef<sys_anh_san_pham_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_anh_san_pham', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.load_san_pham();
    }

    load_san_pham(): void {
        this.http
            .post('/sys_anh_san_pham.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_san_pham = resp;
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
    // submitFile() {
    //     var formData = new FormData();

    //     this.Progress_image = 0;
    //     for (var i = 0; i < this.file_image.length; i++) {
    //         formData.append('list_file[]', this.file_image[i]);
    //     }
    //     formData.append('list_file[]', this.file_image);
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

    //                 this.record.db.image = item.location;
    //                 this.file_image = null
    //                 this.Progress_image = -1;



    //             }

    //         })
    // }



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

    //FILE
    //File
    downloadfile(id: any): string {
        var url = '/sys_anh_san_pham.ctr/download?id=' + id;
        return url;

    }


    deleteFile(pos: void) {
        this.record.list_file.splice(pos, 1)
    }
    private uploadStatus = new Subject<FileStatus[]>();
    uploadProgress = this.uploadStatus.asObservable();

    fileStatusArr: FileStatus[] = [];

    uploadFile(pos, item, file: File, filename: string) {
        const fileStatus: FileStatus = { filename, progress: 0, hash: '', uuid: '' };
        this.fileStatusArr.push(fileStatus);
        debugger
        this.uploadStatus.next(this.fileStatusArr);
        const upload = new Upload(file, {
            endpoint: "/tusFiles",
            retryDelays: [0, 3000, 6000, 12000, 24000],
            chunkSize: 2000000,
            metadata: {
                fileName: filename,
                id: item.db.id_san_pham,
            },
            onError: async (error) => {
                console.log(error);
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
                        this.record.list_file[pos].db.file_path = "file_upload" + "\\" + item.db.id_san_pham + "\\" + value.uuid + "." + filename.split(".").pop();
                        this.record.list_file[pos].uuid = value.uuid;
                        var checksync = true;
                        for (var i = 0; i < this.record.list_file.length; i++) {
                            if (this.record.list_file[i].percent_complete != 100) {
                                checksync = false;
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
