import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { MainComponent } from './main.component';
import { HomeComponent } from './home/home.component';
import { TopbarComponent } from '../common/topbar/topbar.component';
import { FooterComponent } from '../common/footer/footer.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    MainComponent,
    HomeComponent,
    DashboardComponent
  ],
  imports: [
    CommonModule,
    MainRoutingModule,TopbarComponent,FooterComponent,SharedModule
  ],
  exports:[MainComponent]
})
export class MainModule { }
