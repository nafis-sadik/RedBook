import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { NbToastrService } from '@nebular/theme';
import { Observable } from 'rxjs';
import { ICommonAttribute } from 'src/app/dashboard/Models/ICommonAttribute';
import { CommonAttributeService } from 'src/app/dashboard/services/common-attribute.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-quantity-unit',
  templateUrl: './quantity-unit.component.html',
  styleUrls: ['./quantity-unit.component.scss']
})
/**
 * QuantityUnitComponent handles managing quantity units and brands.
 * 
 * It receives the list of quantity units and brands as inputs. 
 * It has methods to select a unit/brand, save changes to the API,
 * and reset the selection.
 * 
 * The component coordinates updating the selection, calling the API,
 * and updating the UI list when data changes.
 */
export class QuantityUnitComponent {
  @Input() quantityUnits: Array<ICommonAttribute>;
  @Input() brands: Array<ICommonAttribute>;
  selectedUnit: ICommonAttribute;
  selectedBrand: ICommonAttribute;

  constructor(
    private commonAttributeService: CommonAttributeService,
    private toasterService: NbToastrService,
    private changeDetectionRef: ChangeDetectorRef
  ) {
    this.selectedUnit = {
      attributeId: 0,
      attributeName: '',
      attributeType: ''
    }
    this.selectedBrand = {
      attributeId: 0,
      attributeName: '',
      attributeType: ''
    }
  }

  selectedAttribute(attrId: number, event: any, containerId: string): void {
    let listItems = document.querySelectorAll(`#${containerId} nb-list-item`);

    let currentNode = event.target;
    let targetListItem: Element;

    while (currentNode) {
      // Found section parent
      if (currentNode.tagName === 'NB-LIST-ITEM') {
        targetListItem = currentNode;
        break;
      }

      // Go up to parent node
      currentNode = currentNode.parentElement;
    }

    listItems.forEach(element => {
      // Remove active class from every list item
      element.classList.remove('active');
      if (element == targetListItem) {
        // Add active class to selected list item
        element.classList.add('active');
        return;
      }
    });

    if (containerId == environment.attributeTypes.brands) {
      // Selects the clicked brand from the list
      this.selectedBrand = this.brands.filter(x => x.attributeId == attrId)[0];
    } else {
      // Selects the clicked unit from the list
      this.selectedUnit = this.quantityUnits.filter(x => x.attributeId == attrId)[0];
    }
  }

  saveAttribute(containerId: string): void {
    // Update UI & prepare object for request
    let selectedAttr: ICommonAttribute;
    let attrList: Array<ICommonAttribute>;
    if (containerId == environment.attributeTypes.quantity) {
      // Save Quantity Unit
      selectedAttr = this.selectedUnit;
      selectedAttr.attributeType = environment.attributeTypes.quantity;

      attrList = this.quantityUnits;

      // Reset the form
      this.selectedUnit = {
        attributeId: 0,
        attributeName: '',
        attributeType: ''
      };
    } else {
      // Save Brand
      selectedAttr = this.selectedBrand;
      selectedAttr.attributeType = environment.attributeTypes.brands;

      attrList = this.brands;

      // Reset the form
      this.selectedBrand = {
        attributeId: 0,
        attributeName: '',
        attributeType: ''
      };
    }

    if (selectedAttr.attributeName == ''){
      this.toasterService.danger('Please enter a name for the attribute or select the one you want to update', 'Error');
      return;
    }

    // Send the HTTP request
    let event: Observable<ICommonAttribute>;
    if (selectedAttr.attributeId == 0) {
      event = this.commonAttributeService.addNewAttribute(selectedAttr);
    } else {
      event = this.commonAttributeService.updateExistingAttribute(selectedAttr);
    }

    // Process HTTP response
    event.subscribe((attr: ICommonAttribute) => {
      // Update the list in UI
      let targetAttr = attrList.filter(unit => unit.attributeId == attr.attributeId);
      if (targetAttr.length > 0) {  // For update operation
        targetAttr[0].attributeId = attr.attributeId;
        targetAttr[0].attributeName = attr.attributeName;
        targetAttr[0].attributeType = attr.attributeType;
      } else {                         // For add operation
        attrList.push(attr);
      }
      
      this.resetAttribute(containerId);
      this.toasterService.success('Attribute saved successfully', 'Success');
    });
  }

  resetAttribute(containerId: string): void {
    let listItems = document.querySelectorAll(`#${containerId} nb-list-item`);

    listItems.forEach(element => {
      // Remove active class from every list item
      element.classList.remove('active');
    });

    if (containerId == environment.attributeTypes.quantity) {
      this.selectedUnit = {
        attributeId: 0,
        attributeName: '',
        attributeType: ''
      }
    } else if (containerId == environment.attributeTypes.brands) {
      this.selectedBrand = {
        attributeId: 0,
        attributeName: '',
        attributeType: ''
      }
    } else {
      console.log('Unknown container ID');
    }

    this.changeDetectionRef.detectChanges();
  }

  removeAttribute(attrId: number, containerId: string): void{
    this.commonAttributeService.removeExistingAttribute(attrId)
      .subscribe(() => {
        if (containerId == environment.attributeTypes.quantity) {
          let index = this.quantityUnits.findIndex(x => x.attributeId == attrId);
          this.quantityUnits.splice(index, 1);
        } else if (containerId == environment.attributeTypes.brands) {
          let index = this.brands.findIndex(x => x.attributeId == attrId);
          this.brands.splice(index, 1);
        } else {
          console.log('Unknown container ID');
        }
        this.changeDetectionRef.detectChanges();
      });
  }
}
