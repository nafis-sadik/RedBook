import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UmsComponent } from './ums/ums.component';
import { RoleManagementComponent } from './role-management/role-management.component';
import { SettingsComponent } from './settings.component';

const routes: Routes = [  
  {
    path: '',
    component: SettingsComponent,
    children: [
      { path: '', component: UmsComponent },
      { path: 'ums', component: UmsComponent },
      { path: 'role-management', component: RoleManagementComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule {
  constructor() {
    console.log('Settings Routing Module') 
  }
}
