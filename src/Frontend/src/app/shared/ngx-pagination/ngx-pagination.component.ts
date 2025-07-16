import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { IPaginationModel } from './Models/IPaginationModel';
import { ITableModel } from './Models/ITableModel';
import { IAddNewModel } from './Models/IAddNewModel';
import { NGXPaginationService } from './ngx-pagination.service';
import { IParamModel } from './Models/IParamModel';

@Component({
  selector: 'ngx-pagination',
  templateUrl: './ngx-pagination.component.html',
  styleUrls: ['./ngx-pagination.component.scss'],
})
export class NgxPaginationComponent<T> {
  @Input() paginationModel: IPaginationModel<T>;

  pageNumbersToPrint: number[] = [];
  pageLengthArray: number[] = [];
  maxNumberOfPagesToRender: number = 5;
  totalPageCount: number;

  tableConfig: ITableModel;
  addNewButtonConfig: IAddNewModel;

  constructor(private ngxPaginationService: NGXPaginationService<T>, private changeDetector: ChangeDetectorRef) {
    this.pageLengthArray = [5, 10, 100];
  }

  ngOnInit() {
    this.ngxPaginationService.get().subscribe(updatedModel => {
      this.paginationModel = updatedModel;

      if (this.paginationModel.pagingConfig) {
        if (this.paginationModel.pagingConfig.totalItems <= 0) {
          console.log('this.paginationModel.pagingConfig.totalItems', this.paginationModel.pagingConfig.totalItems);
        }
        this.totalPageCount = Math.ceil(this.paginationModel.pagingConfig.totalItems / this.paginationModel.pagingConfig.pageLength);

        // Preparing page numbers to print
        // Starting from the pivot, going all the way to the bottom    
        this.pageNumbersToPrint = [];
        for (let i = this.paginationModel.pagingConfig.pageNumber; i > 0 && this.pageNumbersToPrint.length <= this.maxNumberOfPagesToRender; i--) {
          this.pageNumbersToPrint.push(i);
        }
        this.pageNumbersToPrint = this.pageNumbersToPrint.sort();

        // If there was anything above the pivot
        let indexer: number = this.paginationModel.pagingConfig.pageNumber;
        while (this.pageNumbersToPrint[this.pageNumbersToPrint.length - 1] < this.totalPageCount && this.pageNumbersToPrint.length - 1 < this.maxNumberOfPagesToRender) {
          indexer++;
          this.pageNumbersToPrint.push(indexer);
        }

        this.pageNumbersToPrint = this.pageNumbersToPrint.sort();
      }

      this.loadData();
    });
  }

  loadData(): void {
    if (!this.paginationModel)
      throw new Error('Object of type IPaginationModel is expected for paginationModel.');

    // Load table config to render
    if (this.paginationModel.tableConfig) {
      // Works for the table
      // Get column labels for table UI
      let collumnLabels: string[] = Object.keys(
        this.paginationModel.tableConfig.tableMaping,
      );

      // Get variable names from dynamic type T view model for mapping
      // As we are running the loop on label names and extracting the variable names from label and variable mapping,
      // We shall get the variable names in the correct sequence (the sequence of table column label)
      let variableNames: any[] = [];
      collumnLabels.forEach(label => {
        variableNames.push(this.paginationModel?.tableConfig?.tableMaping[label]);
      });

      // Here we filter the source data for UI
      let sourceData: Array<Array<any>> = [];
      this.paginationModel.tableConfig.sourceData.forEach((element) => {
        let elementKeyValuePair: string[] = [];
        Object.getOwnPropertyNames(element).forEach((val: string, idx, array) => {
          if (variableNames.includes(val)) {
            elementKeyValuePair[variableNames.indexOf(val)] = (
              element as any
            )[val];
          }
        },
        );
        sourceData.push(elementKeyValuePair);
      });

      // Load table config to render
      this.tableConfig = {
        columnNames: collumnLabels,
        mappedData: sourceData,
        sourceData: this.paginationModel.tableConfig.sourceData,
        onDelete: this.paginationModel.tableConfig.onDelete,
        onEdit: this.paginationModel.tableConfig.onEdit,
        onView: this.paginationModel.tableConfig.onView,
      };
    }

    // Load add new button config to render
    if (this.paginationModel.addNewElementButtonConfig)
      this.addNewButtonConfig = {
        addNewButtonLabel: this.paginationModel.addNewElementButtonConfig.buttonLabel,
        showIcon: this.paginationModel.addNewElementButtonConfig.showIcon,
        onClick: this.paginationModel.addNewElementButtonConfig.onAdd,
      };

    this.changeDetector.detectChanges();
  }

  search(): void {
    if (this.paginationModel.pagingConfig && this.paginationModel.searchingConfig && typeof (this.paginationModel.searchingConfig.onSearch) == typeof Function) {
      let searchParams: IParamModel = {
        pageLength: this.paginationModel.pagingConfig.pageLength,
        pageNumber: this.paginationModel.pagingConfig.pageNumber,
        searchString: this.paginationModel.searchingConfig?.searchString
      };

      this.paginationModel.searchingConfig.onSearch(searchParams);
    }
  }

