import { Component, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { Format } from '../../pipes/format'
export interface GridAction {
    action: string,
    items: {
        key: string,
        item: string
    }[]
}
@Component({
    selector: 'poc-custom-grid',
    styleUrls: ['./poc-custom-grid.component.scss'],
    templateUrl: './poc-custom-grid.component.html',
})
export class POCCustommGrid {
    //Input Variables
    @Input() columns: any[];
    @Input() data: any[];
    @Input() sort: any;
    @Input() gridbtns: any[]=[];
    @Input() hdrbtns: any[];
    @Input() isEdit: boolean = false;
    @Input() isView: boolean = false;    
    @Input() isDelete: boolean = false;
    @Input() isPrint: boolean = false;
    @Input() isRefresh: boolean = false;
    @Input() isAttachment: boolean = false;
    @Input() showSearch: boolean = true; 
    @Input() showPaging: boolean = true;
    @Input() itemsPerPage: number = 10;
    @Input() filter: any;

    //Output Variable
    @Output()
    btnclick: EventEmitter<GridAction> = new EventEmitter<GridAction>();

    //paging variables
    public page: number = 1;
    public length: number = 0;
    public maxSize: number = 5;
    public numPages: number = 1;
    public rows: Array<any> = [];
    public colcount: number = 0;

    //Local Variable
    pdata: any[] = this.data;
    listFilter: string;
    config = {
        paging: true,
        sorting: { columns: this.columns },
        filtering: { filterString: '' },
        className: ['table-bordered']
    };
    constructor(private cd: ChangeDetectorRef) {
    }
    ngOnChanges(changes: any) {
        if (JSON.stringify(changes).indexOf("data") != -1) {
            if (this.data != undefined && this.data != null) {
                this.pdata = this.data;
                this.criteriaChange(this.listFilter);
                this.length = this.data.length;
                this.colcount = this.columns.length;
                if (this.isView == true) {
                    this.colcount = this.colcount + 1;
                }
                if (this.isEdit == true) {
                    this.colcount = this.colcount + 1;
                }
                if (this.isDelete == true) {
                    this.colcount = this.colcount + 1;
                }
                if (this.isPrint == true) {
                    this.colcount = this.colcount + 1;
                }
                if (this.isRefresh == true) {
                    this.colcount = this.colcount + 1;
                }
            }
        }
    }
    selectedClass(columnName: string): any {
        return columnName == this.sort.column ? 'sort-' + this.sort.descending : false;
    }
    changeSorting(columnName: string): void {
        var sort = this.sort;
        if (sort.column == columnName) {
            sort.descending = !sort.descending;
        } else {
            sort.column = columnName;
            sort.descending = false;
        }
    }
    convertSorting(): string {
        return this.sort.descending ? '-' + this.sort.column : this.sort.column;
    }
    click(action: any, row: any): void {
        let keyds = <GridAction>{};
        keyds.action = action;
        if (row != null) {
            keyds.items = [];
            keyds.items.push({ key: action, item: row });
        }
        this.btnclick.emit(keyds);
    }
    criteriaChange(filtervalue: any) {
        if (filtervalue != '[object Event]') {
            this.config.filtering.filterString = filtervalue
            this.pdata = this.changeFilter(this.data, this.config);
        }
    }
    changeFilter(data: any, config: any): any {
        var filterString = config.filtering.filterString;
        let filteredData: Array<any> = data;
        if (!filterString) {
            return filteredData;
        }
        let tempArray: Array<any> = [];
        filteredData.forEach((item: any) => {
            let flag = false;
            this.columns.forEach((column: any) => {
                if (item[column.name] != null && item[column.name] != undefined) {
                    if (this.compareString(item[column.name].toString(), (filterString))) {
                        flag = true;
                    }
                }

            });
            if (flag) {
                tempArray.push(item);
            }
        });
        filteredData = tempArray;
        return filteredData;
    }
    compareString(s1: string, s2: string): boolean {
        return s1.toLowerCase().indexOf(s2.toLowerCase()) != -1;
    }
    onSearch($ev, filtervalue) {
        $ev.preventDefault();
        this.config.filtering.filterString = filtervalue
        this.page = 1;
        let filteredData = this.changeFilter(this.data, this.config);
        let sortedData = this.changeSort(filteredData);
        this.pdata = sortedData;
        this.length = sortedData.length;
    }
    changePage(page: any, data: Array<any> = this.pdata): Array<any> {
        let start = (page.page - 1) * page.itemsPerPage;
        let end = page.itemsPerPage > -1 ? (start + page.itemsPerPage) : data.length;
        return data.slice(start, end);
    }
    changeSort(data: any): any {
        let columns = this.columns || [];
        let columnName: string = void 0;
        let sort: string = void 0;
        for (let i = 0; i < columns.length; i++) {
            if (columns[i].sort !== '' && columns[i].sort !== false) {
                columnName = columns[i].name;
                sort = columns[i].sort;
            }
        }
        if (!columnName) {
            return data;
        }
        // simple sorting
        return data.sort((previous: any, current: any) => {
            if (previous[columnName] > current[columnName]) {
                return sort === 'desc' ? -1 : 1;
            } else if (previous[columnName] < current[columnName]) {
                return sort === 'asc' ? -1 : 1;
            }
            return 0;
        });
    }
    onChangeTable(config: any, page: any = { page: this.page, itemsPerPage: this.itemsPerPage }): any {
        if (this.config.filtering) {
            (<any>Object).assign(this.config.filtering, this.config.filtering);
        }
        if (this.config.sorting) {
            (<any>Object).assign(this.config.sorting, this.config.sorting);
        }
        let filteredData = this.changeFilter(this.pdata, this.config);
        let sortedData = this.changeSort(filteredData);
        this.pdata = sortedData;
        //  this.pdata = page && this.config.paging ? this.changePage(page, sortedData) : sortedData;
        this.length = sortedData.length;
    }
    ngAfterViewChecked(): void {
        this.cd.detectChanges();
    }
}

