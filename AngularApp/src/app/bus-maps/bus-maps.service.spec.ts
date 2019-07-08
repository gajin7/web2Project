import { TestBed } from '@angular/core/testing';

import { BusMapsService } from './bus-maps.service';

describe('BusMapsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BusMapsService = TestBed.get(BusMapsService);
    expect(service).toBeTruthy();
  });
});
