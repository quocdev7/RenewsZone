import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_degree_popupAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_degree_popUpAddComponent extends BasePopUpAddComponent {
    public file_logo: any;
    public Progress_logo: any = -1;
    public group_field: any;
    constructor(public dialogRef: MatDialogRef<sys_degree_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_degree', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.http
            .post('/sys_field.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.group_field = resp;
            });
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
}
