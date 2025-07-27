import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HospitalDashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from '../shared/auth/login/login.component';
import { RegisterComponent } from '../shared/auth/register/register.component'; 

const routes: Routes = [
  //  {
  //   path: '',
  //   redirectTo: 'login',
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'login',
  //   component: LoginComponent,
  //   data: { loginFor: 'hospital' }
  // },
  // {
  //   path: 'register',
  //   component: RegisterComponent,
  //   data: { loginFor: 'hospital' }
  // },
  {
    path : 'dashboard', component : HospitalDashboardComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HospitalRoutingModule { }
