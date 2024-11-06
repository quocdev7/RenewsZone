import { Component, ViewEncapsulation, OnInit, Inject } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse, HttpEventType } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseCardComponent } from '@fuse/components/card';
import { popupInfoEditComponent } from './popupInfoEdit.component';

import { popupAddCertificateComponent } from './popupAddCertificate.component';
import { popupAddUngTuyenComponent } from './popupAddUngTuyen.component';

import { popupAddExperienceComponent } from './popupAddExperience.component';
import { popupAddSuccessComponent } from './popupAddSuccess.component';
import { popupAddWorkHistoryComponent } from './popupAddWorkHistory.component';
import { popupSocialEditComponent } from './popupSocialEdit.component';
import { popupMainImageComponent } from './popupMainImage.component';
import { popupAvatarComponent } from './popupAvatar.component';
import { popupAddEducationComponent } from './popupAddEducation.component';
import Swal from 'sweetalert2';
import { DOCUMENT, Location } from '@angular/common';
import { AuthService } from '../../../core/auth/auth.service';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
//import { param } from 'jquery';

@Component({
    selector     : 'portal-profile',
    templateUrl: './profile.component.html',
    encapsulation: ViewEncapsulation.None
})
export class PortalProfileComponent implements OnInit
{
    /**
     * Constructor
     */
    public  record: any = {
        db: {
            FirstName: "",
            LastName: "",
            email: "",
            phone: "",
            id_khoa: "",
            school_year: 0,
            status_graduate: 0,
            avatar_path:""

        }
    }
    public is_ket_noi: any;
    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public thong_tin_ung_tuyen: any;
    public lst_quyen: any;
    public file_cv: any;
    public file: any;
    public list_company: any = [];

    public list_user_education: any = [];
    public list_user_success: any = [];
    public list_user_certificate: any = [];
    public list_user_work_history: any = [];
    public list_user_experience: any = [];
    public user_cv: any;
    public is_show_hide_education: any = true;
    public is_show_hide_success: any = true;
    public is_show_hide_certificate: any = true;
    public is_show_hide_work_history: any = true;
    public is_show_hide_experience: any = true;
    public is_show_hide_social: any = true;
    selectedFile: any = null;

