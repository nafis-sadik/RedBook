import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { Observable, map, of } from "rxjs";
import { ICommonAttribute } from "../Models/ICommonAttribute";

@Injectable({
    providedIn: 'root',
})
export class CommonAttributeService{
    baseUrl = environment.baseUrlInventory;

    constructor(private http: HttpClient) { }

    addNewAttribute(commonAttributeModel: ICommonAttribute): Observable<ICommonAttribute>{
        return this.http
            .post<ICommonAttribute>(`${this.baseUrl}/api/CommonAttributes`, commonAttributeModel)
            .pipe(map((response) => response));
    }

    updateExistingAttribute(commonAttributeModel: ICommonAttribute): Observable<ICommonAttribute>{
        return this.http
            .patch<ICommonAttribute>(`${this.baseUrl}/api/CommonAttributes/`, commonAttributeModel)
            .pipe(map((response) => response));
    }

    getAttributes(attrType: string): Observable<Array<ICommonAttribute>>{
        return this.http
            .get<Array<ICommonAttribute>>(`${this.baseUrl}/api/CommonAttributes/${attrType}`)
            .pipe(map(response => response));
    }

    removeExistingAttribute(attributeId: number): Observable<ICommonAttribute>{
        return this.http
            .delete<ICommonAttribute>(`${this.baseUrl}/api/CommonAttributes/${attributeId}`)
            .pipe(map((response) => response));
    }
}