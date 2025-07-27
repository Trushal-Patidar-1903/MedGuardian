import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewHospitalComponent } from './view-hospital.component';

describe('ViewHospitalComponent', () => {
  let component: ViewHospitalComponent;
  let fixture: ComponentFixture<ViewHospitalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewHospitalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewHospitalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
