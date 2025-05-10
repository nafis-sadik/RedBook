import { Injectable } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { IPaginationModel } from 'src/app/shared/ngx-pagination/Models/IPaginationModel';

@Injectable({
  providedIn: 'root',
})

export class DashboardService {
  baseUrl = environment.baseUrlUMS;

  selectedOutletId: number = 0;
  selectedOutletName: string = '';

  public readonly ngDialogService: NbDialogService;

  constructor(
    private http: HttpClient,
    private dialogService: NbDialogService) {
    this.ngDialogService = dialogService;
  }

  getPagingConfig<T>(
    dialogueComponent: any,
    cardHeader: string,
    addButtonLabel: string | null = null,
    searchButtonLabel: string | null = null,
    searchFieldPlaceholder: string | null = null
  ): IPaginationModel<T> {
    return {
      organizationId: 0,
      tableCardHeader: cardHeader,
      tableConfig: {
        onEdit: null,
        onDelete: null,
        onView: null,
        actionColWidth: '100px',
        tableMaping: {
          "Invoice Number": "InvoiceNo",
          "Client Name": "ClientName",
          "Issue Date": "IssueDate",
          "Payment Status": "PaymentStatus",
          "Invoice Total": "InvoiceTotal",
          "Paid Amount": "PaidAmount"
        },
        sourceData: []
      },
      pagingConfig: {
        pageNumber: 1,
        totalItems: 268,
        pageLength: 5,
        onUpdate: (data: any) => {
          console.log('Page length change callback', data);
        }
      },
      searchingConfig: {
        searchString: '',
        inputFieldPlaceholder: searchFieldPlaceholder,
        buttonLabel: searchButtonLabel,
        showIcon: true,
        onSearch: (data: any) => {
          console.log('Search Callback', data)
        }
      },
      addNewElementButtonConfig: {
        buttonLabel: addButtonLabel,
        showIcon: true,
        onAdd: () => {
          if (dialogueComponent)
            this.dialogService.open(dialogueComponent);
        }
      },
    };
  }
}
