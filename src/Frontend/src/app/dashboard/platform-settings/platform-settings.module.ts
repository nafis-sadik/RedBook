import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlatformSettingsRoutingModule } from './platform-settings-routing.module';
import { RouteSettingsComponent } from './route-settings/route-settings.component';
import { RouteFormComponent } from './route-form/route-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { NbButtonModule, NbCardModule, NbOptionModule, NbSelectModule, NbToggleModule } from '@nebular/theme';
import { PlatformSettingsComponent } from './platform-settings.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    RouteFormComponent,
    RouteSettingsComponent,
    PlatformSettingsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    NbSelectModule,
    NbOptionModule,
    NbSelectModule,
    NbCardModule,
    NbToggleModule,
    ReactiveFormsModule,
    NbButtonModule,
    PlatformSettingsRoutingModule
  ]
})

export class PlatformSettingsModule { }