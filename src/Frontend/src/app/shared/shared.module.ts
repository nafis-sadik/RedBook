import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  NbButtonModule,
  NbCardModule,
  NbFormFieldModule,
  NbIconModule,
  NbInputModule,
  NbLayoutModule,
  NbSelectModule,
} from '@nebular/theme';
import { NgxPaginationComponent } from './ngx-pagination/ngx-pagination.component';
import { PagedTableComponent } from './ngx-pagination/paged-table/paged-table.component';
import { AddNewComponent } from './ngx-pagination/add-new/add-new.component';
import { AddDialogueComponent } from './ngx-dialogues/add-dialogue/add-dialogue.component';
import { RemoveDialogueComponent } from './ngx-dialogues/remove-dialogue/remove-dialogue.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { BlumeDropDirective } from './blume-drop.directive';
import { SelectOnFocusDirective } from './select-on-focus.directive';

@NgModule({
  declarations: [
    PagedTableComponent,
    NgxPaginationComponent,
    AddNewComponent,
    AddDialogueComponent,
    RemoveDialogueComponent,
    NotFoundComponent,
    BlumeDropDirective,
    SelectOnFocusDirective
  ],
  imports: [
    NbCardModule,
    CommonModule,
    NbIconModule,
    NbSelectModule,
    NbInputModule,
    NbButtonModule,
    FormsModule,
    NbLayoutModule,
    NbFormFieldModule
  ],
  exports: [ NgxPaginationComponent, CommonModule, FormsModule, BlumeDropDirective, SelectOnFocusDirective ],
})
export class SharedModule {}
