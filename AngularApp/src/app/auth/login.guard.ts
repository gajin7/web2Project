import { Injectable } from '@angular/core';
import {
  CanActivate, Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
} from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate, CanActivateChild {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {    
    if (localStorage.role === 'Admin') {
      this.router.navigate(['admin']);
      return false;
    }
    if (localStorage.role === 'AppUser') {
      this.router.navigate(['app-user-home']);
      return false;
    }
    if (localStorage.role === 'Controller') {
      this.router.navigate(['control-ticket']);
      return false;
    }
    // not logged in so redirect to login page
    else {
      
      return true;
    }
  }

  

  canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivate(route, state);
  }

}
