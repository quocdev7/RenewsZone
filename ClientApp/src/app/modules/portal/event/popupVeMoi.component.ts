import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';

import { AvailableLangs, TranslocoService } from '@ngneat/transloco';

import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'popupVeMoi',
    templateUrl: 'popupVeMoi.component.html',
})
export class popupVeMoiComponent extends BasePopUpAddComponent {
   
    public user_info: any;
    
    public activeLang: any;
    constructor(public dialogRef: MatDialogRef<popupVeMoiComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_event', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));


        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this.actionEnum = data.actionEnum;
        this.http
            .post('/sys_user.ctr/getUserInfo', {

            }
            ).subscribe(resp => {
                this.user_info = resp;
            });

        if (this.actionEnum != 2) {
            this.dialogRef.keydownEvents().subscribe(event => {
                if (event.key === "Escape") {
                    //this.onCancel();
                    this.dialogRef.close();
                }
            });
        }
        
    }
   
}
