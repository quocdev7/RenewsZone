import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import Swal from 'sweetalert2';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

import { NgxImageCompressService } from "ngx-image-compress";
import { base64ToFile, ImageCroppedEvent, LoadedImage } from 'ngx-image-cropper';
@Component({
    selector: 'popupMainImage',
    templateUrl: 'popupMainImage.component.html',
})
export class popupMainImageComponent extends BasePopUpAddComponent {

    public user_info: any;

    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public file_image: any;
    public Progress_image: any = -1;

    public imgResultBeforeCompression: string = "";
    public imgResultAfterCompression: string = "";

    imageChangedEvent: any = '';
    croppedImage: any = '';

    constructor
        (
        public imageCompress: NgxImageCompressService,
        public dialogRef: MatDialogRef<popupMainImageComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record =  JSON.parse(JSON.stringify(data));
        this.Oldrecord = JSON.parse(JSON.stringify(data));

        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_user.ctr/getUserInfo', {

            }
            ).subscribe(resp => {
                this.user_info = resp;
            });

        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });



        if (this.actionEnum != 2) {
           
        }
      
        
    }
 
    compressFileN() {
        this.imageCompress.uploadFile().then(
            ({ image, orientation }) => {

                this.imgResultBeforeCompression = image;
                console.log("Size in bytes of the uploaded image was:", this.imageCompress.byteCount(image));

                this.imageCompress.compressFile(image, orientation, 50, 50) // 50% ratio, 50% quality
                    .then(
                        (compressedImage) => {
                            this.imgResultAfterCompression = compressedImage;
                            console.log("Size in bytes after compression is now:", this.imageCompress.byteCount(compressedImage));
                        }
                    );
            }
        );
    }
     compressFile(img:any) {
       
       this.imageCompress.compressFile(img, 1, 100, 100) // 50% ratio, 50% quality
             .then(
                 (compressedImage) => {
                     this.imgResultAfterCompression = compressedImage;
                     this.file_image = this.base64ToFile(
                         this.imgResultAfterCompression,
                         this.imageChangedEvent.target.files[0].name,
                     );
                     this.submitFile();
                 });
       
    }

    update(): void {
        //1 main image , 2 avatar , 3 info , 4 link youtobe

        this.record.type_update = 1;
        this.http
            .post(this.controller + '.ctr/updateProfile/',
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

    fileChangeEvent(event: any): void {
        this.imageChangedEvent = event;
    }
    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
        this.compressFile(this.croppedImage);
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
    imageLoaded(image: LoadedImage) {
        // show cropper
      
    }
    cropperReady() {
        // cropper ready
    }
    loadImageFailed() {
        // show message
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

                    this.record.db.cover_image = item.location;
                    this.file_image = null
                    this.Progress_image = -1;



                }

            })
    }

}
