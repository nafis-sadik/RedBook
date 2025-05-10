import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbToastrService } from '@nebular/theme';
import { RoleModel } from 'src/app/dashboard/Models/role.model';
import { UserModel } from 'src/app/dashboard/Models/user.model';
import { EmployeeService } from 'src/app/dashboard/services/employee.service';
import { OrganizationService } from 'src/app/dashboard/services/organization.service';
import { RoleService } from 'src/app/dashboard/services/role.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  roleList: RoleModel[];
  @Input() userModel: UserModel;
  @Input() selectedBusinessId: number;
  @Input() addUser: Function;
  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private roleService: RoleService,
    private orgService: OrganizationService,
    private toasterService: NbToastrService,
  ) { }

  ngOnInit(): void {
    this.roleService.getOrganizationRoles(this.selectedBusinessId)
      .subscribe((roles) => {
        this.roleList = roles;
      });

    this.userForm = this.fb.group({
      firstName: [this.userModel.firstName],
      lastName: [this.userModel.lastName],
      userName: [this.userModel.userName, Validators.required],
      email: [this.userModel.email, [Validators.required, Validators.email]],
      phoneNumber: [this.userModel.phoneNumber],
      roles: [this.userModel.userRoleIds, Validators.required]
    });

    this.userForm.valueChanges.subscribe((value) => {
      if(this.userModel){
        this.userModel.firstName = value.firstName;
        this.userModel.lastName = value.lastName;
        this.userModel.userName = value.userName;
        this.userModel.email = value.email;
        this.userModel.userRoleIds = value.roles;
        this.userModel.phoneNumber = value.phoneNumber.toString();
      }
    });
  }

  saveUser(): any {
    if (this.userModel) {
      this.userModel.organizationId = this.selectedBusinessId;
      if (this.userModel.userRoleIds) {
        this.userModel.userRoles = [];
        this.userModel.userRoleIds.forEach(roleId => {
          let roleModel = this.roleList.find(role => role.roleId == roleId);
          if(roleModel)
            this.userModel.userRoles.push(roleModel);
        });
      }

      if(this.userModel.userId && this.userModel.userId > 0){
        return this.employeeService.updateEmployeeRoles(this.userModel, this.selectedBusinessId)
          .subscribe(() => {
            setTimeout(() => {
              location.reload();
            }, 500);
            this.toasterService.success('Operation Successfull', 'New user added successfully');
          });
      } else {
        return this.employeeService.registerEmployee(this.userModel, this.selectedBusinessId)
          .subscribe((newUser) => {
            this.addUser(newUser);
            this.toasterService.success('Operation Successfull', 'New user added successfully');
          });
      }
    }
  }
}
