import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbStepChangeEvent } from '@nebular/theme';
import { CustomerModel } from 'src/app/dashboard/Models/customer.model';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { CustomerService } from 'src/app/dashboard/services/customer.services';

@Component({
  selector: 'app-add-sales',
  templateUrl: './add-sales.component.html',
  styleUrls: ['./add-sales.component.scss']
})
export class AddSalesComponent {
  @Input() selectOrganization: number = 0;

  // Customer forms 
  @Input() allowCustomerFormEdit: boolean = false;
  clientDetailsForm: FormGroup;
  customerModel: CustomerModel = new CustomerModel();
  customerContactNumbers: Array<string> = [];

  linearMode = true;

  outletProductList: Array<ProductModel> = [];

  // salesModel: ISalesModel;
  memoProductIds: number[] = [];

  // selectedProductsForSale: IInvoiceProductModel[];

  constructor(private formBuilder: FormBuilder, private customerService: CustomerService) {
    // this.selectedProductsForSale = [];

    // this.salesModel = {
    //   id: 0,
    //   MemoNumber: "",
    //   NetTotal: 0,
    //   PaymentAmount: 0,
    //   SalesDate: new Date,
    //   ProductsSold: this.selectedProductsForSale,
    //   CustomerName: '',
    //   DeliveryLocation: '',
    //   CustomerPhoneNumber: '',
    //   Terms: '',
    //   PaymentHistory: []
    // }

    // this.salesModel.PaymentHistory = addSalesService.getPaymentsByMemoId(dashboardService.selectedOutletId);
    this.initializeCustomerForm();
  }

  initializeCustomerForm(): void {
    this.clientDetailsForm = this.formBuilder.group({
      customerName: ['', [Validators.maxLength(100)]],
      customerPhoneNumber: ['', [Validators.required, Validators.minLength(10)]],
      email: ['', [Validators.email]],
      deliveryLocation: [''],
      remarks: [''],
    });

    if(this.allowCustomerFormEdit){
      this.clientDetailsForm.disable();
    }
    else{
      this.clientDetailsForm.valueChanges.subscribe(formData => {
        this.customerModel.customerName = formData.customerName;
        this.customerModel.contactNumber = String(formData.customerPhoneNumber);
        this.customerModel.email = formData.email;
        this.customerModel.address = formData.deliveryLocation;
        this.customerModel.remarks = formData.remarks;
        
        if(this.customerModel.contactNumber.length >= 8 && this.selectOrganization > 0){
          this.customerService.getCustomerByContactNumber(this.customerModel.contactNumber, this.selectOrganization)
            .subscribe((similarCustomerContactNumbers: Array<string>) => {
              this.customerContactNumbers = similarCustomerContactNumbers;
            });
        }
      });
    }
  }

  syncCustomerInfo(event: NbStepChangeEvent): void {
    if(this.selectOrganization > 0){
      this.customerModel.orgId = this.selectOrganization;
      this.customerService.addCustomer(this.customerModel)
        .subscribe((response: CustomerModel) => {
          console.log('Response (CustomerModel)', response);
          this.customerModel = response;
          this.initializeProductDetailsForm();
        });
    }
  }

  initializePaymentDetailsForm() {
    throw new Error('Method not implemented.');
  }

  updateProductNetTotalAmount(): void {
    // this.salesModel.NetTotal = 0;
    // this.salesModel.ProductsSold.forEach(product=>{
    //   product.ProductNetTotalPrice = product.Quantity * product.RetailPrice;
    //   this.salesModel.NetTotal += product.ProductNetTotalPrice;
    // });
  }

  initializeProductDetailsForm(): void {
    // this.salesModel.ProductsSold = [];
    // this.outletProductList = this.productService.getProductList(this.dashboardService.selectedOutletId);
  }

  selectProductToSell(selectedProductIds: number[]): void {
    // This contains the previously selected object that are still selected
    // So, if the product was previously selected and now has been unselected, that is being removed here
    // let previouslySelectedItem: IInvoiceProductModel[] = [];
    // this.selectedProductsForSale.forEach(product => {
    //   selectedProductIds.forEach(productId => {
    //     if(product.ProductId == productId){
    //       previouslySelectedItem.push(product);
    //     }
    //   })
    // });

    // Load previously selected item removing the unselected items
    // this.selectedProductsForSale = previouslySelectedItem;

    // This shall add newly added products based on product selection from ui
    let newlyAddedProductId: number[] = [];
    let itemFound: boolean = false;
    selectedProductIds.forEach(productId => {
      itemFound = false;
      // previouslySelectedItem.forEach(product => {
      //   console.log(productId, product.ProductId, productId == product.ProductId)
      //   if(productId == product.ProductId){
      //     itemFound = true;
      //   }
      // })
      console.log(productId, itemFound)
      if(!itemFound){
        newlyAddedProductId.push(productId);
      }
    });

    if(newlyAddedProductId.length > 0){
      newlyAddedProductId.forEach(() => {
        this.outletProductList.forEach(() => {
          // if(productId == product.productId)
            // this.selectedProductsForSale.push({
            //   ProductId: product.productId,
            //   ProductName: product.productName,
            //   ProductNetTotalPrice: 0,
            //   PurchasePrice: product.purchasePrice,
            //   Quantity: 0,
            //   RetailPrice: product.retailPrice
            // });
        })
      })
    }

    // this.salesModel.ProductsSold = this.selectedProductsForSale;

    this.updateProductNetTotalAmount();
  }
}
