import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundComponent } from './common/not-found/not-found.component';
import { LoginComponent } from './common/login/login.component';

const routes: Routes = [
  {
    path:'',
    loadChildren:()=> import('../app/main/main.module').then( m => m.MainModule)
  },
  {
    path:'login',
    component:LoginComponent
  },
  {
    path:'**',
    component:NotFoundComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
