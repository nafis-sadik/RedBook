import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbAutocompleteComponent, NbDialogService, NbStepperComponent, NbToastrService } from '@nebular/theme';
import { CustomerModel } from 'src/app/dashboard/Models/customer.model';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { SalesInvoiceDetailsModel } from 'src/app/dashboard/Models/sales-invoice-details.model';
import { SalesInvoiceModel } from 'src/app/dashboard/Models/sales-invoice.model';
import { CustomerService } from 'src/app/dashboard/services/customer.services';
import { InventoryService } from 'src/app/dashboard/services/inventory.service';
import { ProductService } from 'src/app/dashboard/services/products.service';
import { LotSelectionComponent } from './lot-selection/lot-selection.component';
import { PurchaseInvoiceDetailsModel } from 'src/app/dashboard/Models/purchase-invoice-details.model';
import { SalesService } from 'src/app/dashboard/services/sell.service';
import { consumerPollProducersForChange } from '@angular/core/primitives/signals';

@Component({
  selector: 'app-add-sales',
  templateUrl: './add-sales.component.html',
  styleUrls: ['./add-sales.component.scss']
})

export class AddSalesComponent {
  @ViewChild('SalesMemo') salesMemoStepper: NbStepperComponent;
  @ViewChild('autoControl') autoComplete!: NbAutocompleteComponent<{ name: string }>;

  @Input() selectedOrg: number = 0;

