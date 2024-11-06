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
@Component({
    selector: 'popupInfo',
    templateUrl: 'popupInfoEdit.component.html',
})
export class popupInfoEditComponent extends BasePopUpAddComponent {

    public user_info: any;
    list_school_year: any;
    list_status_graduate: any = [];
    list_khoa: any;
    listjob_title: any;
    listcompany: any;
    listdepartment: any;

    public list_sex: any;

    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public file_image: any;
    public Progress_image: any = -1;
    public trang_thais: any;
    public list_trang_thai: any;
    constructor(public dialogRef: MatDialogRef<popupInfoEditComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = JSON.parse(JSON.stringify(data));
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.record.type_update = 3;
        this.record.action = 2;
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


        this.http
            .post('/sys_company.ctr/getListUse', {}
            ).subscribe(resp => {
                this.listcompany = resp;
            });
        if (this.actionEnum != 2) {
            this.create();
        }
        this.list_school_year = [];
        var yearCurrent = new Date().getFullYear();
        for (var i = 1970; i < yearCurrent; i++) {
            this.list_school_year.push({
                id: i,
                name: i.toString(),
            });

        }

        this.list_sex = [
            {
                id: 1,
                name: this._translocoService.translate("system.male")
            },
            {
                id: 2,
                name: this._translocoService.translate("system.female")
            },
            {
                id: 3,
                name: this._translocoService.translate("system.khac")
            },
        
        ]
        this.list_status_graduate = [
            {
                id: 1,
                name: this._translocoService.translate("portal.student")
            },
            {
                id: 2,
                name: this._translocoService.translate("portal.alumni")
            },
            {
                id: 3,
                name: this._translocoService.translate("portal.teacher")
            },
            {
                id: 4,
                name: this._translocoService.translate("portal.CBCNV")
            },
            {
                id: 5,
                name: this._translocoService.translate("portal.retire")
            },
        ]
        this.http
            .post('/sys_khoa.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_khoa = resp;
            });
        this.trang_thais = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Ông" },
            { id: 2, name: "Bà" },
            { id: 3, name: "Khác" }
        ]
        this.list_trang_thai = this.trang_thais;


    }

    create(): void {
       
            //1 main image , 2 avatar , 3 info , 4 link youtobe
            this.http
                .post(this.controller + '.ctr/updateProfile/',
                    {
                        data: this.record,
                    }
                ).subscribe(resp => {
                    this.record = resp;
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
    update(): void {
       
            //1 main image , 2 avatar , 3 info , 4 link youtobe
      
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

                    this.record.db.avatar_path = item.location;
                    this.file_image = null
                    this.Progress_image = -1;



                }

            })
    }

}
