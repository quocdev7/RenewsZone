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
    selector: 'portal_event_popupViewImg',
    templateUrl: 'popupViewImg.html',
})
export class portal_event_popupViewImgComponent  {
    public record:any=null;
  
    thumbsSwiper: any;
    title:any="";
    public lst_hinh_anh: any = [];
    constructor(public dialogRef: MatDialogRef<portal_event_popupViewImgComponent>,
        public clipboard: Clipboard,
        public http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {

        this.record = data;
        this.title =this.record.name;
        this.loadAnhNoiBat();
        
     
        //   this.http
        //     .post('/sys_event.ctr/getListHinhAnhNew/', {
        //             id:data.id_su_kien
        //         }
        //     ).subscribe(resp => {
        //         this.lst_hinh_anh = resp;
        //     });
    }
   
    loadAnhNoiBat(): void {
        this.http
            .post('/sys_anh_noi_bat_su_kien.ctr/getListHinhAnhNew/', {
                id:this.record.id
            }
            ).subscribe(resp => {
                this.lst_hinh_anh = resp;
            });
    }
    
     close(): void {
            this.dialogRef.close();
    }

}
