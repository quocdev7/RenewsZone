import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_user_typenews_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_user_typenews_popUpAddComponent extends BasePopUpAddComponent {
     public list_type_news: any = [];
     public list_khoa: any = [];
     
    public item_chose: any;

    constructor(public dialogRef: MatDialogRef<sys_user_typenews_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_user_typenews', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }

        if (this.actionEnum == 2) {
            
             // this.record.db.id_type_news = (this.record.db.id_type_news ?? "").split(",");
        }


          this.http.post("/sys_type_news.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_type_news = resp;

            this.list_type_news.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
        });
        this.load_khoa();
    }
    load_khoa(): void {
        var all= {id:"-1" , name:this._translocoService.translate("common.all")};
        this.http
            .post('/sys_khoa.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_khoa = resp;
              
                this.list_khoa.splice(0, 0,all );
            });
    }
     bind_data_item_chose(): void {
        
        this.record.db.id_user = this.item_chose.id;

    }
    beforesave(): void {
     
            //if (this.record.db.id_type_news !== undefined && this.record.db.id_type_news !== null && this.record.db.id_type_news !== "") {
             //           this.record.db.id_type_news = this.record.db.id_type_news.join();

            //}
        
    }

}
