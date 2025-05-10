import { Component, OnInit } from '@angular/core';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';
import { RouteModel } from '../../Models/route.model';
import { NbToastrService, NbDialogService } from '@nebular/theme';
import { IParamModel } from 'src/app/shared/ngx-pagination/Models/IParamModel';
import { NGXPaginationService } from 'src/app/shared/ngx-pagination/ngx-pagination.service';
import { DashboardService } from '../../services/dashboard.service';
import { RouteService } from '../../services/route.service';
import { RouteFormComponent } from '../route-form/route-form.component';

@Component({
  selector: 'app-route-settings',
  templateUrl: './route-settings.component.html',
  styleUrl: './route-settings.component.scss'
})
export class RouteSettingsComponent implements OnInit {
  public pagedRouteModel: IPaginationModel<RouteModel>;

  constructor(
    dashboardService: DashboardService,
    toasterService: NbToastrService,
    private routeService: RouteService,
    public dialogService: NbDialogService,
    private ngxPaginationService: NGXPaginationService<RouteModel>
  ) {
    this.pagedRouteModel = dashboardService.getPagingConfig<RouteModel>(RouteFormComponent, 'Routes', 'Add Route');

    if (this.pagedRouteModel.tableConfig) {
      this.pagedRouteModel.tableConfig.tableMaping = {
        "Route Name": "routeName",
        "Route": "routeValue",
        "Description": "description",
        "Application": "applicationName"
      };

      this.pagedRouteModel.tableConfig.onEdit = (routeData: RouteModel) => {
        console.log('update', routeData);
        this.dialogService.open(RouteFormComponent, {
          context: {
            inputModel: routeData
          },
        });
      }

      this.pagedRouteModel.tableConfig.onView = null;
      // this.pagedRouteModel.tableConfig.onView = (routeModel: RouteModel) => {
      //   console.log(routeModel);
      // }

      // this.pagedRouteModel.tableConfig.onDelete = null;
      this.pagedRouteModel.tableConfig.onDelete = (routeModel: RouteModel) => {
        this.routeService.deleteRoute(routeModel.routeId)
          .subscribe(() => {
            toasterService.success('Feature under development');
            // location.reload();
          });
      }

      this.pagedRouteModel.tableConfig.actionColWidth = '50px';
    }

    if (this.pagedRouteModel.addNewElementButtonConfig) {
      this.pagedRouteModel.addNewElementButtonConfig.onAdd = () => {
        this.dialogService.open(RouteFormComponent, {
          context: {
            inputModel: new RouteModel(),
          }
        });
      }
    }

    // This if is to ensure that the event method binding is successful
    // This if won't be executed on event trigger
    if (this.pagedRouteModel.pagingConfig) {
      this.pagedRouteModel.pagingConfig.onUpdate = (pagingInfo: any) => {
        // This shall be executed during event triggers
        if (this.pagedRouteModel.pagingConfig) {
          this.pagedRouteModel.pagingConfig.pageLength = pagingInfo.pageLength;
          this.pagedRouteModel.pagingConfig.pageNumber = pagingInfo.pageNumber;
        }

        this.loadPagedRoutes();
      }

      if (this.pagedRouteModel.searchingConfig) {
        this.pagedRouteModel.searchingConfig.onSearch = (tableParameters: IParamModel) => {
          if (this.pagedRouteModel.searchingConfig) {
            if (tableParameters) {
              if (tableParameters.searchString)
                this.pagedRouteModel.searchingConfig.searchString = tableParameters.searchString;
              if (this.pagedRouteModel.pagingConfig) {
                this.pagedRouteModel.pagingConfig.pageNumber = 1;
                this.pagedRouteModel.pagingConfig.pageLength = tableParameters.pageLength;
              }

              this.routeService.getPagedRoute(this.pagedRouteModel);
            }

            this.loadPagedRoutes();
          }
        }
      }
    }
  }

  ngOnInit(): void {
    this.loadPagedRoutes();
  }

  loadPagedRoutes(): void {
    let loaderContainer: HTMLElement | null = document.getElementById('LoadingScreen');
    if (loaderContainer) {
      loaderContainer.classList.add('d-block');
      loaderContainer.classList.remove('d-none');
    }

    this.routeService.getPagedRoute(this.pagedRouteModel)
      .subscribe(response => {
        if (this.pagedRouteModel.tableConfig) {
          this.pagedRouteModel.tableConfig.sourceData = response.sourceData;
          console.log('response.sourceData', response.sourceData);
        }

        if (this.pagedRouteModel.pagingConfig) {
          this.pagedRouteModel.pagingConfig.pageNumber = response.pageNumber;
          this.pagedRouteModel.pagingConfig.pageLength = response.pageLength;
          this.pagedRouteModel.pagingConfig.totalItems = response.totalItems;
        }

        if (this.pagedRouteModel.searchingConfig) {
          this.pagedRouteModel.searchingConfig.searchString = response.searchString;
        }

        this.ngxPaginationService.set(this.pagedRouteModel);
        
        if (loaderContainer) {
          loaderContainer.classList.remove('d-block');
          loaderContainer.classList.add('d-none');
        }
      });
  }
}
