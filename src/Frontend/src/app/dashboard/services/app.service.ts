import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map, of } from "rxjs";
import { environment } from "src/environments/environment";
import { CachingService } from "./caching.service";
import { IApplicationModel } from "../Models/IApplicationModel";

@Injectable({
  providedIn: 'root',
})
export class AppService {
  baseUrl = environment.baseUrlUMS;

  constructor(
    private http: HttpClient,
    private cacheingService: CachingService
  ) { }

  getAllApplications(): Observable<Array<IApplicationModel>> {
    let cachedData: Array<IApplicationModel> = this.cacheingService.get(`${this.baseUrl}/api/Application/GetAll`);
    if (!cachedData) {
      return this.http
        .get<Array<IApplicationModel>>(`${this.baseUrl}/api/Application/GetAll`)
        .pipe(map((response) => {
          this.cacheingService.set(`${this.baseUrl}/api/Application/GetAll`, response)
          return response
        }));
    }

    return of(cachedData);
  }
}
