import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { ThrowStmt } from '@angular/compiler';


@Component({
    selector: 'portal_event_popup_user',
    templateUrl: 'popup_user.html',
})
export class portal_event_popup_userComponent extends BasePopUpAddComponent {
  
    
    public eventinfo: any;
    public userinfo: any;

    constructor(public dialogRef: MatDialogRef<portal_event_popup_userComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_scan_checkin', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.userinfo = data.userinfo;
        this.eventinfo = data.eventinfo;
       
    }
    close(){
        this.dialogRef.close(this.record);
    }
 
}
