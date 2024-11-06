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
import { Title } from '@angular/platform-browser';


@Component({
    selector     : 'portal-linh-vuc-detail',
    templateUrl: './linh_vuc_detail.component.html',
     styleUrls: ['./linh_vuc_detail.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class linh_vuc_detailComponent implements OnInit
{
    /**
     * Constructor
     */
    public id_linh_vuc: any;
    public news: any={};
    public lst_comment: any=[];
    public linh_vuc: any;

    public expandableCard02: any = {
    };
    public id_group_news:any=[];
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public is_login: any = false;
    public lst_type_news: any = [];
    public user:any=[];
    public status_del: any;
    
    public activeLang: any;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _authService: AuthService,
        private location: Location,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, public dialog: MatDialog
        , public _translocoService: TranslocoService,
        private activatedRoute: ActivatedRoute,
        private titleService: Title,
    )
    {
        this.expandableCard02.expanded = true;
    }

    goback(): void {

        const url = '/portal-type-news/' + this.news.id_type_news ;
        this.location.back();


    }
    load_linh_vuc(): void {
        this.http
            .post('/sys_linh_vuc.ctr/get_linh_vuc/', {
                id_linh_vuc: this.id_linh_vuc
            }
            ).subscribe(resp => {
                this.linh_vuc = resp;
                this.setDocTitle(this.linh_vuc.db.name + ' - Xelex');           
            });
    }

    goto_linh_vuc_page(id_linh_vuc): void {

        const url = '/portal-linh-vuc-detail/' + id_linh_vuc;
        this.router.navigateByUrl(url);

        //this.router.navigate(["/portal-news-detail'", { id: id_news_type } ]);

    }
    opendialog: any = false;  
    setDocTitle(title: string) {
        //console.log('current title:::::' + this.titleService.getTitle());
        this.titleService.setTitle(title);
     }
  
    ngOnInit() {
        AOS.init({
            duration:1000
        });


        this._translocoService.langChanges$.subscribe((activeLang) => {
            //en
            this.activeLang = activeLang;
        });
        this._authService.check().subscribe((data: any) => this.is_login = data);

        this._authService.getUser().subscribe((data: any) => {
            if (data != undefined)  this.user = data;
        });     
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                // Check if the screen is small
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => {
            this.id_linh_vuc = params["id"];
            this.load_linh_vuc();
            //this.setDocTitle(this._translocoService.translate(linh_vuc.db.name));
        });
       
       }
}
