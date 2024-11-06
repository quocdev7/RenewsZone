import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Upload } from 'tus-js-client';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { Clipboard } from "@angular/cdk/clipboard";

// import Swiper core and required modules
import SwiperCore, { Lazy, Pagination, Navigation } from "swiper";

// install Swiper modules
SwiperCore.use([Lazy, Pagination, Navigation]);

@Component({
    selector: 'portal_nang_luc_popupViewImg',
    templateUrl: 'popupViewImg.component.html',
    styleUrls: ['./popupViewImg.component.scss'],
})

export class portal_nang_luc_popupViewImgComponent extends BasePopUpAddComponent {

    constructor(public dialogRef: MatDialogRef<portal_nang_luc_popupViewImgComponent>,
        public http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_nang_luc_hinh_anh', dialogRef, dialogModal);
        this.record = data;
        console.log("load data:", this.record);
        this.Oldrecord = JSON.parse(JSON.stringify(data));
    }
}
