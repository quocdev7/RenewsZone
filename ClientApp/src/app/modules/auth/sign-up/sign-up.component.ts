


import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';

import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
    selector: 'auth-sign-up',
    templateUrl: './sign-up.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthSignUpComponent implements OnInit {
    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    loading = false;
    showAlert: boolean = false;
    list_khoa: any;
    actionEnum: any = 1;
    record:any= {
        db: {
            FirstName: "",
            LastName: "",
            email: "",
            phone: "",
            id_khoa: "",
            sex:0,
            school_year: 0,
            status_graduate:0
        }
    }
    list_school_year: any;
    list_status_graduate: any=[];
    public errorModel: any;
    public event_id: any;
    public isUser: any;
    public trang_thais: any;
    public list_trang_thai: any;
    public user: any;
    /**
     * Constructor
     */
    constructor(

        private formBuilder: FormBuilder,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient, dialog: MatDialog
        , public _translocoService: TranslocoService,) {

        this.errorModel = [];
        this.list_school_year = [];
        var yearCurrent = new Date().getFullYear();
        for (var i = 1970; i < yearCurrent; i++) {
            this.list_school_year.push({
                id: i,
                name: i.toString(),
            });

        }


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

        this.route.params.subscribe(params => {

            this.event_id = params['event_id'];
        
        });
        this.trang_thais = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Ông" },
            { id: 2, name: "Bà" },
            { id: 3, name: "Khác" }
        ]
        this.list_trang_thai = this.trang_thais;
        this.record.event_id = this.event_id;
        this.signUp();
    }


    gobackhomepage() {
        const url = '/homepage-index';

        this.router.navigateByUrl(url);
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

    }
    ngOnInit(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
       
    }

    get currentYear(): number {
        return new Date().getFullYear();
    }
    signUp(): void {
        this.loading = true;
        this.http.post('sys_user.ctr/register/',
            {
                data: this.record
            }
        ).subscribe(
            (resp) => {
                
                this.user  = resp;
                         const url = '/confirmation-otp/' + this.user.db.Id;
                        this.router.navigateByUrl(url);
            },
            (error) => {
                if (error.status == 400) {
                    this.errorModel = error.error;
                   
                }
                // Set the alert
                this.alert = {
                    type: 'error',
                    message: 'Không hợp lệ, Vui lòng kiểm tra lại'
                };

                // Show the alert
                this.loading = false;
            }
        );
    }

    public showMessagewarningN(title, msg): void {
        Swal.fire({
            title: title,
            text: msg,
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }
 
    register(): void {
        //this.loading = true;
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
                    .post('sys_event_participate.ctr/register_event/',

                        this.record,

                    ).subscribe(resp => {
                        this.errorModel = [];


                        Swal.fire({
                            title: this._translocoService.translate('system.dangkythanhcong'),
                            text: "",
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: this._translocoService.translate('close'),
                        }).then((result) => {
                            this.router.navigate(['sys_event_thong_bao_dang_ky']);
                        })

                    },
                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;
                                this.showMessagewarningN(this._translocoService.translate("notvalid"), this._translocoService.translate("pleasecheck"));
                            }
                            //this.loading = false;
                        }
                    );
            }
        })
    }
}
