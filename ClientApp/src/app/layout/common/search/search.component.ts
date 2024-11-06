import { Component, ElementRef, EventEmitter, HostBinding, Input, OnChanges, OnDestroy, OnInit, Output, Renderer2, SimpleChanges, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { debounceTime, filter, map, takeUntil } from 'rxjs/operators';
import { fuseAnimations } from '@fuse/animations/public-api';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
    selector     : 'search',
    templateUrl  : './search.component.html',
    encapsulation: ViewEncapsulation.None,
    exportAs     : 'fuseSearch',
    animations   : fuseAnimations
})
export class SearchComponent implements OnChanges, OnInit, OnDestroy
{
    @Input() appearance: 'mobile' | 'basic' | 'bar' = 'basic';
    @Input() debounce: number = 300;
    @Input() minLength: number = 2;
    @Output() search: EventEmitter<any> = new EventEmitter<any>();

    opened: boolean = false;
    results: any[];
    searchControl: FormControl = new FormControl();
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
private router: Router,
        private _elementRef: ElementRef,
        private _httpClient: HttpClient,
        private _renderer2: Renderer2
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Host binding for component classes
     */
    @HostBinding('class') get classList(): any
    {
        return {
            'search-appearance-bar'  : this.appearance === 'bar',
            'search-appearance-basic': this.appearance === 'basic',
            'search-appearance-mobile': this.appearance === 'mobile',
            'search-opened'          : this.opened
        };
    }

    
    searchFunc(): void {
        
        console.log(this.searchControl);
        var value = this.searchControl.value;
        if(value !="" && value !=null && value !=undefined){

            const url = '/homepageSearch/' + encodeURIComponent(value);
             this.router.navigateByUrl(url);
             this.close();
        }
       
    }

    /**
     * Setter for bar search input
     *
     * @param value
     */
    @ViewChild('barSearchInput')
    set barSearchInput(value: ElementRef)
    {
        // If the value exists, it means that the search input
        // is now in the DOM and we can focus on the input..
        if ( value )
        {
            // Give Angular time to complete the change detection cycle
            setTimeout(() => {

                // Focus to the input element
                value.nativeElement.focus();
            });
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On changes
     *
     * @param changes
     */
    ngOnChanges(changes: SimpleChanges): void
    {
        // Appearance
        if ( 'appearance' in changes )
        {
            // To prevent any issues, close the
            // search after changing the appearance
            this.close();
        }
    }

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Subscribe to the search field value changes
       // this.searchControl.valueChanges
        //             .pipe(
        //                 debounceTime(this.debounce),
        //                 takeUntil(this._unsubscribeAll),
        //                 map((value) => {

        //                     // Set the search results to null if there is no value or
        //                     // the length of the value is smaller than the minLength
        //                     // so the autocomplete panel can be closed
        //                     if ( !value || value.length < this.minLength )
        //                     {
        //                         this.results = null;
        //                     }

        //                     // Continue
        //                     return value;
        //                 }),
        //                 // Filter out undefined/null/false statements and also
        //                 // filter out the values that are smaller than minLength
        //                 filter(value => value && value.length >= this.minLength)
        //             )
        //             .subscribe((value) => {
        //                 this._httpClient.post('api/common/search', {query: value})
        //                     .subscribe((response: any) => {

        //                         // Store the results
        //                         this.results = response.results;

        //                         // Execute the event
        //                         this.search.next(this.results);
        //                     });
        //             });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * On keydown of the search input
     *
     * @param event
     */
    onKeydown(event: KeyboardEvent): void
    {
        // Listen for escape to close the search
        // if the appearance is 'bar'
        if ( this.appearance === 'bar' )
        {
            // Escape
            if ( event.code === 'Escape' )
            {
                // Close the search
                this.close();
            }
        }
    }

    /**
     * Open the search
     * Used in 'bar'
     */
    open(): void
    {
        // Return if it's already opened
        if ( this.opened )
        {
            this.opened = false;
            return;
        }

        // Open the search
        this.opened = true;
    }

    /**
     * Close the search
     * * Used in 'bar'
     */
    close(): void
    {
        // Return if it's already closed
        if ( !this.opened )
        {
            return;
        }

        // Clear the search input
        this.searchControl.setValue('');

        // Close the search
        this.opened = false;
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any
    {
        return item.id || index;
    }
}
