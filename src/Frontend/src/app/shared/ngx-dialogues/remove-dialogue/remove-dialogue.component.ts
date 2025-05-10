import { Component, Input, Optional } from '@angular/core';
import { NbWindowRef } from '@nebular/theme';

@Component({
  selector: 'app-remove-category',
  template: `
    <section id="RemoveCategory">
      <div class="row">
        <p>The operation you are attempting to execute is irreversible.</p>
        <p>Are you certain that you intend to proceed?</p>
      </div>
      <div class="row d-flex justify-content-between">
        <button
          nbButton hero
          status="success"
          class="col-md-2 container-custom"
          nbTooltip="Cancel"
          nbTooltipStatus="primary"
          (click)="submit(false)"
        >
          <nb-icon icon="close"></nb-icon>
        </button>
        <button
          nbButton hero
          status="danger"
          class="col-md-2 container-custom"
          nbTooltip="Save"
          nbTooltipStatus="danger"
          (click)="submit(true)"
        >
          <nb-icon icon="checkmark"></nb-icon>
        </button>
      </div>
    </section>
    <style>
      .container-custom{
        margin-right: calc(var(--bs-gutter-x) * .5);
        margin-left: calc(var(--bs-gutter-x) * .5);
      }
    </style>
  `,
})
export class RemoveDialogueComponent {  
  @Input() deleteMethod: Function | undefined;

  constructor(@Optional() private ref: NbWindowRef<RemoveDialogueComponent>) {}

  submit(deleteEntry: boolean) {
    if(deleteEntry && this.deleteMethod){ this.deleteMethod(); }      
    this.ref.close(deleteEntry);
  }
}
