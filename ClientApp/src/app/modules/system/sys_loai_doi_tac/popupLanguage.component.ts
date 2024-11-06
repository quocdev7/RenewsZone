import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
    selector: 'popupLanguage',
    templateUrl: 'popupLanguage.component.html',
})
export class sys_loai_doi_tac_popUpLanguageComponent extends BasePopUpAddComponent {
    public group_news: any;
    public type_news: any;
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
    constructor(public dialogRef: MatDialogRef<sys_loai_doi_tac_popUpLanguageComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_loai_doi_tac', dialogRef, dialogModal);
        debugger
        this.record = data;

        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
    }


    save_language(): void {
        debugger
        this.beforesave();
        this.loading = true;
        this.http.post(this.controller + '.ctr/edit_language/',
            {
                data: this.record,
            }
        ).subscribe(resp => {
            this.record = resp;
            this.Oldrecord = this.record;
            this.basedialogRef.close(this.record);
            Swal.fire('Lưu thành công', '', 'success');
            this.aftersave();
        },
            error => {
                if (error.status == 400) {
                    this.errorModel = error.error;
                    this.aftersavefail();
                }
                if (error.status == 403) {
                    this.basedialogRef.close();
                    Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                }
                this.loading = false;

            });
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
