import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Observable, map } from "rxjs";
import { ProductVariantModel } from "../Models/product-variant.model";
import { PurchaseInvoiceDetailsModel } from "../Models/purchase-invoice-details.model";

@Injectable({
    providedIn: 'root',
})

export class InventoryService {
    baseUrl = environment.baseUrlInventory;

    constructor(private http: HttpClient) { }

    getVariantInventory(variantId: number): Observable<Array<PurchaseInvoiceDetailsModel>> {
        return this.http
            .get<Array<PurchaseInvoiceDetailsModel>>(`${this.baseUrl}/api/Inventory/Variant/${variantId}`)
            .pipe(map((response) => response));
    }
}