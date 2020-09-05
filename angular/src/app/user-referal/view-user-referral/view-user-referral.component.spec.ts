import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewUserReferralComponent } from './view-user-referral.component';

describe('ViewUserReferralComponent', () => {
  let component: ViewUserReferralComponent;
  let fixture: ComponentFixture<ViewUserReferralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewUserReferralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewUserReferralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
