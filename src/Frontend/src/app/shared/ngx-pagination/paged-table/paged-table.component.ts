import { Component, ElementRef, Input, OnInit, Renderer2 } from '@angular/core';
import { ITableModel } from '../Models/ITableModel';

@Component({
  selector: 'ngx-paged-table',
  templateUrl: './paged-table.component.html',
  styleUrls: ['./paged-table.component.scss'],
})
export class PagedTableComponent {
  @Input() config: ITableModel;

  constructor(private el: ElementRef) { }

  ngAfterViewInit(): void {
    let activeActions = [this.onEdit, this.onView, this.onDelete].filter(fn => fn != null);
    let width = activeActions.length * 25 + 'px';
    this.el.nativeElement.style.setProperty('--action-width', width);
  }

  onEdit(index: number) {
    if (this.config.onEdit && typeof (this.config.onEdit) == typeof (Function))
      this.config.onEdit(this.config.sourceData[index]);
  }

  onView(index: number) {
    if (this.config.onView && typeof (this.config.onView) == typeof (Function))
      this.config.onView(this.config.sourceData[index]);
  }

  onDelete(index: number) {
    if (this.config.onDelete && typeof (this.config.onDelete) == typeof (Function))
      this.config.onDelete(this.config.sourceData[index]);
  }
}
