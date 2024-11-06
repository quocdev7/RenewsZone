import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Clipboard } from "@angular/cdk/clipboard";

@Component({
    selector: 'portal_homepage_popupShare',
    templateUrl: 'popupShare.html',
})
export class portal_homepage_popupShareComponent  {
    public copied: any = false;
    public link: any="";
    public shares: any = [];
    constructor(public dialogRef: MatDialogRef<portal_homepage_popupShareComponent>,
        public clipboard: Clipboard,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
      
        this.link = data.link;
        this.shares = [
            {
                "link": "https://www.facebook.com/sharer/sharer.php?u=" + encodeURI(this.link),
                "image": "/assets/images/portal/facebook.png",
            },
            {
                "link": "https://www.linkedin.com/sharing/share-offsite/?url=" + encodeURI(this.link),
                "image": "/assets/images/portal/linkedin.png",
            },
            {
                "link": "https://twitter.com/intent/tweet?url=" + encodeURI(this.link) + "&text="  ,
                "image": "/assets/images/portal/twitter.png",
            }
        ]
    }

    copy(): void {
        this.clipboard.copy(this.link);
        this.copied = true;
    }
     close(): void {
            this.dialogRef.close();
    }

}
