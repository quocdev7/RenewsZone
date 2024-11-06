import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';

import Swal from 'sweetalert2';
@Component({
    selector: 'portal_person_event_popUpAdd',
    templateUrl: 'popupAdd.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class portal_person_event_popUpAddComponent extends BasePopUpAddComponent {
    public file_logo: any;
    public Progress_logo: any = -1;
    public group_field: any;
    public dtOptions: any;
    public list_type: any;
    public list_mau_email: any;
    public list_mau_email_moi: any;
    public list_mau_email_camon: any;
    public list_trangthai_dangky: any;
     public list_hinh_thuc:any;
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

    
    public lst_tiente:any=[];
    public lst_quyen_rieng_tu:any=[];
    public list_khoa:any=[];
    constructor(public dialogRef: MatDialogRef<portal_person_event_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_person_event', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.record.hinhthuc=['-1'];
            this.record.khoa=['-1']
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
        this.dtOptions = {
            language: {
                zeroRecords: "",
                infoEmpty: "",
                info: "",
                search: this._translocoService.translate('search') 
            },
            ordering: false,
            "paging": false,
        }
        this.http
            .post('/sys_template_mail.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.list_mau_email = resp;
                this.list_mau_email_camon = this.list_mau_email.filter(d => d.type_code == "11");
                this.list_mau_email_moi = this.list_mau_email.filter(d => d.type_code == "9");
            });
      this.list_trangthai_dangky = [
               
                { id: 1, name: this._translocoService.translate('system.cho_phep_dang_ky') },
                { id: 2, name: this._translocoService.translate('system.khong_cho_phep_dang_ky') },
         ]
        this.load_hinh_thuc();
        this.load_tien_te();
        this.load_trang_thai_dang_ky();
        this.load_khoa();
    }
    load_tien_te(): void {
        this.http
            .post('/sys_tien_te.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.lst_tiente = resp;
            });
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
    load_trang_thai_dang_ky() {

        this.lst_quyen_rieng_tu = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Công khai" },
            { id: 4, name: "Người dùng chưa là thành viên" },
            { id: 2, name: "Thành viên" },
            { id: 3, name: "Bạn bè" },
          
           // { id: 5, name: "Trả phí" }
        ]
    }
    load_hinh_thuc() {
        this.list_type = [
            //{ id: '-1', name: this._translocoService.translate('system.all') },
            { id: '1', name: this._translocoService.translate('system.offline') },
            { id: '2', name: this._translocoService.translate('system.online') },
            { id: '3', name: this._translocoService.translate('system.hoc_bong') },
            { id: '4', name: this._translocoService.translate('system.tai_tro') }
        ]
    }
    bindthumoi() {
        var thumoi = this.list_mau_email.filter(d => d.id == this.record.db.id_template_invite)[0];
        this.record.tieu_de_mau_thu_moi = thumoi.tieu_de;
        this.record.noi_dung_mau_thu_moi = thumoi.noi_dung;
    }
    bindthucamon() {
        var thumoi = this.list_mau_email.filter(d => d.id == this.record.db.id_template_thanks)[0];
        this.record.tieu_de_thu_cam_on = thumoi.tieu_de;
        this.record.noi_dung_thu_cam_on = thumoi.noi_dung;
    }
    chose_file_logo(fileInput: any) {

        this.file_logo = fileInput.target.files;
        this.submitFile();
        fileInput.target.value = null;
    }
    DragAndDrop_logo(files: any) {

        this.file_logo = files;
        this.submitFile();
    }
    submitFile() {
        var formData = new FormData();

        this.Progress_logo = 0;
        for (var i = 0; i < this.file_logo.length; i++) {
            formData.append('list_file[]', this.file_logo[i]);
        }
        formData.append('list_file[]', this.file_logo);
        this.http.post('FileManager/uploadimage', formData, {
            reportProgress: true,
            observe: 'events'
        })
            .subscribe(res => {
                if (res.type == HttpEventType.UploadProgress) {

                    this.Progress_logo = Math.round((res.loaded / res.total) * 100);


                } else if (res.type === HttpEventType.Response) {
                    var item: any;
                    item = res.body;

                    this.record.db.logo = item.location;
                    this.file_logo = null
                    this.Progress_logo = -1;



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
            if (result.value) { this.basedialogRef.close(); }
                 
          })
  
      }
}
