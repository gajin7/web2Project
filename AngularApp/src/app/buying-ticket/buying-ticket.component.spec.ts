import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BuyingTicketComponent } from './buying-ticket.component';

describe('BuyingTicketComponent', () => {
  let component: BuyingTicketComponent;
  let fixture: ComponentFixture<BuyingTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BuyingTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BuyingTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
