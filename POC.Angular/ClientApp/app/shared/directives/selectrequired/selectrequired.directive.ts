import { Directive, ElementRef, Input } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[appSelectValidator]',
    providers: [{ provide: NG_VALIDATORS, useExisting: SelectRequiredValidatorDirective, multi: true }]
})
export class SelectRequiredValidatorDirective implements Validator {
    //  @Input('appSelectRequired') appSelectRequired: string;
    validate(control: AbstractControl): { [key: string]: any } | null {
        return control.value === "-1" || control.value==-1? { 'defaultselected': true } : null;
    }
}



