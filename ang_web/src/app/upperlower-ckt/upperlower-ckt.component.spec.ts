import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpperlowerCKTComponent } from './upperlower-ckt.component';

describe('UpperlowerCKTComponent', () => {
  let component: UpperlowerCKTComponent;
  let fixture: ComponentFixture<UpperlowerCKTComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpperlowerCKTComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpperlowerCKTComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
