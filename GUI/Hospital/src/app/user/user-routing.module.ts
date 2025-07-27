import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../shared/auth/login/login.component';
import { RegisterComponent } from '../shared/auth/register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserMedicalHistoryComponent } from './user-medical-history/user-medical-history.component';

const routes: Routes = [
  //  {
  //   path: '',
  //   redirectTo: 'login',
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'login',
  //   component: LoginComponent,
  //   data: { loginFor: 'user' }
  // },
  // {
  //   path: 'register',
  //   component: RegisterComponent,
  //   data: { loginFor: 'user' }
  // },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'medical-history', component: UserMedicalHistoryComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
