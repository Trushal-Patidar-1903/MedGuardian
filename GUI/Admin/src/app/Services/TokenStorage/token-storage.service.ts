import { Injectable } from '@angular/core';
import { defaultPerPageRecords } from '../../shared/constants/constants';


const perPage = `ex-admin-perPage`
@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {

  constructor() { }



  getPerPage() {
    const perPageRecords = window.localStorage.getItem(perPage);
    if (perPageRecords) {
      return +perPageRecords
    }
    return defaultPerPageRecords;
  }
}
