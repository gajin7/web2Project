import { TestBed } from '@angular/core/testing';

import { ControlTicketService } from './control-ticket.service';

describe('ControlTicketService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ControlTicketService = TestBed.get(ControlTicketService);
    expect(service).toBeTruthy();
  });
});
