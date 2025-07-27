import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMedicalHistoryComponent } from './user-medical-history.component';

describe('UserMedicalHistoryComponent', () => {
  let component: UserMedicalHistoryComponent;
  let fixture: ComponentFixture<UserMedicalHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UserMedicalHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserMedicalHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
