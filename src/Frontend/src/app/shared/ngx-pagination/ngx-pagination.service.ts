import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IPaginationModel } from './Models/IPaginationModel';

@Injectable({
  providedIn: 'root',
})
export class NGXPaginationService<T> {
  private subject = new BehaviorSubject<IPaginationModel<T>>({
    tableCardHeader: '',
    addNewElementButtonConfig: null,
    pagingConfig: null,
    searchingConfig: null,
    tableConfig: null,
    organizationId: 0,
  });

  get() {
    return this.subject.asObservable();
  }

  set(newModel: IPaginationModel<T>) {
    this.subject.next(newModel);
  }
}
