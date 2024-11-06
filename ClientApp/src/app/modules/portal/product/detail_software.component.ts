import { Component, Inject, OnInit,ViewEncapsulation } from '@angular/core';
import { DOCUMENT, Location } from '@angular/common';

import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';

import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import * as AOS from 'aos';


import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
import Swal from 'sweetalert2';


@Component({
    selector     : 'portal_software_detail',
    templateUrl: './detail_software.component.html',
     styleUrls: ['./detail_software.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class portal_software_detailComponent implements OnInit
{
    /**
     * Constructor
     */
     public loading:any = false;
     
     public activeLang: any;
    public id_san_pham: any;
    public san_pham: any={};
    public user: any;
    
    public expandableCard02: any = {
    };
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public is_login: any = false;
    public status_del: any;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _authService: AuthService,
        private location: Location,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, public dialog: MatDialog
        , public _translocoService: TranslocoService,
        private activatedRoute: ActivatedRoute
    )
    {
        this.expandableCard02.expanded = true;
    }
 
  
    
   goto_loai_san_pham(id_loai_san_pham): void {
      

    const url = '/portal-loai-san-pham/' + id_loai_san_pham;
    this.router.navigateByUrl(url);
    }
    goback(): void {

        const url = '/portal-loai-san-pham/' + this.id_san_pham;
        this.location.back();

    }
    load_san_pham_phan_mem(): void {
        this.http
            .post('/sys_san_pham.ctr/get_san_pham/', {
                id_san_pham: this.id_san_pham
            }
            ).subscribe(resp => {
                this.san_pham = resp;
            });
     
    }

    
    opendialog: any = false;
 
    ngOnInit() {
        AOS.init({
            duration:1000
        });
        


        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        //this._authService.check().subscribe((data: any) => this.is_login = data);

        // this._authService.getUser().subscribe((data: any) => {
        //     if (data != undefined)  this.user = data;
        // });     
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => {
            this.id_san_pham = params["id"];
            this.load_san_pham_phan_mem();
        });
       
       }
}
