import { TestBed } from '@angular/core/testing';

import { CheckInTicketService } from './check-in-ticket.service';

describe('CheckInTicketService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CheckInTicketService = TestBed.get(CheckInTicketService);
    expect(service).toBeTruthy();
  });
});
