import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from './datatable/datatable.component';
import { NgbdListSortableHeader } from './directives/ngbd-list-sortable-header.directive';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    DatatableComponent,
    NgbdListSortableHeader
  ],
  imports: [
    CommonModule,FormsModule
  ],
  exports:[DatatableComponent]
})
export class SharedModule { }
