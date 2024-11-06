import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';

import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
@Component({
    selector: 'sys_news_popupTinNoiBat',
    templateUrl: 'popupTinNoiBat.html',
})
export class sys_news_popUpTinNoiBatComponent extends BasePopUpAddComponent {
   
    public list_hotnews:any;
    public list_item_new: any;
    public loading: any;
    public item_news: any;
    
    constructor(public dialogRef: MatDialogRef<sys_news_popUpTinNoiBatComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_news', dialogRef, dialogModal);
        debugger
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
      
      
        this.load_hotnews(); 
    }
    load_hotnews(): void {
        this.http
            .post('/sys_news.ctr/getListTinNoiBat/', {
                    id_nhom:this.record.db.id_group_news
                }
            ).subscribe(resp => {
                this.list_hotnews = resp;
            });
    }
   onDrop(event: CdkDragDrop<string[]>) {
        moveItemInArray(this.list_hotnews, event.previousIndex, event.currentIndex);

        this.reloadList(this.list_hotnews);
       


    }
   reloadList(hotnews): void {

        this.list_item_new = [];


        for (let i = 0; i <  5 ; i++) {

            var item = hotnews[i];


            item.db.stt = i+1;
    
            var newItem = JSON.parse(JSON.stringify(item))
            this.list_item_new.push(newItem);


        }
        this.list_hotnews = this.list_item_new;

    }
    resetAddItem(): void {
        this.item_news = {
            db: {
                id:"",
                user_id: null,
                step_num: 0,
                name: "",
                note: "",
                duration_hours:null
            },
           
        };

    }
     save(): void {
      
        this.loading = true;
     
            this.http
                .post(this.controller + '.ctr/update_vi_tri_tin_noi_bat/',
                    {
                        data: this.list_hotnews,
                    }
                ).subscribe(resp => {
                   Swal.fire('Lưu thành công', '', 'success');
                    this.basedialogRef.close();
                  
                  
                },
                   
                );
     
    }
}
