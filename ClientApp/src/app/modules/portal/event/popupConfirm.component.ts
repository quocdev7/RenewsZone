import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
    selector: 'popupConfirm',
    templateUrl: 'popupConfirm.component.html',
})
export class popupConfirmComponent{
   
    public record: any;
    
    public activeLang: any;
    constructor(public dialogRef: MatDialogRef<popupConfirmComponent>,
        
        public http: HttpClient,private _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        this.record = data;   
        
        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
    }
    confirm(name){
        Swal.fire({
            title:  'Bạn có chắc tham gia sự kiện: ' +name + " ?",
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
                    .post('/sys_event.ctr/register_event/', {
                        event_id: this.record.db.id,
                    }
                ).subscribe(resp => {
                    this.record.cho_phep_dang_ky = 2;
                        Swal.fire("Đăng ký tham gia thành công", '', 'success');
                        this.http
                            .post('/sys_event.ctr/getDetailEvent/', {
                                id:  this.record.db.id,
                            }
                        ).subscribe(resp => {
                            this.record.cho_phep_dang_ky = 2;
                            Swal.fire("Đăng ký tham gia thành công", '', 'success');
                            this.dialogRef.close(this.record);
                        });
                    });
            }
        })
        
    }
    close(){
        this.dialogRef.close(this.record);
    }

    ngOnInit() {

       
        

        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
    }
}
