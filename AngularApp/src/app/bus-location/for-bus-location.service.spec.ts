import { TestBed } from '@angular/core/testing';

import { ForBusLocationService } from './for-bus-location.service';

describe('ForBusLocationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ForBusLocationService = TestBed.get(ForBusLocationService);
    expect(service).toBeTruthy();
  });
});
