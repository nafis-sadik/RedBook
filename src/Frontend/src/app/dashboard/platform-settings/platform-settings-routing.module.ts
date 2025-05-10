import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PlatformSettingsComponent } from './platform-settings.component';
import { RouteSettingsComponent } from './route-settings/route-settings.component';

const routes: Routes = [
  {
    path: '',
    component: PlatformSettingsComponent,
    children: [
      {
        path: '',
        component: RouteSettingsComponent
      },
      {
        path: 'route-settings',
        component: RouteSettingsComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlatformSettingsRoutingModule { }
