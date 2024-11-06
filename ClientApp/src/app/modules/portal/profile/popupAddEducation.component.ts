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
    selector: 'popupAddEducation',
    templateUrl: 'popupAddEducation.component.html',
})
export class popupAddEducationComponent extends BasePopUpAddComponent {

    public user_info: any;
    list_from_year: any;
    list_to_year: any;
    list_degree: any;
  

    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(public dialogRef: MatDialogRef<popupAddEducationComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
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

        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });


        this.http
            .post('/sys_user.ctr/getListDegree', {}
            ).subscribe(resp => {
                this.list_degree = resp;
            });
        if (this.actionEnum != 2) {
           
        }
        if (this.actionEnum == 1) {
            this.save();
        }
        this.list_from_year = [];
        this.list_to_year = [];
        var yearCurrent = new Date().getFullYear();
        for (var i = 1970; i < yearCurrent; i++) {
            this.list_from_year.push({
                id: i,
                name: i.toString(),
            });

        }
        var yearCurrent = new Date().getFullYear() +1;
        for (var i = 1970; i < yearCurrent; i++) {
            this.list_to_year.push({
                id: i,
                name: i.toString(),
            });

        }
        if (this.actionEnum == 1) {
            this.save();
        }


    }
   
    save() {
        this.http
            .post(this.controller + '.ctr/createEducation/',
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
