import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckInTicketComponent } from './check-in-ticket.component';

describe('CheckInTicketComponent', () => {
  let component: CheckInTicketComponent;
  let fixture: ComponentFixture<CheckInTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckInTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckInTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
