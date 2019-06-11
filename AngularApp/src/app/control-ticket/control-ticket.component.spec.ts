import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlTicketComponent } from './control-ticket.component';

describe('ControlTicketComponent', () => {
  let component: ControlTicketComponent;
  let fixture: ComponentFixture<ControlTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
