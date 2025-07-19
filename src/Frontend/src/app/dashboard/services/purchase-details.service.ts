import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PurchaseInvoiceDetailsModel } from "../Models/purchase-invoice-details.model";
import { Observable, map } from "rxjs";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root',
})

export class PurchaseDetailsService {
    constructor(private http: HttpClient) { }

    getPurchaseDetailsList(purchaseId: number): Observable<Array<PurchaseInvoiceDetailsModel>> {
        return this.http
            .get<any>(`${environment.baseUrlInventory}/api/PurchaseDetails/List/${purchaseId}`)
            .pipe(map(response => response));
    }
}