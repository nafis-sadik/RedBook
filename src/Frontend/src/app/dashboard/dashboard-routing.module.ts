import { NgModule } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';

const routes: Routes = [{
  path: '',
  component: DashboardComponent,
  children: [
    {
      path: '',
      component: HomeComponent
    },
    {
      path: 'home',
      component: HomeComponent
    },
    {
      path: 'settings',
      loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule)
    },
    {
      path: 'products',
      loadChildren: () => import('./product-management/product-management.module').then(m => m.ProductManagementModule)
    },
    {
      path: 'operations',
      loadChildren: () => import('./business-operations/business-operations.module').then(m => m.BusinessOperationsModule)
    },
    {
      path: 'retailer',
      loadChildren: () => import('./retailer/retailer.module').then(m => m.RetailerModule)
    },
    {
      path: 'platform-settings',
      loadChildren: () => import('./platform-settings/platform-settings.module').then(m => m.PlatformSettingsModule)
    }
  ]
}]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})

export class DashboardRoutingModule { }
