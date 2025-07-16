import { Component, ElementRef, Input, OnInit, QueryList, ViewChildren } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { PurchaseInvoiceDetailsModel } from 'src/app/dashboard/Models/purchase-invoice-details.model';
import { PurchaseInvoiceModel } from 'src/app/dashboard/Models/purchase-invoice.model';
import { ProductTrackerComponent } from 'src/app/dashboard/product-management/product-tracker/product-tracker.component';
import { PurchaseDetailsService } from 'src/app/dashboard/services/purchase-details.service';

@Component({
  selector: 'app-purchase-details',
  templateUrl: './purchase-details.component.html',
  styleUrls: ['./purchase-details.component.scss']
})
export class PurchaseDetailsComponent implements OnInit {
  @Input() invoiceModel: PurchaseInvoiceModel;
  @Input() outletName: string;

  constructor(private invoiceDetailsService: PurchaseDetailsService, private dialogService: NbDialogService) { }
  @ViewChildren('barcode') barcodeElements!: QueryList<ElementRef>;
  ngOnInit(): void {
    this.invoiceDetailsService
      .getPurchaseDetailsList(this.invoiceModel.invoiceId)
      .subscribe(invoiceDetails => {
        this.invoiceModel.purchaseDetails = invoiceDetails;
        for (let index = 0; index < invoiceDetails.length; index++) {
          invoiceDetails[index].totalCostPrice = invoiceDetails[index].quantity * invoiceDetails[index].purchasePrice;
          invoiceDetails[index].totalCostPrice -= invoiceDetails[index].purchaseDiscount;
        }
      });
  }

  viewBarcode(purchaseInvoiceDetailsModel: PurchaseInvoiceDetailsModel) {
    this.dialogService.open(ProductTrackerComponent, {
      context: {
        invoiceDetailsModel: purchaseInvoiceDetailsModel,
        invoiceNumber: this.invoiceModel.chalanNumber,
        outletName: this.outletName
      }
    });
  }
}
