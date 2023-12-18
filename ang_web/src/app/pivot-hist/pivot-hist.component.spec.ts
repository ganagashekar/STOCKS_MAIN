import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PivotHistComponent } from './pivot-hist.component';

describe('PivotHistComponent', () => {
  let component: PivotHistComponent;
  let fixture: ComponentFixture<PivotHistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PivotHistComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PivotHistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
