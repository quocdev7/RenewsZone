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
    selector: 'popupLanguage',
    templateUrl: 'popupLanguage.component.html',
})
export class sys_doi_tac_popUpLanguageComponent extends BasePopUpAddComponent {
    public group_news: any;
    public type_news: any;
    public list_loai_doi_tac: any;
    public file_logo: any;
    public Progress_logo: any = -1;
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig: any;
    constructor(public dialogRef: MatDialogRef<sys_doi_tac_popUpLanguageComponent>,
        http: HttpClient, _translocoService: TranslocoService,private imageCompress: NgxImageCompressService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_doi_tac', dialogRef, dialogModal);
        debugger
        this.record = data;

        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.getListLoaiDoiTac();
        this.timyconfig = {
            base_url: '/tinymce'
            , suffix: '.min',
            height: 500,

            file_picker_callback: function (cb, value, meta) {
                var croppedImage: any = '';
                var imgResultBeforeCompression: string = "";
                var imgResultAfterCompression: string = "";
                var input = document.createElement('input');
                var controller = "sys_doi_tac";
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
                    this.record.db.image = item.location;
                    this.file_logo = null
                    this.Progress_logo = -1;
                }

            })
    }
    getListLoaiDoiTac():void  {
        this.http
        .post('/sys_loai_doi_tac.ctr/getListUse/', {
        }
        ).subscribe(resp => {
         this.list_loai_doi_tac=resp;
        });
    }
    save_language(): void {
        debugger
        this.beforesave();
        this.loading = true;
        this.http.post(this.controller + '.ctr/edit_language/',
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
    close_create() {
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
                if (this.actionEnum == 3) {
                    this.basedialogRef.close(this.record);
                } else {
                    this.basedialogRef.close(this.Oldrecord);
                }
            }

        })

    }

}
