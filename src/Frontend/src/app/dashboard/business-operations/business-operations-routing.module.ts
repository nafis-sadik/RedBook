import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PurchaseComponent } from './purchase/purchase.component';
import { BusinessOperationsComponent } from './business-operations.component';
import { VendorsComponent } from './vendors/vendors.component';
import { PosComponent } from './pos/pos.component';
import { SalesComponent } from './sales/sales.component';

const routes: Routes = [
  {
    path: '',
    component: BusinessOperationsComponent,
    children: [
      {
        path: '',
        component: PurchaseComponent
      },
      {
        path: 'purchase',
        component: PurchaseComponent
      },
      {
        path: 'sales',
        component: SalesComponent
      },
      {
        path: 'pos',
        component: PosComponent
      },
      {
        path: 'vendors',
        component: VendorsComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class BusinessOperationsRoutingModule { }
