import { Component } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {

  constructor(private service : AuthService, private router : Router) { }
Logout()
  {
    this.service.logout();
    this.router.navigate(['login']);

  }
}
