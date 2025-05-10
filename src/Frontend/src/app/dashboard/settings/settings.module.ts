import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsRoutingModule } from './settings-routing.module';
import { 
  NbButtonModule,
  NbCardModule, 
  NbCheckboxModule, 
  NbIconModule, 
  NbInputModule, 
  NbListModule, 
  NbOptionModule, 
  NbRadioModule, 
  NbSelectModule, 
  NbToggleModule 
} from '@nebular/theme';
import { SettingsComponent } from './settings.component';
import { UmsComponent } from './ums/ums.component';
import { UserFormComponent } from './ums/user-form/user-form.component';
import { RoleManagementComponent } from './role-management/role-management.component';
import { RoleFormComponent } from './role-management/role-form/role-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    SettingsComponent,
    UmsComponent,
    UserFormComponent,
    RoleManagementComponent,
    RoleFormComponent
  ],
  imports: [
    CommonModule,
    SettingsRoutingModule,
    NbCardModule,
    NbOptionModule,
    NbSelectModule,
    NbRadioModule,
    NbListModule,
    NbCheckboxModule,
    NbIconModule,
    NbToggleModule,
    NbOptionModule,
    NbButtonModule,
    NbInputModule,
    ReactiveFormsModule,
    SharedModule
  ]
})

export class SettingsModule {
  constructor() { console.log('Settings module'); }
}