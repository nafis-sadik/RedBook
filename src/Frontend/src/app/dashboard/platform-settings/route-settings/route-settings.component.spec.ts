import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteSettingsComponent } from './route-settings.component';

describe('RouteSettingsComponent', () => {
  let component: RouteSettingsComponent;
  let fixture: ComponentFixture<RouteSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouteSettingsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RouteSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
