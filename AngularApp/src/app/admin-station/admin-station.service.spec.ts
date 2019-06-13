import { TestBed } from '@angular/core/testing';

import { AdminStationService } from './admin-station.service';

describe('AdminStationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdminStationService = TestBed.get(AdminStationService);
    expect(service).toBeTruthy();
  });
});
