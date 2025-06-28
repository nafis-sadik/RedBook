import { Component, ElementRef, Input, OnInit, QueryList, ViewChildren } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { SalesInvoiceModel } from 'src/app/dashboard/Models/sales-invoice.model';
import { SalesDetailsService } from 'src/app/dashboard/services/sales-details.service';

@Component({
  selector: 'app-Sales-details',
  templateUrl: './sales-details.component.html',
  styleUrls: ['./sales-details.component.scss']
})
export class SaleseDetailsComponent implements OnInit {
  @Input() invoiceModel: SalesInvoiceModel;
  @Input() outletName: string;

  constructor(private invoiceDetailsService: SalesDetailsService, private dialogService: NbDialogService) { }
  @ViewChildren('barcode') barcodeElements!: QueryList<ElementRef>;
  ngOnInit(): void {
    this.invoiceDetailsService
      .getSalesDetailsList(this.invoiceModel.invoiceId)
      .subscribe(invoiceDetails => {
        this.invoiceModel.salesDetails = invoiceDetails;
        for(let index = 0; index < invoiceDetails.length; index++){
          invoiceDetails[index].totalCostPrice = invoiceDetails[index].quantity * invoiceDetails[index].retailPrice;
          invoiceDetails[index].totalCostPrice -= invoiceDetails[index].retailDiscount;
        }
      });
  }
}
