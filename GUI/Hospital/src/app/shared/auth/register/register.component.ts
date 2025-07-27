import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
// import { environment } from 's';

// @Injectable({
//   providedIn: 'root'
// })

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit{
  loginFor: string = '';
  registerForm!: FormGroup;
  private baseUrl = 'https://localhost:44338/';
  // private baseUrl = environment.apiUrl;
  
  //load the dropdown
  genderList: any[] = [];
  userTypeList: any[] = [];
  stateList: any[] = [];
  cityList: any[] = [];
  hospitalTypeList: any[] = [];
  
  //For Document Upload
  profileImageUrl: string = '';
  qrCodeUrl: string = '';

  constructor(private route: ActivatedRoute,private fb: FormBuilder, private http: HttpClient) {}

  ngOnInit() {
    this.loginFor = this.route.snapshot.data['loginFor'] || 'user';
    this.initForm();
    this.loadDropdowns();
  }
  
  initForm() {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      idGender: [null, Validators.required],
      idUserType: [null, Validators.required],
      mobile: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      profileImageURL: [''],
      qrCode: [''],

      // Only for hospital
      hospitalName: [''],
      hospitalTypeId: [''],
      stateId: [''],
      cityId: [''],
      address: ['']
    });
  }

  
  loadDropdowns() {
    this.http.get<any[]>(`${this.baseUrl}/api/Gender/get-all-gender`).subscribe(res => this.genderList = res);
    this.http.get<any[]>(`${this.baseUrl}/api/User/GetAllUserTypes`).subscribe(res => this.userTypeList = res);
     this.http.get<any[]>(`${this.baseUrl}/api/State/get-states-by-country/${1}`).subscribe(res => this.stateList = res);
    this.http.get<any[]>(`${this.baseUrl}/api/City/get-cities-by-state/${1}`).subscribe(res => this.cityList = res);
    this.http.get<any[]>(`${this.baseUrl}/api/GetHospitalTypes`).subscribe(res => this.hospitalTypeList = res);
  }

  onFileChange(event: any, type: 'profile' | 'qr') {
    const file = event.target.files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      if (type === 'profile') {
        this.profileImageUrl = reader.result as string;
        this.registerForm.patchValue({ profileImageURL: this.profileImageUrl });
      } else {
        this.qrCodeUrl = reader.result as string;
        this.registerForm.patchValue({ qrCode: this.qrCodeUrl });
      }
    };
    reader.readAsDataURL(file);
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    const payload = this.registerForm.value;
    console.log('Payload:', payload);

    this.http.post(`${this.baseUrl}/api/User/RegisterOrUpdateUser`, payload).subscribe({
      next: res => alert('User registered successfully!'),
      error: err => alert('Registration failed.')
    });
  }

  onRegister() {
    console.log(`Registering for: ${this.loginFor}`);
  }
}
