import { Injectable } from "@angular/core";
import { SalesInvoiceModel } from "../Models/sales-invoice.model";
import { Observable, map } from "rxjs";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "src/environments/environment.development";
import { IPaginationModel } from "src/app/shared/ngx-pagination/Models/IPaginationModel";
import { SharedService } from "src/app/shared/common-methods";

@Injectable({
    providedIn: 'root',
})

export class SalesService {
  baseUrl = environment.baseUrlInventory;

  constructor(private http: HttpClient, private sharedService: SharedService) { }

  saveSell(salesModel: SalesInvoiceModel): Observable<SalesInvoiceModel> {
    return this.http
      .post<any>(`${environment.baseUrlInventory}/api/Sales/`, salesModel)
      .pipe(map(() => { return salesModel; }));
  }

  getSalesPaged(pagedSalesModel: IPaginationModel<SalesInvoiceModel>): Observable<IPaginationModel<SalesInvoiceModel>> {
    let paramsObject: HttpParams = this.sharedService.paginationToParams<SalesInvoiceModel>(pagedSalesModel);
    return this.http
      .get<any>(`${environment.baseUrlInventory}/api/Sales/PagedAsync`, { params: paramsObject })
      .pipe(map(response => response));
  }
}
