import { Component, OnInit, ViewEncapsulation, Input, EventEmitter, Output, ChangeDetectorRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { TranslocoService } from '@ngneat/transloco';
import Swal from 'sweetalert2';
@Component({
    selector     : 'cm_select',
    templateUrl  : './cm_select.component.html',
    styleUrls    : ['./cm_select.component.scss']
})
export class cm_selectComponent implements OnInit
{
    @Input() errorModel: any;
    @Input() keyError: string;
    @Input() label: string;
    @Input() type: string;
    @Input() paramCallBack: string;
    @Input() model: any;
    @Input() actionEnum: any = 1;
    @Input() listData: any;
    @Input() callbackChange: Function;
    @Input() callbackChangeWithParam: Function;
    @Input() callbackChangeSecond: Function;
    @Output() modelChange: EventEmitter<any> = new EventEmitter<any>();
    @Input() objectChose: any;
    @Output() objectChoseChange: EventEmitter<any> = new EventEmitter<any>();
    search: string = '';
    public selected: any;
    public old_value:any='';
    @ViewChild('mySel') skillSel: MatSelect;
    constructor(
        private _translocoService: TranslocoService, private _changeDetectorRef: ChangeDetectorRef
    ) {

        if (this.type == '' || this.type == null) this.type = 'single';
        var old_value =  this.model;
    }

    ngOnInit() {




    }


    setChose(data): void {
        if(this.type == 'multiple'){
            if(this.old_value.includes('-1')){
                this.skillSel.options.forEach( (item : MatOption) => item.value=='-1'? item.deselect():null );
                this.model=this.model.filter(d=>d!='-1');
            }else if(data.includes('-1') && !this.old_value.includes('-1')){
                this.model=['-1'];
                this.skillSel.options.forEach( (item : MatOption) => item.value!='-1'? item.deselect():item.select() );
            }
            this.old_value=this.model;
            this._changeDetectorRef.detectChanges();
        }
            this.objectChose = this.listData.filter(d => d.id == data)[0];
            this.objectChoseChange.emit(this.objectChose);
        if (this.callbackChange != undefined && this.callbackChange != null)
            this.callbackChange();

        if(this.callbackChangeSecond != undefined && this.callbackChangeSecond != null){
            this.callbackChangeSecond();
        }
        if (this.callbackChangeWithParam != undefined && this.callbackChangeWithParam != null)
            if (this.paramCallBack != undefined && this.paramCallBack != null) {
                this.callbackChangeWithParam(this.paramCallBack, this.model);
            } else {
                this.callbackChangeWithParam(this.label, this.model);
            }
    }








}

