import { TestBed } from '@angular/core/testing';

import { NotificationsForBusLocService } from './notification-for-bus-loc.service';

describe('NotificationForBusLocService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NotificationsForBusLocService = TestBed.get(NotificationsForBusLocService);
    expect(service).toBeTruthy();
  });
});
