import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { sys_dat_hang_popUpAddProductComponent } from './popupAddProduct.component';


@Component({
    selector: 'sys_dat_hang_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_dat_hang_popUpAddComponent extends BasePopUpAddComponent {
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

    public gioi_tinh: any;
    public lst_tinh_thanh: any;
    public lst_quan_huyen: any;
    public lst_quan_huyen_user: any;
    public lst_quan_huyen_cong_ty: any;
    public list_phuong_thuc_thanh_toan: any;
    constructor(public dialogRef: MatDialogRef<sys_dat_hang_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_dat_hang', dialogRef, dialogModal);
        this.record = data;

        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.get_list_quan_huyen();
        this.get_list_tinh_thanh();
        this.get_list_gioi_tinh();
        this.set_phuong_thuc_thanh_toan();
        this.get_list_phuong_thuc_thanh_toan();
        if (this.actionEnum == 1) {
            this.record.db.gioi_tinh = 1
            this.record.db.phuong_thuc_thanh_toan = 1
            this.baseInitData();
        } else {
        }
    }
    openDialogAddProduct() {
        const dialogRef = this.dialogModal.open(sys_dat_hang_popUpAddProductComponent, {
            disableClose: true,
            width: '768px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                },
                list_detail: this.record.list_detail
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            this.record.list_detail = result
        });
    }
    delete_row(pos) {
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
                if (this.record.list_detail.length == 1)
                    Swal.fire('Không được dưới 1 dòng!', '', 'warning')
                else {
                    this.record.list_detail.splice(pos, 1);
                }
            }
        })
    }
    get_list_phuong_thuc_thanh_toan() {
        this.list_phuong_thuc_thanh_toan = [
            {
                id: 1,
                name: this._translocoService.translate('system.tien_mat')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.chuyen_khoan')
            }
        ];
    }
    set_phuong_thuc_thanh_toan() {
        if (this.record.db.phuong_thuc_thanh_toan == 1) {
            this.record.db.note_phuong_thuc = "Thanh toán tiền khi nhận được hàng. Quý khách ở ngoại tỉnh sẽ chịu thêm 1% giá trị đơn hàng từ chi phí thu hộ. Quý khách ở Hà Nội được miễn phí.";

        } else {
            this.record.db.note_phuong_thuc = "Chuyển khoản Quý khách chuyển khoản trước theo thông tin dưới đây:";
        }
    };
    get_list_tinh_thanh() {
        this.http.post('/sys_tinh_thanh.ctr/getListUse/', {}).subscribe(resp => {
            this.lst_tinh_thanh = resp;
        });
    }
    get_list_quan_huyen() {
        this.http.post('/sys_quan_huyen.ctr/getListUse/', {}).subscribe(resp => {
            this.lst_quan_huyen = resp;
            if (this.actionEnum != 1) {
                this.change_list_quan_huyen_cong_ty()
                this.change_list_quan_huyen_user()
            }
        });
    }
    change_list_quan_huyen_user() {

        this.lst_quan_huyen_user = this.lst_quan_huyen.filter(q => q.id_tinh == this.record.db.tinh_thanh)
    }
    change_list_quan_huyen_cong_ty() {
        this.lst_quan_huyen_cong_ty = this.lst_quan_huyen.filter(q => q.id_tinh == this.record.db.tinh_thanh_cong_ty)
    }
    get_list_gioi_tinh() {
        this.gioi_tinh = [
            {
                id: 1,
                name: this._translocoService.translate('system.nam')
            },
            {
                id: 2,
                name: this._translocoService.translate('system.nu')
            }
        ];
    }
}
