import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Observable, map } from "rxjs";
import { PurchaseInvoiceDetailsModel } from "../Models/purchase-invoice-details.model";
import { VariantInventoryStatusModel } from "../Models/variant-inventory-status.model";

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

    getProductInventory(productId: number): Observable<Array<VariantInventoryStatusModel>> {
        return this.http
            .get<Array<VariantInventoryStatusModel>>(`${this.baseUrl}/api/Inventory/Product/${productId}`)
            .pipe(map((response) => response));
    }
}