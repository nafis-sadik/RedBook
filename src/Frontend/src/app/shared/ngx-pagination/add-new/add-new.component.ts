import { Component, Input } from '@angular/core';
import { IAddNewModel } from '../Models/IAddNewModel';
import { NbDialogService } from '@nebular/theme';

@Component({
  selector: 'ngx-paged-add-new',
  templateUrl: './add-new.component.html',
  styleUrls: ['./add-new.component.scss'],
})
export class AddNewComponent {
  @Input() addNewModel: IAddNewModel;

  constructor(private dialogService: NbDialogService) {
    this.addNewModel = {
      showIcon: true,
      addNewButtonLabel: null,
      onClick: null,
    };
  }

  addNew(): void {
    if (
      this.addNewModel.onClick != null &&
      typeof this.addNewModel.onClick == typeof Function
    )
    
    this.addNewModel.onClick();
  }
}
