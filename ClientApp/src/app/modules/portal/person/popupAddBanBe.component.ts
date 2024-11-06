import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';


import { HttpClient, HttpEventType } from '@angular/common/http';
import { BasePopUpAddComponent } from 'app/Basecomponent/BasePopupAdd.component';
import { TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';

@Component({
    selector: 'person_popupAddBanBe',
    templateUrl: 'popupAddBanBe.html',
})
export class person_popupAddBanBeComponent implements OnInit {
    public total_page:any=0;
    public total_item:any=0;
    public isScreenSmall: any = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public reload:any=false;
    public lst_ban_be:any=[];
    public loading:any=false;
    public filter:any=  { search_key: "",id_khoa:"-1",nien_khoa:"-1"}
    public page:any=0;
    public pageList:any=[];
    public list_khoa:any=[];
    public list_school_year:any=[];

    constructor(public dialogRef: MatDialogRef<person_popupAddBanBeComponent>,
        private router: Router,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private http: HttpClient,public _translocoService: TranslocoService,
        _fuseNavigationService: FuseNavigationService, route: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string,
        public dialogModal: MatDialog,
        @Inject(MAT_DIALOG_DATA) data: any) {
        this.filter.search_key =data;
        this.search(0);
    }
    close(): void {
        this.dialogRef.close(this.reload);
    }
    
    ngOnInit(): void {
        this._fuseMediaWatcherService.onMediaChange$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe(({ matchingAliases }) => {
            // Check if the screen is small
            this.isScreenSmall = !matchingAliases.includes('md');
        });
        this.load_khoa();
    }
  
    ketBan(user_id,name,pos): void {
      this.loading=true;
        Swal.fire({
            title: this._translocoService.translate('portal.ban_ket_ban_voi')+' ' + name +" ?",
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                .post('/sys_user.ctr/invite_user/', {
                    type:"1",
                    email:"",
                    id: user_id,
                }).subscribe(resp => {
                    this.reload =true;
                    this.loading=false;
                    this.lst_ban_be[pos].id_invite = resp;
                    this.lst_ban_be[pos].status_del =3;
                 });
            }else{
                this.loading=false;
            }
        });


           
    }

    dong_y_ket_ban(id1, name,pos): void {
        this.loading=true;
        Swal.fire({
            title: this._translocoService.translate('portal.ban_ket_ban_voi')+' ' + name +" ?",
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: this._translocoService.translate('yes'),
            cancelButtonText: this._translocoService.translate('no')
        }).then((result) => {
            if (result.value) {
                this.http
                .post('/sys_user.ctr/action_invite/', {
                    id: id1,
                    action: 1
                }).subscribe(resp => {
                 
                    this.lst_ban_be[pos].status_del =1;
                    this.reload =true;
                    this.loading=false;
                 });
            }else{
                this.loading=false;
            }
        });

     
           
            
          
        }
        tu_choi_ket_ban(id1, name,pos): void {
            this.loading=true;
            Swal.fire({
                title: this._translocoService.translate('portal.ban_tu_choi_ket_ban')+' ' + name +" ?",
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http
                    .post('/sys_user.ctr/action_invite/', {
                        id: id1,
                        action: 2
                    }).subscribe(resp => {
                   
                        this.lst_ban_be[pos].status_del =0;
                        this.reload =true;
                        this.loading=false;
                     });
                }else{
                    this.loading=false;
                }
            });
        }

    openProfile(id): void {
        this.router.navigate(["/portal-profile-user/"+id, { } ]);
        this.close();

    }

  

   
        beginSearch(){
            this.search(0);
        }
        load_khoa(): void {
            var all= {id:"-1" , name:this._translocoService.translate("common.all")};
            this.http
                .post('/sys_khoa.ctr/getListUse/', {}
                ).subscribe(resp => {
                    this.list_khoa = resp;
                  
                    this.list_khoa.splice(0, 0,all );
                });
                this.list_school_year = [];
                var yearCurrent = new Date().getFullYear();
                for (var i = 1970; i < yearCurrent; i++) {
                    this.list_school_year.push({
                        id: i,
                        name: i.toString(),
                    });

                }  
                this.list_school_year.splice(0, 0,all );
        }
     
    generate_page(){
        this.pageList=[];
        for(var i=1;i<this.total_page-1;i++){
             this.pageList.push(i);
        }
   }

    transform(value: any,type:string): string {
      
        if(!this.filter.search_key) return value;
        if(type==='full'){
          const re = new RegExp("\\b("+ this.filter.search_key.toLowerCase()+"\\b)", 'igm');
          value= value.replace(re, '<span class="highlighted-text">$1</span>');
        }
        else{
          const re = new RegExp(this.filter.search_key.toLowerCase(), 'igm');
          value= value.replace(re, '<span class="highlighted-text">$&</span>');
        }
          return value;
      }
  
    search(page): void {
        if(this.total_page!=0 && (this.page >= this.total_page || this.page < 0 )){
                return
        }
        this.page=page;
        this.lst_ban_be =[];
        this.http.post('/sys_user.ctr/search_common/', {
                                         "filter":this.filter,
                                         "page":this.page
                                        }).subscribe(resp => {
                                            var a:any =resp;
                                            this.lst_ban_be = a.lst_ban_be;
                                            this.total_page=a.total_page;
                                            this.total_item=a.total_item;
                                            this.generate_page()
                                        });

   
       
    }
     
}
