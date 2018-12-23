import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
    name: 'equal',
    pure: false
})

export class EqualPipe implements PipeTransform {
    transform(items: any, filter: any): any {
      if (filter && Array.isArray(items)) {
          let filterKeys = Object.keys(filter);
          return items.filter(item =>
              filterKeys.reduce((memo, keyName) => {
                  return item[keyName] === filter[keyName];}, true)
                  );
      } else {
          return items;
      }
    }
}
