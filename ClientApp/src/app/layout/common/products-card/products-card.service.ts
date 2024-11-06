import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class ShoppingCardsService {
    /**
     * Variables
     */
    private product: BehaviorSubject<any> =
        new BehaviorSubject<any>(null);
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) { }
    /**
     * Getter for notifications
     */
    get product$(): Observable<any> {
        return this.product.asObservable();
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------
    /**
     * Create a product
     *
     * @param product
     */
    create(product: any) {
        this.product.next(product);
    }
}