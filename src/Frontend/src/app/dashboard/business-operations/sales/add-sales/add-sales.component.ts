import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbDialogService, NbStepperComponent } from '@nebular/theme';
import { CustomerModel } from 'src/app/dashboard/Models/customer.model';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { SalesInvoiceDetailsModel } from 'src/app/dashboard/Models/sales-invoice-details.model';
import { SalesInvoiceModel } from 'src/app/dashboard/Models/sales-invoice.model';
import { CustomerService } from 'src/app/dashboard/services/customer.services';
import { InventoryService } from 'src/app/dashboard/services/inventory.service';
import { ProductService } from 'src/app/dashboard/services/products.service';
import { LotSelectionComponent } from './lot-selection/lot-selection.component';
import { PurchaseInvoiceDetailsModel } from 'src/app/dashboard/Models/purchase-invoice-details.model';

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
  productList: Array<ProductModel> = [];

  orderDetailsForm: FormGroup;
  get orderDetailsFormArray(): FormArray { return this.orderDetailsForm.get('salesDetails') as FormArray; }
  orderInvoiceModel: SalesInvoiceModel = new SalesInvoiceModel();        

  constructor(
    private formBuilder: FormBuilder, 
    private customerService: CustomerService, 
    private productService: ProductService,
    private inventoryService: InventoryService,
    private diagService: NbDialogService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.customerModel = new CustomerModel();
    this.customerModel.orgId = this.selectOrganization;
    this.initializeCustomerForm();
    this.initializeOrderForm();
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
        this.salesMemoStepper.next();
      });
    } else {
      this.customerModel = new CustomerModel();
      this.customerModel.customerId = 0;
      this.salesMemoStepper.next();
    }
  }

  selectedOrderDetails: SalesInvoiceDetailsModel = new SalesInvoiceDetailsModel();
  initializeOrderForm(): void {
    this.orderDetailsForm = this.formBuilder.group({
      totalDiscount: [0, [Validators.required, Validators.min(0)]],
      salesDetails: this.formBuilder.array([], [Validators.required, Validators.min(1)]),
    });

    this.orderDetailsForm.valueChanges.subscribe(formData => {
      this.orderInvoiceModel.totalDiscount = formData.totalDiscount;
      this.orderInvoiceModel.invoiceTotal = 0;
      for(let i = 0; i < this.orderInvoiceModel.salesDetails.length; i++){
        this.orderInvoiceModel.salesDetails[i].quantity = formData.salesDetails[i].quantity;
        this.orderInvoiceModel.salesDetails[i].retailPrice = formData.salesDetails[i].retailPrice;
        this.orderInvoiceModel.salesDetails[i].totalCostPrice = this.orderInvoiceModel.salesDetails[i].quantity * this.orderInvoiceModel.salesDetails[i].retailPrice;
        this.orderInvoiceModel.salesDetails[i].totalCostPrice -= this.orderInvoiceModel.salesDetails[i].retailDiscount;
        this.orderInvoiceModel.invoiceTotal += this.orderInvoiceModel.salesDetails[i].totalCostPrice;
      }
      this.orderInvoiceModel.invoiceTotal -= this.orderInvoiceModel.totalDiscount;
    });
    this.productService.getProductList(this.selectOrganization)
      .subscribe(prodArr => this.productList = prodArr);
  }

  selectProduct(selectedProdId: number): void {
    this.selectedOrderDetails.productId = Number(selectedProdId);
    let selectedProd: ProductModel | undefined = this.productList.find(x => x.productId == this.selectedOrderDetails.productId);
    if(selectedProd == undefined || selectedProd == null) return;

    this.selectedOrderDetails.variants = this.productList.find(x => x.productId == this.selectedOrderDetails.productId)?.productVariants ?? [];
    this.selectedOrderDetails.productVariantId = 0;
    this.cdRef.detectChanges();
  }

  selectVariant(selectedVariantId: number): void {
    let selectedVariant = this.selectedOrderDetails.variants.find(x => x.variantId == Number(selectedVariantId));
    if(selectedVariant == undefined) return;
    let selectedProd = this.productList.find(x => x.productId == this.selectedOrderDetails.productId);
    if(selectedProd == undefined) return;

    this.selectedOrderDetails.productId = selectedProd.productId;
    this.selectedOrderDetails.productName = selectedProd.productName;
    this.selectedOrderDetails.productVariantId = selectedVariantId;
    this.selectedOrderDetails.productVariantName = selectedVariant.variantName;
  }

  addToCart(): void {
    if(this.selectedOrderDetails.productVariantId > 0){
      this.inventoryService.getVariantInventory(this.selectedOrderDetails.productVariantId)
        .subscribe((lotsAvailable: Array<PurchaseInvoiceDetailsModel>) => {
          this.diagService.open(LotSelectionComponent,{
              context: {
                availableLots: lotsAvailable,
                selectionCallback: (lot: PurchaseInvoiceDetailsModel) => {
                  this.selectedOrderDetails.lot = lot;
                  this.selectedOrderDetails.lotId = lot.recordId;
                  this.selectedOrderDetails.maxQuantity = lot.quantity;
                  this.selectedOrderDetails.maxRetailPrice = lot.retailPrice;
                  this.selectedOrderDetails.minRetailPrice = lot.retailPrice - lot.maxRetailDiscount;
                  this.orderInvoiceModel.salesDetails.push(this.selectedOrderDetails);

                  this.orderDetailsFormArray.push(this.formBuilder.group({
                    productId: [this.selectedOrderDetails.productId],
                    productName: [this.selectedOrderDetails.productName],
                    productVariantId: [this.selectedOrderDetails.productVariantId],
                    productVariantName: [this.selectedOrderDetails.productVariantName],
                    quantity: [1, [Validators.required, Validators.min(1), Validators.max(lot.quantity)]],
                    retailPrice: [
                        {
                          value: lot.retailPrice, 
                          disabled: this.selectedOrderDetails.maxRetailPrice == this.selectedOrderDetails.minRetailPrice
                        },
                        [
                          Validators.required, 
                          Validators.min(this.selectedOrderDetails.minRetailPrice), 
                          Validators.max(this.selectedOrderDetails.maxRetailPrice)]
                        ]
                      }));
                      
                  this.selectedOrderDetails = new SalesInvoiceDetailsModel();
                }
              }
            }
          );
        });
    }
  }

  initializePaymentDetailsForm() {
    throw new Error('Method not implemented.');
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
