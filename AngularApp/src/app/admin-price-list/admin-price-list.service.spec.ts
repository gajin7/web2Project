import { TestBed } from '@angular/core/testing';

import { AdminPriceListService } from './admin-price-list.service';

describe('AdminPriceListService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdminPriceListService = TestBed.get(AdminPriceListService);
    expect(service).toBeTruthy();
  });
});
