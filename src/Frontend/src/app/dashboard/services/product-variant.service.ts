import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { ProductVariantModel } from "../Models/product-variant.model";
import { map, Observable } from "rxjs";

@Injectable({
  providedIn: 'root',
})

export class ProductVariantService {
  baseUrl = environment.baseUrlInventory;

  constructor(private http: HttpClient) { }

  // getProductVariantList(outletId: number): Observable<Array<ProductVariantModel>> {
  //   return this.http
  //     .get<Array<ProductVariantModel>>(`${environment.baseUrlInventory}/api/Product/${outletId}`)
  //     .pipe(map(response => response));
  // }

  getProductVariants(productId: number): Observable<Array<ProductVariantModel>> {
    return this.http
      .get<Array<ProductVariantModel>>(`${environment.baseUrlInventory}/api/ProductVariant/Product/${productId}`)
      .pipe(map(response => response));
  }
}