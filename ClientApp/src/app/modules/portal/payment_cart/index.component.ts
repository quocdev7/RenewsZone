import { Component, ViewEncapsulation, OnInit, Inject } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse, HttpEventType } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseCardComponent } from '@fuse/components/card';
import { popupInfoEditComponent } from 'app/modules/portal/profile/popupInfoEdit.component';

import { popupAddCertificateComponent } from 'app/modules/portal/profile/popupAddCertificate.component';

import { popupAddExperienceComponent } from 'app/modules/portal/profile/popupAddExperience.component';
import { popupAddSuccessComponent } from 'app/modules/portal/profile/popupAddSuccess.component';
import { popupAddWorkHistoryComponent } from 'app/modules/portal/profile/popupAddWorkHistory.component';
import { popupSocialEditComponent } from 'app/modules/portal/profile/popupSocialEdit.component';
import { popupMainImageComponent } from 'app/modules/portal/profile/popupMainImage.component';
import { popupAvatarComponent } from 'app/modules/portal/profile/popupAvatar.component';
import { popupAddEducationComponent } from 'app/modules/portal/profile/popupAddEducation.component';
import Swal from 'sweetalert2';
import { DOCUMENT, Location } from '@angular/common';
import { AuthService } from '../../../core/auth/auth.service';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
import { param } from 'jquery';
import * as AOS from 'aos';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ShoppingCardsService } from 'app/layout/common/products-card/products-card.service';
@Component({
    selector: 'portal-payment-cart',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PaymentCartComponent implements OnInit {
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public errorModel: any
    public loading: any = false;
    public list_product_card: any = [];
    public record: any;
    public gioi_tinh: any;
    public lst_tinh_thanh: any;
    public lst_quan_huyen: any;
    public lst_quan_huyen_user: any;
    public lst_quan_huyen_cong_ty: any;
    public list_phuong_thuc_thanh_toan: any;
    srcCaptcha: any;
    constructor(private route: ActivatedRoute,
        public http: HttpClient,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        public _translocoService: TranslocoService
        , private router: Router
        , private _shoppingCardsService: ShoppingCardsService
        , _fuseNavigationService: FuseNavigationService
        , @Inject('BASE_URL') baseUrl: string
    ) {
        this.record = {
            db: {
                gioi_tinh: 1,
                full_name: "",
                email: "",
                phone: "",
                dia_chi: "",
                phuong_thuc_thanh_toan: 1,
                note_phuong_thuc: "",
                ten_cong_ty: "",
                ma_so_thue: "",
                tinh_thanh: "",
                quan_huyen: "",
                tinh_thanh_cong_ty: "",
                quan_huyen_cong_ty: "",
                dia_chi_cong_ty: "",
                id_tai_khoan_ngan_hang: "",
                thanh_tien_van_chuyen: 0,
                thanh_tien_thu_ho: 0,
                thanh_tien: 0,
                thanh_tien_giam_gia: 0,
            },
            list_product_card: []
        }
        this.get_list_gioi_tinh();
        this.get_list_quan_huyen();
        this.get_list_tinh_thanh();
        this.set_phuong_thuc_thanh_toan();
        this.get_list_phuong_thuc_thanh_toan();
    }
    check_show_khuyen_mai(index: number) {
        this.record.list_product_card[index].is_showKhuyenMai = !this.record.list_product_card[index].is_showKhuyenMai;
    }
    change_thanh_tien() {
        this.record.db.thanh_tien = 0;
        this.record.list_product_card.forEach(element => {
            this.record.db.thanh_tien += element.db.so_tien * element.so_luong
        });
    }
    reloadCaptcha(): void {
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;

    }
    add_san_pham_to_card(
        item: any,
        type: number
    ) {
        if (item.so_luong == null || item.so_luong == undefined)
            item.so_luong = 0;
        let so_luong = item.so_luong;

        if (type == 1) {
            item.so_luong = 1;
        }
        if (type == 2) {
            item.so_luong = -1;
        }
        item.so_luong = so_luong;
        return item;
    }
    delete(product: any): void {
        this.record.list_product_card = this.record.list_product_card.filter(
            (q) =>
                q.db.id != product.db.id
        );
        this.setlocalStorage();
        this._shoppingCardsService.create(null)
        this.change_thanh_tien();
    }
    plus_or_minus_product(index: number, type: number): void {
        let product = this.record.list_product_card[index];
        debugger
        if (product.so_luong == null || product.so_luong == undefined)
            product.so_luong = 0;
        if (type == 1) {
            product.so_luong += 1;
        }
        if (type == 2 && product.so_luong > 1) {
            product.so_luong -= 1;
        }
        product.so_tien = product.db.so_tien * product.so_luong
        if (product.so_luong != 0) {
            this.record.list_product_card[index].so_tien = product.so_tien;
            this.record.list_product_card[index].so_luong = product.so_luong;
        }
        this.setlocalStorage();
        this.change_thanh_tien();
    }
    setlocalStorage() {
        localStorage.setItem(
            'list_product_card',
            JSON.stringify(this.record.list_product_card)
        );
    }
    resetlocalStorage() {
        localStorage.removeItem('list_product_card');
    }
    getlocalStorage() {
        return JSON.parse(localStorage.getItem('list_product_card'));
    }

    get_list_tinh_thanh() {
        this.http.post('/sys_tinh_thanh.ctr/getListUse/', {}).subscribe(resp => {
            this.lst_tinh_thanh = resp;
        });
    }
    get_list_quan_huyen() {
        this.http.post('/sys_quan_huyen.ctr/getListUse/', {}).subscribe(resp => {
            this.lst_quan_huyen = resp;
        });
    }
    change_list_quan_huyen_user() {
        debugger
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
    SendUp(): void {
        this.http.post('sys_dat_hang.ctr/create_portal/', { data: this.record }).subscribe((resp) => {
            debugger
            this.resetlocalStorage()
            this._shoppingCardsService.create(null)
            this.record.list_product_card = []
            var data = resp as any
            const url = 'send-cart/' + data.db.id;
            this.router.navigateByUrl(url);
            Swal.fire('Đã gửi thành công, chúng tôi sẽ phản hồi lại sau', '', 'success').then(res => { });
        }, (error) => {
            if (error.status == 400) {
                this.errorModel = error.error;
            }
        });
    }
    goBuyProduct(): void {
        const url = '/portal_product';
        this.router.navigateByUrl(url);

    }

    ngOnInit(): void {
        var list = this.getlocalStorage();
        if (list != null && list != undefined)
            this.record.list_product_card = list;
        this.change_thanh_tien();
        var d = new Date();
        var n = d.getTime();
        this.srcCaptcha = '/Captcha/GetCaptchaImage?' + n;
        AOS.init({
            duration: 1000
        });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => { });
    }
}
