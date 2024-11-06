import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';

import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { Title } from '@angular/platform-browser';
@Component({
    selector: 'portal_quy_dinh_doi_tra_hoan_tien',
    templateUrl: './quy_dinh_doi_tra_hoan_tien.component.html',
    encapsulation: ViewEncapsulation.None
}) 
export class PortalQuyDinhDoiTraHoanTienComponent implements OnInit
{
    /**
     * Constructor
     */
     
     public activeLang: any;
    public record: any = {};
    constructor(
        private titleService: Title,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient, dialog: MatDialog
        , public _translocoService: TranslocoService,) {

    }
    loadThongTin(): void {
        this.http
            .post('/sys_cau_hinh_thong_tin.ctr/getCauHinhThongTin/', {
                loai: 10
            }
            ).subscribe(resp => {
                this.record = resp;
            });
    }
    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }
    ngOnInit() {


        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this.setDocTitle(this._translocoService.translate('common.quy_dinh_doi_tra_hoan_tien') + ' - Xelex')
        this.loadThongTin();
    }
}