import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ModuleSelectorComponent } from './module-selector/module-selector.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { LoginComponent } from './shared/auth/login/login.component';
import { RegisterComponent } from './shared/auth/register/register.component';
// import { HospitalDashboardComponent } from './hospital/dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent },

  // Auth routes
  { path: 'login', component: LoginComponent },
  // { path: 'register/:type', component: RegisterComponent }, // :type = 'user' or 'hospital'
  {
    path: 'hospital/register',
    component: RegisterComponent,
    data: { loginFor: 'hospital' }
  },
  {
    path: 'user/register',
    component: RegisterComponent,
    data: { loginFor: 'user' }
  },
  { path: 'module-selector', component: ModuleSelectorComponent },

  // Lazy loaded hospital module
  {
    path: 'hospital',
    loadChildren: () =>
      import('./hospital/hospital.module').then(m => m.HospitalModule)
  },

  // Lazy loaded user module
  {
    path: 'user',
    loadChildren: () =>
      import('./user/user.module').then(m => m.UserModule)
  },

  // Wildcard for 404
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
