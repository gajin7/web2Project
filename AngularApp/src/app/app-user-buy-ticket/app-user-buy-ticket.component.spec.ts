import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppUserBuyTicketComponent } from './app-user-buy-ticket.component';

describe('AppUserBuyTicketComponent', () => {
  let component: AppUserBuyTicketComponent;
  let fixture: ComponentFixture<AppUserBuyTicketComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppUserBuyTicketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppUserBuyTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
