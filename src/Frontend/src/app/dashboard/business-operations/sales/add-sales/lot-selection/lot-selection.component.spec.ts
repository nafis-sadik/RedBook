import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LotSelectionComponent } from './lot-selection.component';

describe('LotSelectionComponent', () => {
  let component: LotSelectionComponent;
  let fixture: ComponentFixture<LotSelectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LotSelectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LotSelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
