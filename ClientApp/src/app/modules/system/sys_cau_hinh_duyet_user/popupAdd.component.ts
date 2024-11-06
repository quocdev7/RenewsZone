import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_cau_hinh_duyet_user_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_cau_hinh_duyet_user_popUpAddComponent extends BasePopUpAddComponent {
    public list_hinh_thuc: any;
    public list_khoa: any;
       public item_chose: any;
   
       constructor(public dialogRef: MatDialogRef<sys_cau_hinh_duyet_user_popUpAddComponent>,
           http: HttpClient, _translocoService: TranslocoService,
           _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
           @Inject('BASE_URL') baseUrl: string,
           public dialogModal: MatDialog,
           @Inject(MAT_DIALOG_DATA) data: any) {
           super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_cau_hinh_duyet_user', dialogRef, dialogModal);
           this.record = data;
           this.Oldrecord = JSON.parse(JSON.stringify(data));
           this.actionEnum = data.actionEnum;
           if (this.actionEnum == 1) {
               this.baseInitData();
           }
   
           if (this.actionEnum == 2) {
               
                // this.record.db.id_type_news = (this.record.db.id_type_news ?? "").split(",");
           }
   
   
      
          this.load_hinh_thuc();
          this.load_khoa();
       }
     
        bind_data_item_chose(): void {
           
           this.record.db.id_user = this.item_chose.id;
   
       }
    
       load_hinh_thuc(): void {
        this.list_hinh_thuc = [
            {
                id: 1,
                name: this._translocoService.translate("portal.student")
            },
            {
                id: 2,
                name: this._translocoService.translate("portal.alumni")
            },
            {
                id: 3,
                name: this._translocoService.translate("portal.teacher")
            },
            {
                id: 4,
                name: this._translocoService.translate("portal.CBCNV")
            },
            {
                id: 5,
                name: this._translocoService.translate("portal.retire")
            },
        ]
    }
    load_khoa(): void {
        var all= {id:"-1" , name:this._translocoService.translate("common.all")};
        this.http
            .post('/sys_khoa.ctr/getListUse/', {

            }
            ).subscribe(resp => {
                this.list_khoa = resp;
              
                this.list_khoa.splice(0, 0,all );
            });
    }

}

