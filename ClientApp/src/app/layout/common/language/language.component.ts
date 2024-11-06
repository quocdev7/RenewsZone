import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { take } from 'rxjs/operators';
import { AvailableLangs, TranslocoService } from '@ngneat/transloco';
import { FuseNavigationService, FuseVerticalNavigationComponent } from '@fuse/components/navigation';

@Component({
    selector       : 'language',
    templateUrl    : './language.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    exportAs       : 'language'
})
export class LanguageComponent implements OnInit, OnDestroy
{
    availableLangs: AvailableLangs;
    activeLang: string;
    activeLanglabel: string;
    flagCodes: any;

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseNavigationService: FuseNavigationService,
        private _translocoService: TranslocoService
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        this._translocoService.setAvailableLangs([{ id: 'vi', label: 'Tiếng việt' }, { id: 'en', label: 'English' } ]);
        // this._translocoService.setAvailableLangs([{ id: 'vi', label: 'Tiếng việt' } ]);
        
        // Get the available languages from transloco
        this.availableLangs = this._translocoService.getAvailableLangs();

      
        // Subscribe to language chancoges
        this._translocoService.langChanges$.subscribe((activeLang) => {

            // Get the active lang
            this.activeLang = activeLang;
            var array:any =this.availableLangs;
            this.activeLanglabel =  array.filter(d=>d.id ==   this.activeLang )[0].label;
            // Update the navigation
            this._updateNavigation(activeLang);
        });
        var lang=localStorage.getItem("lang");
        if(lang!=null && lang!="" && lang!= undefined)
        {
            this.activeLang = lang;
            var array:any =this.availableLangs;
            this._translocoService.setActiveLang(this.activeLang);
            this.activeLanglabel =  array.filter(d=>d.id ==   this.activeLang )[0].label;      
        }

        // Set the country iso codes for languages for flags
        this.flagCodes = {
            'vi': 'vi',
            'en': 'us',
            'tr': 'tr'
        };
        

      
    }
  

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Set the active lang
     *
     * @param lang
     */
    setActiveLang(lang: string): void
    {  
        
        localStorage.setItem('lang', lang);
        // Set the active lang
        this._translocoService.setActiveLang(lang);
        this.activeLang = lang;
        var array:any =this.availableLangs;
        this.activeLanglabel =  array.filter(d=>d.id ==   this.activeLang )[0].label;      
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

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Update the navigation
     *
     * @param lang
     * @private
     */
    public _updateNavigation(lang: string): void
    {
        // For the demonstration purposes, we will only update the Dashboard names
        // from the navigation but you can do a full swap and change the entire
        // navigation data.
        //
        // You can import the data from a file or request it from your backend,
        // it's up to you.

        // Get the component -> navigation data -> item

      
        const navComponent = this._fuseNavigationService.getComponent<FuseVerticalNavigationComponent>('mainNavigation');

        // Return if the navigation component does not exist
        if ( !navComponent )
        {
            return null;
        }

        // Get the flat navigation data
        const navigation = navComponent.navigation;

        // Get the Project dashboard item and update its title
        const projectDashboardItem = this._fuseNavigationService.getItem('dashboards.project', navigation);
        if ( projectDashboardItem )
        {
            this._translocoService.selectTranslate('Project').pipe(take(1))
                .subscribe((translation) => {

                    // Set the title
                    projectDashboardItem.title = translation;

                    // Refresh the navigation component
                    navComponent.refresh();
                });
        }

        // Get the Analytics dashboard item and update its title
        const analyticsDashboardItem = this._fuseNavigationService.getItem('dashboards.analytics', navigation);
        if ( analyticsDashboardItem )
        {
            this._translocoService.selectTranslate('Analytics').pipe(take(1))
                .subscribe((translation) => {

                    // Set the title
                    analyticsDashboardItem.title = translation;

                    // Refresh the navigation component
                    navComponent.refresh();
                });
        }
    }
}
