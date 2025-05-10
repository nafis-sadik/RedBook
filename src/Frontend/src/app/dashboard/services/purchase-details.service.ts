import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PurchaseInvoiceDetailsModel } from "../Models/purchase-invoice-details.model";
import { Observable, Subject, map } from "rxjs";
import { SharedService } from "src/app/shared/common-methods";
import { environment } from "src/environments/environment.development";

@Injectable({
    providedIn: 'root',
})

export class PurchaseDetailsService{
    constructor(private http: HttpClient) { }
    
    getPurchaseDetailsList(purchaseId: number): Observable<Array<PurchaseInvoiceDetailsModel>> {
        return this.http
        .get<any>(`${environment.baseUrlInventory}/api/PurchaseDetails/List/${purchaseId}`)
        .pipe(map(response => response));
    }
}