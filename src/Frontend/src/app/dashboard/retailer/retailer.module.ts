import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RetailerRoutingModule } from './retailer-routing.module';
import { OnboardingComponent } from './onboarding/onboarding.component';
import { NbButtonModule, NbCardModule, NbIconModule, NbInputModule, NbStepperModule } from '@nebular/theme';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { RouterModule } from '@angular/router';
import { RetailerComponent } from './retailer.component';
import { NbEvaIconsModule } from '@nebular/eva-icons';

@NgModule({
  declarations: [ OnboardingComponent, RetailerComponent ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NbStepperModule,
    NbCardModule,
    RetailerRoutingModule,
    SharedModule,
    NbButtonModule,
    NbInputModule,
    NbIconModule
  ]
})
export class RetailerModule { }
