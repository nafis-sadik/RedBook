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
        this.inventory = response;
        for (let index = 0; index < this.inventory.length; index++) {
          let element: VariantInventoryStatusModel = this.inventory[index];
          element.totalStockQuantity = 0;
          for (let i = 0; i < element.lots.length; i++) {
            let lot = element.lots[i];
            if (lot.purchaseDate != null) {
              lot.purchaseDate = new Date(lot.purchaseDate).toLocaleString();
              lot.purchaseDate = lot.purchaseDate.split(',').join(' - ');
            }
            element.totalStockQuantity += lot.quantity;
          }
        }
      })
  }
}
