import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { DashboardService } from 'src/app/dashboard/services/dashboard.service';
import { VendorModel } from 'src/app/dashboard/Models/vendor.model';
import { PurchaseInvoiceModel } from 'src/app/dashboard/Models/purchase-invoice.model';
import { PurchaseService } from 'src/app/dashboard/services/purchase.service';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { ProductService } from 'src/app/dashboard/services/products.service';
import { InvoicePaymentModel } from 'src/app/dashboard/Models/invoice-payment.model';
import { PurchaseInvoiceDetailsModel } from 'src/app/dashboard/Models/purchase-invoice-details.model';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { VendorService } from 'src/app/dashboard/services/vendor.service';
import { ProductVariantModel } from 'src/app/dashboard/Models/product-variant.model';
import { NbToastrService } from '@nebular/theme';

@Component({
  selector: 'app-add-purchase',
  templateUrl: './add-purchase.component.html',
  styleUrls: ['./add-purchase.component.scss']
})
export class AddPurchaseComponent implements OnInit {
  linearMode = true;

  @Input() purchaseMasterModel: PurchaseInvoiceModel;

  // Order master form
  orderMasterForm: FormGroup;
  vendorList: Array<VendorModel> = [];
  calculatedTotalAmount: number = 0;

  // Order details form
  orderDetailsForm: FormGroup;
  selectedProdId: number;
  selectedVariantId: number;
  productList: Array<ProductModel> = [];
  variantList: Array<ProductVariantModel> = [];

  // Order payment form
  paymentHistory: Array<InvoicePaymentModel> = [];
  paymentInvoice: InvoicePaymentModel = new InvoicePaymentModel();

  constructor(
    private datePipe: DatePipe,
    private formBuilder: FormBuilder,
    private cdRef: ChangeDetectorRef,
    private vendorService: VendorService,
    private toastrService: NbToastrService,
    private productService: ProductService,
    private purchaseService: PurchaseService,
    private dashboardService: DashboardService,
  ) {
    // load vendor data for the selected outlet
    this.vendorService.getList(this.dashboardService.selectedOutletId)
      .subscribe(response => this.vendorList = response);
    
    // load product list for the selected outlet
    this.productService.getProductList(this.dashboardService.selectedOutletId)
      .subscribe(response => this.productList = response);

    // this.toastrService.warning('Please select a business', 'Warning');
  }

  ngOnInit(): void {
    this.initializeOrderMasterForm();
    this.initializeOrderDetailsForm();
  }
  
  initializeOrderDetails(): void {
    if(!this.orderMasterForm.valid){
      this.orderMasterForm.markAllAsTouched();
      this.orderMasterForm.markAsDirty();
      return;
    }

    this.purchaseMasterModel.purchaseDetails = [];
    if (!this.orderMasterForm.valid) {
      let invalidControls: Array<string> = Object.keys(
        this.orderMasterForm.controls
      ).filter((controlName) => this.orderMasterForm.controls[controlName].invalid);

      invalidControls.forEach((controlName) => {
        this.orderMasterForm.get(controlName)?.markAsDirty();
      });
      // return;
    }

    this.initializeOrderDetailsForm();

    // Add event listener to calculate total amount
    document.querySelectorAll('#InvoiceForm nb-card-body')?.forEach(elems => {
      elems.addEventListener('keyup', (event) => {
        this.updateTotalAmount();
      });
    });
  }

  initializeOrderMasterForm(): void{
    this.orderMasterForm = this.formBuilder.group({
      chalanNumber: ['', Validators.required],
      purchaseDate: ['', Validators.required],
      vendorId: [0, [Validators.required, Validators.min(1)]],
      invoiceTotal: [0, [Validators.required, Validators.min(1)]],
      terms: [''],
      remarks: [''],
    });

    this.orderMasterForm.valueChanges.subscribe(formData => {
      this.purchaseMasterModel.vendorId = Number(formData.vendorId);
      this.purchaseMasterModel.chalanNumber = formData.chalanNumber;
      
      if (!Number.isNaN(Number(formData.invoiceTotal)))
        this.purchaseMasterModel.invoiceTotal = Number(formData.invoiceTotal);

      if (formData.purchaseDate) {
        let purchaseDateTime = new Date(formData.purchaseDate);
        let displayDate = this.datePipe.transform(purchaseDateTime, 'MMM d, yyyy');
        if (displayDate)
          this.purchaseMasterModel.chalanDate = displayDate;
      }
    });
  }

