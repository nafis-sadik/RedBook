import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductTrackerComponent } from './product-tracker.component';

describe('ProductTrackerComponent', () => {
  let component: ProductTrackerComponent;
  let fixture: ComponentFixture<ProductTrackerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductTrackerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProductTrackerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
