// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-helper-methods',
//   templateUrl: './helper-methods.component.html',
//   styleUrls: ['./helper-methods.component.scss']
// })
export class HelperMethodsComponent {
  
}

export class Converter {
  public static toNumber(value: number | undefined | null): number {
    if (value == null || value == undefined) {
      return 0;
    }
    return value;
  }
}
