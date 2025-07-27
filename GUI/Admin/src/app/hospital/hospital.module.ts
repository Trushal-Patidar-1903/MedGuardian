import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HospitalRoutingModule } from './hospital-routing.module';
import { ListHospitalComponent } from './hospital/list-hospital/list-hospital.component';
import { AddEditHospitalComponent } from './hospital/add-edit-hospital/add-edit-hospital.component';
import { ViewHospitalComponent } from './hospital/view-hospital/view-hospital.component';
import { SharedModule } from "../shared/shared.module";


@NgModule({
  declarations: [
    ListHospitalComponent,
    AddEditHospitalComponent,
    ViewHospitalComponent
  ],
  imports: [
    CommonModule,
    HospitalRoutingModule,
    SharedModule
]
})
export class HospitalModule { }
