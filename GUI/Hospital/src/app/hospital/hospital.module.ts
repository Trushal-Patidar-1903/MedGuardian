import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HospitalRoutingModule } from './hospital-routing.module';
// import { LoginComponent } from './login/login.component';
// import { RegisterComponent } from './register/register.component';
import { HospitalDashboardComponent } from './dashboard/dashboard.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    HospitalDashboardComponent
  ],
  imports: [
    CommonModule,
    HospitalRoutingModule,
    FormsModule
  ]
})
export class HospitalModule { }
