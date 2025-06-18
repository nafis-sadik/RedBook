import { Component, Input } from '@angular/core';
import { NbDialogRef } from '@nebular/theme';
import { PurchaseInvoiceDetailsModel } from 'src/app/dashboard/Models/purchase-invoice-details.model';

@Component({
  selector: 'app-lot-selection',
  templateUrl: './lot-selection.component.html',
  styleUrl: './lot-selection.component.scss'
})
export class LotSelectionComponent {
  @Input() availableLots:  Array<PurchaseInvoiceDetailsModel>;
  @Input() selectionCallback: Function;

  constructor(private dialogRef: NbDialogRef<LotSelectionComponent>) {}

  closeDialog() {
    this.dialogRef.close();
  }

  select(): void {
    if(this.selectionCallback){
      let radioValue: any = document.querySelector('input[type="radio"]:checked');
      if(radioValue != null){
        let selectedRecord: number = radioValue.value;
        let selectedLot = this.availableLots.find(x => x.recordId == selectedRecord);
        if(selectedLot){
          this.selectionCallback(selectedLot);
        }
      }
    }

    this.cancel();
  }

  cancel(): void { this.dialogRef.close(); }
}
