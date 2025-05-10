import { Component } from '@angular/core';
import { AppConfigurationService } from '../../services/app-config.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbStepperComponent, NbToastrService } from '@nebular/theme';
import { OrganizationService } from '../../services/organization.service';
import { OrganizationModel } from '../../Models/organization.model';
import { UserModel } from '../../Models/user.model';
import { OnboardingModel } from '../../Models/onboarding.model';
import { OnboardingService } from '../../services/onboarding.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-onboarding',
  templateUrl: './onboarding.component.html',
  styleUrls: ['./onboarding.component.scss']
})
export class OnboardingComponent {
  onboardignModel: OnboardingModel;
  onboardingFormGroup: FormGroup;

  AdminUserForm: FormGroup;
  OrganizationForm: FormGroup;
  loaderContainer: HTMLElement| null;

  isMobile: boolean;
  linearMode: boolean = true;
  
  userModel: UserModel;
  orgModel: OrganizationModel;
  
  constructor(
    private fb: FormBuilder,
    private onboardingService: OnboardingService,
    // private blumeDrop: BlumeDropDirective,
    private toasterService: NbToastrService,
    private organizationService: OrganizationService,
    private appConfigService: AppConfigurationService,
  ) { this.loaderContainer = document.getElementById('LoadingScreen'); }

  ngOnInit() {
    this.isMobile = this.appConfigService.isMobilePhone();
    
    this.OrganizationForm = this.fb.group({
      organizationName: ['', Validators.required],
      organizationAddress: ['', Validators.required],
      subscriptionFee: [1000, Validators.required],
    });

    this.AdminUserForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', Validators.required],
      accountBalance: ['', Validators.required],
    });

    // Organization model binding
    this.OrganizationForm?.valueChanges.subscribe(value => {
      this.orgModel = value;
    });

    // Admin User model binding
    this.AdminUserForm?.valueChanges.subscribe(value => {
      this.userModel = value;
    });
    
    setTimeout(() => {
      if(this.loaderContainer){
        this.loaderContainer.classList.remove('d-block');
        this.loaderContainer.classList.add('d-none');
      }
    }, 1.5 * 1000);
  }

  addNewOrganization(stepper: NbStepperComponent): void{
    if(!this.OrganizationForm.valid){
      this.OrganizationForm.markAllAsTouched();
      return;
    }

    this.organizationService.addNewOrganization(this.orgModel)
    .subscribe(
      (response) => {
        this.toasterService.success('Operation Successfull', 'Organization added successfully');
        this.orgModel = response;
      },
      (err) => {
        console.log(err)
        let errRes = err.error.split(',');
        let mainMsg: string = '';
        let subMsg: string = '';
        if(errRes.length > 0){
          mainMsg = errRes[0].charAt(0).toUpperCase() + errRes[0].slice(1);
        }
        if(errRes.length > 1){
          subMsg = errRes[1];
        } else {
          subMsg = 'Error';
        }
        this.toasterService.danger(mainMsg, subMsg);
        console.log(err);
      });

      stepper.next();
  }

  imgRawData: string = '';
  handleOutputFiles(event: any) {
    console.log('files', event[0]);
    if (document.getElementsByClassName('drop-label').length > 0) {
      let elem = document.getElementsByClassName('drop-label')[0];
      // elem.innerHTML = '';
      this.imgRawData = event[0];
      let data = document.getElementsByClassName('drop-view');
      console.log('data', elem.innerHTML);
    }
  }

  /**
   * Resets all forms and the stepper to their initial state.
   * @param stepper - The NbStepperComponent instance to reset.
   */
  resetAllForms(stepper: NbStepperComponent): void {
    this.orgModel = new OrganizationModel();
    this.userModel = new UserModel();
    stepper.reset();
  }

  /**
   * Validates the organization registration details and moves to the next step in the stepper if the details are valid.
   * @param stepper - The NbStepperComponent instance to move to the next step.
   */
  validateOrgReg(stepper: NbStepperComponent) {
    if (
      this.orgModel != undefined &&
      this.orgModel.organizationName.length > 0 &&
      this.orgModel.organizationAddress.length > 0 &&
      this.orgModel.subscriptionFee >= environment.minimumSubscriptionFee
    )
      stepper.next();
    else {
      if (
        this.orgModel == undefined ||
        this.orgModel.organizationName.length > 0 ||
        this.orgModel.organizationAddress.length > 0
      ) {
        this.toasterService.danger(
          'Please provide valid organization details',
          'Invalid Organization Details'
        );
        return;
      }
      if (this.orgModel.subscriptionFee < environment.minimumSubscriptionFee) {
        this.toasterService.danger(
          'Subscription fee can not be lower than ' + environment.minimumSubscriptionFee,
          'Invalid Subscription Fee'
        );
        return;
      }
    }
  }

  /**
   * Onboards the user with the provided user and organization models.
   */
  onboardUser(stepper: NbStepperComponent): void {
    this.onboardingService.onboardUser(this.userModel, this.orgModel)
      .subscribe((response) => {
        console.log('response', response);
        this.toasterService.success('Operation Successfull', 'User onboarded successfully');
        this.resetAllForms(stepper);
      });
  }
}
