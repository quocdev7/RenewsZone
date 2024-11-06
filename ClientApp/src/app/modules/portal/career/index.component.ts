import { Component, Inject, ViewChild } from '@angular/core';


import { HttpClient, HttpResponse } from '@angular/common/http';

import { DataTablesResponse } from 'app/Basecomponent/datatable';
import { TranslocoService } from '@ngneat/transloco';
import { MatDialog } from '@angular/material/dialog';
import { DataTableDirective } from 'angular-datatables';
import { Subject } from 'rxjs';
import { BaseIndexDatatableComponent } from 'app/Basecomponent/BaseIndexDatatable.component';
import Swal from 'sweetalert2';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ActivatedRoute } from '@angular/router';
/*import { changePassComponent } from './changePass.component';*/


@Component({
    selector: 'portal_career_index',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.scss']
})

export class portal_career_indexComponent  {
    public file: any;
    public list_status: any;
    public listtype: any;
    constructor(http: HttpClient, dialog: MatDialog
        , _translocoService: TranslocoService
        , _fuseNavigationService: FuseNavigationService, route: ActivatedRoute
        , @Inject('BASE_URL') baseUrl: string
    ) {
        
    }

    career = [
        {
            "id": 1,
            "img": "https://i.ibb.co/zJ5W7L5/logo-ws.png",
            "name": "Worldsoft Corporation",
            "adress":"Hồ Chí Minh",
            "title": "Có 5 vị trí đang tuyển dụng"
          },
          {
            "id": 2,
            "img": "https://i.ibb.co/qxzvmGM/logo-xl.png",
            "name": "Xelex Corporation",
            "adress":"Hồ Chí Minh",
            "title": "Có 1 vị trí đang tuyển dụng"
          },
          {
            "id": 3,
            "img": "https://i.ibb.co/G2fqkbL/logo-bk.png",
            "name": "Trường ĐH Bách Khoa",
            "adress":"Hồ Chí Minh",
            "title": "Có 13 vị trí đang tuyển dụng"
          },
          {
            "id": 4,
            "img": "https://i.ibb.co/zJ5W7L5/logo-ws.png",
            "name": "Worldsoft Corporation",
            "adress":"Hồ Chí Minh",
            "title": "Có 2 vị trí đang tuyển dụng"
          },
          {
            "id": 5,
            "img": "https://i.ibb.co/qxzvmGM/logo-xl.png",
            "name": "Xelex Corporation",
            "adress":"Hồ Chí Minh",
            "title": "Có 20 vị trí đang tuyển dụng"
          },
      ];
    ngOnInit(): void {
    }


}