  // On page select method shall only update the values but shall not anything to the UI
  // [ngClass] is used in html to update the UI, so click methods shall call this method and UI shall be updated automatically
  // If some other method calls this method, dealing with UI is upto that method
  onPageSelect(pageNumber: number): void {
    if (!this.paginationModel.pagingConfig)
      throw 'Failed to execute operation: Object of IPagingModel can not be undefined';

    if (pageNumber > this.totalPageCount) return;

    // Selected page must be updated in view model
    this.paginationModel.pagingConfig.pageNumber = pageNumber;
    if (this.paginationModel.pagingConfig.onUpdate && typeof (this.paginationModel.pagingConfig.onUpdate) == 'function') {
      let searchParams: IParamModel = {
        pageLength: this.paginationModel.pagingConfig.pageLength,
        pageNumber: this.paginationModel.pagingConfig.pageNumber,
        searchString: this.paginationModel.searchingConfig == undefined ? null : this.paginationModel.searchingConfig.searchString
      };

      this.paginationModel.pagingConfig.onUpdate(searchParams);
    }
    else {
      console.log(`Unable to call method onUpdate. Method hook : ${this.paginationModel.pagingConfig.onUpdate}, type : ${typeof (this.paginationModel.pagingConfig.onUpdate)}`);
    }
  }

  loadNext(): void {
    if (!this.paginationModel.pagingConfig)
      throw 'Failed to execute operation: Object of IPagingModel can not be undefined';

    let selectedPageNumber = this.paginationModel.pagingConfig.pageNumber + 1;

    // Limit page number generation beyond max number of pages
    if (selectedPageNumber > this.totalPageCount) return;

    // Beyond display - high
    // If next page number is not presented as a clickable button in UI
    if (!this.pageNumbersToPrint.includes(selectedPageNumber)) {
      for (let i = 0; i < this.maxNumberOfPagesToRender && this.pageNumbersToPrint[i] < this.totalPageCount; i++) {
        this.pageNumbersToPrint[i] = this.pageNumbersToPrint[i] + 1;
      }
    }

    this.paginationModel.pagingConfig.pageNumber = selectedPageNumber;
    this.onPageSelect(selectedPageNumber);
  }

  loadPrevious(): void {
    if (!this.paginationModel.pagingConfig)
      throw 'Failed to execute operation: Object of IPagingModel can not be undefined';

    let selectedPageNumber = this.paginationModel.pagingConfig.pageNumber - 1;

    if (selectedPageNumber <= 0) return;

    if (!this.pageNumbersToPrint.includes(selectedPageNumber)) {
      for (let i = 0; i < this.maxNumberOfPagesToRender && this.pageNumbersToPrint[i] < this.totalPageCount; i++) {
        this.pageNumbersToPrint[i] = this.pageNumbersToPrint[i] - 1;
      }
    }

    this.paginationModel.pagingConfig.pageNumber = selectedPageNumber;
    this.onPageSelect(selectedPageNumber);
  }

  loadFirst(): void {
    if (!this.paginationModel.pagingConfig)
      throw 'Failed to execute operation: Object of IPagingModel can not be undefined';

    if (this.paginationModel.pagingConfig.pageNumber == 1) return;

    // Beyond display - high
    // If next page number is not presented as a clickable button in UI
    this.pageNumbersToPrint = [];
    for (let i = 0; i < this.maxNumberOfPagesToRender && i < this.totalPageCount; i++) {
      this.pageNumbersToPrint.push(i + 1);
    }

    this.onPageSelect(1);
  }

  loadLast(): void {
    if (!this.paginationModel.pagingConfig)
      throw 'Failed to execute operation: Object of IPagingModel can not be undefined';

    if (this.paginationModel.pagingConfig.pageNumber == this.totalPageCount) return;

    let counter = this.maxNumberOfPagesToRender;
    this.pageNumbersToPrint = [];
    for (let i = this.totalPageCount; i > 0; i--) {
      if (counter > 0) {
        this.pageNumbersToPrint.push(i);
        counter--;
      }
    }

    this.pageNumbersToPrint = this.pageNumbersToPrint.reverse();
    this.onPageSelect(this.totalPageCount);
  }

  onPageLengthChange(): void {
    if (this.paginationModel.pagingConfig?.onUpdate != null && typeof (this.paginationModel.pagingConfig?.onUpdate) == 'function') {
      this.paginationModel.pagingConfig.pageNumber = 1;
      this.paginationModel.pagingConfig.onUpdate(this.paginationModel.pagingConfig);

      this.totalPageCount = Math.ceil(this.paginationModel.pagingConfig.totalItems / this.paginationModel.pagingConfig.pageLength);

      // Preparing page numbers to print
      // When we are creating the pagination for the first time, the pagination shall always start from 1
      this.pageNumbersToPrint = [];
      for (let i = 1; i <= this.totalPageCount; i++) {
        this.pageNumbersToPrint.push(i);
      }
    }
  }
}
