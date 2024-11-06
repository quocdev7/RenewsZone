import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';

@Component({
    selector: 'sys_person_news_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class portal_person_news_popUpAddComponent extends BasePopUpAddComponent {
    public file_image: any;
    public Progress_image: any = -1;
    public group_news: any;
    public type_news: any ;
    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public list_khoa: any;
    public list_hinh_thuc:any;
    public lst_quyen_rieng_tu: any;
    public plugintiny = [
        "advlist autolink lists link image charmap print preview anchor",
        "searchreplace visualblocks code fullscreen",
        "insertdatetime media table paste imagetools wordcount"
    ];
    public toolbartiny = "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image";

    public timyconfig = {
        base_url: '/tinymce'
        , suffix: '.min',
        height: 500,
        images_upload_url: '/FileManager/uploadimage',
        plugins: this.plugintiny,
        toolbar: this.toolbartiny
    }
    constructor(public dialogRef: MatDialogRef<portal_person_news_popUpAddComponent>,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_news', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
       if (this.actionEnum == 1) {
            this.record.hinhthuc=['-1'];
            this.record.khoa=['-1'];
           this.baseInitData();
        }
     
        this.list_hinh_thuc = [
            {
                id: "-1",
                name: this._translocoService.translate("common.all")
            },
            {
                id: "1",
                name: this._translocoService.translate("portal.student")
            },
            {
                id: "2",
                name: this._translocoService.translate("portal.alumni")
            },
            {
                id: "3",
                name: this._translocoService.translate("portal.teacher")
            },
            {
                id: "4",
                name: this._translocoService.translate("portal.CBCNV")
            },
            {
                id: "5",
                name: this._translocoService.translate("portal.retire")
            },
        ]
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });

        this.http
            .post('/sys_group_news.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.group_news = resp;
            });
        if (this.actionEnum != 1) {
            this.changeGroupNews();
        }
        this.load_khoa();
        this.load_quyen_rieng_tu();
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
    load_quyen_rieng_tu(): void {

        this.lst_quyen_rieng_tu = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Công khai" },
            // { id: 4, name: "Người dùng chưa là thành viên" },
            // { id: 2, name: "Thành viên" },
            // { id: 3, name: "Bạn bè" },
           // { id: 4, name: "Khoa" },
            //{ id: 5, name: "Trả phí" }
        ]
    }
    changeGroupNews(): void {
        this.http
            .post('/sys_type_news.ctr/getListUseByGroup/', {
                id: this.record.db.id_group_news
                }
            ).subscribe(resp => {
                this.type_news = resp;
            });
    }
    chose_file_image(fileInput: any) {

        this.file_image = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDrop_image(files: any) {

        this.file_image = files;
        this.submitFile();
    }
    submitFile() {
        var formData = new FormData();

        this.Progress_image = 0;
        for (var i = 0; i < this.file_image.length; i++) {
            formData.append('list_file[]', this.file_image[i]);
        }
        formData.append('list_file[]', this.file_image);
        this.http.post('FileManager/uploadimage', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_image = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.db.image = item.location;
                    this.file_image = null
                    this.Progress_image = -1;



                }

            })
    }
    close_create() {
        Swal.fire({
                  title: this._translocoService.translate('areYouSure'),
                  text: "",
                  icon: 'warning',
                  showCancelButton: true,
                  confirmButtonColor: '#3085d6',
                  cancelButtonColor: '#d33',
                  confirmButtonText: this._translocoService.translate('yes'),
                  cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) { 
                if (this.actionEnum == 3) {
                    this.basedialogRef.close(this.record);
                } else {
                    this.basedialogRef.close(this.Oldrecord);
                }
     
             }
                 
          })
  
      }
}
