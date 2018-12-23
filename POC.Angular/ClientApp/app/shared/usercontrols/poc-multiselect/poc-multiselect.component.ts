import { Component, Input,  OnInit, ChangeDetectorRef, Renderer, ElementRef, forwardRef } from '@angular/core';
import { Pipe, PipeTransform } from '@angular/core';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/throttleTime';
import 'rxjs/add/observable/fromEvent';
import {  FormControl } from '@angular/forms';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { EqualPipe } from '../../pipes/EqualPipe';
@Pipe({
    name: 'filter'
})
export class FilterPipe implements PipeTransform {
    transform(items: any, filter: any, isAnd: boolean): any {
        if (filter && Array.isArray(items)) {
            let filterKeys = Object.keys(filter);
            if (isAnd) {
                return items.filter(item =>
                    filterKeys.reduce((memo, keyName) =>
                        (memo && new RegExp(filter[keyName], 'gi').test(item[keyName])) || filter[keyName] === "", true));
            } else {
                return items.filter(item => {
                    return filterKeys.some((keyName) => {
                        console.log(keyName);
                        return new RegExp(filter[keyName], 'gi').test(item[keyName]) || filter[keyName] === "";
                    });
                });
            }
        } else {
            return items;
        }
    }
}
const MULTISELECT_VALUE_ACCESSOR = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => Multiselect),
    multi: true
};
@Component({
    selector: 'poc-multiselect',
    styleUrls: ['./poc-multiselect.component.scss'],
    templateUrl: './poc-multiselect.component.html',
    host: { '(change)': 'manualChange($event)', '(document:click)': 'hostClick($event)' },
    providers: [MULTISELECT_VALUE_ACCESSOR]
})
export class Multiselect implements OnInit, ControlValueAccessor {
    public _selectedItems: Array<any>;
    public localHeader: string;
    public isOpen: boolean = false;
    public enableFilter: boolean;
    public filterText: string;
    public filterPlaceholder: string;
    public filterInput = new FormControl();
    @Input() items: Array<any>;
    @Input() header: string = "--Select--";
    @Input() selectedHeader: string = "options selected";
  
    private _onChange = (_: any) => { };
    private _onTouched = () => { };
    constructor(private _elRef: ElementRef, private _renderer: Renderer, private _equalPipe: EqualPipe, private _changeDetectorRef: ChangeDetectorRef) {
    }
    get selected(): any {
        return this._selectedItems;
    };
    writeValue(value: any) {
        if (value !== undefined) {
            this._selectedItems = value;
        } else {
            this._selectedItems = [];
        }
    }
    setHeaderText() {
        this.localHeader = this.header;
        var isArray = this._selectedItems instanceof Array;
        if (isArray && this._selectedItems.length > 1) {
            this.localHeader = this._selectedItems.length + ' ' + this.selectedHeader;
        } else if (isArray && this._selectedItems.length === 1) {
            this.localHeader = this._selectedItems[0].label;
        }
       
    }
    registerOnChange(fn: (value: any) => any): void { this._onChange = fn; }
    registerOnTouched(fn: () => any): void { this._onTouched = fn; }
    setDisabledState(isDisabled: boolean): void {
        this._renderer.setElementProperty(this._elRef.nativeElement, 'disabled', isDisabled);
    }
    manualChange() {
        this._onChange(this._selectedItems);
    }
    select(item: any) {
        item.checked = !item.checked;
        this._selectedItems = this._equalPipe.transform(this.items, { checked: true });
        this.setHeaderText();
        this._onChange(this._selectedItems);
    }
    toggleSelect() {
        this.isOpen = !this.isOpen;
    }
    clearFilter() {
        this.filterText = "";
    }
    hostClick(event) {
        if (this.isOpen && !this._elRef.nativeElement.contains(event.target))
            this.isOpen = false;
    }
    ngOnInit() {
        if (this.items != undefined) {
            this.enableFilter = true;
            this.filterText = "";
            this.filterPlaceholder = "Filter..";
            this._selectedItems = this._equalPipe.transform(this.items, { checked: true });
            this.setHeaderText();
            this.filterInput
                .valueChanges
                .debounceTime(200)
                .distinctUntilChanged()
                .subscribe(term => {
                    this.filterText = term;
                    this._changeDetectorRef.markForCheck();
                });
        }
    }
}
//Reference :https://long2know.com/2016/11/angular2-multiselect-dropdown/