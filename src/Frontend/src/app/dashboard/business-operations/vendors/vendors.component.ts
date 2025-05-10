import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';
import { VendorModel } from '../../Models/vendor.model';
import { DashboardService } from '../../services/dashboard.service';
import { OrganizationService } from '../../services/organization.service';
import { OrganizationModel } from '../../Models/organization.model';
import { VendorService } from '../../services/vendor.service';
import { NGXPaginationService } from 'src/app/shared/ngx-pagination/ngx-pagination.service';
import { AddVendorsComponent } from './add-vendors/add-vendors.component';
import { Observable } from 'rxjs';
import { NbDialogRef } from '@nebular/theme';

@Component({
  selector: 'app-vendors',
  templateUrl: './vendors.component.html',
  styleUrl: './vendors.component.scss'
})
export class VendorsComponent implements OnInit {
  cardHeader: string = 'Vendor Management';
  outlets: Array<OrganizationModel>;
  pagedVendorModel: IPaginationModel<VendorModel>;
  diagBox: NbDialogRef<AddVendorsComponent>;
  loaderContainer: HTMLElement = document.getElementById('LoadingScreen') as HTMLElement;

  constructor(
    private dashboardService: DashboardService,
    private vendorService: VendorService,
    private orgService: OrganizationService,
    private cdRef: ChangeDetectorRef,
    private paginationService: NGXPaginationService<VendorModel>
  ) {
    this.pagedVendorModel = dashboardService.getPagingConfig(AddVendorsComponent, 'Vendor List', 'Add New Vendor');

    if (this.pagedVendorModel.tableConfig) {
      this.pagedVendorModel.tableConfig.tableMaping = {
        "Memo No": "vendorName",
        "Contact Person": "contactPerson",
        "Contact Number": "phoneNumber",
        "Email": "emailAddress",
        "Total Payable": "totalPayable",
        "Total Recievable": "totalRecievable",
      };

      this.pagedVendorModel.tableConfig.onEdit = (vendorModel: VendorModel) => {
        this.vendorService.getById(vendorModel.vendorId)
          .subscribe(res => {
            this.diagBox = dashboardService.ngDialogService.open(AddVendorsComponent, {
              context: {
                vendorModel: res
              }
            });
          });
      }

      this.pagedVendorModel.tableConfig.onDelete = () => { console.log('onDelete'); };
    }

    if (this.pagedVendorModel.searchingConfig) {
      this.pagedVendorModel.searchingConfig.onSearch = (searchConfig: any) => {
        if (this.pagedVendorModel.pagingConfig && this.pagedVendorModel.searchingConfig) {
          this.pagedVendorModel.pagingConfig.pageLength = searchConfig.pageLength;
          this.pagedVendorModel.searchingConfig.searchString = searchConfig.searchString;
          this.pagedVendorModel.pagingConfig.pageNumber = searchConfig.pageNumber;
          this.selectOutlet(1, null);
        } else {
          return;
        }
      }
    }

    if (this.pagedVendorModel.addNewElementButtonConfig) {
      this.pagedVendorModel.addNewElementButtonConfig.onAdd = () => {
        this.diagBox = dashboardService.ngDialogService.open(AddVendorsComponent, {
          context: {
            vendorModel: new VendorModel(),
            organizationId: this.dashboardService.selectedOutletId
          }
        });
      }
    }

    this.vendorService.listenFormData()
      .subscribe((formModel: VendorModel) => {
        let vendorObservable: Observable<VendorModel>;
        formModel.organizationId = this.dashboardService.selectedOutletId;
        if (formModel.vendorId <= 0) {
          vendorObservable = this.vendorService.addVendor(formModel);
        } else {
          vendorObservable = this.vendorService.updateVendor(formModel);
        }

        vendorObservable.subscribe(() => {
          if (this.pagedVendorModel.tableConfig && this.pagedVendorModel.pagingConfig) {
            let updatedData = this.pagedVendorModel.tableConfig.sourceData.filter(model => model.vendorId == formModel.vendorId);
            // If item is not in UI, it's a new item
            if (updatedData == undefined || updatedData.length == 0) {
              // If current page is bellow page length, we are at the last page already, just append to the list
              if (this.pagedVendorModel.tableConfig.sourceData.length < this.pagedVendorModel.pagingConfig.pageLength) {
                this.pagedVendorModel.tableConfig.sourceData.push(formModel);
              }
              else {
                this.pagedVendorModel.pagingConfig.pageNumber = Math.ceil(this.pagedVendorModel.pagingConfig.totalItems / this.pagedVendorModel.pagingConfig.pageLength);
              }

              this.paginationService.set(this.pagedVendorModel);
            }
            else {
              for (let i = 0; i < this.pagedVendorModel.tableConfig.sourceData.length; i++) {
                if (this.pagedVendorModel.tableConfig.sourceData[i].vendorId == formModel.vendorId) {
                  this.pagedVendorModel.tableConfig.sourceData[i] = formModel;
                  break;
                }
              }
            }

            this.diagBox.close();
          }
        });
      });
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

  selectOutlet(outletId: number, event: any): void {
    this.dashboardService.selectedOutletId = outletId;

    // If data table display is hidden, make it visible
    let dataTableCard = Array.from(document.getElementsByTagName('ngx-pagination'))[0];
    if (dataTableCard && dataTableCard.classList.contains('d-none'))
      dataTableCard.classList.remove('d-none');

    // Add active class to source element and remove from sibling elements
    if (event) {
      let sourceElem = event.srcElement;
      Array.from(sourceElem.parentNode.children)
        .forEach((element: any) => {
          if (element != sourceElem)
            element.classList.remove('active');
          else
            element.classList.add('active');
        });
    }

    // API Call
    if (this.pagedVendorModel.tableConfig) {
      this.pagedVendorModel.organizationId = outletId;
      this.vendorService.getPaged(this.pagedVendorModel)
        .subscribe(response => {
          this.loadDataOnUI(response);
        });
    }
  }

  loadDataOnUI(response: any) {
    if (this.pagedVendorModel.tableConfig == null) return;
    if (this.pagedVendorModel.pagingConfig == null) return;
    if (this.pagedVendorModel.searchingConfig == null) return;

    if (response.searchString)
      this.pagedVendorModel.searchingConfig.searchString = response.searchString;
    else this.pagedVendorModel.searchingConfig.searchString = '';

    // Preparing pagination data
    this.pagedVendorModel.pagingConfig.pageLength = response.pageLength;
    this.pagedVendorModel.pagingConfig.pageNumber = response.pageNumber;
    this.pagedVendorModel.pagingConfig.totalItems = response.totalItems;

    // Preparing data table
    this.pagedVendorModel.tableConfig.sourceData = response.sourceData;
    
    this.paginationService.set(this.pagedVendorModel);
    this.cdRef.detectChanges();
  }
}
