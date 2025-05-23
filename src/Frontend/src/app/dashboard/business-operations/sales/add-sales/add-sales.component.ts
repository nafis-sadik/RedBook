import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbStepperComponent } from '@nebular/theme';
import { CustomerModel } from 'src/app/dashboard/Models/customer.model';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { CustomerService } from 'src/app/dashboard/services/customer.services';
import { ProductService } from 'src/app/dashboard/services/products.service';

@Component({
  selector: 'app-add-sales',
  templateUrl: './add-sales.component.html',
  styleUrls: ['./add-sales.component.scss']
})

export class AddSalesComponent {
  @ViewChild('SalesMemo') salesMemoStepper: NbStepperComponent;
  
  @Input() selectOrganization: number = 0;

  // Customer forms
  @Input() customerModel: CustomerModel;
  customerDetailsForm: FormGroup;
  customerContactNumbers: Array<string> = [];

  // Sales form
  productDetailsForm: FormGroup;
  outletProductList: Array<ProductModel> = [];
  selectedProductList: Array<ProductModel> = [];
  invoicePrice: number = 0;

  // salesModel: ISalesModel;
  memoProductId: number = 0;

  constructor(
    private formBuilder: FormBuilder, 
    private customerService: CustomerService, 
    private productService: ProductService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.customerModel = new CustomerModel();
    this.customerModel.orgId = this.selectOrganization;
    this.initializeCustomerForm();
  }

  initializeCustomerForm(): void {
    if(this.customerModel != null){
      this.customerDetailsForm = this.formBuilder.group({
        customerName: [this.customerModel.customerName, [Validators.maxLength(100)]],
        customerPhoneNumber: [this.customerModel.contactNumber, [Validators.required, Validators.minLength(10)]],
        email: [this.customerModel.email, [Validators.email]],
        deliveryLocation: [this.customerModel.address],
        remarks: [this.customerModel.remarks],
      });
    }
    
    this.customerDetailsForm.valueChanges.subscribe(formData => {
      if (this.customerModel == null) return;

      this.customerModel.customerName = formData.customerName;
      this.customerModel.contactNumber = String(formData.customerPhoneNumber);
      this.customerModel.email = formData.email;
      this.customerModel.address = formData.deliveryLocation;
      this.customerModel.remarks = formData.remarks;

      if (this.customerModel.contactNumber.length >= 8 && this.selectOrganization > 0) {
        this.customerService.getCustomerByContactNumber(this.customerModel.contactNumber, this.selectOrganization)
          .subscribe((similarCustomerContactNumbers: Array<string>) => {
            this.customerContactNumbers = similarCustomerContactNumbers;
          });
      }
    });
  }

  syncCustomerInfo(skip: boolean): void {
    if(!skip) {
      this.customerService.addCustomer(this.customerModel)
      .subscribe((response: CustomerModel) => {
        this.customerModel = response;
        this.initializeProductDetailsForm();
        this.salesMemoStepper.next();
      });
    } else {
      this.customerModel = new CustomerModel();
      this.salesMemoStepper.next();
      this.initializeProductDetailsForm();
    }
  }

  initializeProductDetailsForm(): void {
    alert(this.selectOrganization);
    this.productService.getProductList(this.selectOrganization)
      .subscribe(prodArr => this.outletProductList = prodArr);
  }

  selectProductToSell(selectedProductId: number): void {
    let selectedProd: ProductModel | undefined = this.outletProductList.find(x => x.productId == selectedProductId);
    if(selectedProd == undefined || selectedProd == null) return;

    this.selectedProductList.push(selectedProd);
    this.cdRef.detectChanges();
    this.updateProductNetTotalAmount();
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

  typingPhoneNumber(): void {
    this.customerDetailsForm.disable();
    let typedPhoneNumber: string = this.customerModel.contactNumber;

    if(typedPhoneNumber.length > 8 || typedPhoneNumber.includes('@')){
      this.customerService.searchCustomer(typedPhoneNumber, this.selectOrganization)
        .subscribe((customerData: CustomerModel) => {
          this.customerDetailsForm.enable();
          this.customerModel = customerData;
          this.cdRef.detectChanges();
        });
    }
  }
}
