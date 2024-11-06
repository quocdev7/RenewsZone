import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import Swal from 'sweetalert2';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, tap, takeUntil, debounceTime, map, delay } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';
import { ThemePalette } from '@angular/material/core';
@Component({
    selector: 'cm_select_server_side',
    templateUrl: './cm_select_server_side.component.html',
    styleUrls: ['./cm_select_server_side.component.scss']
})
export class cm_select_server_sideComponent implements OnInit {
    @Input() keyError: string;
    @Input() label: string;
    @Input() placeholder: string;
    @Input() model: any;
    @Input() objectChose: any;
    @Input() link: string;
    @Input() dataFilter: any;
    @Input() callbackChange: () => void;
    @Input() errorModel: any;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Output() objectChoseChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() listData: any;
  
     
    public color: ThemePalette = 'primary';
    public mode: ProgressSpinnerMode = 'indeterminate';
    public value = 50;

    txtQueryChanged: Subject<string> = new Subject<string>();
    search: string = '';
    public loading: boolean = false;
    

    constructor(
        public http: HttpClient,
    ) {
        this.txtQueryChanged
            .pipe(debounceTime(700))
            .subscribe((model) => {
                this.listData = [];
                this.http
                    .post(this.link,
                        {
                            search: this.search,
                            data: this.dataFilter
                        }
                    ).subscribe((resp) => {
                        this.listData = resp;
                        this.loading = false;
                    });
            });
    }

    ngOnInit(): void {

    }
    setChose(data): void {
        this.objectChose = this.listData.filter(d => d.id === data)[0];
        this.objectChoseChange.emit(this.objectChose);
        if (this.callbackChange !== undefined && this.callbackChange !== null) { this.callbackChange(); }


    }
    onChange(query: string): void {

        if (this.search === '' || this.search === undefined) {
            this.loading = false;
            return;
        }
        this.loading = true;
        this.txtQueryChanged.next(query);

    }









}

