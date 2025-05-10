import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from 'src/app/shared/shared.module';
import { BusinessOperationsRoutingModule } from './business-operations-routing.module';
import { BusinessOperationsComponent } from './business-operations.component';
import { PurchaseComponent } from './purchase/purchase.component';
import { 
  NbAccordionModule, 
  NbAutocompleteModule, 
  NbButtonModule, 
  NbCardModule, 
  NbDatepickerModule, 
  NbFormFieldModule, 
  NbIconModule, 
  NbInputModule, 
  NbListModule, 
  NbOptionModule, 
  NbRadioModule, 
  NbSelectModule, 
  NbStepperModule, 
  NbTabsetModule, 
  NbTooltipModule } from '@nebular/theme';
import { PurchaseDetailsComponent } from './purchase/purchase-details/purchase-details.component';
import { AddPurchaseComponent } from './purchase/add-purchase/add-purchase.component';
import { ReactiveFormsModule } from '@angular/forms';
import { VendorsComponent } from './vendors/vendors.component';
import { AddVendorsComponent } from './vendors/add-vendors/add-vendors.component';
import { SalesComponent } from './sales/sales.component';
import { AddSalesComponent } from './sales/add-sales/add-sales.component';


@NgModule({
  declarations: [
    SalesComponent,
    VendorsComponent,
    AddSalesComponent,
    PurchaseComponent,
    AddVendorsComponent,
    AddPurchaseComponent,
    PurchaseDetailsComponent,
    BusinessOperationsComponent,
  ],
  imports: [
    CommonModule,
    NbCardModule,
    SharedModule,
    NbIconModule,
    NbListModule,
    NbRadioModule,
    NbButtonModule,
    NbInputModule,
    NbTooltipModule,
    NbTabsetModule,
    NbStepperModule,
    NbOptionModule,
    NbSelectModule,
    NbAccordionModule,
    NbFormFieldModule,
    NbDatepickerModule,
    ReactiveFormsModule,
    NbAutocompleteModule,
    BusinessOperationsRoutingModule
  ]
})

export class BusinessOperationsModule { }