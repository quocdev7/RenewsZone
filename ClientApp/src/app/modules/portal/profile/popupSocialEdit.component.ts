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
    selector: 'popupSocialEdit',
    templateUrl: 'popupSocialEdit.component.html',
})
export class popupSocialEditComponent extends BasePopUpAddComponent {
   
    public user_info: any;
    list_school_year: any;
    list_status_graduate: any = [];
    list_khoa: any;
    listjob_title: any;
    listcompany: any;
    listdepartment: any;
    
    public file_image: any;
    public Progress_image: any = -1;
    public trang_thais: any;
    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public list_trang_thai: any;
    constructor(public dialogRef: MatDialogRef<popupSocialEditComponent>,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user', dialogRef, dialogModal);
        this.record = JSON.parse(JSON.stringify(data));;
        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_user.ctr/getUserInfo', {

            }
            ).subscribe(resp => {
                this.user_info = resp;
            });

        if (this.actionEnum != 2) {
         
        } this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });

  
    
    }
    update(): void {
        //1 main image , 2 avatar , 3 info , 4 link youtobe
        this.record.type_update = 4;
        this.loading = true;
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

  

}
