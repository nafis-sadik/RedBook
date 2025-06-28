import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { environment } from "src/environments/environment";
import { SalesInvoiceDetailsModel } from "../Models/sales-invoice-details.model";
import { SalesInvoiceModel } from "../Models/sales-invoice.model";
import { SharedService } from "src/app/shared/common-methods";

@Injectable({ 
    providedIn: 'root',
})
export class SalesDetailsService {
    baseUrl = environment.baseUrlUMS;

  constructor(private http: HttpClient, private sharedService: SharedService) { }

    getSalesDetailsList(invoiceId: number): Observable<Array<SalesInvoiceDetailsModel>> {
        return this.http
            .get<any>(`${environment.baseUrlInventory}/api/SalesDetails/GetByInvoiceId/${invoiceId}`)
            .pipe(map(response => response));
    }
}