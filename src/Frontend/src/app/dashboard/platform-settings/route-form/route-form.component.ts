import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppService } from '../../services/app.service';
import { RouteService } from '../../services/route.service';
import { IApplicationModel } from '../../Models/IApplicationModel';
import { NbDialogRef, NbToastrService } from '@nebular/theme';
import { RouteModel } from '../../Models/route.model';
import { RouteTypeModel } from '../../Models/route-type.model';

@Component({
  selector: 'app-route-form',
  templateUrl: './route-form.component.html',
  styleUrls: ['./route-form.component.scss']
})

export class RouteFormComponent implements OnInit {
  routeForm: FormGroup;
  inputModel: RouteModel;
  routeTypeList: Array<RouteTypeModel>;
  appList: Array<IApplicationModel>;
  routeList: Array<RouteModel>;
  loaderContainer: HTMLElement;

  constructor(
    private fb: FormBuilder,
    private appService: AppService,
    private routeService: RouteService,
    private toasterService: NbToastrService,
    private dialogRef: NbDialogRef<RouteFormComponent>,
  ) {
    this.loaderContainer = document.getElementById('LoadingScreen') as HTMLElement;
  }

  ngOnInit(): void {
    // Generate form group
    // 2 way model binding - Load data to form and add validation
    this.routeForm = this.fb.group({
      routeName: [this.inputModel.routeName, Validators.required],
      routeValue: [this.inputModel.routeValue],
      applicationId: [this.inputModel.applicationId, Validators.required],
      description: [this.inputModel.description],
      parentRouteId: [this.inputModel.parentRouteId],
      routeTypeId: [this.inputModel.routeTypeId],
    });

    // 2 way model binding - Load data from form to view model
    this.routeForm.valueChanges.subscribe((value) => {
      this.inputModel.applicationId = value.applicationId;
      this.inputModel.description = value.description;
      this.inputModel.routeName = value.routeName;
      this.inputModel.routeValue = value.routeValue;
      this.inputModel.parentRouteId = value.parentRouteId;
      this.inputModel.routeTypeId = value.routeTypeId;
    });

    // Load data for dropdown
    this.appService.getAllApplications().subscribe(value => {
      this.appList = value;
    });

    this.routeService.getAppRoute().subscribe(routeListResponse => {
      this.routeList = routeListResponse;
    });

    this.routeService.getRouteTypes().subscribe(routeTypeList => {
      this.routeTypeList = routeTypeList;
    })
  }

  saveRoute(): void {
    if (this.loaderContainer) {
      this.loaderContainer.classList.add('d-block');
      this.loaderContainer.classList.remove('d-none');
    }

    if (this.inputModel.routeId == undefined || this.inputModel.routeId == null || this.inputModel.routeId <= 0) {
      this.routeService.addNewRoute(this.inputModel)
        .subscribe(() => {
          this.loaderContainer.classList.add('d-none');
          this.loaderContainer.classList.remove('d-block');
          this.toasterService.success('Success', 'Saved Successfully');
          this.dialogRef.close();
        }, (error) => {
          this.loaderContainer.classList.add('d-none');
          this.loaderContainer.classList.remove('d-block');
          this.toasterService.danger('Error', 'Failed to save');
          console.log(error);
        });
    } else {
      this.routeService.updateNewRoute(this.inputModel)
        .subscribe(
          (response) => {
            // Switched to reload as actions have method bindings which unbinds
            // UI update works fine
            // Multiple set not required anymore
            if (this.loaderContainer) {
              this.loaderContainer.classList.remove('d-block');
              this.loaderContainer.classList.add('d-none');
              this.dialogRef.close();
              this.toasterService.success('Success', 'Saved Successfully')
            }
          },
          (err) => {
            console.log(err)
            this.toasterService.danger(err.error, 'Failed to execute operation');

            if (this.loaderContainer) {
              this.loaderContainer.classList.remove('d-block');
              this.loaderContainer.classList.add('d-none');
            }
          });
    }
  }
}
