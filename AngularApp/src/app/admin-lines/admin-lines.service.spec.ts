import { TestBed } from '@angular/core/testing';

import { AdminLinesService } from './admin-lines.service';

describe('AdminLinesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdminLinesService = TestBed.get(AdminLinesService);
    expect(service).toBeTruthy();
  });
});
