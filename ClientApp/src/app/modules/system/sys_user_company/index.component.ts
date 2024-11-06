import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';

import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';


@Component({
    selector: 'sys_user_company_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_user_company_indexComponent extends BaseIndexDatatableComponent
{
   
    public list_company: any = [];
    public user: any = [];
    public item_chose: any;
    public user_id: any;
    public errorModel: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_user_company',
            { search: "", id_company: "" }
        )
        
     
        this.http.post("/sys_company.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_company = resp;
        });
    }

    bind_data_item_chose(): void {
        this.user_id = this.item_chose.id;

    }
  
    //checkUser(): void {
    //    this.http.post("/sys_user.ctr/getUserByCompany/", {

    //        search: this.filter.search,
    //        id_company: this.filter.id_company ?? "",

    //    }).subscribe((resp) => {
    //        this.user = resp;
    //    });
    //}
    addDetail(): void {
        var valid = true;
        var error = '';

        if (this.filter.id_company === "" || this.filter.id_company === null) {
            error += this._translocoService.translate('system.phaichoncongty');
            valid = false;
        }



        if (this.user_id == null || this.user_id == undefined) {
            error += this._translocoService.translate('system.phaichonnhanvien');
            valid = false;
        } else {
            if (this.listData.filter(d => d.db.user_id == this.user_id).length > 0) {
                error += this._translocoService.translate('existed');
                valid = false;
            }
        }



        if (!valid) {
            this.showMessagewarning2("", error);
            return;
        } else {
            this.http
                .post('/sys_user_company.ctr/add_nhan_vien/', {
                    user_id: this.user_id,
                    id_company: this.filter.id_company
                }
                )

                .subscribe(resp => {
                    this.errorModel = [];
                    this.rerender();
                    Swal.fire('Lưu thành công', '', 'success')
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;

                        }
                        //this.loading = false;
                    }
                );
        }
    }
    /******/

    ngOnInit(): void {
        this.baseInitData();
    }


}


