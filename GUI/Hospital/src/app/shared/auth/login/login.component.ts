import { Component, OnInit } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit{
    loginForm!: FormGroup;
    loginFor: string = '';
    isSubmitting = false;
    private baseUrl = 'https://localhost:44338/';
      constructor(private route: ActivatedRoute,private fb: FormBuilder, private http: HttpClient,private router: Router) {}

  ngOnInit() {
    this.loginFor = this.route.snapshot.data['loginFor'] || 'user';

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  onLogin() {
    console.log(`Logging in for: ${this.loginFor}`);
  }

  //  onSubmit(): void {
  //   if (this.loginForm.invalid) return;

  //   this.isSubmitting = true;

  //   const loginData = this.loginForm.value;

  //   const apiUrl = this.loginFor === 'hospital'
  //     ? '/api/hospital/login'
  //     : '/api/user/login';

  //   this.http.post<any>('api/postlogin', loginData).subscribe({
  //     next: (res) => {
  //       console.log('Login Success:', res);
  //       // handle success (e.g. store token, navigate)
  //     const token = res?.token;
  //     const userType = res?.userType?.toLowerCase();
  //     const userName = res?.userName;
      
  //     // success Story, Crousel, Ad In Home Page
  //     // Save to localStorage/session if needed
  //     localStorage.setItem('userName', userName);
  //     localStorage.setItem('userType', userType);
  //     },
  //     error: (err) => {
  //       console.error('Login Failed:', err);
  //       // handle error
  //     },
  //     complete: () => this.isSubmitting = false
  //   });
  // }

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.isSubmitting = true;

    const loginData = this.loginForm.value;

    const apiUrl = this.loginFor === 'hospital'
      ? `${this.baseUrl}/api/User/post-user-logIn`
      : `${this.baseUrl}/api/User/post-user-logIn`;

    this.http.post<any>(apiUrl, loginData).subscribe({
      next: (res) => {
        const token = res?.token;
        const userType = res?.userType?.toLowerCase();
        const userName = res?.userName;

        localStorage.setItem('userName', userName);
        localStorage.setItem('userType', userType);
        this.router.navigate([`/user/dashboard`]);
        this.router.navigate([`/${userType}/dashboard`]);
      },
      error: (err) => {
        console.error('Login Failed:', err);
        const userInput = prompt('Invalid login. Please try again. Enter "2" to go to hospital dashboard.');
        if(userInput == "2")
        this.router.navigate([`/hospital/dashboard`]);
        if(userInput == "1")
        this.router.navigate([`/user/dashboard`]);
      },
      complete: () => this.isSubmitting = false
    });
  }
}
