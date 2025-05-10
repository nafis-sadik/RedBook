import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { OrganizationModel } from 'src/app/dashboard/Models/organization.model';
import { UserModel } from 'src/app/dashboard/Models/user.model';
import { DashboardService } from 'src/app/dashboard/services/dashboard.service';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';
import { NGXPaginationService } from 'src/app/shared/ngx-pagination/ngx-pagination.service';
import { UserFormComponent } from './user-form/user-form.component';
import { NbDialogService } from '@nebular/theme';
import { OrganizationService } from 'src/app/dashboard/services/organization.service';
import { EmployeeService } from 'src/app/dashboard/services/employee.service';
import { RoleModel } from 'src/app/dashboard/Models/role.model';

@Component({
  selector: 'app-ums',
  templateUrl: './ums.component.html',
  styleUrls: ['./ums.component.scss']
})

export class UmsComponent implements OnInit {
  selectedBusinessId: number;
  pagedUserModel: IPaginationModel<UserModel>;
  ownedBusinesses: Array<OrganizationModel> = [];
  loaderContainer: HTMLElement = document.getElementById('LoadingScreen') as HTMLElement;

  constructor(
    dashboardService: DashboardService,
    private dialogService: NbDialogService,
    private orgService: OrganizationService,
    private employeeService: EmployeeService,
    private cdRef: ChangeDetectorRef,
    private ngxPaginationService: NGXPaginationService<UserModel>
  ) {
    this.pagedUserModel = dashboardService.getPagingConfig(UserFormComponent, 'User Management', 'Add User', 'Search User');

    // Dropdown change event binding
    if (this.pagedUserModel.pagingConfig) {
      this.pagedUserModel.pagingConfig.onUpdate = () => {
        this.orgService.getUserByBusinessId(this.pagedUserModel, this.selectedBusinessId)
          .subscribe((response) => {
            this.loadDataOnUI(response);
          });
      }
    }

    if (this.pagedUserModel.addNewElementButtonConfig) {
      this.pagedUserModel.addNewElementButtonConfig.onAdd = () => {
        let dialogueRef = this.dialogService.open(UserFormComponent, {
          context: {
            userModel: new UserModel(),
            selectedBusinessId: this.selectedBusinessId,
            addUser: () => {
              if (this.pagedUserModel.pagingConfig) {
                this.pagedUserModel.pagingConfig.pageNumber = Math.ceil(this.pagedUserModel.pagingConfig.totalItems / this.pagedUserModel.pagingConfig.pageLength);
                this.loadUsersUnderBusiness(this.selectedBusinessId);
              }
              dialogueRef.close();
            }
          },
        });
      };
    }

    if (this.pagedUserModel.tableConfig) {
      this.pagedUserModel.tableConfig.tableMaping = {
        "First Name": "firstName",
        "Last Name": "lastName",
        "User Name": "userName",
        "Role": "roleNames"
      };

      this.pagedUserModel.tableConfig.onEdit = (userModel: UserModel) => {
        // Send the data to the pop up to load the data from api on the form
        if (this.pagedUserModel.tableConfig) {
          let selectedUser = this.pagedUserModel.tableConfig.sourceData.find(x => x.userId == userModel.userId);
          this.dialogService.open(UserFormComponent, {
            context: {
              userModel: selectedUser ? selectedUser : new UserModel(),
              selectedBusinessId: this.selectedBusinessId
            },
          });
        }
      }

      this.pagedUserModel.tableConfig.onDelete = (data: any) => {
        this.orgService.removeUserFromBusiness(data.userId, this.selectedBusinessId)
          .subscribe(() => {
            location.reload();
          });
      }
    }

    if (this.pagedUserModel.searchingConfig) {
      this.pagedUserModel.searchingConfig.onSearch = (searchConfig: any) => {
        if (this.pagedUserModel.pagingConfig && this.pagedUserModel.searchingConfig) {
          this.pagedUserModel.pagingConfig.pageLength = searchConfig.pageLength;
          this.pagedUserModel.searchingConfig.searchString = searchConfig.searchString;
          this.pagedUserModel.pagingConfig.pageNumber = searchConfig.pageNumber;
          this.loadUsersUnderBusiness(this.selectedBusinessId);
        } else {
          return;
        }
      }
    }
  }

  ngOnInit(): void {    
    this.orgService.getUserOrgs()
      .subscribe((orgList: Array<OrganizationModel>) => {
        this.ownedBusinesses = orgList;
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

  /**
   * Loads the users associated with the specified business ID.
   *
   * @param businessId - The ID of the business to load users for.
   */
  loadUsersUnderBusiness(businessId: number): void {
    this.selectedBusinessId = businessId;

    let dataTableCard = Array.from(
      document.getElementsByTagName('ngx-pagination')
    )[0];
    if (dataTableCard && dataTableCard.classList.contains('d-none'))
      dataTableCard.classList.remove('d-none');

    this.employeeService
      .getPagedEmployeeList(this.pagedUserModel, businessId)
      .subscribe((response) => {
        this.loadDataOnUI(response);
      });
  }

  /**
   * Loads data onto the UI based on the response received from the server.
   * This method updates the pagination configuration, search configuration, and the table data.
   * It also processes the user roles for each user in the table data.
   *
   * @param response - The response object containing the data to be loaded onto the UI.
   */
  loadDataOnUI(response: any) {
    if (this.pagedUserModel.tableConfig == null) return;
    if (this.pagedUserModel.pagingConfig == null) return;
    if (this.pagedUserModel.searchingConfig == null) return;

    if (response.searchString)
      this.pagedUserModel.searchingConfig.searchString = response.searchString;
    else this.pagedUserModel.searchingConfig.searchString = '';

    // Preparing pagination data
    this.pagedUserModel.pagingConfig.pageLength = response.pageLength;
    this.pagedUserModel.pagingConfig.pageNumber = response.pageNumber;
    this.pagedUserModel.pagingConfig.totalItems = response.totalItems;

    // Preparing data table
    this.pagedUserModel.tableConfig.sourceData = response.sourceData;
    // Load the ids for multiple select on pop up form
    this.pagedUserModel.tableConfig.sourceData.forEach((user: UserModel) => {
      let userRoleNames = '';
      user.userRoleIds = [];
      user.userRoles.forEach((x: RoleModel) => {
        user.userRoleIds.push(x.roleId);
        if (!userRoleNames.includes(x.roleName)) {
          userRoleNames += x.roleName + ', ';
        }
      });

      // slice last 2 characters from the string to remove the garbage from the tail that we needed to add during the loop two lines above
      user.roleNames = userRoleNames.slice(0, -2);
    });

    this.ngxPaginationService.set(this.pagedUserModel);
  }
}
