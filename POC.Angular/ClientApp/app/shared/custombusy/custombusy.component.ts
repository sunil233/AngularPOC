import { Component, OnInit, Inject } from '@angular/core';
import { InstanceConfigHolderService } from 'ng-busy';
@Component({
    selector: 'default-busy',
    templateUrl: './custombusy.component.html',
    
})
export class CustomBusyComponent {
    constructor(@Inject('instanceConfigHolder') private instanceConfigHolder: InstanceConfigHolderService) {
    }

    get message() {
        return this.instanceConfigHolder.config.message;
    }
}


