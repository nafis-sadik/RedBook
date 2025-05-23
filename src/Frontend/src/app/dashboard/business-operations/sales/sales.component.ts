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
        "Memo No": "MemoNumber",
        "Net Total": "NetTotal",
        "Paid Amount": "PaidAmount",
        "Sales Date": "SalesDate",
      };

      this.pagedSalesModel.tableConfig.onEdit = () => {
        this.isUpdateOperation = true;
        
        dashboardService.ngDialogService.open(AddSalesComponent, {
          context: {
            customerModel: new CustomerModel(),
            selectOrganization: this.dashboardService.selectedOutletId
          }
        });
      };

      this.pagedSalesModel.tableConfig.onDelete = () => {
        console.log('onDelete');
      };
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
    let pageLength: number = this.pagedSalesModel.pagingConfig? this.pagedSalesModel.pagingConfig.pageLength : 5;
    let searchString: string = this.pagedSalesModel.searchingConfig? this.pagedSalesModel.searchingConfig.searchString : "";

    // Set data to observable view model to render in UI
    if(this.pagedSalesModel.tableConfig)
      this.pagedSalesModel.tableConfig.sourceData = this.salesService.getSalesList(outletId, 1, pageLength, searchString);
    
    this.ngxPaginationService.set(this.pagedSalesModel);
  }
}
