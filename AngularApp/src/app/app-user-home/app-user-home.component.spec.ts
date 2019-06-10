import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppUserHomeComponent } from './app-user-home.component';

describe('AppUserHomeComponent', () => {
  let component: AppUserHomeComponent;
  let fixture: ComponentFixture<AppUserHomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppUserHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppUserHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
