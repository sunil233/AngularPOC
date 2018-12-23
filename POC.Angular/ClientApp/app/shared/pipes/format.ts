import { Pipe, PipeTransform, Injectable } from '@angular/core';
import {DatePipe} from '@angular/common';

@Pipe({
    name: 'format'
})

@Injectable()
export class Format implements PipeTransform {
    datePipe: DatePipe = new DatePipe('yMd');
    transform(input: any, args: any): any {
        if (input == null) return '';
        var format = '';
        var parsedFloat = 0;
        var pipeArgs = args.split(':');
        for (var i = 0; i < pipeArgs.length; i++) {
            pipeArgs[i] = pipeArgs[i].trim(' ');
        }

        switch (pipeArgs[0].toLowerCase()) {
            case 'text':
                return input;
            case 'date':
                return this.getDate(input);
            case 'csv':
                if (input.length == 0)
                    return "";
                if (input.length == 1)
                    return input[0].text;
                let finalstr: string = "";
                for (let i = 0; i < input.length; i++) {
                    finalstr = finalstr + input[i].text + ", ";
                }
                return finalstr.substring(0, finalstr.length - 2);
            default:
                return input;
        }
    }

    private getDate(date: string): any {
        return new Date(date).toLocaleDateString();
    }
}