import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
    selector: 'sys_event_thong_bao_dang_kyComponent',
    templateUrl: 'thongbaodangky.html',
    styleUrls: ['./thongbaodangky.component.scss']
})
export class sys_event_thong_bao_dang_kyComponent {
   
    loading = false;
    showAlert: boolean = false;
    list_khoa: any;
    actionEnum: any = 1;
  
    list_school_year: any;
    list_status_graduate: any = [];
    public errorModel: any;

    public listcountry: any;

    public list_congty: any;
    public trang_thais: any;
    public list_trang_thai: any;
 
    public event_id: any;
    public isUser: any = false;
    public event: any;
    public record: any;
    fileData: any;
    previewUrl: any = null;
    fileUploadProgress: any = -1;
    uploadedFilePath: string = null;
    constructor(

        private http: HttpClient,
        private dialog: MatDialog,
        private route: ActivatedRoute,
        private router: Router,
        private _translocoService: TranslocoService,
        private _fuseNavigationService: FuseNavigationService)
       
       {
     
  
       
        this.trang_thais = [
            //{ id: '-1', name: "Tất cả" },
            { id: 1, name: "Ông" },
            { id: 2, name: "Bà" }
        ]
        this.list_trang_thai = this.trang_thais;


        this.record = {
            email: "",
            sex: "",
            lastName: "",
            day_of_birth: "",
            dienthoai: "",
            event_id: "",
        };
        this.loading = true;
        this.route.params.subscribe(params => {


            this.event_id = params['event_id'];
            this.loading = true;
            this.record.event_id = this.event_id;
            this.loadThongTinCongTy();
        });
    }
    gobackhomepage() {
        const url = '/homepage-index';

        this.router.navigateByUrl(url);
    }
    srcCaptcha: any;
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

    }
    public loadThongTinCongTy(): void {
        this.http
            .post('/sys_event.ctr/getElementById/'
                , {
                    id: this.event_id
                }
            ).subscribe(resp => {
                this.event = resp;
                this.loading = false;
            });
        this.http
            .post('sys_event_participate.ctr/register_event/',

                this.record,

            ).subscribe(resp => {
                this.errorModel = [];

                Swal.fire('Lưu thành công', '', 'success')
            },
                error => {
                    if (error.status == 400) {
                        this.errorModel = error.error;

                    }
                }
            );

    }

    emailchange(): void {
        this.loading = true;
        this.http
            .post('sys_event_participate.ctr/checkemail_register/',
                this.record,
            ).subscribe(resp => {
                this.isUser = true;
                this.record.lastName = resp["FullName"];
                this.loading = false;
            },
                error => {
                    if (error.status == 400) {
                        this.record.lastName = "";
                        this.isUser = false;
                        this.loading = false;
                    }
                }
            );
    }
    public showMessagewarningN(title, msg): void {
        Swal.fire({
            title: title,
            text: msg,
            icon: 'warning',
            confirmButtonColor: '#3085d6',
            confirmButtonText: this._translocoService.translate('close'),
        }).then((result) => {

        })

    }
    register(): void {
        //this.loading = true;
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
                    .post('sys_event_participate.ctr/register_event/',

                        this.record,

                    ).subscribe(resp => {
                        this.errorModel = [];


                        Swal.fire({
                            title: this._translocoService.translate('system.dangkythanhcong'),
                            text: "",
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: this._translocoService.translate('close'),
                        }).then((result) => {
                            this.router.navigate(['sys_event_thong_bao_dang_ky']);
                        })

                    },
                        error => {
                            if (error.status == 400) {
                                this.errorModel = error.error;
                                this.showMessagewarningN(this._translocoService.translate("notvalid"), this._translocoService.translate("pleasecheck"));
                            }
                            //this.loading = false;
                        }
                    );
            }
        })
    }
}
