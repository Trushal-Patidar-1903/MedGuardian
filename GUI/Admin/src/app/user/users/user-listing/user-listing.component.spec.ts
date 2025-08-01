import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserListingComponent } from './user-listing.component';

describe('UserListingComponent', () => {
  let component: UserListingComponent;
  let fixture: ComponentFixture<UserListingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserListingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserListingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
