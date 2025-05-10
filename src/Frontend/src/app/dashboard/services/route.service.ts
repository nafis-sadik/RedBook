import { Injectable } from "@angular/core";
import { RouteModel } from "../Models/route.model";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment.development";
import { Observable, Subject, map, of } from "rxjs";
import { IPaginationModel } from "src/app/shared/ngx-pagination/Models/IPaginationModel";
import { CachingService } from "./caching.service";
import { RouteTypeModel } from "../Models/route-type.model";

@Injectable({
  providedIn: 'root',
})
export class RouteService {
  private formData: Subject<RouteModel>;
  baseUrl = environment.baseUrlUMS;

  constructor(
    private http: HttpClient,
    private cachingService: CachingService
  ) { }

  emitFormData(model: RouteModel): void {
    this.formData.next(model);
  }

  listenFormData(): Observable<RouteModel> {
    return this.formData.asObservable();
  }

  addNewRoute(routeModel: RouteModel): Observable<RouteModel> {
    return this.http
      .post<RouteModel>(`${this.baseUrl}/api/Route`, routeModel)
      .pipe(map(response => {
        this.cachingService.remove(`${this.baseUrl}/api/Route/GetAppRoutes`);
        return response
      }));
  }

  updateNewRoute(routeModel: RouteModel): Observable<RouteModel> {
    return this.http
      .put<RouteModel>(`${this.baseUrl}/api/Route`, routeModel)
      .pipe(map(response => {
        this.cachingService.remove(`${this.baseUrl}/api/Route/GetAppRoutes`);
        return response
      }));
  }

  getPagedRoute(pagedRouteModel: IPaginationModel<RouteModel>): Observable<any> {
    if (pagedRouteModel.searchingConfig?.searchString) {
      return this.http
        .get<IPaginationModel<RouteModel>>(`${this.baseUrl}/api/Route/GetPaged?PageNumber=${pagedRouteModel.pagingConfig?.pageNumber}&PageLength=${pagedRouteModel.pagingConfig?.pageLength}&SearchString=${pagedRouteModel.searchingConfig?.searchString}`)
        .pipe(map(response => response));
    } else {
      return this.http
        .get<IPaginationModel<RouteModel>>(`${this.baseUrl}/api/Route/GetPaged?PageNumber=${pagedRouteModel.pagingConfig?.pageNumber}&PageLength=${pagedRouteModel.pagingConfig?.pageLength}`)
        .pipe(map(response => response));
    }
  }

  getRoutesByRole(roleId: number): Observable<Array<RouteModel>> {
    return this.http
      .get<Array<RouteModel>>(`${this.baseUrl}/api/Route/Role/${roleId}`)
      .pipe(map(response => response));
  }

  getMenuRoute(): Observable<Array<RouteModel>> {
    let cachedRoutes: Array<RouteModel> = this.cachingService.get(`${this.baseUrl}/api/Route/Menu`);
    if (!cachedRoutes) {
      return this.http
        .get<Array<RouteModel>>(`${this.baseUrl}/api/Route/Menu`)
        .pipe(map((response) => {
          if (response && response.length > 0)
            this.cachingService.set(`${this.baseUrl}/api/Route/Menu`, response);
          return response;
        }));
    }

    return of(cachedRoutes);
  }

  getAppRoute(): Observable<Array<RouteModel>> {
    let cachedRoutes: Array<RouteModel> = this.cachingService.get(`${this.baseUrl}/api/Route/App`);
    if (!cachedRoutes) {
      return this.http
        .get<Array<RouteModel>>(`${this.baseUrl}/api/Route/App`)
        .pipe(map((response) => {
          if (response && response.length > 0)
            this.cachingService.set(`${this.baseUrl}/api/Route/App`, response);
          return response;
        }));
    }

    return of(cachedRoutes);
  }

  getRouteTypes(): Observable<Array<RouteTypeModel>> {
    return this.http
      .get<Array<any>>(`${this.baseUrl}/api/Route/Types`)
      .pipe(map(response => response));
  }

  deleteRoute(routeId: number): Observable<any> {
    this.cachingService.remove(`${this.baseUrl}/api/Route/GetAppRoutes`);
    return this.http
      .delete(`${this.baseUrl}/api/Route/${routeId}`)
      .pipe(map(response => response));
  }
}
