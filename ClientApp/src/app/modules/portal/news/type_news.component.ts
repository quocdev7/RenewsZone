import { Component, Inject, OnInit,ViewEncapsulation } from '@angular/core';

import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { Location } from '@angular/common';
import { DOCUMENT } from '@angular/common';

import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';

import * as AOS from 'aos';
import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
import { Title } from '@angular/platform-browser';
@Component({
    selector     : 'portal-type-news',
    templateUrl: './type_news.component.html',
  styleUrls: ['./type_news.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PortalTypeNewsComponent   implements OnInit
{
    /**
     * Constructor
     */
     
     public activeLang: any;
    public id_type_news: any;
    public lst_type_news: any=[];
    public lst_news: any=[];
    group_news_name: any;
    id_group_news: any;
    public type_news_name: any;
    public search_key:any="";
     public filter:any=  { search_key: "", type_info: "1" ,  id_group_news:"-1",  id_type_news:""};

    public total_page:any=0;
    public total_item:any=0;
    public page:any=0;
    public pageList:any=[];

    public type_news: any=[];
    public group_news: any=[];

   private _unsubscribeAll: Subject<any> = new Subject<any>();
   public isScreenSmall: any = false;
    constructor(
        @Inject(DOCUMENT) private document: Document,
        private location: Location,
        private router: Router, private route: ActivatedRoute,
        public http: HttpClient, private dialog: MatDialog
        ,
public _translocoService: TranslocoService,
private _fuseMediaWatcherService: FuseMediaWatcherService,
        private activatedRoute: ActivatedRoute,
        private titleService: Title,
    )
    {
            
      
     
    }
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
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });

        this.route.params.subscribe(params => {
            this.id_type_news = params["id"];
            this.filter.id_type_news= this.id_type_news ;
             this.http
            .post('/sys_type_news.ctr/getInfoByTypeNews/', {
                id: this.id_type_news
            }
            ).subscribe(resp => {
        
                var data: any = resp;
                this.type_news = data.type_news;
                this.group_news = data.group_news;
                this.id_group_news = data.group_news.id;
                this.lst_type_news =data.lst_type;
                
                this.loadNewsByTypeNews(0);
                var title =  "" ;
                if(this.activeLang == 'vi'){
                title= this.type_news.name + ' - Xelex';
                }else{
                title= this.type_news.name_en + ' - Xelex';
                }
                
                this.setDocTitle(title);
            });
        });
    }

    openShareNews(id): void {
        let url = 'https://' + this.document.location.hostname + '/portal-news-detail/' + id;
        var that = this;
        const dialogRef = this.dialog.open(portal_homepage_popupShareComponent, {
            disableClose: true,
          
            data: {
                link: url
            },
        });
        dialogRef.afterClosed().subscribe(result => { });
    }

    generate_page(){
        this.pageList=[];
        for(var i=1;i<this.total_page-1;i++){
             this.pageList.push(i);
        }
    }
    loadNewsByTypeNews(page): void {
        if(this.total_page!=0 && (this.page >= this.total_page || this.page < 0 )){
                return
        }
        this.page=page;
        this.lst_news =[];
        this.http.post('/sys_news.ctr/search_news_common/', {
                                        "filter": this.filter,
                                         "page":this.page
                                        }).subscribe(resp => {
                                            var a:any =resp;
                                            this.lst_news = a.lst_news;
                                            this.total_page=a.total_page;
                                            this.total_item=a.total_item;
                                            this.generate_page()
                                        });
       
    }



    go_back(): void {
        this.location.back();
    }
   
    gotoTypeNewsPage(id_type_news): void {
      

        const url = '/portal-type-news/' + id_type_news;
        this.router.navigateByUrl(url);
    }
    gotoNewsDetailPage(id_news): void {
        this.http
        .post('/sys_news.ctr/get_title_news/', {
            id_news: id_news
        }).subscribe(resp => {
            var title_news = resp;
            const url = '/sys_post.ctr/news_detail?tieu_de=' + title_news + "&id=" + id_news + "&t=" + "1";
            window.location.href = url
        });
        // const url = '/portal-news-detail/' + id_news;
        // this.router.navigateByUrl(url);
    }
   
}
