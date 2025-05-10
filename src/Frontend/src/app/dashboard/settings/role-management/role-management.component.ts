import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { NbToastrService, NbWindowService } from '@nebular/theme';
import { OrganizationModel } from 'src/app/dashboard/Models/organization.model';
import { IRoutePermissionModel } from 'src/app/dashboard/Models/IRoutePermissionModel';
import { OrganizationService } from 'src/app/dashboard/services/organization.service';
import { RoleService } from 'src/app/dashboard/services/role.service';
import { RouteService } from 'src/app/dashboard/services/route.service';
import { AddDialogueComponent } from 'src/app/shared/ngx-dialogues/add-dialogue/add-dialogue.component';
import { RemoveDialogueComponent } from 'src/app/shared/ngx-dialogues/remove-dialogue/remove-dialogue.component';
import { RoleFormComponent } from './role-form/role-form.component';
import { RoleModel } from 'src/app/dashboard/Models/role.model';
import { RouteModel } from 'src/app/dashboard/Models/route.model';

@Component({
  selector: 'app-role-management',
  templateUrl: './role-management.component.html',
  styleUrls: ['./role-management.component.scss']
})

export class RoleManagementComponent {
  ownedBusinesses: OrganizationModel[];
  rolesUnderThisBusiness: RoleModel[];
  roleRouteMapping: IRoutePermissionModel[];
  loaderContainer: HTMLElement = document.getElementById('LoadingScreen') as HTMLElement;

  selectedRoleId: number = 0;
  selectedBusinessId: number = 0;

