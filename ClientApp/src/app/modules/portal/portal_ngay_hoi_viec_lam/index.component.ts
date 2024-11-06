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
    selector: 'portal_ngay_hoi_viec_lam_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
}) 
export class portal_ngay_hoi_viec_lam_indexComponent implements OnInit
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
                loai: '1174d76b-11ed-48ab-8fcd-b4a248940966'
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
        this.setDocTitle(this._translocoService.translate('common.khao_sat_sinh_vien') + ' - Xelex')
        this.loadThongTin();
    }
}
