import { Injectable } from "@angular/core";
import { UserModel } from "../Models/user.model";
import { OrganizationModel } from "../Models/organization.model";
import { environment } from "src/environments/environment.development";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root',
})
export class OnboardingService {
    baseUrl = environment.baseUrlUMS;

    constructor(private http: HttpClient) {}
    
    public onboardUser(userModel: UserModel, orgModel: OrganizationModel) {
        let model = {
            User: userModel,
            Organization: orgModel
        };
        return this.http.post<any>(
            `${this.baseUrl}/api/Onboarding/Redbook`, model);
    }
}