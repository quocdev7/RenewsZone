import { Component, ViewEncapsulation, OnInit, Inject } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient, HttpResponse, HttpEventType } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseCardComponent } from '@fuse/components/card';
import { popupInfoEditComponent } from 'app/modules/portal/profile/popupInfoEdit.component';

import { popupAddCertificateComponent } from 'app/modules/portal/profile/popupAddCertificate.component';

import { popupAddExperienceComponent } from 'app/modules/portal/profile/popupAddExperience.component';
import { popupAddSuccessComponent } from 'app/modules/portal/profile/popupAddSuccess.component';
import { popupAddWorkHistoryComponent } from 'app/modules/portal/profile/popupAddWorkHistory.component';
import { popupSocialEditComponent } from 'app/modules/portal/profile/popupSocialEdit.component';
import { popupMainImageComponent } from 'app/modules/portal/profile/popupMainImage.component';
import { popupAvatarComponent } from 'app/modules/portal/profile/popupAvatar.component';
import { popupAddEducationComponent } from 'app/modules/portal/profile/popupAddEducation.component';
import Swal from 'sweetalert2';
import { DOCUMENT, Location } from '@angular/common';
import { AuthService } from '../../../core/auth/auth.service';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { portal_homepage_popupShareComponent } from '../homepage/popupShare.component';
import { param } from 'jquery';
import * as AOS from 'aos';
import { FuseNavigationService } from '@fuse/components/navigation';
@Component({
    selector     : 'portal-profile-user',
    templateUrl: './index.component.html',
    encapsulation: ViewEncapsulation.None
})
export class PortalProfileUserComponent implements OnInit
{   private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public currentUser: any=null;
    public quyen_loi: any = {};
    public user_id: any = "";
    
    public list_user_education: any = [];
    public list_user_success: any = [];
    public list_user_certificate: any = [];
    public list_user_work_history: any = [];
    public list_user_experience: any = [];
    public lst_users_manager:any=[];
    public lst_news_manager:any=[];
    public lst_event_manager:any=[];
    
    public is_show_hide_education: any = true;
    public is_show_hide_success: any = true;
    public is_show_hide_certificate: any = true;
    public is_show_hide_work_history: any = true;
    public is_show_hide_experience: any = true;
    public is_show_hide_social: any = true;


    public lst_events:any=[];
    
    constructor(private http: HttpClient, dialog: MatDialog,
    private router: Router, private route: ActivatedRoute,
    
        private _fuseMediaWatcherService: FuseMediaWatcherService,
         _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService
        , @Inject('BASE_URL') baseUrl: string
        ) {
     
    }
    loadUser(): void {
             this.http
                    .post('/sys_user.ctr/getAnotherUserInfo/', {
                        "user_id":this.user_id
                    }).subscribe(resp => {
                    this.currentUser =resp;
                    this.currentUser.show_ket_noi_voi_toi = 
                    (this.currentUser.db.website_link!=null && this.currentUser.db.website_link!='') ||
                    (this.currentUser.db.linkedin_link!=null && this.currentUser.db.linkedin_link!='') ||
                    (this.currentUser.db.facebook_link!=null && this.currentUser.db.facebook_link!='') ||
                    (this.currentUser.db.twitter_link!=null && this.currentUser.db.twitter_link!='') ||
                    (this.currentUser.db.youtube_link!=null && this.currentUser.db.youtube_link!='') ||
                    (this.currentUser.db.instagram_link!=null && this.currentUser.db.instagram_link!='') 
                    ;
                    
                    this.list_user_success =this.currentUser.list_user_success;
                    this.list_user_education =this.currentUser.list_user_education;
                    this.list_user_certificate =this.currentUser.list_user_certificate;
                    this.list_user_work_history =this.currentUser.list_user_work_history;
                    this.list_user_experience =this.currentUser.list_user_experience;
             });
          
        }
      
        showHideEducation(): void {
            this.is_show_hide_education = !this.is_show_hide_education;
        }
        
        showHideSuccess(): void {
            this.is_show_hide_success = !this.is_show_hide_success;
        }
        showHideCertificate(): void {
            this.is_show_hide_certificate = !this.is_show_hide_certificate;
    
        }
        showHideWorkHistory(): void {
            this.is_show_hide_work_history = !this.is_show_hide_work_history;
        }
        showHideExperience(): void {
            this.is_show_hide_experience = !this.is_show_hide_experience;
        }
        showHideSocial(): void {
            this.is_show_hide_social = !this.is_show_hide_social;
        }



    ngOnInit(): void {
        AOS.init({
            duration:1000
        });
     this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
            this.route.params.subscribe(params => {
                this.user_id = params["id"];
                 
                this.loadUser();
            });
            
    }

    
  
}
