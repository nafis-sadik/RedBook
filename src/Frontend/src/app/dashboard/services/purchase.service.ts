import { Injectable } from '@angular/core';
import { PurchaseInvoiceModel } from '../Models/purchase-invoice.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject, map } from 'rxjs';
import { SharedService } from 'src/app/shared/common-methods';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})

export class PurchaseService {
  private formDataSubject: Subject<PurchaseInvoiceModel>;
  constructor(private http: HttpClient, private sharedService: SharedService) {
    this.formDataSubject = new Subject<PurchaseInvoiceModel>();
  }

  emitInvoiceModel(model: PurchaseInvoiceModel): void {
    this.formDataSubject.next(model);
  }

  listenInvoiceModel(): Observable<PurchaseInvoiceModel> {
    return this.formDataSubject.asObservable();
  }

  addPurchaseIncoice(model: PurchaseInvoiceModel): Observable<PurchaseInvoiceModel> {
    return this.http
      .post<PurchaseInvoiceModel>(`${environment.baseUrlInventory}/api/Purchase/`, model)
      .pipe(map(response => response));
  }

  getPagedPurchaseInvoice(outletId: number, pagedPurchaseModel: IPaginationModel<PurchaseInvoiceModel>): Observable<IPaginationModel<PurchaseInvoiceModel>> {
    let paramsObject: HttpParams = this.sharedService.paginationToParams<PurchaseInvoiceModel>(pagedPurchaseModel);
    paramsObject = paramsObject.append('organizationId', outletId.toString());
    return this.http
      .get<any>(`${environment.baseUrlInventory}/api/Purchase/PagedAsync/`, { params: paramsObject })
      .pipe(map(response => response));
  }
}
