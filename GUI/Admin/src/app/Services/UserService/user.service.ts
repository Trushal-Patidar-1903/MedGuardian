import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export interface User {
  id: number;
  name: string;
  email: string;
  // Add other fields
}

@Injectable({
  providedIn: 'root'
})
export class UserService {


  private baseUrl = 'https://localhost:44338//api/User/get-all-user';

  constructor(private http: HttpClient) {}

  getUserById(id: string | number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`);
  }
}
