import { Component, Input, OnInit, Renderer2 } from '@angular/core';
import { ITableModel } from '../Models/ITableModel';

@Component({
  selector: 'ngx-paged-table',
  templateUrl: './paged-table.component.html',
  styleUrls: ['./paged-table.component.scss'],
})
export class PagedTableComponent implements OnInit{
  @Input() config: ITableModel;

  constructor(private renderer:Renderer2) { }

  ngOnInit(): void {
    let column = Array.from(document.getElementsByClassName('table-actions'));

    // Use Renderer2 to set the CSS style dynamically
    column.forEach(row => {
      this.renderer.setStyle(row, 'width', this.config.actionColWidth);
    });
  }

  onEdit(index: number){
    if(this.config.onEdit && typeof(this.config.onEdit) == typeof(Function))
      this.config.onEdit(this.config.sourceData[index]);
  }

  onView(index: number){
    if(this.config.onView && typeof(this.config.onView) == typeof(Function))
      this.config.onView(this.config.sourceData[index]);
  }

  onDelete(index: number){
    if(this.config.onDelete && typeof(this.config.onDelete) == typeof(Function))
      this.config.onDelete(this.config.sourceData[index]);
  }
}
