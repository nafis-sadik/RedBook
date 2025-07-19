import { Component, Input } from '@angular/core';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { VariantInventoryStatusModel } from 'src/app/dashboard/Models/variant-inventory-status.model';
import { InventoryService } from 'src/app/dashboard/services/inventory.service';

@Component({
  selector: 'product-details-modal',
  templateUrl: './product-details-modal.component.html',
  styleUrl: './product-details-modal.component.scss'
})
export class ProductDetailsModalComponent {
  @Input() productModelInput: ProductModel;
  inventory: Array<VariantInventoryStatusModel> = [];

  constructor(private inventoryService: InventoryService) { }

  ngOnInit(): void {
    if (!this.productModelInput) return;
    this.inventoryService.getProductInventory(this.productModelInput.productId)
      .subscribe(response => {
        console.log('response', response);
        this.inventory = response;
      })
  }
}
