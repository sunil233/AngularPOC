import { Component, Inject, ViewEncapsulation } from '@angular/core';
declare var $: any;

@Component({
    selector: 'app-root',
    encapsulation: ViewEncapsulation.None,
  templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})

export class AppComponent {
 
  constructor() {
 
  }
    ngOnInit() {
        $(document).on('click', '[href="#"]', e => e.preventDefault());
    }
}