    public file_image: any;
    public Progress_image: any = -1;
    listNews: any;
    list_type_news: any;
    public loading: boolean = false;
    public is_check: any;
    constructor(

        @Inject(DOCUMENT) private document: Document,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, public dialog: MatDialog
        , public _translocoService: TranslocoService, public dialogModal: MatDialog) {
        //this.loadNews();
        //this.loadDanhMuc();

        this.lst_quyen = [
            {
                id: 1,
                name: this._translocoService.translate("system.cong_khai")
            },
            {
                id: 2,
                name: this._translocoService.translate("system.ban_be")
            },
            {
                id: 3,
                name: this._translocoService.translate("system.chi_minh_toi")
            },

        ]
    
        this.load_quyen_rieng_tu();
        this.is_check = 1;
        this.loading = true;
    }
  
      
    change_quyen_rieng_tu(is_check): void {
        if (is_check == 1 && this.loading == true) {
            this.is_check = 0;
            this.loading = true;
            return;
        } 
        if (is_check == 0 && this.loading == false) {
            this.loading = false;
        }
    }

     
    save_quyen_rieng_tu(): void {
        this.loading = true;
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

                this.http.post("/sys_user.ctr/createQuyenRiengTu", {
                    data: this.record.cau_hinh_quyen_rieng_tu
                }).subscribe((resp) => {
                    this.load_quyen_rieng_tu();
                    Swal.fire('Lưu thành công', '', 'success');
                })
            }
        })
    }
    load_quyen_rieng_tu(): void {
        this.http.post("/sys_user.ctr/getQuyenRiengTu", {
        }).subscribe((resp) => {
            this.loading = true;
            this.is_check = 1
            this.record.cau_hinh_quyen_rieng_tu = resp;
        })

    }
 
    guiDuyetHoSo(id): void {
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
                this.http
                    .post('sys_user.ctr/guiDuyetHoSo/',
                        {
                            id: id,
                        }
                ).subscribe(resp => {

                   

                        Swal.fire('Gửi duyệt thành công', '', 'success').then(
                            // Navigate to the confirmation required page
                            res => {
                                this.loadUser();
                            });
      
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    loadNews(): void {
        console.log("news");
        this.http.post("/sys_news.ctr/getNewsByUser", {}).subscribe((resp) => {
            this.listNews = resp;
        })
    }
    openShareNews(id): void {
        let url = 'https://' + this.document.location.hostname + '/portal-news-detail/' + id;
        var that = this;
        const dialogRef = this.dialog.open(portal_homepage_popupShareComponent, {
            disableClose: true,
           
            data: {
                link: url
            },
        });
        dialogRef.afterClosed().subscribe(result => { });
    }
    loadDanhMuc(): void {
      
        this.http.post("/sys_type_news.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_type_news = resp;
        });
    }
    onFileSelectedNew(event: any): void {
        debugger
        if(this.record.file !=null){
            Swal.fire('Tối đa 1 CV ứng tuyển', '', 'warning');
        }else{
            this.file = event.target.files[0] ?? null;
            console.log(this.file);
            if (this.file.size > 1048576 * 3) {
                Swal.fire('File không vượt quá 3MB', '', 'warning');
                return;
            }
            if (this.file.type != "image/jpeg" && this.file.type != "image/png" && this.file.type != "application/pdf" ) {
                Swal.fire('Không hỗ trợ định dạng này', '', 'warning');
                return;
            }


            this.onSubmitFile();
    
        }
       
    }
    formatSizeUnits(bytes) {
        if (bytes >= 1073741824) { bytes = (bytes / 1073741824).toFixed(2) + " GB"; }
        else if (bytes >= 1048576) { bytes = (bytes / 1048576).toFixed(2) + " MB"; }
        else if (bytes >= 1024) { bytes = (bytes / 1024).toFixed(2) + " KB"; }
        else if (bytes > 1) { bytes = bytes + " bytes"; }
        else if (bytes == 1) { bytes = bytes + " byte"; }
        else { bytes = "0 bytes"; }
        return bytes;
    }

    onSubmitFile() {

        if (this.file == null || this.file == undefined) {

            Swal.fire('Phải chọn file import', '', 'warning')

        } else {
            var formData = new FormData();

            formData.append('file', this.file);

            this.http.post('/sys_user.ctr/upload_file/', formData, {
                //reportProgress: true,
                //observe: 'events'
            })
                .subscribe(res => {
                    this.file_cv = res;
                    if (res != "") {
                        Swal.fire('Lưu thành công', '', 'success');
                        //this.record.db.cv_link = this.file_cv.file.db.file_name;
                        this.loadUser();
                    } else {
                        Swal.fire(res.toString(), '', 'warning')
                    }

                })
        }

    }
    chose_file(fileInput: any) {

        this.file = fileInput.target.files;
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
        for (var i = 0; i < this.file.length; i++) {
            formData.append('list_file[]', this.file[i]);
        }
        formData.append('list_file[]', this.file);
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

                    this.record.db.cv_link = item.location;
                    this.file = null
                    this.Progress_image = -1;
                }

            })
    }
   
    gotoNewsDetailPage(id_news): void {
        this.http
            .post('/sys_news.ctr/get_title_news/', {
                id_news: id_news
            }).subscribe(resp => {
                var title_news = resp;
                const url = '/sys_post.ctr/news_detail?tieu_de=' + title_news + "&id=" + id_news + "&t=" + "1";
                window.location.href = url
            });
        // const url = '/portal-news-detail/' + id_news;
        // this.router.navigateByUrl(url);
    }
    gotoTypeNewsDetailPage(id_type_news): void {
        const url = '/portal-type-news/' + id_type_news;
        this.router.navigateByUrl(url);
    }
    loadEducation(): void {
        this.http.post("/sys_user.ctr/getEducationUser/", {}).subscribe((resp) => {
            this.list_user_education = resp;
        });
    }
    loadCVUser(): void {
        this.http.post("/sys_user.ctr/getCVUser/", {}).subscribe((resp) => {
            this.user_cv = resp;
        });
    }
    loadSuccess(): void {
        this.http.post("/sys_user.ctr/getSuccessUser/", {}).subscribe((resp) => {
            this.list_user_success = resp;
        });
    }
    loadUngTuyen(): void {
        this.http.post("/sys_user.ctr/getUngTuyen/", {}).subscribe((resp) => {
            this.record.user_ung_tuyen = resp;
        });
    }
    loadCertificate(): void {

        this.http.post("/sys_user.ctr/getCertificateUser/", {}).subscribe((resp) => {
            this.list_user_certificate = resp;
        });
    }
    loadWorkHistory(): void {

        this.http.post("/sys_user.ctr/getWorkHistoryUser/", {}).subscribe((resp) => {
            this.list_user_work_history = resp;
        });
    }
    loadExperience(): void {
        this.http.post("/sys_user.ctr/getExperienceUser/", {}).subscribe((resp) => {
            this.list_user_experience = resp;
        });
    }
    // openCity(evt, cityName) {
    //// Declare all variables
    //var i, tabcontent, tablinks;

    //// Get all elements with class="tabcontent" and hide them
    //tabcontent = document.getElementsByClassName("tabcontent");
    //for (i = 0; i < tabcontent.length; i++) {
    //    tabcontent[i].style.display = "none";
    //}

    //// Get all elements with class="tablinks" and remove the class "active"
    //tablinks = document.getElementsByClassName("tablinks");
    //for (i = 0; i < tablinks.length; i++) {
    //    tablinks[i].className = tablinks[i].className.replace(" active", "");
    //}

    //// Show the current tab, and add an "active" class to the button that opened the tab
    //document.getElementById(cityName).style.display = "block";
    //evt.currentTarget.className += " active";
    //}
    loadUser(): void {
     this.http
            .post('/sys_user.ctr/getUserInfo/', {}
     ).subscribe(resp => {
             
                this.record = resp;
            });
    }
  

    cancelFile(id) {
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
                this.http
                    .post('sys_user.ctr/deleteFile/',
                        {
                            id: id,
                        }
                ).subscribe(resp => {
                    this.loadUser();
                        Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })
    }
    //openDialogAdd(): void {
    //    //model.actionEnum = 3;

    //    const dialogRef = this.dialogModal.open(popupAddComponent, {
    //        disableClose: true,
  
    //        //data: model
    //    });
    //    dialogRef.afterClosed().subscribe(result => {

    //    });
    //}

    showHideEducation(): void {
        this.is_show_hide_education = !this.is_show_hide_education;
    }
    
    showHideSuccess(): void {
        this.is_show_hide_success = !this.is_show_hide_success;
    }
    showHideCertificate(): void {
        this.is_show_hide_certificate = !this.is_show_hide_certificate;

    }
    showHideWorkHistory(): void {
        this.is_show_hide_work_history = !this.is_show_hide_work_history;
    }
    showHideExperience(): void {
        this.is_show_hide_experience = !this.is_show_hide_experience;
    }
    showHideSocial(): void {
        this.is_show_hide_social = !this.is_show_hide_social;
    }


    openDialogMainImage(model): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupMainImageComponent, {
            disableClose: true,
            width: '768px',

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()

        });
    }
    openDialogDetailMainImage(model): void {
        model.actionEnum = 3;
        const dialogRef = this.dialogModal.open(popupMainImageComponent, {
            disableClose: true,
            width: '768px',

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()

        });
    }
    openDialogAvatar(model): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAvatarComponent, {
            disableClose: true,
            width: '768px',

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()

        });
    }
    openDialogDetailAvatar(model): void {
        model.actionEnum = 3;
        const dialogRef = this.dialogModal.open(popupAvatarComponent, {
            disableClose: true,
            width: '768px',

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()

        });
    }

    
    openDialogEducation(): void {
       
        const dialogRef = this.dialogModal.open(popupAddEducationComponent, {
            disableClose: true,
            width: '768px',

             data: {
                actionEnum: 1,
                db: {
                    id: 0,
                }
        },
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadEducation()

        });
    }
    
  
    openDialogEditEducation(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAddEducationComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadEducation();
        });
    }
    public deleteEducation(id1): void {
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
                this.http
                    .post('sys_user.ctr/deleteEducation/',
                        {
                            id: id1,
                        }
                ).subscribe(resp => {
                    this.loadEducation();
                    Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    
    openDialogCreateUserInfo(model): void {
        model.actionEnum = 1;
        const dialogRef = this.dialogModal.open(popupInfoEditComponent, {
            disableClose: true,
            width: '768px',

            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()

        });
    }
    openDialogEditInfo(model): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupInfoEditComponent, {
            disableClose: true,
            width: '768px',
           
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()
            
        });
    }
    openDialogEditSocial(model): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupSocialEditComponent, {
            disableClose: true,
            width: '768px',
          
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            this.loadUser()
        });
    }

    
    openUngTuyen(): void {

        var data = {
            actionEnum: 1,
            db: {
                id: 0,
            }
        };
        //if (this.list_user_success.length>0) {
        //    data = this.list_user_success[0];
        //}
        const dialogRef = this.dialogModal.open(popupAddUngTuyenComponent, {
            disableClose: true,
            width: '768px',

            data: data
        });
        dialogRef.afterClosed().subscribe(result => {

            this.loadUser();

        });
    }

    openDialogEditUngTuyen(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAddUngTuyenComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadUser();
        });
    }
    openDialogDetailUngTuyen(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialogModal.open(popupAddUngTuyenComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadUser();
        });
    }

    openDialogSuccess(): void {

        var data = {
            actionEnum: 1,
            db: {
                id: 0,
            }
        };
        //if (this.list_user_success.length>0) {
        //    data = this.list_user_success[0];
        //}
        const dialogRef = this.dialogModal.open(popupAddSuccessComponent, {
            disableClose: true,
            width: '768px',
           
            data: data
        });
        dialogRef.afterClosed().subscribe(result => {
       
            this.loadSuccess();
           
        });
    }

    openDialogEditSuccess(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAddSuccessComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadSuccess();
        });
    }
    openDialogDetailSuccess(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialogModal.open(popupAddSuccessComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadSuccess();
        });
    }
    public deleteSuccess(id1): void {
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
                this.http
                    .post('sys_user.ctr/deleteSuccess/',
                        {
                            id: id1,
                        }
                ).subscribe(resp => {
                    this.loadSuccess();
                        Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    public deleteUngTuyen(id1): void {
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
                this.http
                    .post('sys_user.ctr/deleteUngTuyen/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.loadUser();
                        Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    openDialogExperience(): void {
        var data = {
            actionEnum: 1,
            db: {
                id: 0,
            }
        };
        //if (this.list_user_experience.length > 0) {
        //    data = this.list_user_experience[0];
        //}
        const dialogRef = this.dialogModal.open(popupAddExperienceComponent, {
            disableClose: true,
            width: '768px',

            data: data
        });
        dialogRef.afterClosed().subscribe(result => {

            this.loadExperience();

        });
       
    }
    openDialogEditExperience(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAddExperienceComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadExperience();
        });
    }
    public deleteExperience(id1): void {
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
                this.http
                    .post('sys_user.ctr/deleteExperience/',
                        {
                            id: id1,
                        }
                ).subscribe(resp => {
                    this.loadExperience();
                        Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }

   openDialogWorkHistory(): void {
       var data = {
           actionEnum: 1,
           db: {
               id: 0,
           }
       };
     
       const dialogRef = this.dialogModal.open(popupAddWorkHistoryComponent, {
           disableClose: true,
           width: '768px',

           data: data
       });
       dialogRef.afterClosed().subscribe(result => {

           this.loadWorkHistory();

       });
   }

    openDialogEditWorkHistory(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAddWorkHistoryComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadWorkHistory();
        });
    }
    public deleteWorkHistory(id1): void {
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
                this.http
                    .post('sys_user.ctr/deleteWorkHistory/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.loadWorkHistory();
                        Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }

    openDialogCertificate(): void {

        var data = {
            actionEnum: 1,
            db: {
               id: 0,
            }
        }
        //if (this.list_user_certificate.length > 0) {
        //    data = this.list_user_certificate[0];
        //}
        const dialogRef = this.dialogModal.open(popupAddCertificateComponent, {
            disableClose: true,
            width: '768px',

            data: data
        });
        dialogRef.afterClosed().subscribe(result => {

            this.loadCertificate();

        });
    }
    openDialogEditCertificate(model, pos): void {
        model.actionEnum = 2;
        const dialogRef = this.dialogModal.open(popupAddCertificateComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadCertificate();
        });
    }
    openDialogDetailCertificate(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialogModal.open(popupAddCertificateComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.loadCertificate();
        });
    }
    public deleteCertificate(id1): void {
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
                this.http
                    .post('sys_user.ctr/deleteCertificate/',
                        {
                            id: id1,
                        }
                    ).subscribe(resp => {
                        this.loadCertificate();
                        Swal.fire('Xóa thành công', '', 'success');
                    },
                        error => {
                            if (error.status == 403) {
                                Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                            }


                        }
                    );
            }
        })

    }
    ngOnInit(): void {
        //this.loadUngTuyen();
        this.loadUser();
        this.loadExperience();
        this.loadCertificate();
        this.loadSuccess();
        this.loadWorkHistory();
        this.loadEducation();
        this.loadCVUser();
        //this.openCity(event, 'ho_so')
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
    }
}
