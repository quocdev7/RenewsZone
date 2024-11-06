import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Clipboard } from "@angular/cdk/clipboard";
import { SwiperComponent } from "swiper/angular";

// import Swiper core and required modules
import SwiperCore, { Lazy, Pagination, Navigation } from "swiper";

// install Swiper modules
SwiperCore.use([Lazy, Pagination, Navigation]);


@Component({
    selector: 'portal_homepage_popupViewImg',
    templateUrl: 'popupViewImg.html',
})
export class portal_homepage_popupViewImgComponent  {
  
    thumbsSwiper: any;
    title:any="";
    public lst_hinh_anh: any = [];
    constructor(public dialogRef: MatDialogRef<portal_homepage_popupViewImgComponent>,
        public clipboard: Clipboard, 
        public http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {

        this.title =data.title;
          this.http
            .post('/sys_thu_vien_hinh_anh.ctr/getListUse/', {
                    id:data.id_group
                }
            ).subscribe(resp => {
                this.lst_hinh_anh = resp;
            });
    }
    
    close(): void {
            this.dialogRef.close();
    }

}