  // Customer forms
  @Input() customerModel: CustomerModel | null;
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
    private salesService: SalesService,
    private toastr: NbToastrService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.customerModel = new CustomerModel();
    this.customerModel.orgId = this.selectedOrg;
    this.initializeCustomerForm(this.customerModel);
    this.initializeOrderForm();
  }

  initializeCustomerForm(customerModel: CustomerModel): void {
    if (this.customerDetailsForm) {
      this.customerDetailsForm.patchValue({
        customerName: customerModel.customerName,
        customerPhoneNumber: customerModel.contactNumber,
        email: customerModel.email,
        deliveryLocation: customerModel.address,
        remarks: customerModel.remarks,
      });
      return;
    }
    this.customerDetailsForm = this.formBuilder.group({
      customerName: [customerModel.customerName, [Validators.maxLength(100)]],
      customerPhoneNumber: [customerModel.contactNumber, [Validators.required, Validators.minLength(10)]],
      email: [customerModel.email, [Validators.email]],
      deliveryLocation: [customerModel.address],
      remarks: [customerModel.remarks],
    });

    this.customerDetailsForm.valueChanges.subscribe(formData => {
      if (this.customerModel == null) return;

      this.customerModel.customerName = formData.customerName;
      this.customerModel.contactNumber = String(formData.customerPhoneNumber);
      this.customerModel.email = formData.email;
      this.customerModel.address = formData.deliveryLocation;
      this.customerModel.remarks = formData.remarks;

      if (this.customerModel.contactNumber.length >= 8 && this.selectedOrg > 0) {
        this.customerService.getCustomerByContactNumber(this.customerModel.contactNumber, this.selectedOrg)
          .subscribe((similarCustomerContactNumbers: Array<string>) => {
            this.customerContactNumbers = similarCustomerContactNumbers;
          });
      }
    });
  }

  syncCustomerInfo(skip: boolean): void {
    if (!skip && this.customerModel != null) {
      this.customerService.addCustomer(this.customerModel)
        .subscribe((response: CustomerModel) => {
          this.customerModel = response;
          this.customerModel.orgId = this.selectedOrg;
          this.salesMemoStepper.next();
        });
    } else {
      this.customerModel = null;
      this.salesMemoStepper.linear = false;
      this.salesMemoStepper.next();
      this.salesMemoStepper.linear = true;
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
      for (let i = 0; i < this.orderInvoiceModel.salesDetails.length; i++) {
        this.orderInvoiceModel.salesDetails[i].quantity = formData.salesDetails[i].quantity;
        if (this.orderInvoiceModel.salesDetails[i].maxRetailPrice != this.orderInvoiceModel.salesDetails[i].minRetailPrice) {
          this.orderInvoiceModel.salesDetails[i].retailPrice = formData.salesDetails[i].retailPrice;
        } else {
          this.orderInvoiceModel.salesDetails[i].retailPrice = this.orderInvoiceModel.salesDetails[i].minRetailPrice;
        }
        this.orderInvoiceModel.salesDetails[i].totalCostPrice = this.orderInvoiceModel.salesDetails[i].quantity * this.orderInvoiceModel.salesDetails[i].retailPrice;
        this.orderInvoiceModel.salesDetails[i].totalCostPrice -= this.orderInvoiceModel.salesDetails[i].retailDiscount;
        this.orderInvoiceModel.invoiceTotal += this.orderInvoiceModel.salesDetails[i].totalCostPrice;
      }
      this.orderInvoiceModel.invoiceTotal -= this.orderInvoiceModel.totalDiscount;
    });
    this.productService.getProductList(this.selectedOrg)
      .subscribe(prodArr => this.productList = prodArr);
  }

  selectProduct(selectedProdId: number): void {
    this.selectedOrderDetails.productId = Number(selectedProdId);
    let selectedProd: ProductModel | undefined = this.productList.find(x => x.productId == this.selectedOrderDetails.productId);
    if (selectedProd == undefined || selectedProd == null) return;

    this.selectedOrderDetails.variants = this.productList.find(x => x.productId == this.selectedOrderDetails.productId)?.productVariants ?? [];
    this.selectedOrderDetails.productVariantId = 0;
    this.cdRef.detectChanges();
  }

  selectVariant(selectedVariantId: number): void {
    let selectedVariant = this.selectedOrderDetails.variants.find(x => x.variantId == Number(selectedVariantId));
    if (selectedVariant == undefined) return;
    let selectedProd = this.productList.find(x => x.productId == this.selectedOrderDetails.productId);
    if (selectedProd == undefined) return;

    this.selectedOrderDetails.productId = selectedProd.productId;
    this.selectedOrderDetails.productName = selectedProd.productName;
    this.selectedOrderDetails.productVariantId = selectedVariantId;
    this.selectedOrderDetails.productVariantName = selectedVariant.variantName;
  }

  addToCart(): void {
    if (this.selectedOrderDetails.productVariantId > 0) {
      this.inventoryService.getVariantInventory(this.selectedOrderDetails.productVariantId)
        .subscribe((lotsAvailable: Array<PurchaseInvoiceDetailsModel>) => {
          this.diagService.open(LotSelectionComponent, {
            context: {
              availableLots: lotsAvailable,
              selectionCallback: (lot: PurchaseInvoiceDetailsModel) => {
                let existingLot: SalesInvoiceDetailsModel | undefined = this.orderInvoiceModel.salesDetails.find(x => x.lotId == lot.recordId);
                if (existingLot == undefined) {
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
                        Validators.max(this.selectedOrderDetails.maxRetailPrice)
                      ]
                    ]
                  }));

                  this.selectedOrderDetails = new SalesInvoiceDetailsModel();
                }
                else {
                  let targetCartObj = this.orderInvoiceModel.salesDetails.find(x => x.lotId == existingLot?.lotId);
                  if (!targetCartObj) return;
                  let indexOfItem: number = this.orderInvoiceModel.salesDetails.indexOf(targetCartObj);
                  let row = document.querySelectorAll('#CartPage table tbody tr').item(indexOfItem);
                  row.classList.add('highlight-active');
                  setTimeout(() => {
                    row.classList.add('highlight-fade-out');      // start transition
                    row.classList.remove('highlight-active');     // remove highlight

                    setTimeout(() => {
                      row.classList.remove('highlight-fade-out'); // clean up
                    }, 1000); // match transition duration
                  }, 2000); // wait before fading
                }
              }
            }
          }
          );
        });
    }
  }

  removeFromCart(index: number): void {
    // Remove from FormArray
    this.orderDetailsFormArray.removeAt(index);

    // Remove from orderInvoiceModel.salesDetails array
    if (index > -1 && index < this.orderInvoiceModel.salesDetails.length) {
      this.orderInvoiceModel.salesDetails.splice(index, 1);
    }
  }

  initializePaymentDetailsForm() {
    if (this.orderDetailsForm.valid) {
      this.orderInvoiceModel.organizationId = this.selectedOrg;
      this.salesService.saveSell(this.orderInvoiceModel)
        .subscribe(
          (invoice: SalesInvoiceModel) => {
            this.orderInvoiceModel = invoice;
            this.salesMemoStepper.next();
          },
          (err) => {
            console.log(err);
            this.toastr.danger(err);
          }
        )
    }
    else {
      this.toastr.warning('Invalid invoice', 'Unable to save');
    }
  }

  searchSimilarPhoneNumbers() {
    if (this.customerModel == null) { return; }

    this.customerService.getCustomerByContactNumber(this.customerModel.contactNumber, this.selectedOrg)
      .subscribe((similarCustomerContactNumbers: Array<string>) => {
        this.customerContactNumbers = similarCustomerContactNumbers;
      }, (error) => {
        console.log('error : searchSimilarPhoneNumbers : ', error);
      }, () => {
        let field = document.getElementById('ContactNumber');
        if (field) {
          field.focus();
        }

        this.allowAutoCompleteEvent = true;
      });
  }

  allowAutoCompleteEvent: boolean = true;
  selectPhoneNumber(searchString: string): void {
    if (this.allowAutoCompleteEvent) {
      this.allowAutoCompleteEvent = false;
      this.searchCustomer(searchString);
      setTimeout(() => {
        this.allowAutoCompleteEvent = true;
      }, 100);
    } else {
      this.allowAutoCompleteEvent = true;
    }
  }

  searchCustomer(searchString: string): void {
    if (!searchString || searchString.length <= 1) return;
    this.customerService.searchCustomer(searchString, this.selectedOrg)
      .subscribe((customerData: CustomerModel) => {
        this.customerModel = customerData;
        this.initializeCustomerForm(customerData);
      }, (error) => {
        console.log('error : searchCustomer : ', error);
      }, () => { });
  }
}
