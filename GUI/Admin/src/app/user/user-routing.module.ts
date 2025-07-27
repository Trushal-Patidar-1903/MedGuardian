import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListingComponent } from './users/user-listing/user-listing.component';
import { AddEditUserComponent } from './users/add-edit-user/add-edit-user.component';
import { UserResolver } from './resolver/user.resolver';

const routes: Routes = [
  {
    path:'',
    component:UserListingComponent
  },
  {
    path:'create',
    component:AddEditUserComponent
  },
  {
    path:'edit/:id',
    component:AddEditUserComponent,
    resolve: {
      user: UserResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule { }
