import { Component, OnInit, Optional } from '@angular/core';
import { NbToastrService, NbWindowRef } from '@nebular/theme';

@Component({
  selector: 'app-add-dialogue',
  templateUrl: './add-dialogue.component.html',
  styles: [
    `
      #AddCategoryComponent .row {
        justify-content: space-between;
        align-items: center;
      }
    `,
  ],
})
export class AddDialogueComponent implements OnInit{
  saveMethod: Function;
  textValue: string;

  constructor(
    @Optional() private ref: NbWindowRef<AddDialogueComponent>,
    private toastrService: NbToastrService,
  ) { }

  ngOnInit() {
    this.saveMethod = (this.ref.config.context as any)['saveMethod'];
  }

  cancel() {
    this.ref.close();
  }

  submit(title: string) {
    if (title.length <= 0) {
      this.toastrService.warning('Can not save empty title', 'Warning');
      return;
    }
    this.saveMethod(this.textValue);
    this.ref.close(title);
  }
}
