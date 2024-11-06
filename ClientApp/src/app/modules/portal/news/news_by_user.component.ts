import { Component, OnInit,ViewEncapsulation } from '@angular/core';

import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
@Component({
    selector: 'portal-news_by_user',
    templateUrl: './news_by_user.component.html',
    encapsulation: ViewEncapsulation.None
})
export class PortalNewsByUserComponent implements OnInit
{
    /**
     * Constructor
     */
    public id_user: any;

    public lst_news_by_user: any;
    public user_info: any;
    constructor(

        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, dialog: MatDialog
        , public _translocoService: TranslocoService,
        private activatedRoute: ActivatedRoute
    )
    {

      
    }
    gotoProfile(): void {

        const url = '/portal-profile';
        this.router.navigateByUrl(url);


    }
    loadNewsByUser(): void {
        debugger
        this.http
            .post('/sys_news.ctr/getNewsByUser/', {
                id_user: this.id_user
            }
            ).subscribe(resp => {
                var data: any = resp;

                this.lst_news_by_user = data.data;
                this.user_info = data.user;
            });
    }
    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id_user = params["id"];
            console.log(params.id);
        });
        this.loadNewsByUser()
       }
}
