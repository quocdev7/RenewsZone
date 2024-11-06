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

import { Title } from '@angular/platform-browser';
    
  
@Component({
  selector: 'user_view',
  templateUrl: 'user_view.component.html',
})

export class user_viewComponent implements OnInit
{
  
   private _unsubscribeAll: Subject<any> = new Subject<any>();
   public isScreenSmall: any = false;
    public lst_tat_ca: any = [];
    public lst_events: any = [];
    public eventinfo: any;
    public userinfo: any;
    public event: any;
    public infoevent: any;
    public id_event: any;
    public user_id: any;
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

    }

    loadSuKien(): void {
      this.http
        .post('/sys_scan_checkin.ctr/userview/', {
          id: this.id_event,
          user_id: this.user_id
                    }
                ).subscribe(resp => {
                    var data: any;
                    data = resp;
                    this.eventinfo = data.eventinfo;
                    this.userinfo = data.userinfo;
                  
                },
                );
  }
  setDocTitle(title: string) {
    this.titleService.setTitle(title);
 }

    ngOnInit() {
     this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
      
            this.route.params.subscribe(params => {
              this.id_event = params["id_event"];
              this.user_id = params["user_id"];
              this.loadSuKien();
          });
          this.setDocTitle('Xelex - ' + this._translocoService.translate('portal.checkin'))
   }
    
  
    
   
}



