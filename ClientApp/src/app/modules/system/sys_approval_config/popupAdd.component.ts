import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { HttpClient } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { translateDataTable } from '@fuse/components/commonComponent/VietNameseDataTable';


@Component({
    selector: 'sys_approval_config_popupAdd',
    templateUrl: 'popupAdd.html',
})
export class sys_approval_config_popUpAddComponent extends BasePopUpAddComponent {
    public additem: any;
    public item_chose: any;
    public dtOptions: any;
    public listMenu: any;
    constructor(public dialogRef: MatDialogRef<sys_approval_config_popUpAddComponent>,
        http: HttpClient, _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        super(_translocoService, _fuseNavigationService, route, baseUrl, http, 'sys_approval_config', dialogRef, dialogModal);
        this.record = data;
        this.Oldrecord = JSON.parse(JSON.stringify(data));
        this.resetAddItem();
        this.actionEnum = data.actionEnum;
        this.listMenu = [{
            id: 'inventory_delivery',
            name: this._translocoService.translate("NAV.inventory_delivery")
        },
            {
                id: 'inventory_receiving',
                name: this._translocoService.translate("NAV.inventory_receiving")
            },
            {
                id: 'business_purchase_order',
                name: this._translocoService.translate("NAV.business_purchase_order")
            },
            {
                id: 'business_sale_order',
                name: this._translocoService.translate("NAV.business_sale_order")
            }
            ,
            {
                id: 'production_order',
                name: this._translocoService.translate("NAV.production_order")
            }]

        if (this.actionEnum == 1) {
            this.baseInitData();
        }

        if (this.actionEnum != 1) {
            this.record.form_name = this._translocoService.translate("NAV." + this.record.db.menu);
            this.http
                .post('/sys_approval_config.ctr/getListItem/', {
                    id: this.record.db.id
                }
                ).subscribe(resp => {
                    this.record.list_item = resp;
                });


        }
        this.dtOptions = {
            language: translateDataTable,
            scrollY: '50vh',
            scrollCollapse: true,
            scrollX: true,
            ordering: false,
            searching: false,
            paging: false,
        }
    }
    resetAddItem(): void {
        this.additem = {
            db: {
                user_id: null,
                step_num: 0,
                name: "",
                note: "",

            },
            user_name: "",
        };

    }
    bind_data_item_chose(): void {
        this.additem.db.user_id = this.item_chose.id;
        this.additem.user_name = this.item_chose.name;
    }
    addDetail(): void {
        var valid = true;
        var error = '';

        if (this.record.list_item.filter(d => d.db.user_id == this.additem.db.user_id && d.db.step_num == this.additem.db.step_num).length > 0) {
            error += this._translocoService.translate('existed') + '<br>';
            valid = false;
        }

        if ( this.additem.db.user_id == null || this.additem.db.user_id == undefined) {
            error += this._translocoService.translate('must_chose_item') + '<br>';
            valid = false;
        }

        if (this.additem.db.step_num == 0 || this.additem.db.step_num == null || this.additem.db.step_num == '') {
            error += this._translocoService.translate('system.step_num') +" "+ this._translocoService.translate('must_chose_item') + '<br>';
            valid = false;
        }

        if (!valid) {
            this.showMessagewarning(error);
            return;
        }
        this.record.list_item.push(this.additem);
        this.record.list_item.sort(function (a, b) {
            return a.db.step_num - b.db.step_num;
        });

        this.resetAddItem();
    }
    deleteDetail(pos): void {
        this.record.list_item.splice(pos, 1);
    }
}
