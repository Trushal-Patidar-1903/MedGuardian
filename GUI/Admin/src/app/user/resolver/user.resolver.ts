import { ActivatedRouteSnapshot, Resolve, ResolveFn, Router } from '@angular/router';
import { User, UserService } from '../../Services/UserService/user.service';
import { Observable, catchError, of } from 'rxjs';

export class UserResolver implements Resolve<User | null> {
  constructor(private userService: UserService, private router: Router) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User | null> {
    const id = route.paramMap.get('id');

    if (!id) {
      this.router.navigate(['/users/create']);
      return of(null);
    }

    return this.userService.getUserById(id).pipe(
      catchError(() => {
        this.router.navigate(['/users']);
        return of(null);
      })
    );
  }
}
