import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RetailerComponent } from './retailer.component';
import { OnboardingComponent } from './onboarding/onboarding.component';

const routes: Routes = [
  {
    path: '',
    component: RetailerComponent,
    children: [
      {
        path: '',
        component: OnboardingComponent
      },
      {
        path: 'onboarding',
        component: OnboardingComponent
      }
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class RetailerRoutingModule { }
