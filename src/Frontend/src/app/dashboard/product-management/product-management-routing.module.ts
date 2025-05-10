import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { ProductSettingsComponent } from './product-settings/product-settings.component';
import { ProductManagementComponent } from './product-management.component';

const routes: Routes = [
  {
    path: '',
    component: ProductManagementComponent,
    children: [
      {
        path: '',
        component: ProductsComponent
      },
      {
        path: 'list',
        component: ProductsComponent
      },
      {
        path: 'settings',
        component: ProductSettingsComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductManagementRoutingModule { }
