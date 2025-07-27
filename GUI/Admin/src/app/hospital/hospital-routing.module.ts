import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListHospitalComponent } from './hospital/list-hospital/list-hospital.component';
import { AddEditHospitalComponent } from './hospital/add-edit-hospital/add-edit-hospital.component';

const routes: Routes = [
  {
    path:'',
    component:ListHospitalComponent
  },
  {
    path:'create',
    component:AddEditHospitalComponent
  },
  {
    path:'edit/:id',
    component:AddEditHospitalComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HospitalRoutingModule { }
