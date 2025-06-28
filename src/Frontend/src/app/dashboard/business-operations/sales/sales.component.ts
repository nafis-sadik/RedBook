import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AddSalesComponent } from './add-sales/add-sales.component';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';
import { NGXPaginationService } from 'src/app/shared/ngx-pagination/ngx-pagination.service';
import { OrganizationModel } from '../../Models/organization.model';
import { SalesInvoiceModel } from '../../Models/sales-invoice.model';
import { DashboardService } from '../../services/dashboard.service';
import { OrganizationService } from '../../services/organization.service';
import { SalesService } from '../../services/sell.service';
import { CustomerModel } from '../../Models/customer.model';

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html',
  styleUrl: './sales.component.scss'
})

export class SalesComponent implements OnInit {
  isUpdateOperation: boolean = false;
  cardHeader: string = 'Product Sales';
  outlets: Array<OrganizationModel> = [];
  pagedSalesModel: IPaginationModel<SalesInvoiceModel>;
  loaderContainer: HTMLElement = document.getElementById('LoadingScreen') as HTMLElement;

  constructor(
    private cdRef: ChangeDetectorRef,
    private orgService: OrganizationService,
    private dashboardService: DashboardService,
    private salesService: SalesService,
    private ngxPaginationService: NGXPaginationService<SalesInvoiceModel>
  ) {
    this.pagedSalesModel = dashboardService.getPagingConfig(AddSalesComponent, 'Sales Records', 'Add New Sales');

    if(this.pagedSalesModel.tableConfig){
      this.pagedSalesModel.tableConfig.tableMaping = {
        "Memo No": "invoiceNumber",
        "Customer Name": "customerName",
        "Invoice Total": "invoiceTotal",
        "Paymeny Status": "paymentStatus",
        "Paid Amount": "totalPaid",
        "Sales Date": "salesDate",
      };

      this.pagedSalesModel.tableConfig.onEdit = null;

      this.pagedSalesModel.tableConfig.onDelete = () => {
        console.log('onDelete');
      };

      this.pagedSalesModel.tableConfig.onView = (data: any) => {
        console.log('on view', data);
      }
    }

    if(this.pagedSalesModel.addNewElementButtonConfig) {
      this.pagedSalesModel.addNewElementButtonConfig.onAdd = () => {
        this.isUpdateOperation = false;
        
        dashboardService.ngDialogService.open(AddSalesComponent, {
          context: {
            customerModel: new CustomerModel(),
            selectOrganization: this.dashboardService.selectedOutletId
          }
        });
      };
    }
  }

  ngOnInit(): void {
    this.orgService.getUserOrgs()
      .subscribe((orgList: Array<OrganizationModel>) => {
        this.outlets = orgList;
        this.cdRef.detectChanges();
      },
      (error) => {
        console.log('error', error);
      }).add(() => {        
        if(this.loaderContainer && this.loaderContainer.classList.contains('d-block')){
          this.loaderContainer.classList.remove('d-block');
          this.loaderContainer.classList.add('d-none');
        }
      });
  }

  selectOutlet(outletId: number, event: any): void{
    this.dashboardService.selectedOutletId = outletId;

    // Is display is hidden, make it visible
    let dataTableCard = Array.from(document.getElementsByTagName('ngx-pagination'))[0];
    if(dataTableCard && dataTableCard.classList.contains('d-none'))
      dataTableCard.classList.remove('d-none');

    // Add active class to source element and remove from sibling elements
    let sourceElem = event.srcElement;
    Array.from(sourceElem.parentNode.children).forEach((element: any) => {
      if(element != sourceElem)
        element.classList.remove('active');
      else
        element.classList.add('active');
    });

    // Get data from service
    if(this.pagedSalesModel.pagingConfig){
      this.pagedSalesModel.pagingConfig.pageNumber = 1;
    }
    
    this.loadPagedData();
  }

  loadPagedData() {
    this.pagedSalesModel.organizationId = this.dashboardService.selectedOutletId;
    this.salesService.getSalesPaged(this.pagedSalesModel)
      .subscribe((response: any) => {
        console.log('response', response);
        if (this.pagedSalesModel.tableConfig) {
          this.pagedSalesModel.tableConfig.sourceData = response.sourceData;

          this.pagedSalesModel.tableConfig.sourceData.forEach((salesInvoiceData: SalesInvoiceModel) => {
            if(salesInvoiceData.customer)
              salesInvoiceData.customerName = salesInvoiceData.customer.customerName;
          })
        
          this.pagedSalesModel.tableConfig.sourceData.forEach(invoice => {
            // Parse the UTC date string to a Date object
            let utcDate = new Date(invoice.salesDate.toString());

            // Convert the UTC date to local time
            let localDate = new Date(utcDate.toLocaleString());

            // Format the local date as needed (e.g., ISO string)
            invoice.salesDate = localDate.toDateString();
          });
        }

        if(this.pagedSalesModel.pagingConfig) {
          this.pagedSalesModel.pagingConfig.pageNumber = response.pageNumber;
          this.pagedSalesModel.pagingConfig.pageLength = response.pageLength;
          this.pagedSalesModel.pagingConfig.totalItems = response.totalItems;
        }

        this.ngxPaginationService.set(this.pagedSalesModel);
      });
  }
}
