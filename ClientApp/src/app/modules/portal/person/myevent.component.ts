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
@Component({
    selector: 'person_myevent',
    templateUrl: './myevent.component.html',
    styleUrls: ['./myevent.component.scss']
})

export class person_myeventComponent extends BaseIndexDatatableComponent
{
   
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public currentUser: any={ db:{

        }
    };
    public events: any = {};

    public stauts_filter:any=1;
    public events_filter:any=[];
    public badges:any=[];
    
    public lst_event_manager:any=[];
    
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
                    .post('/sys_user.ctr/getEventInfo/', {}
             ).subscribe(resp => {
             
                            this.badges =resp;
                            this.lst_event_manager =[

                                { id:1,
                                    name:"Được mời",
                                number:this.badges["event_duoc_moi"]
                                },
                                { id:6, name:"Đã đăng ký",
                                number:this.badges["event_da_dang_ky"]
                            },
                                { id:3,name:"Sẽ tham gia",
                                number:this.badges["event_se_tham_gia"]
                            },
                              
                            { id:5,name:"Không đủ điều kiện tham dự",
                            number:this.badges["event_khong_du_dieu_kien_tham_du"]
                        },

                                    { id:2,name:"Từ chối tham dự",
                                    number:this.badges["event_tu_choi_tham_du"]
                                },  { id:4,name:"Đã tham dự",
                                number:this.badges["event_da_tham_du"]
                            },

                            ]    
                            
                            
                        
             });
             this.http.get('/sys_event.ctr/getMyEvent/').subscribe(resp => {
                            
                                        this.events = resp;
                                        this.filterEvent()
                            });
          
        }
        filterEvent(){
            this.events_filter = this.events.filter(it=>this.stauts_filter==it.check_in_status);
        }


        thamgia(id,name){
            Swal.fire({
                title:    'Bạn có chắc tham gia sự kiện: ' +name + " ?" ,
                text: "",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: this._translocoService.translate('yes'),
                cancelButtonText: this._translocoService.translate('no')
            }).then((result) => {
                if (result.value) {
                    this.http.post("/sys_event_khach_moi.ctr/thamgia/", {
                        id:id

                    }).subscribe((resp) => {
                        this.loadUser();
                    });
                }else{
                   
                }
            });
            
        }

        tuchoi(id,name){
            Swal.fire({
                title: "Lý do không tham dự sự kiện",
                input: 'text',
                inputAttributes: {
                    autocapitalize: 'off'
                },
                showCancelButton: true,
                cancelButtonText: this._translocoService.translate('close'),
                confirmButtonText: this._translocoService.translate('common.confirm'),
                showLoaderOnConfirm: true,
                inputValidator: (value) => {
                    if (!value) {
                        return this._translocoService.translate('common.must_input_reason')
                    }
                },
                allowOutsideClick: () => false,
            }).then((result) => {
                if (result.value) {
                                this.http.post("/sys_event_khach_moi.ctr/tuchoi/", {
                                    id:id,
                                    ly_do:result.value,
                                }).subscribe((resp) => {
                                    this.loadUser();
                                });

                            }
                
                })
        
           
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
}


