import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-listing',
  standalone: false,
  templateUrl: './user-listing.component.html',
  styleUrl: './user-listing.component.scss'
})
export class UserListingComponent {


  columns = [
    { field: 'id', header: 'ID' },
    { field: 'name', header: 'Name' },
    { field: 'email', header: 'Email' }
  ];
  
  data = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' }
  ];

  constructor( private router: Router){}
  
  handleEdit(row: any) {
    console.log('Edit started for:', row);
    this.router.navigate(['users/edit/'+ row.id]).then();
  }
  
  handleUpdate(row: any) {
    console.log('Updated row:', row);
  }
  
  handleDelete(row: any) {
    this.data = this.data.filter(r => r !== row);
  }
  

  

}
