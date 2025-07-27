import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserListingComponent } from './users/user-listing/user-listing.component';
import { AddEditUserComponent } from './users/add-edit-user/add-edit-user.component';
import { ViewUserComponent } from './users/view-user/view-user.component';
import { SharedModule } from "../shared/shared.module";
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    UserListingComponent,
    AddEditUserComponent,
    ViewUserComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule,ReactiveFormsModule
]
})
export class UserModule { }
