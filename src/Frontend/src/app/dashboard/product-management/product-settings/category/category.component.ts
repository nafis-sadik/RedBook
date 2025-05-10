import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { NbToastrService, NbWindowService } from '@nebular/theme';
import { OrganizationModel } from '../../../Models/organization.model';
import { CategoryModel } from '../../../Models/category.model';
import { AddDialogueComponent } from '../../../../shared/ngx-dialogues/add-dialogue/add-dialogue.component';
import { RemoveDialogueComponent } from '../../../../shared/ngx-dialogues/remove-dialogue/remove-dialogue.component';
import { CategoryService } from 'src/app/dashboard/services/category.service';
import { SubcategoryService } from 'src/app/dashboard/services/subcategory.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})

export class CategoryComponent{
  selectedBusinessId: number | undefined = undefined;
  selectedCategoryId: number | undefined = undefined;

  categories: CategoryModel[] = [];

  subcategories: CategoryModel[] = [];
  
  @Input() ownedBusinesses: OrganizationModel[];


  constructor(
    private windowService: NbWindowService,
    private toastrService: NbToastrService,
    private categoryService: CategoryService,
    private subcategoryService: SubcategoryService,
    private chageDetectorRef: ChangeDetectorRef
  ) { this.ownedBusinesses = []; }

  loadCategories(selectedBusiness: number) {
    this.selectedBusinessId = selectedBusiness;
    this.selectedCategoryId = undefined;
    this.categoryService.getCategoriesUnderOrganization(selectedBusiness)
      .subscribe((categories: any) => {
        this.categories = categories;
        this.subcategories = [];
        this.chageDetectorRef.detectChanges();
      },
      (error) => {console.log(error)}
    );
  }

  loadSubcategories(categoryId: number) {
    this.selectedCategoryId = categoryId;
    this.subcategoryService.getSubcategoriesUnderCategoryId(categoryId)
      .subscribe((subcategories) => {
        this.subcategories = subcategories;
        this.chageDetectorRef.detectChanges();
      });
  }

  openSaveCategoryWindow(windowMessage: string, isSubcategory: boolean, categoryObj: CategoryModel | null) {
    // You can not add/update a category without having a business selected
    if(this.selectedBusinessId == null || this.selectedBusinessId == undefined){
      this.toastrService.warning('Please select a business', 'Warning');
      return;
    }

    // You can not add/update a subcategory without having a category selected
    if (isSubcategory) {
      if (this.selectedCategoryId == null || this.selectedCategoryId == undefined) {
        this.toastrService.warning('Please select a category first', 'Warning');
        return;
      }
    }

    // If UI sent an object, it's an update operation
    // Otherwise it's an incertion operation
    let windowRef = this.windowService.open(AddDialogueComponent, {
      title: windowMessage,
      buttons: {
        close: false,
        fullScreen: true,
        maximize: true,
        minimize: true
      },
      context: {
        saveMethod: (categoryTitle: string) => {
          this.saveCategoryAndSubcategory(categoryTitle, categoryObj, isSubcategory);
        },
        categoryObj: categoryObj,
        textValue: categoryObj?.catagoryName
      }
    });
  }

  saveCategoryAndSubcategory(categoryTitle: string, categoryObj: CategoryModel | null, isSubcategory: boolean) {
    if (categoryTitle != undefined && categoryTitle != null) {
      if (categoryObj == null) {                                        // If the user used create button, categoryObj shall remain null
        if(isSubcategory) {                                             // If the user is working with a subcategory
          if(this.selectedBusinessId){
            this.subcategoryService.addNewSubcategory({
              categoryId: 0,
              catagoryName: categoryTitle,
              organizationId: this.selectedBusinessId,
              parentCategoryId: this.selectedCategoryId
            }).subscribe((category: CategoryModel) => {
              this.subcategories.push(category);
              this.chageDetectorRef.detectChanges();
            });
          }
        } else {                                                        // If the user is working with a category
          if(this.selectedBusinessId){
            this.categoryService.addNewCategory({
              categoryId: 0,
              catagoryName: categoryTitle,
              organizationId: this.selectedBusinessId,
              parentCategoryId: undefined
            }).subscribe((category: CategoryModel) => {
              this.categories.push(category);
              this.chageDetectorRef.detectChanges();
            });
          }
        }
      } else {                                                          // If user used update button, categoryObj brought the stock object for us
        if(isSubcategory) {                                             // If the user is working with a subcategory
          if(this.selectedBusinessId){
            this.subcategoryService.updateSubcategory({
              categoryId: categoryObj.categoryId,
              catagoryName: categoryTitle,
              organizationId: this.selectedBusinessId,
              parentCategoryId: this.selectedCategoryId
            }).subscribe((category: CategoryModel) => {
              for(let i = 0; i < this.subcategories.length; i++){
                if(this.subcategories[i].categoryId == categoryObj.categoryId){
                  this.subcategories[i] = category;
                  break;
                };
              }

              this.chageDetectorRef.detectChanges();
            });
          }
        } else {                                                        // If the user is working with a category
          if(this.selectedBusinessId){
            this.categoryService.updateCategory({
              categoryId: categoryObj.categoryId,
              catagoryName: categoryTitle,
              organizationId: this.selectedBusinessId,
              parentCategoryId: undefined
            }).subscribe((category: CategoryModel) => {
              for (let i = 0; i < this.categories.length; i++){
                if(this.categories[i].categoryId == categoryObj.categoryId){
                  this.categories[i] = category;
                  break;
                }
              }

              this.chageDetectorRef.detectChanges();
            });
          }
        }
      }
    }
  }

  openDeleteCategoryWindow(windowMessage: string, categoryId: number) {
    // Load pop up dialogue
    let windowRef = this.windowService.open(RemoveDialogueComponent, {
      title: windowMessage,
      buttons: {
        close: false,
        fullScreen: true,
        maximize: true,
        minimize: true
      },
      context: {
        deleteMethod: () => { 
          this.categoryService.deleteCategory(categoryId).subscribe(() => {
            this.categories = this.categories.filter(category => category.categoryId != categoryId);
          });
        }
      }
    });

    // Remove element
    windowRef.onClose.subscribe((deleteEntry) => {
      if (deleteEntry) {
        this.categories.forEach(element => {
          if (element.categoryId == categoryId) {
            let index = this.categories.indexOf(element);
            this.categories.splice(index);
            return;
          }
        });
      }
    });
  }
}
