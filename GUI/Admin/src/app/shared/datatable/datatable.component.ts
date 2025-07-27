import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DataTableColumn } from '../models/datatable.model';
import { listSortEvent } from '../directives/ngbd-list-sortable-header.directive';
import { SearchInterface } from '../models/common';
import { TokenStorageService } from '../../Services/TokenStorage/token-storage.service';

@Component({
  selector: 'app-datatable',
  standalone: false,
  templateUrl: './datatable.component.html',
  styleUrl: './datatable.component.scss'
})
export class DatatableComponent {
  @Input() columns: { field: string; header: string }[] = [];
  @Input() data: any[] = [];
  @Input() pageSize = 10;
  @Input() tableTitle: string = 'Data Table';


  @Output() onEdit = new EventEmitter<any>();
  @Output() onUpdate = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();

  currentPage = 1;
  editingRow: any = null;
  editedRow: any = {};

  get paginatedData() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.data.slice(start, start + this.pageSize);
  }

  get totalPages() {
    return Math.ceil(this.data.length / this.pageSize);
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  editRow(row: any) {
    this.editingRow = row;
    this.editedRow = { ...row }; // clone to avoid direct mutation
    this.onEdit.emit(row);
  }

  updateRow() {
    Object.assign(this.editingRow, this.editedRow);
    this.onUpdate.emit(this.editingRow);
    this.editingRow = null;
  }

  cancelEdit() {
    this.editingRow = null;
  }

  deleteRow(row: any) {
    this.onDelete.emit(row);
  }
}
