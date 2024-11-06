import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    Inject,
    OnChanges,
    OnDestroy,
    OnInit,
    SimpleChanges,
    TemplateRef,
    ViewChild,
    ViewContainerRef,
    ViewEncapsulation,
} from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { MatButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TranslocoService } from '@ngneat/transloco';
import { takeUntil } from 'rxjs/operators';
import { ShoppingCardsService } from './products-card.service';
import { FuseNavigationService } from '@fuse/components/navigation';
import { Router } from '@angular/router';

@Component({
    selector: 'products-card',
    templateUrl: './products-card.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    exportAs: 'products-card',
})
export class ProductsCardPageComponent implements OnChanges, OnInit, OnDestroy {
    @ViewChild('list_product_cardOrigin')
    private _list_product_cardOrigin: MatButton;
    @ViewChild('list_product_cardPanel')
    private _list_product_cardPanel: TemplateRef<any>;
    public price_products_card: number = 0;
    public price_sum: number = 0;
    public productCount: number = 0;
    private _overlayRef: OverlayRef;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    public list_product_card: any[] = [];
    public loading: boolean = false;
    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        public dialog: MatDialog,
        private _shoppingCartService: ShoppingCardsService,
        private _overlay: Overlay,
        public http: HttpClient,
        public _fuseNavigationService: FuseNavigationService,
        public _translocoService: TranslocoService,
        private _router: Router,
        private _viewContainerRef: ViewContainerRef
    ) { }
    ngOnChanges(changes: SimpleChanges): void { }
    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------
    /**
     * On init
     */
    ngOnInit(): void {
        // Subscribe to notification changes
        this._shoppingCartService.product$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((product: any) => {
                this.list_product_card = this.getlocalStorage();
                if (product != null) {
                    // Load the list_product_card
                    var item = this.list_product_card.find(
                        (q) =>
                            q.id == product.id
                    );
                    if (item != undefined) {
                        this.list_product_card =
                            this.list_product_card.filter(
                                (q) =>
                                    q.id != product.id
                            );
                        product.so_luong = item.so_luong + product.so_luong;
                    }
                    if (product.so_luong > 0)
                        this.list_product_card = [
                            ...this.list_product_card,
                            product,
                        ];
                    this.setlocalStorage();
                }
                // Calculate the unread count
                this._calculateUnreadCount();
                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
        this.loading = true;
        //update UnreadCount real time
        this._changeDetectorRef.markForCheck();
    }
    setlocalStorage() {
        localStorage.setItem(
            'list_product_card',
            JSON.stringify(this.list_product_card)
        );
    }

    resetlocalStorage() {
        localStorage.removeItem('list_product_card');
    }
    getlocalStorage() {
        return JSON.parse(localStorage.getItem('list_product_card'));
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();

        // Dispose the overlay
        if (this._overlayRef) {
            this._overlayRef.dispose();
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Open the list_product_card panel
     */
    openPanel(): void {
        const url = '/payment-cart';
        this._router.navigateByUrl(url);
    }

    /**
     * Close the messages panel
     */
    closePanel(): void {
        this._overlayRef.detach();
    }
    /**
     * Delete the given notification
     */
    delete(product: any): void {
        // Delete the notification
        this.list_product_card = this.list_product_card.filter(
            (q) =>
                q.id != product.id &&
                q.id_size != product.id_size &&
                q.id_color != product.id_color
        );
        this._calculateUnreadCount();
    }
    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------
    /**
     * Create the overlay
     */
    private _createOverlay(): void {
        // Create the overlay
        this._overlayRef = this._overlay.create({
            hasBackdrop: true,
            backdropClass: 'fuse-backdrop-on-mobile',
            scrollStrategy: this._overlay.scrollStrategies.block(),
            positionStrategy: this._overlay
                .position()
                .flexibleConnectedTo(
                    this._list_product_cardOrigin._elementRef.nativeElement
                )
                .withLockedPosition()
                .withPush(true)
                .withPositions([
                    {
                        originX: 'start',
                        originY: 'bottom',
                        overlayX: 'start',
                        overlayY: 'top',
                    },
                    {
                        originX: 'start',
                        originY: 'top',
                        overlayX: 'start',
                        overlayY: 'bottom',
                    },
                    {
                        originX: 'end',
                        originY: 'bottom',
                        overlayX: 'end',
                        overlayY: 'top',
                    },
                    {
                        originX: 'end',
                        originY: 'top',
                        overlayX: 'end',
                        overlayY: 'bottom',
                    },
                ]),
        });

        // Detach the overlay from the portal on backdrop click
        this._overlayRef.backdropClick().subscribe(() => {
            this._overlayRef.detach();
        });
    }
    /**
     * Calculate the unread count
     *
     * @private
     */
    private _calculateUnreadCount(): void {
        let count = 0;
        this.price_sum = 0;
        if (this.list_product_card && this.list_product_card.length) {
            count = this.list_product_card.length;
            this.price_products_card = this.list_product_card
                .map((c) => c.gia_ban * c.so_luong)
                .reduce((sum, current) => sum + current);
            this.price_sum = this.price_products_card;
        }
        this.productCount = count;
        this._changeDetectorRef.markForCheck();
    }
}