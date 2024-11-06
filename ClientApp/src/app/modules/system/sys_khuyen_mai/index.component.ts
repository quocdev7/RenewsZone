import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { sys_khuyen_mai_popUpAddComponent } from './popupAdd.component';
import { MatDialog } from '@angular/material/dialog';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
    selector: 'sys_khuyen_mai_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class sys_khuyen_mai_indexComponent extends BaseIndexDatatableComponent {
    public list_status_del: any
    constructor(http: HttpClient, private titleService: Title, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_khuyen_mai',
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

    openDialogAdd(): void {
        const dialogRef = this.dialog.open(sys_khuyen_mai_popUpAddComponent, {
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
        const dialogRef = this.dialog.open(sys_khuyen_mai_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }
    openDialogDetail(model, pos): void {
        model.actionEnum = 3;
        const dialogRef = this.dialog.open(sys_khuyen_mai_popUpAddComponent, {
            disableClose: true,
            width: '768px',
            data: model
        });
        dialogRef.afterClosed().subscribe(result => {
            if (result != undefined && result != null) this.listData[pos] = result;
        });
    }

    setDocTitle(title: string) {
        this.titleService.setTitle(title);
    }

    ngOnInit(): void {
        this.baseInitData();
        this.setDocTitle('Khuyến mãi - Xelex');
    }
}


