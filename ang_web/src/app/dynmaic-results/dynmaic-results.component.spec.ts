import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynmaicResultsComponent } from './dynmaic-results.component';

describe('DynmaicResultsComponent', () => {
  let component: DynmaicResultsComponent;
  let fixture: ComponentFixture<DynmaicResultsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DynmaicResultsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DynmaicResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
