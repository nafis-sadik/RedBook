import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';
import { ProductsDetailsFormComponent } from './products-details-form/products-details-form.component';
import { NGXPaginationService } from 'src/app/shared/ngx-pagination/ngx-pagination.service';
import { OrganizationModel } from 'src/app/dashboard/Models/organization.model';
import { DashboardService } from 'src/app/dashboard/services/dashboard.service';
import { OrganizationService } from 'src/app/dashboard/services/organization.service';
import { NbToastrService } from '@nebular/theme';
import { ProductModel } from 'src/app/dashboard/Models/product.model';
import { ProductService } from '../../services/products.service';
import { ProductVariantService } from '../../services/product-variant.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})

export class ProductsComponent implements OnInit {
  selectedOutletId: number;
  isUpdateOperation: boolean = false;
  pagedProductModel: IPaginationModel<ProductModel>;
  organizationList: Array<OrganizationModel>;
  loaderContainer: HTMLElement = document.getElementById('LoadingScreen') as HTMLElement;

  constructor(
    private cdRef: ChangeDetectorRef,
    dashboardService: DashboardService,
    private toastrService: NbToastrService,
    private productService: ProductService,
    private orgService: OrganizationService,
    private ngxPaginationService: NGXPaginationService<ProductModel>,
  ) {
    this.pagedProductModel = dashboardService.getPagingConfig(ProductsDetailsFormComponent, 'Product List', 'Add New Product');

    if (this.pagedProductModel.pagingConfig) {
      this.pagedProductModel.pagingConfig.onUpdate = (pagingConfigObj: any) => {
        if (this.pagedProductModel.pagingConfig) {
          this.pagedProductModel.pagingConfig.pageNumber = pagingConfigObj.pageNumber;
          this.pagedProductModel.pagingConfig.pageLength = pagingConfigObj.pageLength;
          if (this.pagedProductModel.searchingConfig)
            this.pagedProductModel.searchingConfig.searchString = pagingConfigObj.searchString;
          this.fetchProductsOfOutlet(this.selectedOutletId);
        }
      }
    }

    if (this.pagedProductModel.tableConfig) {
      this.pagedProductModel.tableConfig.tableMaping = {
        "Product Name": "productName",
        "Brand Name": "brandName",
        "Product Category": "categoryName",
        "Product Subcategory": "subcategoryName",
      };

      this.pagedProductModel.tableConfig.onEdit = (product: ProductModel) => {
        dashboardService.ngDialogService.open(ProductsDetailsFormComponent, {
          context: {
            productModelInput: product,
            selectedBusinessId: this.selectedOutletId,
            saveMethod: (product: ProductModel) => {
              this.productService.updateProduct(product)
                .subscribe((response: ProductModel) => {
                  this.pagedProductModel.tableConfig?.sourceData.forEach(product => {
                    if (product.productId === response.productId) {
                      Object.assign(product, response);
                      this.toastrService.success('Product Updated Successfully', 'Success');
                      return;
                    }
                  });

                  this.ngxPaginationService.set(this.pagedProductModel);
                });
            }
          }
        });
      };

      this.pagedProductModel.tableConfig.onDelete = () => { console.log('onDelete'); };

      this.pagedProductModel.tableConfig.onView = null;
      // this.pagedProductModel.tableConfig.onView = (data: any) => {
      //   console.log('data', data);
      // }
    }

    if (this.pagedProductModel.addNewElementButtonConfig) {
      this.pagedProductModel.addNewElementButtonConfig.onAdd = () => {
        dashboardService.ngDialogService.open(ProductsDetailsFormComponent, {
          context: {
            productModel: undefined,
            selectedBusinessId: this.selectedOutletId,
            saveMethod: (product: ProductModel) => {
              product.organizationId = this.selectedOutletId;

              this.productService.addProduct(product)
                .subscribe((response: ProductModel) => {
                  this.pagedProductModel.tableConfig?.sourceData.push(response);
                  this.toastrService.success('Product Added Successfully', 'Success');
                  if (this.pagedProductModel.pagingConfig) {
                    let targetPageNumber = Math.ceil((this.pagedProductModel.pagingConfig?.totalItems + 1) / this.pagedProductModel.pagingConfig?.pageLength);
                    console.log(`Navigating to page ${targetPageNumber}`);
                    this.pagedProductModel.pagingConfig.pageNumber = targetPageNumber;
                    this.fetchProductsOfOutlet(this.selectedOutletId);
                  }
                  this.cdRef.detectChanges();
                });
            }
          }
        });
      };
    }

    this.organizationList = [];
  }

  ngOnInit(): void {
    this.orgService.getUserOrgs()
      .subscribe((orgList: Array<OrganizationModel>) => {
        this.organizationList = orgList;
        this.cdRef.detectChanges();
      },
        (error) => {
          console.log('error', error);
        }).add(() => {
          if (this.loaderContainer && this.loaderContainer.classList.contains('d-block')) {
            this.loaderContainer.classList.remove('d-block');
            this.loaderContainer.classList.add('d-none');
          }
        });
  }

  fetchProductsOfOutlet(outletId: number) {
    // Fetch Products for selected outlet
    this.productService.getProductListPaged(outletId, this.pagedProductModel)
      .subscribe((pagedProducts: any) => {
        // Get pagination data to update table
        if (this.pagedProductModel.tableConfig) {
          this.pagedProductModel.tableConfig.sourceData = pagedProducts.sourceData;
          console.log('this.pagedProductModel.tableConfig.sourceData', this.pagedProductModel.tableConfig.sourceData);
        }

        if (this.pagedProductModel.pagingConfig) {
          this.pagedProductModel.pagingConfig.pageNumber = pagedProducts.pageNumber;
          this.pagedProductModel.pagingConfig.pageLength = pagedProducts.pageLength;
          this.pagedProductModel.pagingConfig.totalItems = pagedProducts.totalItems;
        }

        if (this.pagedProductModel.searchingConfig) {
          this.pagedProductModel.searchingConfig.searchString = pagedProducts.searchString;
        }

        this.ngxPaginationService.set(this.pagedProductModel);
      });
  }

  selectOutlet(outletId: number, event: any): void {
    this.selectedOutletId = outletId;

    // Is display is hidden, make it visible
    let dataTableCard = Array.from(document.getElementsByTagName('ngx-pagination'))[0];
    if (dataTableCard && dataTableCard.classList.contains('d-none'))
      dataTableCard.classList.remove('d-none');

    // Add active class to source element and remove from sibling elements
    let sourceElem = event.srcElement;
    Array.from(sourceElem.parentNode.children)
      .forEach((element: any) => {
        if (element != sourceElem)
          element.classList.remove('active');
        else
          element.classList.add('active');
      });

    this.fetchProductsOfOutlet(outletId);
  }
}
