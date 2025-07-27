import { Component } from '@angular/core';
import { FormArray,FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-user-medical-history',
  standalone: false,
  templateUrl: './user-medical-history.component.html',
  styleUrl: './user-medical-history.component.scss'
})
export class UserMedicalHistoryComponent {
   medicalHistoryForm: FormGroup;
   bloodGroups: any[] = [];
   private baseUrl = 'https://localhost:44338/';

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.medicalHistoryForm = this.fb.group({
      idUserMedicalData: [0],
      createdBy: [0],
      modifiedBy: [0],
      idUser: [0],
      idBloodGroup: [0, Validators.required],
      age: [0, Validators.required],
      weight: [0, Validators.required],
      height: [0, Validators.required],
      isActive: [true],
      isDeleted: [true],
      userContacts: this.fb.array([this.initContact()]),
      allergies: this.fb.array([this.initAllergy()]),
      surgicalHistories: this.fb.array([this.initSurgery()]),
      medicalHistories: this.fb.array([this.initCondition()])
    });
  }

  ngOnInit(): void {
    this.fetchBloodGroups();
  }

   initContact(): FormGroup {
    return this.fb.group({
      idUserContact: [0],
      idRelationType: [0, Validators.required],
      isDefault: [true],
      contactNo: ['', Validators.required]
    });
  }

   initAllergy(): FormGroup {
    return this.fb.group({
      idUserAllergy: [0],
      allergy: ['', Validators.required]
    });
  }

   initSurgery(): FormGroup {
    return this.fb.group({
      idSurgicalHistory: [0],
      surgery: ['', Validators.required]
    });
  }

  initCondition(): FormGroup {
    return this.fb.group({
      idMedicalHistory: [0],
      condition: ['', Validators.required]
    });
  }

   get userContacts() {
    return this.medicalHistoryForm.get('userContacts') as FormArray;
  }

   get allergies() {
    return this.medicalHistoryForm.get('allergies') as FormArray;
  }

  get surgicalHistories() {
    return this.medicalHistoryForm.get('surgicalHistories') as FormArray;
  }

  get medicalHistories() {
    return this.medicalHistoryForm.get('medicalHistories') as FormArray;
  }

  addContact() {
    this.userContacts.push(this.initContact());
  }

  addAllergy() {
    this.allergies.push(this.initAllergy());
  }

  addSurgery() {
    this.surgicalHistories.push(this.initSurgery());
  }

  addCondition() {
    this.medicalHistories.push(this.initCondition());
  }

  // ngOnInit(): void {
  //   this.historyForm = this.fb.group({
  //     condition: ['', Validators.required],
  //     medication: ['', Validators.required],
  //     allergy: [''],
  //     notes: [''],
  //     bloodGroup: ['', Validators.required],
  //     age: ['', [Validators.required, Validators.min(0)]],
  //     weight: ['', [Validators.required, Validators.min(0)]],
  //     height: ['', [Validators.required, Validators.min(0)]]
  //   });

  //   this.fetchBloodGroups();
  // }

  fetchBloodGroups(): void {
    // Replace with actual API URL
    this.http.get<string[]>(`${this.baseUrl}/api/BloodGroup/get-all-bloodgroup`).subscribe({
      next: (res) => {
        this.bloodGroups = res;
      },
      error: (err) => {
        console.error('Failed to fetch blood groups', err);
      }
    });
  }

  // onSubmit() {
  //   if (this.historyForm.valid) {
  //     console.log('Submitted:', this.historyForm.value);
  //     // Send to API here
  //   }
  // }

  submitForm() {
    if (this.medicalHistoryForm.valid) {
      this.http.post(`${this.baseUrl}/UserMedicalData/post-user-medical-data`, this.medicalHistoryForm.value)
        .subscribe({
          next: (res) => alert('Medical history saved successfully!'),
          error: (err) => alert('Error saving medical history.')
        });
    } else {
      alert('Please complete all required fields.');
    }
  }
}