  generateChalanNumber(): void {
    this.purchaseMasterModel.chalanNumber = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
      let r = (Math.random() * 16) | 0;
      let v = c === 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    });
    this.orderMasterForm.get('chalanNumber')?.setValue(this.purchaseMasterModel.chalanNumber);
    this.cdRef.detectChanges();
  }

  initializeOrderDetailsForm(): void {
    this.purchaseMasterModel.purchaseDetails = [];
    
    this.orderDetailsForm = this.formBuilder.group({
      totalDiscount: [0, [Validators.required, Validators.min(0)]],
      purchaseDetails: this.formBuilder.array([], [Validators.required, Validators.min(1)]),
    });

    this.orderDetailsForm.valueChanges.subscribe(formData => {
      this.purchaseMasterModel.purchaseDetails = formData.purchaseDetails;
      this.purchaseMasterModel.totalDiscount = formData.totalDiscount;
      this.calculatedTotalAmount = 0;
      this.purchaseMasterModel.purchaseDetails.forEach(purchaseDetailObj => {
        purchaseDetailObj.totalCostPrice = purchaseDetailObj.quantity * purchaseDetailObj.purchasePrice;
        purchaseDetailObj.totalCostPrice -= purchaseDetailObj.purchaseDiscount;
        this.calculatedTotalAmount += purchaseDetailObj.totalCostPrice;
      });
      this.calculatedTotalAmount -= this.purchaseMasterModel.totalDiscount;
    });
  }

   get orderDetailsFormArray(): FormArray { return this.orderDetailsForm.get('purchaseDetails') as FormArray; }
  
  addProduct(): void {
    let selectedProduct = this.productList.find(x => x.productId == this.selectedProdId);
    if(selectedProduct){
      let selectedVariant = selectedProduct.productVariants.find(x => x.variantId == this.selectedVariantId);
      if(selectedVariant){
        let variantAlreadyAdded: boolean = this.purchaseMasterModel.purchaseDetails.filter(x => x.productId == this.selectedProdId && x.productVariantId == this.selectedVariantId).length > 0;
        if(!variantAlreadyAdded) {
          this.orderDetailsFormArray.push(this.formBuilder.group({
            productId: [selectedProduct.productId],
            productName: [selectedProduct.productName],
            productVariantId: [selectedVariant.variantId],
            productVariantName: [selectedVariant.variantName],
            quantity: [0, [Validators.required, Validators.min(1)]],
            purchasePrice: [0, [Validators.required, Validators.min(1)]],
            purchaseDiscount: [0, [Validators.required, Validators.min(0)]],
            retailPrice: [0, [Validators.required, Validators.min(1)]],
            maxRetailDiscount: [0, [Validators.required, Validators.min(0)]],
            vatRate: [0, [Validators.required, Validators.min(0)]],
            totalCostPrice: [0]
          }));
          this.selectedProdId = 0;
          this.selectedVariantId = 0;
          this.variantList = [];
        }
      }
    }
  }

  removeProduct(index: number): void { 
    this.orderDetailsFormArray.removeAt(index); 
    this.purchaseMasterModel.purchaseDetails.splice(index, 1);
  }

  selectProduct(selectedProdId: number): void {
    this.selectedProdId = selectedProdId;
    this.selectedVariantId = 0;
    this.variantList = this.productList.find(x => x.productId == selectedProdId)?.productVariants ?? [];
  }
  
  addCustom() {
    let purchaseDetails = new PurchaseInvoiceDetailsModel();
    purchaseDetails.productId = 0;
    purchaseDetails.productName = "";
    purchaseDetails.quantity = 1;
    purchaseDetails.purchasePrice = 0;
    this.purchaseMasterModel.purchaseDetails.push(purchaseDetails);
  }
  
  updateTotalAmount() {
    this.calculatedTotalAmount = 0;
    console.log('this.invoiceModel.purchaseDetails', this.purchaseMasterModel.purchaseDetails);
    this.purchaseMasterModel.purchaseDetails.forEach((detail: PurchaseInvoiceDetailsModel) => {
    });
    // this.invoiceModel.purchaseDetails.forEach((detail: PurchaseInvoiceDetailsModel) => {
    //   if(detail.retailPrice == null || detail.retailPrice == undefined
    //     || detail.purchasePrice == null || detail.purchasePrice == undefined
    //     || detail.discount == null || detail.discount == undefined
    //     || detail.quantity == null || detail.quantity == undefined) return;

    //   // Make sure the numbers are really numbers, it's js at the end of the day, you can never be too sure.
    //   if (!detail.purchasePrice.toString().includes('.') && !Number.isNaN(Number(detail.purchasePrice)))
    //     detail.purchasePrice = Number(detail.purchasePrice);
      
    //   if (!detail.discount.toString().includes('.') && !Number.isNaN(Number(detail.discount)))
    //     detail.discount = Number(detail.discount);

    //   if (!detail.retailPrice.toString().includes('.') && !Number.isNaN(Number(detail.retailPrice)))
    //     detail.retailPrice = Number(detail.retailPrice);

    //   if(detail.quantity <= 0) detail.quantity = 1;

    //   let vatRateAmount: number = detail.purchasePrice * (detail.vatRate / 100);
    //   let grossTotalOnItem: number = detail.quantity * (detail.purchasePrice + vatRateAmount);
    //   detail.totalPrice = grossTotalOnItem - detail.discount;
    //   this.calculatedTotalAmount += detail.totalPrice;
    // });
    
    this.calculatedTotalAmount -= this.purchaseMasterModel.totalDiscount;

    this.cdRef.detectChanges();
  }
  
  saveInvoice(): void {
    if(this.purchaseMasterModel.purchaseDetails.length <= 0){
      this.toastrService.danger('Please add purchased products to the invoice', 'Error');
      return;
    }

    this.purchaseService.emitInvoiceModel(this.purchaseMasterModel);
  }
}
