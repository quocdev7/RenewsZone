import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import Swal from 'sweetalert2';
@Component({
    selector: 'popupAddSuccess',
    templateUrl: 'popupAddSuccess.component.html',
})
export class popupAddSuccessComponent extends BasePopUpAddComponent {
   
    public user_info: any;
    
    public isScreenSmall: any = false;

    public file_image: any;
    public Progress_image: any = -1;
    constructor(public dialogRef: MatDialogRef<popupAddSuccessComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));

        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_user.ctr/getUserInfo', {

            }
            ).subscribe(resp => {
                this.user_info = resp;
            });

        if (this.actionEnum != 2) {
            this.dialogRef.keydownEvents().subscribe(event => {
                if (event.key === "Escape") {
                    //this.onCancel();
                    this.dialogRef.close();
                }
            });
        }
        if (this.actionEnum == 1) {
            this.saveSuccess();
        }
        
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
    saveSuccess() {
        this.http
            .post(this.controller + '.ctr/createSuccess/',
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

                }
            );
    }

   
}
