import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductsDetailsFormComponent } from './products-details-form.component';

describe('ProductsDetailsFormComponent', () => {
  let component: ProductsDetailsFormComponent;
  let fixture: ComponentFixture<ProductsDetailsFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductsDetailsFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductsDetailsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
