import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NgxPaginationComponent } from './ngx-pagination.component';

describe('NgxPaginationComponent', () => {
  let component: NgxPaginationComponent;
  let fixture: ComponentFixture<NgxPaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NgxPaginationComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(NgxPaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
