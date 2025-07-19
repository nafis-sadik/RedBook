import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { OrganizationModel } from "../Models/organization.model";
import { Observable, Subject, map, of } from "rxjs";
import { CachingService } from "./caching.service";
import { UserModel } from "../Models/user.model";
import { IPaginationModel } from "src/app/shared/ngx-pagination/Models/IPaginationModel";
import { SharedService } from "src/app/shared/common-methods";

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  baseUrl = environment.baseUrlUMS;
  private orgSubject: Subject<Array<OrganizationModel>>;

  constructor(
    private http: HttpClient,
    private cachingService: CachingService,
    private sharedService: SharedService
  ) {
    this.orgSubject = new Subject<Array<OrganizationModel>>();
  }

  addNewOrganization(orgModel: OrganizationModel): Observable<OrganizationModel> {
    return this.http
      .post<OrganizationModel>(`${this.baseUrl}/api/Organization`, orgModel)
      .pipe(map((response) => response));
  }

  updateOrganization(orgModel: OrganizationModel): Observable<OrganizationModel> {
    return this.http
      .put<OrganizationModel>(`${this.baseUrl}/api/Organization`, orgModel)
      .pipe(map(response => response));
  }

  deleteOrganization(orgId: number) {
    return this.http
      .delete(`${this.baseUrl}/api/Organization/${orgId}`)
      .pipe(map(response => response));
  }

  /// <summary>
  /// Retrieves a list of organizations for the current user.
  /// </summary>
  /// <returns>An observable that emits an array of <see cref="OrganizationModel"/> instances representing the user's organizations.</returns>
  /// <remarks>
  /// This method first checks the cache for the organization data. If the data is not cached, it makes a GET request to the server to retrieve the organization data, caches the response, and returns the data as an observable.
  /// </remarks>
  getUserOrgs(): Observable<OrganizationModel[]> {
    let cachedData = this.cachingService.get(`${this.baseUrl}/api/User/Business`);
    if (!cachedData) {
      return this.http
        .get<Array<OrganizationModel>>(`${this.baseUrl}/api/User/Business`)
        .pipe(
          map((response) => {
            this.cachingService.set(
              `${this.baseUrl}/api/User/Business`,
              response
            );
            return response;
          })
        );
    } else {
      return of(cachedData);
    }
  }

  getUserByBusinessId(pagedUserModel: IPaginationModel<UserModel>, businessId: number): Observable<any> {
    let params = this.sharedService.paginationToParams<UserModel>(pagedUserModel);

    return this.http
      .get<IPaginationModel<UserModel>>(`${this.baseUrl}/api/User/${businessId}`, { params })
      .pipe(map(response => response));
  }

  removeUserFromBusiness(userId: number, orgId: number): Observable<any> {
    return this.http
      .delete(`${this.baseUrl}/api/Organization/User/${userId}/${orgId}`)
      .pipe(map((response) => response));
  }
}
