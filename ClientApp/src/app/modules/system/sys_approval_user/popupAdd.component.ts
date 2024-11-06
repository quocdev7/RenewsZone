import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType} from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
    selector: 'sys_approval_user_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_approval_user_popUpAddComponent extends BasePopUpAddComponent {
   listsex: any;
    listdepartment: any;
    listjob_title: any;
    listtype: any;
    listcompany: any;


    public list_user_education: any = [];
    public list_user_success: any = [];
    public list_user_certificate: any = [];
    public list_user_work_history: any = [];
    public list_user_experience: any = [];
    public user_cv: any;
    public is_show_hide_education: any = true;
    public is_show_hide_success: any = true;
    public is_show_hide_certificate: any = true;
    public is_show_hide_work_history: any = true;
    public is_show_hide_experience: any = true;
    public is_show_hide_social: any = true;
    public list_tru_so: any;
    public file_image: any;
    public Progress_image: any = -1;
    public listNews: any;
    public listDanhMuc: any;
    constructor(public dialogRef: MatDialogRef<sys_approval_user_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_approval_user', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.listtype = [
            {
                id: 1,
                name: this._translocoService.translate('system.user_admin')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.user_guess')
            },
        ];
        
      

        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
           

            this.loadUser();
         
        }
        if (this.actionEnum == 2 || this.actionEnum == 3) {
            this.record.password = "************"

        }
    }
    loadUser(): void {
        this.http
            .post('/sys_user.ctr/getUserInfo/', {}
            ).subscribe(resp => {
                this.record = resp;
            });
    }
    showHideEducation(): void {
        this.is_show_hide_education = !this.is_show_hide_education;
    }

    showHideSuccess(): void {
        this.is_show_hide_success = !this.is_show_hide_success;
    }
    showHideCertificate(): void {
        this.is_show_hide_certificate = !this.is_show_hide_certificate;

    }
    showHideWorkHistory(): void {
        this.is_show_hide_work_history = !this.is_show_hide_work_history;
    }
    showHideExperience(): void {
        this.is_show_hide_experience = !this.is_show_hide_experience;
    }
    showHideSocial(): void {
        this.is_show_hide_social = !this.is_show_hide_social;
    }
  
    approval(id1): void {
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
                this.http
                    .post(this.controller + '.ctr/approval/',
                        {
                            id: id1,
                        }
                ).subscribe(resp => {

                        Swal.fire('Gửi duyệt thành công', '', 'success').then(
                        // Navigate to the confirmation required page
                        res => {
                            this.close();
                        });
                       
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    cancel(id1): void {
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
                this.http
                    .post(this.controller + '.ctr/cancel/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {

                        Swal.fire('Tài khoản đã đóng', '', 'success').then(
                            // Navigate to the confirmation required page
                            res => {
                                this.close();
                            });

                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
}
