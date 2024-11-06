import { Component, ViewEncapsulation, OnInit, Inject } from '@angular/core';
import { TranslocoService } from '@ngneat/transloco';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2';
import { FuseMediaWatcherService } from '../../../../@fuse/services/media-watcher';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import * as AOS from 'aos';
import { FuseNavigationService } from '@fuse/components/navigation';
import { ThousandSuffixesPipe } from '@fuse/pipes/ThousandSuffixes.pipe';
@Component({
    selector: 'portal-send-cart',
    templateUrl: './send-card.component.html',
    styleUrls: ['./send-card.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class SendCartComponent implements OnInit {
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public isScreenSmall: any = false;
    public loading: any = false;
    public errorModel: any
    public id: any
    public don_hang: any

    constructor(private route: ActivatedRoute,
        public http: HttpClient,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        public _translocoService: TranslocoService, private router: Router
        , _fuseNavigationService: FuseNavigationService

        , @Inject('BASE_URL') baseUrl: string
    ) {

    }
    getElementById() {
        this.http.post("sys_dat_hang.ctr/getElementById?id=" + this.id, {}).subscribe(resp => {
            this.don_hang = resp
        })
    } goBuyProduct(): void {
        const url = '/portal_product';
        this.router.navigateByUrl(url);

    }
    ngOnInit(): void {

        AOS.init({
            duration: 1000
        });
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({ matchingAliases }) => {
                this.isScreenSmall = !matchingAliases.includes('md');
            });
        this.route.params.subscribe(params => {
            this.id = params["id"];
            this.getElementById()
        });
    }
}
