import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  message: string;

  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(public authService: AuthService, public router: Router, private fb: FormBuilder) {
    this.setMessage();
  }



  setMessage() {
    this.message = 'Logged ' + (this.authService.isLoggedIn ? 'in' : 'out');
  }

  login() {
    this.authService.login(this.loginForm.value).subscribe((data) => {
     
   
    if(this.authService.isLoggedIn)
    {
      if(localStorage.role == "Admin")
      {
        this.router.navigate(['admin']);
      }
      else if(localStorage.role == "AppUser")
      {
      this.router.navigate(['app-user-home']);
      }
      else if(localStorage.role == "Controller")
      {
      this.router.navigate(['control-ticket']);
      }
    }
    else
    {
      if(data == "Http failure response for http://localhost:52295/oauth/token: 0 Unknown Error")
      {
        this.message = "[CONNECTION ERROR] Please check your connection";
      }
      else
      {
          this.message = "Username or password incorect";
      }
      
    }
  });
    
    
  }

  logout() {
    this.authService.logout();
    this.setMessage();
  }
}
