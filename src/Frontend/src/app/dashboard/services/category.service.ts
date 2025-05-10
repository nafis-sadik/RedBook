import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { Observable, map, of } from "rxjs";
import { CategoryModel } from "../Models/category.model";

@Injectable({
    providedIn: 'root',
})
export class CategoryService{
    baseUrl = environment.baseUrlInventory;

    constructor(private http: HttpClient) { }

    addNewCategory(categoryModel: CategoryModel): Observable<CategoryModel>{
        return this.http
            .post<CategoryModel>(`${this.baseUrl}/api/Category`, categoryModel)
            .pipe(map((response) => response));
    }

    updateCategory(categoryModel: CategoryModel): Observable<CategoryModel>{
        return this.http
            .patch<CategoryModel>(`${this.baseUrl}/api/Category/`, categoryModel)
            .pipe(map((response) => response));
    }

    deleteCategory(categoryId: number): Observable<CategoryModel>{
        return this.http
            .delete<CategoryModel>(`${this.baseUrl}/api/Category/${categoryId}`)
            .pipe(map((response) => response));
    }

    getCategoriesUnderOrganization(orgId: number): Observable<Array<CategoryModel>>{
        return this.http
            .get<Array<CategoryModel>>(`${this.baseUrl}/api/Category/${orgId}`)
            .pipe(map(response => response));
    }
}
