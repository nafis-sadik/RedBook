import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { NbAuthService } from '@nebular/auth';
import { Observable, tap } from 'rxjs';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private authService: NbAuthService, private router: Router) {
  }

  canActivate(): Observable<boolean> {
    return this.authService.isAuthenticatedOrRefresh() // canActive can return Observable<boolean>, which is exactly what isAuthenticated returns
    .pipe(
      tap((authenticated) => {
        if (!authenticated)
          this.router.navigate(['auth/login']);
      }),
    );
  }
}
