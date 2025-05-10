import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ICommonAttribute } from 'src/app/dashboard/Models/ICommonAttribute';
import { OrganizationModel } from 'src/app/dashboard/Models/organization.model';
import { CommonAttributeService } from 'src/app/dashboard/services/common-attribute.service';
import { OrganizationService } from 'src/app/dashboard/services/organization.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-product-settings',
  templateUrl: './product-settings.component.html',
  styleUrls: ['./product-settings.component.scss']
})
export class ProductSettingsComponent implements OnInit {
  ownedBusinesses: Array<OrganizationModel>;
  quantityUnits: Array<ICommonAttribute>;
  brands: Array<ICommonAttribute>;
  loaderContainer: HTMLElement = document.getElementById('LoadingScreen') as HTMLElement;

  constructor(
    private commonAttributeService: CommonAttributeService,
    private orgService: OrganizationService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.orgService.getUserOrgs()
      .subscribe((orgList: Array<OrganizationModel>) => {
        this.ownedBusinesses = orgList;
        this.cdRef.detectChanges();
      },
      (error) => {
        console.log('error', error);
      }).add(() => {        
        this.commonAttributeService.getAttributes(environment.attributeTypes.quantity)
          .subscribe((response: Array<ICommonAttribute>) => {
            this.quantityUnits = response;
          });
    
        this.commonAttributeService.getAttributes(environment.attributeTypes.brands)
          .subscribe((response: Array<ICommonAttribute>) => {
            this.brands = response;
          });

        if(this.loaderContainer && this.loaderContainer.classList.contains('d-block')){
          this.loaderContainer.classList.remove('d-block');
          this.loaderContainer.classList.add('d-none');
        }
      });
  }
}
