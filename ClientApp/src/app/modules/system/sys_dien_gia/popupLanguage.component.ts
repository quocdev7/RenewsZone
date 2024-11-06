import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
    selector: 'sys_dien_gia_popupLanguage',
    templateUrl: 'popupLanguage.component.html',
    styleUrls: ['./popupAdd.component.scss']
})
export class sys_dien_gia_popUpLanguageComponent extends BasePopUpAddComponent {
    public file_logo: any;
    public Progress_logo: any = -1;
    public group_field: any;
    constructor(public dialogRef: MatDialogRef<sys_dien_gia_popUpLanguageComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_dien_gia', dialogRef, dialogModal);

        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        if (this.actionEnum == 1) {
            this.baseInitData();
        }
        this.http
            .post('/sys_field.ctr/getListUse/', {}
            ).subscribe(resp => {
                this.group_field = resp;
            });
    }
    
   
    save_en(): void {
        this.beforesave();
        this.loading = true;
        this.http
        .post(this.controller + '.ctr/edit_en/',
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
}
