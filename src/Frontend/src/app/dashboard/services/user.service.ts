import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map } from "rxjs";
import { environment } from "src/environments/environment.development";
import { UserModel } from "../Models/user.model";
import { CachingService } from "./caching.service";

@Injectable({ 
    providedIn: 'root',
})
export class UserService{
    baseUrl = environment.baseUrlUMS;

    constructor(
        private http: HttpClient,
        private cachingService: CachingService
    ) {}
    
    registerNewUser(userModel: UserModel):Observable<UserModel> {
        return this.http
            .post<UserModel>(`${this.baseUrl}/api/User`, userModel)
            .pipe(map((response) => response));
    }
    
    updateUser(userModel: UserModel):Observable<UserModel> {
        return this.http
            .put<UserModel>(`${this.baseUrl}/api/User`, userModel)
            .pipe(map((response) => response));
    }
}