import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { environment } from "src/environments/environment.development";
import { UserModel } from "../Models/user.model";
import { IPaginationModel } from "src/app/shared/ngx-pagination/Models/IPaginationModel";
import { SharedService } from "src/app/shared/common-methods";

@Injectable({
    providedIn: 'root',
})
export class EmployeeService {
    baseUrl = environment.baseUrlUMS;

    constructor(private http: HttpClient, private sharedService: SharedService) { }

    registerEmployee(userModel: UserModel, selectedBusinessId: number): Observable<UserModel> {
        return this.http
            .post<UserModel>(`${this.baseUrl}/api/Employee/${selectedBusinessId}`, userModel)
            .pipe(map((response) => response));
    }

    updateEmployeeRoles(userModel: UserModel, orgId: number): Observable<UserModel> {
        return this.http
            .put<UserModel>(`${this.baseUrl}/api/Employee/UpdateEmployeeRoles/${orgId}`, userModel)
            .pipe(map((response) => response));
    }

    getPagedEmployeeList(pagedUserModel: IPaginationModel<UserModel>, orgId: number): Observable<IPaginationModel<UserModel>> {
        return this.http
            .get<IPaginationModel<UserModel>>(`${this.baseUrl}/api/Employee/Paged/${orgId}`, { params: this.sharedService.paginationToParams(pagedUserModel) })
            .pipe(map((response) => response));
    }
}