import { Component, EventEmitter, OnDestroy, OnInit, Output, TemplateRef, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { cloneDeep } from 'lodash-es';
import { Calendar } from '../calendar.types';
import { CalendarService } from '../calendar.service';
import { calendarColors } from '../sidebar/calendar-colors';

@Component({
    selector: 'calendar-sidebar',
    templateUrl: './sidebar.component.html',
    encapsulation: ViewEncapsulation.None
})
export class CalendarSidebarComponent implements OnInit, OnDestroy {
    @Output() readonly calendarUpdated: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('editPanel') private _editPanel: TemplateRef<any>;

    calendar: Calendar | null;
    calendarColors: any = calendarColors;
    calendars: Calendar[];
    private _editPanelOverlayRef: OverlayRef;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _calendarService: CalendarService,
        private _overlay: Overlay,
        private _viewContainerRef: ViewContainerRef
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Get calendars
        this._calendarService.calendars$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((calendars) => {

                // Store the calendars
                this.calendars = calendars;
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();

        // Dispose the overlay
        if (this._editPanelOverlayRef) {
            this._editPanelOverlayRef.dispose();
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Open edit panel
     */
    openEditPanel(calendar: Calendar): void {
        // Set the calendar
        this.calendar = cloneDeep(calendar);

        // Create the overlay if it doesn't exist
        if (!this._editPanelOverlayRef) {
            this._createEditPanelOverlay();
        }

        // Attach the portal to the overlay
        this._editPanelOverlayRef.attach(new TemplatePortal(this._editPanel, this._viewContainerRef));
    }

    /**
     * Close the edit panel
     */
    closeEditPanel(): void {
        // Detach the overlay from the portal
        if (this._editPanelOverlayRef) {
            this._editPanelOverlayRef.detach();
        }
    }

    /**
     * Toggle the calendar visibility
     *
     * @param calendar
     */
    toggleCalendarVisibility(calendar: Calendar): void {
        // Toggle the visibility
        calendar.visible = !calendar.visible;
        // Update the calendar
        this.saveCalendar(calendar);
    }
    /**
     * Save the calendar
     *
     * @param calendar
     */
    saveCalendar(calendar: Calendar): void {
        // Close the edit panel
        this.closeEditPanel();

        // Emit the calendarUpdated event
        this.calendarUpdated.emit();
    }

    /**
     * Add calendar
     */
    addCalendar(): void {
        // Create a new calendar with default values
        const calendar = {
            id: null,
            title: '',
            color: 'bg-blue-900',
            visible: true
        };

        // Open the edit panel
        this.openEditPanel(calendar);
    }


    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Create the edit panel overlay
     *
     * @private
     */
    private _createEditPanelOverlay(): void {
        // Create the overlay
        this._editPanelOverlayRef = this._overlay.create({
            hasBackdrop: true,
            scrollStrategy: this._overlay.scrollStrategies.reposition(),
            positionStrategy: this._overlay.position()
                .global()
                .centerHorizontally()
                .centerVertically()
        });

        // Detach the overlay from the portal on backdrop click
        this._editPanelOverlayRef.backdropClick().subscribe(() => {
            this.closeEditPanel();
            this.calendar = null;
        });
    }
}
