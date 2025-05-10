import { Injectable } from "@angular/core";
import { SalesInvoiceModel } from "../Models/sales-invoice.model";

@Injectable({
    providedIn: 'root',
})

export class SalesService {
  getSalesList(outletId: number, pageNumber: number, pageLength: number, searchString: string): SalesInvoiceModel[] {
    let sourceData: SalesInvoiceModel[];
    if(outletId == 1){
      sourceData = [
        {
          id: 1,
          MemoNumber: 'KG-1728',
          NetTotal: 150000,
          PaymentAmount: 0,
          SalesDate: new Date().toISOString().slice(0, 10),
          ProductsSold: [],
          CustomerName: '',
          DeliveryLocation: '',
          CustomerPhoneNumber: '',
          Terms: '',
          // PaymentHistory: []
        },
        {
          id: 2,
          MemoNumber: 'KG-1729',
          NetTotal: 180000,
          PaymentAmount: 0,
          SalesDate: new Date().toISOString().slice(0, 10),
          ProductsSold: [],
          CustomerName: '',
          DeliveryLocation: '',
          CustomerPhoneNumber: '',
          Terms: '',
          // PaymentHistory: []
        }
      ];
    } else if (outletId == 2) {
      sourceData = [
        {
          id: 1,
          MemoNumber: 'FM-1728',
          NetTotal: 2000,
          PaymentAmount: 0,
          SalesDate: new Date().toISOString().slice(0, 10),
          ProductsSold: [],
          CustomerName: '',
          DeliveryLocation: '',
          CustomerPhoneNumber: '',
          Terms: '',
          // PaymentHistory: []
        },
        {
          id: 2,
          MemoNumber: 'FM-1729',
          NetTotal: 1800,
          PaymentAmount: 0,
          SalesDate: new Date().toISOString().slice(0, 10),
          ProductsSold: [],
          CustomerName: '',
          DeliveryLocation: '',
          CustomerPhoneNumber: '',
          Terms: '',
          // PaymentHistory: []
        }
      ];
    } else {
      sourceData = []
    }

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
