import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NbDialogRef } from '@nebular/theme';
import { Observable } from 'rxjs';
import { VendorModel } from 'src/app/dashboard/Models/vendor.model';
import { VendorService } from 'src/app/dashboard/services/vendor.service';

@Component({
  selector: 'app-add-vendors',
  templateUrl: './add-vendors.component.html',
  styleUrl: './add-vendors.component.scss'
})
export class AddVendorsComponent {
  vendorForm: FormGroup;

  @Input() organizationId: number = 0;
  @Input() vendorModel: VendorModel = new VendorModel();

  constructor(
    private formBuilder: FormBuilder,
    private vendorService: VendorService,
    private cdRef: ChangeDetectorRef,
    private digRef: NbDialogRef<AddVendorsComponent>
  ) { }

  ngOnInit() {
    this.initializeForm(this.vendorModel);
    this.vendorForm.valueChanges.subscribe(data => {
      this.vendorModel.vendorName = data.vendorName;
      this.vendorModel.address = data.address;
      this.vendorModel.contactPerson = data.contactPerson;
      this.vendorModel.phoneNumber = data.phoneNumber.toString();
      this.vendorModel.emailAddress = data.emailAddress;
      this.vendorModel.remarks = data.remarks;
    });
  }

  initializeForm(model: VendorModel): void {
    this.vendorForm = this.formBuilder.group({
      vendorName: [model.vendorName, Validators.required],
      address: [model.address],
      contactPerson: [model.contactPerson],
      phoneNumber: [model.phoneNumber],
      emailAddress: [model.emailAddress, Validators.email],
      remarks: [model.remarks, Validators.maxLength(250)],
    });

    this.cdRef.detectChanges();
  }

  saveVendor() {
    this.vendorModel.organizationId = this.organizationId;
    this.vendorService.emitFormData(this.vendorModel);
  }
}
