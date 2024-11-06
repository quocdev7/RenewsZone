import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { sys_loai_doi_tac_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { sys_loai_doi_tac_popUpLanguageComponent } from './popupLanguage.component';
import { Title } from '@angular/platform-browser';


@Component({
    selector: 'sys_loai_doi_tac_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_loai_doi_tac_indexComponent extends BaseIndexDatatableComponent
{

    public list_status_del: any;
    public model_ngon_ngu: any;

    constructor(http: HttpClient,private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog,'sys_loai_doi_tac',
            { search: "", status_del: "1" }
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
    }
    //revertStatus(model, pos): void {
    //    model.db.status_del = 1;
    //    this.http
    //        .post(this.controller + '.ctr/edit/',
    //            {
    //                data: model,
    //            }
    //    ).subscribe(resp => {
    //        this.rerender();
    //        },
    //            error => {
    //                console.log(error);

    //            });
    //    this.rerender();
    //}
    openDialogEditlanguage(model, pos): void {
        debugger
        this.http
        .post('/sys_loai_doi_tac.ctr/load_ngon_ngu/', {
            id_loai_doi_tac:model.db.id
        }
        ).subscribe(resp => {
          this.model_ngon_ngu = resp;

          model.actionEnum = 2;
          const dialogRef = this.dialog.open(sys_loai_doi_tac_popUpLanguageComponent, {
              disableClose: true,
              width: '768px',
              data: this.model_ngon_ngu
          });
          dialogRef.afterClosed().subscribe(result => {
              if (result != undefined && result!=null)
              this.rerender();
              //this.listData[pos] = result;
          });
          
        });
 
      
        }
    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_loai_doi_tac_popUpAddComponent, {
            disableClose: true,
            width: '768px',
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
        const dialogRef = this.dialog.open(sys_loai_doi_tac_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
    model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_loai_doi_tac_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result!=null) this.listData[pos] = result;
        });
    }

    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }

    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Loại đối tác - Xelex');
    }


}


