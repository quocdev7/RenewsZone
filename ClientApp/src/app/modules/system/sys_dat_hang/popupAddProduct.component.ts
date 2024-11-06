import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient } from '@angular/common/http';

import { TranslocoService } from '@ngneat/transloco';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { BasePopupDatatabbleComponent } from 'app/Basecomponent/BasePopupDatatabble.component';


@Component({
    selector: 'sys_dat_hang_popupAdd',
    templateUrl: 'popupAddProduct.html',
})
export class sys_dat_hang_popUpAddProductComponent extends BasePopupDatatabbleComponent {

    public list_loai_san_pham: any;
    public list_status_del: any;
    public list_san_pham: any;
    public check_all_product: any = false;
    constructor(public dialogRef: MatDialogRef<sys_dat_hang_popUpAddProductComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_dat_hang', dialogRef, dialogModal, { search: "", status_del: "1", id_loai: "-1" });
        this.record = data;
        if (this.record.list_detail == undefined) {
            this.record.list_detail = []
        }
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.actionEnum = data.actionEnum;
        this.getListTypeProduct();
        this.getListStatusdel();
        this.getListProduct();
    }
    close(): void {
        debugger
        let list_check = this.list_san_pham.filter(q => q.is_check == true)
        for (let index = 0; index < list_check.length; index++) {
            const element = list_check[index];
            let obj = {
                id_san_pham: element.id,
                khuyen_mai: "",
                ten_san_pham: element.name,
                ma_san_pham: element.ma_san_pham,
                mo_ta: element.mo_ta,
                don_gia: element.so_tien,
                so_tien: element.so_tien,
                so_luong: 1
            }
            var check = this.record.list_detail.findIndex(q => q.id_san_pham == element.id)
            if (check == -1) {
                this.record.list_detail.push(obj)
            }
        }
        this.dialogRef.close(this.record.list_detail)
    }
    set_product(): boolean {
        if (this.list_san_pham == null) {
            return false;
        }
        return this.list_san_pham.filter(t => t.is_check).length > 0 && !this.check_all_product;
    }
    set_all_product(completed: boolean) {
        this.check_all_product = completed;
        if (this.list_san_pham == null) {
            return;
        }
        this.list_san_pham.forEach(t => t.is_check = completed);
    }
    update_all_product() {
        this.check_all_product = this.list_san_pham != null && this.list_san_pham.every(t => t.is_check);
    }

    getListProduct() {
        this.http.post('/sys_san_pham.ctr/getListUse', {}).subscribe(resp => {
            this.list_san_pham = resp;
            debugger
            for (let index = 0; index < this.record.list_detail.length; index++) {
                const element = this.record.list_detail[index];
                var index_product = this.list_san_pham.findIndex(q => q.id == element.id_san_pham)
                if (index_product != -1) {
                    this.list_san_pham[index_product].is_check = true
                }
            }
        });
    }
    getListTypeProduct() {
        this.http.post('/sys_loai_san_pham.ctr/getListUse', {}).subscribe(resp => {
            this.list_loai_san_pham = resp;
            this.list_loai_san_pham.splice(0, 0, { id: '-1', name: this._translocoService.translate('system.all') })
        });
    }
    getListStatusdel() {
        this.list_status_del = [
            {
                id: "1",
                name: this._translocoService.translate('system.use')
            },
            {
                id: "2",
                name: this._translocoService.translate('system.not_use')
            }
        ];
    }
}
