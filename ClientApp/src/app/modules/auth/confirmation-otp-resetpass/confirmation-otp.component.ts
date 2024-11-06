import { Component, ViewEncapsulation, OnInit} from '@angular/core';
import { fuseAnimations } from '@fuse/animations';

import { NgOtpInputModule } from 'ng-otp-input';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { AuthService } from 'app/core/auth/auth.service';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
@Component({
    selector     : 'auth-confirmation-otp-reset-pass',
    templateUrl  : './confirmation-otp.component.html',
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class AuthConfirmationOtpResetPassComponent implements OnInit
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
        // this.route.params.subscribe(params => {
        //     this.user_id = params["id"];
        // });
        //this.getUser();
    }
    onOtpChange(otp) {
        this.code = otp;
    }
   
   
 

    gobackhomepage() {
        const url = '/sign-in';

        this.router.navigateByUrl(url);
    }
 
}
