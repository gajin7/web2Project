import { TestBed } from '@angular/core/testing';

import { MyTicketsService } from './my-tickets.service';

describe('MyTicketsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MyTicketsService = TestBed.get(MyTicketsService);
    expect(service).toBeTruthy();
  });
});
