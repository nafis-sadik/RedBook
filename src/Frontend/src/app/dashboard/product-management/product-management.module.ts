import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductManagementRoutingModule } from './product-management-routing.module';
import { NbAccordionModule, NbButton, NbButtonModule, NbCardModule, NbIconModule, NbInputModule, NbListModule, NbOptionModule, NbRadioModule, NbSelectModule, NbTooltipModule } from '@nebular/theme';
import { ProductManagementComponent } from './product-management.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProductsComponent } from './products/products.component';
import { ProductSettingsComponent } from './product-settings/product-settings.component';
import { QuantityUnitComponent } from './product-settings/quantity-unit/quantity-unit.component';
import { CategoryComponent } from './product-settings/category/category.component';
import { ProductsDetailsFormComponent } from './products/products-details-form/products-details-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ProductTrackerComponent } from './product-tracker/product-tracker.component';

@NgModule({
  declarations: [
    ProductManagementComponent,
    ProductsDetailsFormComponent,
    ProductSettingsComponent,
    QuantityUnitComponent,
    ProductsComponent,
    CategoryComponent,
    ProductTrackerComponent,
  ],
  imports: [
    CommonModule,
    NbCardModule,
    SharedModule,
    NbIconModule,
    NbListModule,
    NbRadioModule,
    NbInputModule,
    NbButtonModule,
    NbSelectModule,
    NbOptionModule,
    NbTooltipModule,
    NbAccordionModule,
    ReactiveFormsModule,
    ProductManagementRoutingModule,
  ]
})

export class ProductManagementModule { }
