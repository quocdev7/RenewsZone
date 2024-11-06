import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { AuthService } from 'app/core/auth/auth.service';
import { Location } from '@angular/common';
import { HttpClient, HttpResponse } from '@angular/common/http';
@Component({
    selector: 'auth-sign-in',
    templateUrl: './sign-in.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class AuthSignInComponent implements OnInit {
    @ViewChild('signInNgForm') signInNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: ''
    };
    signInForm: FormGroup;
    showAlert: boolean = false;
    false_login: number = 0;
    //showCaptcha: boolean = false;
    public year: any;
    public errorModel: any;
    actionEnum: any = 1;
    record: any = {
        db: {

            email: null,
            password: null

        },
        showCaptcha: 0
    }
    loading = false;
    /**
     * Constructor
     */
    constructor(
        public http: HttpClient,
        private _location: Location,
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: FormBuilder,
        private _router: Router,

    ) {
        this.errorModel = [];
        this.http
            .get('/users/checklogincapcha/'
            ).subscribe(resp => {
                if (resp == '2') this.record.showCaptcha = 0;

            });

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------
    get currentYear(): number {
        return new Date().getFullYear();
    }
    /**
     * On init
     */
    goback() {
        const { redirect } = window.history.state;

        if (redirect == '/Captcha/GetCaptchaImage') this._router.navigateByUrl('/homepage-index');
        this._router.navigateByUrl(redirect || '/homepage-index');
    }

    gobackhomepage() {
        const url = '/homepage-index';

        this._router.navigateByUrl(url);
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
        this.signInNew();
        this.record.showCaptcha = 0;

        var date = new Date()
        this.year = date.getFullYear();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    signInNew(): void {
        this.loading = true;
        this._authService.signInNew(this.record)
            .subscribe(
                (resp) => {
                    this.loading = false;
                    this._router.navigateByUrl('/homepage-index');

                },
                (error) => {
                    if (error.status == 400) {
                        this.errorModel = error.error;

                    }
                    this.false_login++;
                    if (this.false_login > 3) this.record.showCaptcha = 1;
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

    /**
     * Sign in
     */
    signIn(): void {
        // Return if the form is invalid
        if (this.signInForm.invalid) {
            return;
        }

        // Disable the form
        this.signInForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Sign in
        this._authService.signIn(this.signInForm.value)
            .subscribe(
                (res) => {
                    this._router.navigateByUrl('/person_setting');
                },
                (response) => {
                    debugger
                    // Re-enable the form
                    this.signInForm.enable();

                    this.false_login++;
                    if (this.false_login > 4) this.record.showCaptcha = true;
                    // Set the alert
                    this.alert = {
                        type: 'error',
                        message: response.error.message
                    };

                    // Show the alert
                    this.showAlert = true;
                }
            );
    }
}
