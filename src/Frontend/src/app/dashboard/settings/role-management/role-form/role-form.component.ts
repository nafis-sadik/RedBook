import { Component, Optional } from '@angular/core';
import { NbToastrService, NbWindowRef } from '@nebular/theme';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.scss']
})
export class RoleFormComponent {
  roleName: string;
  isAdminRole: boolean;
  saveMethod: (roleName: string, isAdminRole: boolean) => void;

  constructor(
    @Optional() private ref: NbWindowRef<RoleFormComponent>,
    private toastrService: NbToastrService,
  ) { }

  cancel() {
    this.ref.close();
  }

  submit(title: string) {
    if (title.length <= 0) {
      this.toastrService.warning('Can not save empty title', 'Warning');
      return;
    }
    this.saveMethod(this.roleName, this.isAdminRole);
    this.ref.close(title);
  }
}
