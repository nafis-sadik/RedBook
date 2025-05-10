import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment.development";
import { Observable, map } from "rxjs";
import { RoleModel } from "../Models/role.model";

@Injectable({ 
    providedIn: 'root',
})
export class RoleService{
    baseUrl = environment.baseUrlUMS;

    constructor(private http: HttpClient) { }

    getOrganizationRoles(orgId: number): Observable<RoleModel[]> {
        return this.http
            .get<Array<any>>(`${this.baseUrl}/api/Role/Organization/${orgId}`)
            .pipe(map((response) => response));
    }

    addRole(role: RoleModel): Observable<RoleModel> {
        return this.http
            .post<RoleModel>(`${this.baseUrl}/api/Role`, role)
            .pipe(map((response) => response));
    }

    updateRole(role: RoleModel): Observable<RoleModel> {
        return this.http
            .put<RoleModel>(`${this.baseUrl}/api/Role`, role)
            .pipe(map((response) => response));
    }

    deleteRole(roleId: number): Observable<any> {
        return this.http
            .delete(`${this.baseUrl}/api/Role/${roleId}`)
            .pipe(map((response) => response));
    }

    mapRolesWithRoute(roleId: number, routeId: number): Observable<any>{ 
        return this.http
            .get<Array<any>>(`${this.baseUrl}/AllowRouteForRole/${roleId}/${routeId}`)
            .pipe(map(response => response));
    }
}