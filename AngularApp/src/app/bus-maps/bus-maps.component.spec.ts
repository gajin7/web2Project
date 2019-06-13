import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BusMapsComponent } from './bus-maps.component';

describe('BusMapsComponent', () => {
  let component: BusMapsComponent;
  let fixture: ComponentFixture<BusMapsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BusMapsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BusMapsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
