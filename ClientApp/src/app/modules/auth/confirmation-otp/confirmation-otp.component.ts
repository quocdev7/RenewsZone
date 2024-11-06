import { Component, ViewEncapsulation, OnInit} from '@angular/core';
import { fuseAnimations } from '@fuse/animations';

import { NgOtpInputModule } from 'ng-otp-input';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { AuthService } from 'app/core/auth/auth.service';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
@Component({
    selector     : 'auth-confirmation-otp',
    templateUrl  : './confirmation-otp.component.html',
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class AuthConfirmationOtpComponent implements OnInit
{
    /**
     * Constructor
     */
    public count: any;
    public code: any;
    public users: any;
    public loading: any;
    public user_id: any;
    constructor(public http: HttpClient, public _translocoService: TranslocoService, public _authService: AuthService,
        public router: Router, public route: ActivatedRoute,

    )
    {
        this.count = 0;
    
    }
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.user_id = params["id"];
        });
        this.getUser();
    }
    onOtpChange(otp) {
        this.code = otp;
    }
    getUser(): void {
       
        this.http
            .post('/sys_user.ctr/getUserOtp/', {
                id: this.user_id,

            }
            ).subscribe(resp => {
              
                this.users = resp;
             
            })
    }
   
 

    gobackhomepage() {
        const url = '/sign-in';

        this.router.navigateByUrl(url);
    }
    xac_nhan(): void {
        this.loading = true;
        this.http
            .post('/sys_user.ctr/xac_thuc/', {
                code: this.code,
                user_id: this.user_id
            }
        ).subscribe(resp => {
            this.loading = false;
                var data = resp;
            if (data == "") {


                Swal.fire(this._translocoService.translate("system.xac_thuc_thanh_cong"), '', 'success').then(
                    // Navigate to the confirmation required page
                    res => {
                        const url = '/sign-in';
                        this.router.navigateByUrl(url);

                    }

                );

                } else {

                this.count = this.count + 1;
                    Swal.fire(this._translocoService.translate("system.msgmaxacthuc"), "", "warning").then(
                        // Navigate to the confirmation required page
                        res => {
                          
                            if (this.count == 3) {
                                const url = '/sign-in';
                                this.router.navigateByUrl(url);
                            }
                        }

                    );

                   
                }
            })
    }
}
