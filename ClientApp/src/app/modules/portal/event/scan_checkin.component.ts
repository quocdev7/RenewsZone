import { Component, Inject, OnInit,ViewEncapsulation,ViewChild } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';

import { FuseCardComponent } from '@fuse/components/card';
import { AuthService } from '../../../core/auth/auth.service';
import { debug } from 'console';
import { Location } from '@angular/common';
import { DOCUMENT } from '@angular/common';


import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';

import Swal from 'sweetalert2';
import { getDate, setDate } from 'date-fns';


import { portal_event_popup_userComponent } from './popup_user.component';
import { Title } from '@angular/platform-browser';



@Component({
  selector: 'scan_checkin',
  templateUrl: 'scan_checkin.component.html',
})

export class scan_checkinComponent implements OnInit
{
    /**
     * Constructor
     */
   
   private _unsubscribeAll: Subject<any> = new Subject<any>();
   public isScreenSmall: any = false;
    public lst_tat_ca: any = [];
    public lst_events: any = [];
    public id_event: any;
    public event: any = {};
    public event_id: any;
    public search: any;
    public errorModel: any;
    public ngay_hien_tai: any;
    
    public lst_su_kien: any;
    public eventinfo: any;
    public userinfo: any;
    public loading:any;
      public is_login: any = false;
    constructor(
  @Inject(DOCUMENT) private document: Document,
  private _fuseMediaWatcherService: FuseMediaWatcherService,
        private location: Location,
       private dialog: MatDialog,
       private titleService : Title,
        private router: Router, private route: ActivatedRoute,
        private _authService: AuthService,
        public http: HttpClient
        , public _translocoService: TranslocoService)
    {
        this.errorModel = [];
        this.search="";
        this.ngay_hien_tai= new Date();
    }

    setDocTitle(title: string) {
        this.titleService.setTitle(title);
     }
    
    ngOnInit() {
      this._authService.check().subscribe((data: any) => this.is_login = data);

     
     this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => {
            this.id_event = params["id"];
            this.loadSuKien();
        });
        this.setDocTitle('Xelex - ' + this._translocoService.translate('portal.checkin'))
   }
    gotoNewsEvenPage(id_event): void {
        this.http
        .post('/sys_event.ctr/get_title_event/', {
            id_event: id_event
        }).subscribe(resp => {
            var title_event = resp;
            const url = '/sys_post.ctr/event_detail?tieu_de=' + title_event + "&id=" + id_event + "&t=" + "2";
            window.location.href = url
        });
        // const url = '/eventDetail/' + id_event;
        // this.router.navigateByUrl(url);
    }
    loadSuKien(): void {
        this.http
            .post('/sys_scan_checkin.ctr/getEvents/', {
                id_event: this.id_event
            }
            ).subscribe(resp => {
              
                 this.event = resp;
                console.log(this.event);
            });
    }
    checkinpage(): void {

        this.loading = true;

        this.http
        .post('/sys_scan_checkin.ctr/phoneemail/', {
            id: this.id_event,
            ph:this.search
                    }
                ).subscribe(resp => {
                    var data: any;
                    data = resp;
                    this.eventinfo = data.eventinfo;
                    this.userinfo = data.userinfo;
                    const url = '/user_view/' + this.eventinfo.id +"/" + this.userinfo.id;
                    this.router.navigateByUrl(url);
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;
                           
                        }
                        if (error.status == 403) {
                         
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    }
                );
     

    }
    checkin(): void {

        this.loading = true;

        this.http
        .post('/sys_scan_checkin.ctr/phoneemail/', {
            id: this.id_event,
            ph:this.search
                    }
                ).subscribe(resp => {
                    var data: any;
                    data = resp;
                    this.openDialogUser(data);
                },
                    error => {
                        if (error.status == 400) {
                            this.errorModel = error.error;
                           
                        }
                        if (error.status == 403) {
                         
                            Swal.fire(this._translocoService.translate('no_permission'), "", "warning");
                        }
                        this.loading = false;

                    }
                );
     

    }
        openDialogUser(model): void {
      
            const dialogRef = this.dialog.open(portal_event_popup_userComponent, {
                disableClose: true,
                width: '768px',
                data: model
            });
            dialogRef.afterClosed().subscribe(result => {
                if (result != undefined && result!=null) 
                this.route.params.subscribe(params => {
                    this.id_event = params["id"];
                    this.loadSuKien();
                });
                
            });
        }
  
    thamgia(record): void {
        if (this.is_login == false) {
            const url = '/sign-in' 
        this.router.navigateByUrl(url);
        }
        else {
            Swal.fire({
                title: this._translocoService.translate('portal.paticipate_event'),
                text: this._translocoService.translate('areYouSure'),
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                        .post('/sys_event.ctr/update_status_event/', {
                            event_id: record.db.id,
                            status: 3
                        }
                        ).subscribe(resp => {
                            record.check_in_status = 3;
                            Swal.fire("Đăng ký tham gia thành công", '', 'success');
                        });
                }
            })
        }
       

    }
  
    
   
}



