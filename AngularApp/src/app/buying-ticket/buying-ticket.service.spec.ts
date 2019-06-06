import { TestBed } from '@angular/core/testing';

import { BuyingTicketService } from './buying-ticket.service';

describe('BuyingTicketService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BuyingTicketService = TestBed.get(BuyingTicketService);
    expect(service).toBeTruthy();
  });
});
