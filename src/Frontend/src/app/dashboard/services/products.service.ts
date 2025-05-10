import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { Observable, map } from "rxjs";
import { IPaginationModel } from "src/app/shared/ngx-pagination/Models/IPaginationModel";
import { ProductModel } from "../Models/product.model";
import { SharedService } from "src/app/shared/common-methods";

@Injectable({
    providedIn: 'root',
})

export class ProductService {
  baseUrl = environment.baseUrlInventory;

  constructor(private http: HttpClient, private sharedService: SharedService) { }

  getProductList(outletId: number): Observable<Array<ProductModel>> {
    return this.http
      .get<Array<ProductModel>>(`${environment.baseUrlInventory}/api/Product/List/${outletId}`)
      .pipe(map(response => response));
  }

  getProductListPaged(outletId: number, pagedProductModel: IPaginationModel<ProductModel>): Observable<IPaginationModel<ProductModel>>{
    let paramsObject: HttpParams = this.sharedService.paginationToParams<ProductModel>(pagedProductModel);
    return this.http
      .get<IPaginationModel<ProductModel>>(`${environment.baseUrlInventory}/api/Product/${outletId}`, { params: paramsObject })
      .pipe(map(response => response));
    // if(pagedProductModel.searchingConfig?.searchString){
    //   return this.http
    //       .get<any>(`${this.baseUrl}/api/Product/${outletId}?PageNumber=${pagedProductModel.pagingConfig?.pageNumber}&PageLength=${pagedProductModel.pagingConfig?.pageLength}&SearchString=${pagedProductModel.searchingConfig?.searchString}`)
    //       .pipe(map(response => response ));
    // } else {
    //   return this.http
    //       .get<any>(`${this.baseUrl}/api/Product/${outletId}?PageNumber=${pagedProductModel.pagingConfig?.pageNumber}&PageLength=${pagedProductModel.pagingConfig?.pageLength}`)
    //       .pipe(map(response => response ));
    // }
  }

  addProduct(productModel: ProductModel): Observable<ProductModel> {
    return this.http
      .post<ProductModel>(`${this.baseUrl}/api/Product`, productModel)
      .pipe(map((response: ProductModel) => response));
  }

  updateProduct(productModel: ProductModel): Observable<ProductModel> {
    return this.http
      .patch<ProductModel>(`${this.baseUrl}/api/Product/${productModel.productId}`, productModel)
      .pipe(map((response: ProductModel) => response));
  }
}
