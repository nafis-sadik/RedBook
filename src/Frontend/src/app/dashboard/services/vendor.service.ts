import { Injectable } from "@angular/core";
import { VendorModel } from "../Models/vendor.model";
import { IPaginationModel } from "src/app/shared/ngx-pagination/Models/IPaginationModel";
import { Observable, Subject } from "rxjs";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "src/environments/environment.development";
import { SharedService } from "src/app/shared/common-methods";

@Injectable({
    providedIn: 'root',
})
export class VendorService {
    private formData: Subject<VendorModel>;
    constructor(private http: HttpClient, private sharedService: SharedService) {
        this.formData = new Subject();
    }

    emitFormData(model: VendorModel): void {
        this.formData.next(model);
    }

    listenFormData(): Observable<VendorModel> {
        return this.formData.asObservable();
    }

    addVendor(model: VendorModel): Observable<VendorModel> {
        return this.http
            .post<VendorModel>(`${environment.baseUrlInventory}/api/Vendor`, model);
    }

    updateVendor(model: VendorModel): Observable<VendorModel> {
        return this.http
            .put<VendorModel>(`${environment.baseUrlInventory}/api/Vendor/${model.vendorId}`, model);
    }

    getList(outletId: number): Observable<Array<VendorModel>> {
        return this.http
            .get<Array<VendorModel>>(`${environment.baseUrlInventory}/api/Vendor/List/${outletId}`);
    }

    getPaged(pagedModel: IPaginationModel<VendorModel>): Observable<IPaginationModel<VendorModel>> {
        let paramsObject: HttpParams = this.sharedService.paginationToParams<VendorModel>(pagedModel);

        return this.http.get<IPaginationModel<VendorModel>>(`${environment.baseUrlInventory}/api/Vendor/PagedAsync`, { params: paramsObject });
    }

    getById(vendorId: number): Observable<VendorModel> {
        return this.http
            .get<VendorModel>(`${environment.baseUrlInventory}/api/Vendor/${vendorId}`);
    }
}