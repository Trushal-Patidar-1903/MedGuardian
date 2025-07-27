import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main.component';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  {
    path:'',
    component:MainComponent,
    children:[
      {
        path:'',
        // component:HomeComponent,
        component:DashboardComponent
      },
      {
        path:'hospitals',
        loadChildren:()=> import('../hospital/hospital.module').then(m=>m.HospitalModule)
      },
      {
        path:'users',
        loadChildren:()=> import('../user/user.module').then(m=>m.UserModule)
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule { }
