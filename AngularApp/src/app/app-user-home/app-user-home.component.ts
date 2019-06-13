import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-app-user-home',
  templateUrl: './app-user-home.component.html',
  styleUrls: ['./app-user-home.component.css']
})
export class AppUserHomeComponent implements OnInit {

  constructor(private service : AuthService, private router : Router) { }

  ngOnInit() {
  }

  Logout()
  {
    this.service.logout();
    this.router.navigate(['login']);

  }

}
