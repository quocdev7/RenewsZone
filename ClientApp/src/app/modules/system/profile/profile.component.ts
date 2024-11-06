import { HttpClient } from '@angular/common/http';
import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { FuseNavigationService } from '../../../../@fuse/components/navigation';
import { BaseIndexDatatableComponent } from '../../../Basecomponent/BaseIndexDatatable.component';
import { AuthService } from '../../../core/auth/auth.service';
import { AuthGuard } from '../../../core/auth/guards/auth.guard';

@Component({
    selector: 'profile',
    templateUrl  : './profile.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ProfileComponent extends BaseIndexDatatableComponent {

    public user: any;

    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService
        , route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_user',
            {}
        )
        this.http
            .post(this.controller + '.ctr/getInfomationUserLogin/',{}
        ).subscribe(resp => {
            this.user= resp
            },
                error => {
                    console.log(error);
                });
        
    }
}
