import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { CustomerModel } from "../Models/customer.model";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root',
})

export class CustomerService {
    baseUrl = environment.baseUrlInventory;

    constructor(private http: HttpClient) { }

    searchCustomer(searchString: string, orgId: number): Observable<CustomerModel> {
        return this.http
            .post<CustomerModel>(`${this.baseUrl}/api/Customer/SearchCustomer/`, {
                searchString: searchString,
                orgId: orgId
            }).pipe(map((response) => response));
    }

    getCustomerByContactNumber(searchString: string, orgId: number): Observable<Array<string>> {
        return this.http
            .post<Array<string>>(`${this.baseUrl}/api/Customer/SearchSimilarNumbers/`, {
                searchString: searchString,
                orgId: orgId
            }, { headers: { 'Content-Type': 'application/json' } })
            .pipe(map((response) => response));
    }

    addCustomer(customerModel: CustomerModel): Observable<CustomerModel> {
        return this.http
            .post<CustomerModel>(`${this.baseUrl}/api/Customer`, customerModel)
            .pipe(map((response) => response));
    }
}