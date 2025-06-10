import { Injectable } from "@angular/core";
import { SalesInvoiceModel } from "../Models/sales-invoice.model";

@Injectable({
    providedIn: 'root',
})

export class SalesService {
  getSalesList(outletId: number, pageNumber: number, pageLength: number, searchString: string): SalesInvoiceModel[] {
    let sourceData: SalesInvoiceModel[] = [
        {
          invoiceId: 1,
          invoiceNumber: 'KG-1728',
          invoiceTotal: 150000,
          totalPaid: 0,
          salesDate: new Date().toISOString().slice(0, 10),
          customerName: '',
          terms: '',
          customerId: 5,
          organizationId: 6,
          paymentRecords: [],
          paymentStatus: "Partially paid",
          remarks: "beda kangta ase",
          salesDetails: [],
          totalDiscount: 0
        },
        {
          invoiceId: 1,
          invoiceNumber: 'KG-1729',
          invoiceTotal: 180000,
          totalPaid: 0,
          salesDate: new Date().toISOString().slice(0, 10),
          customerName: '',
          terms: '',
          customerId: 5,
          organizationId: 6,
          paymentRecords: [],
          paymentStatus: "Partially paid",
          remarks: "beda kangta ase",
          salesDetails: [],
          totalDiscount: 0
        }
      ];
      
    return sourceData;
  }

  getSoldProducts(memoNumber: string){
    if(memoNumber.toLowerCase().includes('kg'))
      return [
        {
          id: 1,
          categoryId: 1,
          categoryName: 'Motors',
          subcategoryId: 2,
          subcategoryName: 'EFI',
          productName: '4E-FE',
          purchasePrice: 80000,
          retailPrice: 100000,
          quantity: null
        },
        {
          id: 2,
          categoryId: 1,
          categoryName: 'Motors',
          subcategoryId: 2,
          subcategoryName: 'Classic',
          productName: '2JZ-GTE',
          purchasePrice: 80000,
          retailPrice: 100000,
          quantity: null
        },
        {
          id: 3,
          categoryId: 1,
          categoryName: 'Motors',
          subcategoryId: 2,
          subcategoryName: 'VVTi',
          productName: '2ZR-FE',
          purchasePrice: 80000,
          retailPrice: 100000,
          quantity: null
        }
      ];

    return [
      {
        id: 4,
        categoryId: 1,
        categoryName: 'Motors',
        subcategoryId: 2,
        subcategoryName: 'VVTi',
        productName: '2ZR-FE',
        purchasePrice: 80000,
        retailPrice: 100000,
        quantity: null
      },
      {
        id: 5,
        categoryId: 1,
        categoryName: 'Motors',
        subcategoryId: 2,
        subcategoryName: 'VVTi',
        productName: '2ZR-FE',
        purchasePrice: 80000,
        retailPrice: 100000,
        quantity: null
      }
    ];
  }
}
