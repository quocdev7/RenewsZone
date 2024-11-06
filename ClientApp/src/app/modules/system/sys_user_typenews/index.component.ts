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

import { sys_user_typenews_popUpAddComponent } from './popupAdd.component';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'sys_user_typenews_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_user_typenews_indexComponent extends BaseIndexDatatableComponent
{
    public list_status_del: any;
    public list_type_news: any = [];
    public item_chose: any;
    public user_id: any;
    public errorModel: any;
    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService,route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_user_typenews',
            { search: "", id_type_news:"-1" }
        )
        
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
        this.http.post("/sys_type_news.ctr/getListUse/", {}).subscribe((resp) => {
            this.list_type_news = resp;

            this.list_type_news.splice(0, 0, {
                    id: "-1",
                    name: this._translocoService.translate('common.all')
                })
        });
    }
   
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }
    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Phân quyền duyệt tin tức - Xelex'); 
    }
    bind_data_item_chose(): void {
        this.user_id = this.item_chose.id;

    }

    before_filter(): void {
      
        //if (this.filter.id_type_news !== undefined && this.filter.id_type_news !== "") {

        //        var checkArr = Array.isArray(this.filter.id_type_news); // true
        //        if (checkArr) {
        //            this.filter.id_type_news = this.filter.id_type_news.join(',');
        //        } else {

        //        }
               
        //    }
        

    }
 openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_user_typenews_popUpAddComponent, {
            disableClose: true,
                 width: '800px',
            data: {
                actionEnum: 1,
                db: {
                    id: 0,
                }
            },
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result.db.id == 0) return;
            this.rerender();
        });
    }
    openDialogEdit(model, pos): void {
    model.actionEnum = 2;
        const dialogRef = this.dialog.open(sys_user_typenews_popUpAddComponent, {
            disableClose: true,
           
            width: '800px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_user_typenews_popUpAddComponent, {
            disableClose: true,
               width: '800px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
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
            if (this.listData.filter(d => d.db.id_user == this.user_id).length > 0) {
                error += this._translocoService.translate('existed');
                valid = false;
            }
        }



        if (!valid) {
            this.showMessagewarning2("", error);
            return;
        } else {
            this.http
                .post('/sys_user_typenews.ctr/add_nguoi_dung/', {
                    user_id: this.user_id,
                    id_type_news: this.filter.id_type_news
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

}


