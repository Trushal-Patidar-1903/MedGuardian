import { Directive, EventEmitter, Input, Output } from '@angular/core';

export type SortDirection = 'asc' | 'desc' | '';
const rotate: {[key: string]: SortDirection} = { 'asc': 'desc', 'desc': '', '': 'asc' };

export interface listSortEvent {
  column: string;
  order: SortDirection; 
}

@Directive({
  selector: 'th[listsortable]',
  host: {
    '[class.asc]': 'order === "asc"',
    '[class.desc]': 'order === "desc"',
    '(click)': 'rotate()'
  },
  standalone:false
})
export class NgbdListSortableHeader {

  @Input() listsortable: string = '';
  @Input() order: SortDirection = '';
  @Output() listsort = new EventEmitter<listSortEvent>();

  rotate() {
    this.order = rotate[this.order] ? rotate[this.order] : 'asc';
    this.listsort.emit({column: this.listsortable, order: this.order});
  }
}
