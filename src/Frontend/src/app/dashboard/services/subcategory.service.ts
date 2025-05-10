import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { CachingService } from "./caching.service";
import { Observable, map, of } from "rxjs";
import { CategoryModel } from "../Models/category.model";

@Injectable({
    providedIn: 'root',
})
export class SubcategoryService{
    baseUrl = environment.baseUrlInventory;

    constructor(
        private http: HttpClient,
        private cacheingService: CachingService
    ) { }

    addNewSubcategory(categoryModel: CategoryModel): Observable<CategoryModel>{
        return this.http
            .post<CategoryModel>(`${this.baseUrl}/api/Subcategory`, categoryModel)
            .pipe(map((response) => response));
    }

    updateSubcategory(categoryModel: CategoryModel): Observable<CategoryModel>{
        return this.http
            .patch<CategoryModel>(`${this.baseUrl}/api/Subcategory/`, categoryModel)
            .pipe(map((response) => response));
    }

    deleteSubcategory(categoryId: number): Observable<CategoryModel>{
        return this.http
            .delete<CategoryModel>(`${this.baseUrl}/api/Subcategory/${categoryId}`)
            .pipe(map((response) => response));
    }

    getSubcategoriesUnderCategoryId(categoryId: number): Observable<Array<CategoryModel>>{
        let cachedData: Array<CategoryModel> = this.cacheingService.get(`${this.baseUrl}/api/Subcategory/${categoryId}`);
        if(!cachedData){
            return this.http
            .get<any>(`${this.baseUrl}/api/Subcategory/${categoryId}`)
            .pipe(map((response) => {
                this.cacheingService.set(`${this.baseUrl}/api/Subcategory/${categoryId}`, response);
                return response;
            }));
        }

        return of(cachedData);
    }
}
