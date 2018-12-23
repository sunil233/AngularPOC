import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { AlertModule } from 'ngx-bootstrap/alert';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxPaginationModule } from 'ngx-pagination';//for table pagination
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { DatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgBusyModule } from 'ng-busy';

import { POCCustommGrid } from './usercontrols/poc-custom-grid/poc-custom-grid.component';
import { POCGrid } from './usercontrols/poc-grid/poc-grid.component';
import { Format } from './pipes/format';
import { OrderBy } from './pipes/orderby';
import { EqualPipe } from './pipes/equalpipe';
import { SelectRequiredValidatorDirective } from './directives/selectrequired/selectrequired.directive';
import { Multiselect, FilterPipe } from './usercontrols/poc-multiselect/poc-multiselect.component';


// https://angular.io/guide/sharing-ngmodules
//https://valor-software.com/ngx-bootstrap/#/getting-started
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        AccordionModule.forRoot(),
        AlertModule.forRoot(),
        ButtonsModule.forRoot(),     
        CollapseModule.forRoot(),
        DatepickerModule.forRoot(),
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        ModalModule.forRoot(),
        PaginationModule.forRoot(),      
        TooltipModule.forRoot(),
        PopoverModule.forRoot()  ,    
        NgxPaginationModule,
        NgBusyModule
    ],
    providers: [
        EqualPipe
    ],
    declarations: [
        POCGrid,
        POCCustommGrid,
        Format,
        OrderBy,       
        Multiselect,
        FilterPipe,
        EqualPipe,
        SelectRequiredValidatorDirective
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        AccordionModule,
        AlertModule,
        ButtonsModule,
        CollapseModule,
        DatepickerModule,
        BsDatepickerModule,
        BsDropdownModule,
        ModalModule,
        PaginationModule,
        TooltipModule,
        PopoverModule,
        POCGrid,
        POCCustommGrid,
        Format,
        OrderBy,
        NgBusyModule,
        SelectRequiredValidatorDirective,
        Multiselect,
        FilterPipe,
        EqualPipe,
    ]
  
})


export class SharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule
        };
    }
}
