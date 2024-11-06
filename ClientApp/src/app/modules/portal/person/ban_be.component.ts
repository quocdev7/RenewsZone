import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute, Router } from '@angular/router';
import { translateDataTable } from '../../../../@fuse/components/commonComponent/VietNameseDataTable';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import * as AOS from 'aos';
import { person_popupAddBanBeComponent } from './popupAddBanBe.component';
@Component({
    selector: 'person_ban_be',
    templateUrl: './ban_be.component.html',
    styleUrls: ['./ban_be.component.scss']
})

export class person_ban_beComponent extends BaseIndexDatatableComponent
{
   
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public lst_ban_be:any=[];
    public lst_ban_be_filter:any=[];
    public badges:any=[];
    public stauts_filter:any=1;
    public lst_badges_ban_be:any=[];
    public search_ban_be:any="";
    constructor(http: HttpClient, dialog: MatDialog,
    private router: Router,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
         _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
        ) {
        super(http, baseUrl, _translocoService, _fuseNavigationService, route, dialog, 'sys_news',
            { search: "", status_del: "-1", id_group_news: "-1", id_type_news: "-1", id_phamvi: "1", tu_ngay: null, den_ngay: null, quyen_rieng_tu: "", is_hot: false }
        )
    }
    loadUser(): void {

        this.http
        .post('/sys_user.ctr/getBanbeInfo/', {}
        ).subscribe(resp => {
        
                        this.badges =resp;
                        this.lst_badges_ban_be =[
                            {   
                                id:1,
                                name: this._translocoService.translate("portal.ban_be"),
                                number:this.badges["ban_be"]
                            },
                            {  
                                id:2,
                                name: this._translocoService.translate("portal.loi_moi_ket_ban"),
                                number:this.badges["loi_moi_ket_ban"]
                            },
                            { 
                                id:3,
                                name: this._translocoService.translate("portal.da_gui_loi_moi"),
                                number:this.badges["da_gui_loi_moi"]
                            },
                        ]    
                        


                        this.http
                        .post('/sys_user.ctr/get_list_ban_be/', {
    
                        }
                 ).subscribe(resp => {
                           this.lst_ban_be=   resp;  
                           this.filterBanBe();
                                
                 });
                        
                        
                    
        });

           
            
          
        }
        removeAccents(str) {
            var AccentsMap = [
                "aàảãáạăằẳẵắặâầẩẫấậ",
                "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
                "dđ", "DĐ",
                "eèẻẽéẹêềểễếệ",
                "EÈẺẼÉẸÊỀỂỄẾỆ",
                "iìỉĩíị",
                "IÌỈĨÍỊ",
                "oòỏõóọôồổỗốộơờởỡớợ",
                "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
                "uùủũúụưừửữứự",
                "UÙỦŨÚỤƯỪỬỮỨỰ",
                "yỳỷỹýỵ",
                "YỲỶỸÝỴ"
            ];
            for (var i = 0; i < AccentsMap.length; i++) {
                var re = new RegExp('[' + AccentsMap[i].substr(1) + ']', 'g');
                var char = AccentsMap[i][0];
                str = str.replace(re, char);
            }
        
            return str;
        }
        openDialogBanBe(): void {
            const dialogRef = this.dialog.open(person_popupAddBanBeComponent, {
                width: '768px',
                disableClose: true,
                data: this.search_ban_be
            });
            dialogRef.afterClosed().subscribe(result => {
                if(result ==true )  this.loadUser();
            });
        }
    dong_y_ket_ban(id1, name): void {
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
                 
                   this.loadUser(); 
                 
                 });
            }else{
            
            }
        });

     
           
            
          
        }
        tu_choi_ket_ban(id1, name): void {
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
                   
                       this.loadUser(); 
                     
                     });
                }else{
                 
                }
            });
    
         
               
                
              
            }
    filterBanBe(){
        this.lst_ban_be_filter = this.lst_ban_be.filter(it=>this.stauts_filter==it.status_del).filter(it =>
            this.removeAccents(it.full_name.toLowerCase()).indexOf(this.removeAccents(this.search_ban_be.toLowerCase())) != -1);
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
            this.loadUser();
    }
  
}


