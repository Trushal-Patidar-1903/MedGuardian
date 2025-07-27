import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule,CommonModule,FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  form:UntypedFormGroup;
  isShowPassword:boolean= false;

  constructor(){
    this.form = new FormGroup({
      email:new FormControl('', [Validators.required,Validators.email]),
      password: new FormControl('',[Validators.required])
    })
  }


  onSubmit(){

  }

  toggleShowPassword() {
    this.isShowPassword = !this.isShowPassword;
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

}
