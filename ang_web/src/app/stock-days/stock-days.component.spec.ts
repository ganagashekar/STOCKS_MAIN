import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockDaysComponent } from './stock-days.component';

describe('StockDaysComponent', () => {
  let component: StockDaysComponent;
  let fixture: ComponentFixture<StockDaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StockDaysComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StockDaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