  constructor(
    private orgService: OrganizationService,
    private roleService: RoleService,
    private routeService: RouteService,
    private toastrService: NbToastrService,
    private windowService: NbWindowService,
    private cdRef: ChangeDetectorRef,
    private businessService: OrganizationService,
  ) {
    this.roleRouteMapping = [];
    this.rolesUnderThisBusiness = [];
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

  // Organization / Business management
  openSaveBusinessWindow(windowMessage: string, businessModel: OrganizationModel | null) {
    let toasterMsg = 'Saved Successfully';
    this.windowService.open(AddDialogueComponent, {
      title: windowMessage,
      buttons: {
        close: false,
        fullScreen: true,
        maximize: true,
        minimize: true
      },
      context: {
        saveMethod: (businessTitle: string) => {
          let observableObj;
          if (businessModel) {
            businessModel.organizationName = businessTitle;
            observableObj = this.businessService.updateOrganization(businessModel);
          } else {
            let org: OrganizationModel = new OrganizationModel();
            org.organizationId = 0;
            org.organizationName = businessTitle;
            observableObj = this.businessService.addNewOrganization(org);
          }

          observableObj.subscribe((response) => {
            let isNewlyAdded = true;
            for (let i = 0; i < this.ownedBusinesses.length; i++) {
              if (this.ownedBusinesses[i].organizationId == response.organizationId) {
                isNewlyAdded = false;
                this.ownedBusinesses[i] = response;
              }
            }

            if (isNewlyAdded) {
              this.ownedBusinesses.push(response);
            }
            this.cdRef.detectChanges();
            this.toastrService.success(toasterMsg, 'Success');
          })
        },

        textValue: businessModel?.organizationName
      }
    });
  }

  openDeleteBusinessWindow(windowMessage: string, businessId: number) {
    // Load pop up dialogue
    let windowRef = this.windowService.open(RemoveDialogueComponent, {
      title: windowMessage,
      buttons: {
        close: false,
        fullScreen: true,
        maximize: true,
        minimize: true
      },
      context: {
        deleteMethod: () => {
          this.businessService.deleteOrganization(businessId)
            .subscribe(() => {
              let filteredList = this.ownedBusinesses.filter(x => x.organizationId != businessId);
              this.ownedBusinesses = filteredList;
              this.cdRef.detectChanges();
            });
        }
      }
    });
  }

  // Role Management
  loadBusinessRoles(businessId: number): void {
    this.selectedBusinessId = businessId;
    this.roleService.getOrganizationRoles(businessId)
      .subscribe((response) => {
        this.rolesUnderThisBusiness = [];
        response.forEach(element => {
          this.rolesUnderThisBusiness.push({
            organizationId: element.organizationId,
            roleId: element.roleId,
            roleName: element.roleName,
            isAdmin: element.isAdmin
          });
        });

        this.cdRef.detectChanges();
      });
  }

  /**
   * Opens a window to save a role. If a `roleModel` is provided, it updates the existing role. Otherwise, it creates a new role.
   *
   * @param windowMessage - The message to display in the window title.
   * @param roleModel - The role model to update, or `null` to create a new role.
   */
  openSaveRoleWindow(
    windowMessage: string,
    roleModel: RoleModel | null
  ): void {
    this.windowService.open(RoleFormComponent, {
      title: windowMessage,
      buttons: {
        close: false,
        fullScreen: true,
        maximize: true,
        minimize: true,
      },
      context: {
        roleName: roleModel?.roleName,
        isAdminRole: roleModel?.isAdmin,
        saveMethod: (roleTitle: string, isAdminRole: boolean) => {
          let observableObj;
          if (roleModel) {
            roleModel.roleName = roleTitle;
            roleModel.isAdmin = isAdminRole;
            observableObj = this.roleService.updateRole(roleModel);
          } else {
            observableObj = this.roleService.addRole({
              roleId: 0,
              organizationId: this.selectedBusinessId,
              roleName: roleTitle,
              isAdmin: isAdminRole,
            });
          }

          observableObj.subscribe((response) => {
            let isNewlyAdded: boolean = true;
            for (let i = 0; i < this.rolesUnderThisBusiness.length; i++) {
              // Update existing record
              if (this.rolesUnderThisBusiness[i].roleId == response.roleId) {
                isNewlyAdded = false;
                this.rolesUnderThisBusiness[i] = {
                  organizationId: response.organizationId,
                  roleId: response.roleId,
                  roleName: response.roleName,
                  isAdmin: response.isAdmin,
                };
              }
            }

            // Append newly added record
            if (isNewlyAdded) {
              this.rolesUnderThisBusiness.push({
                roleId: response.roleId,
                roleName: response.roleName,
                organizationId: response.organizationId,
                isAdmin: response.isAdmin,
              });
            }

            this.cdRef.detectChanges();
            this.toastrService.success('Saved Successfully', 'Success');
          });
        },
      },
    });
  }

  openDeleteRoleWindow(windowMessage: string, roleId: number) {
    // Load pop up dialogue
    // let windowRef = this.windowService.open(RemoveDialogueComponent, {
    this.windowService.open(RemoveDialogueComponent, {
      title: windowMessage,
      buttons: {
        close: false,
        fullScreen: true,
        maximize: true,
        minimize: true
      },
      context: {
        deleteMethod: () => {
          this.roleService.deleteRole(roleId)
            .subscribe(() => {
              let filteredList = this.rolesUnderThisBusiness.filter(x => x.roleId != roleId);
              this.rolesUnderThisBusiness = filteredList;
              this.cdRef.detectChanges();
            });
        }
      }
    });
  }

  findLeafNodes(nodes: RouteModel[]): RouteModel[] {
    let parentIds = nodes.map(node => node.parentRouteId);
    return nodes.filter(node => !parentIds.includes(node.routeId));
  }

  loadRoleRoutes(roleId: any): void {
    this.roleRouteMapping = [];
    this.selectedRoleId = roleId;
    this.routeService.getAppRoute()
      .subscribe((appRoutes) => {
        this.routeService.getRoutesByRole(roleId)
          .subscribe((allowedRoutes) => {
            let allowedRouteIds: number[] = allowedRoutes.map(x => x.routeId);

            appRoutes = this.findLeafNodes(appRoutes);

            appRoutes.forEach(route => {
              this.roleRouteMapping.push({
                routeId: route.routeId,
                routeName: route.routeName,
                isPermitted: allowedRouteIds.includes(route.routeId)
              });
            });


            this.cdRef.detectChanges();
          });
      });
  }

  allowRouteToRole(event: any, route: IRoutePermissionModel): void {
    event.preventDefault();
    
    this.roleService.mapRolesWithRoute(this.selectedRoleId, route.routeId)
      .subscribe((response) => {
        this.toastrService.success('Route mapped successfully', 'Success');
        route.isPermitted = response;
        this.cdRef.detectChanges();
      }, (error) => {
        console.log(error);
        this.toastrService.danger(error.error, "Error");
        route.isPermitted = route.isPermitted;
        this.cdRef.detectChanges();
      });
  }
}
