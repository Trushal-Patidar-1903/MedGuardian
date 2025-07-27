import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditHospitalComponent } from './add-edit-hospital.component';

describe('AddEditHospitalComponent', () => {
  let component: AddEditHospitalComponent;
  let fixture: ComponentFixture<AddEditHospitalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddEditHospitalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEditHospitalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
