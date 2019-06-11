import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  mssg :string = '';

  registerForm = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Email: ['', Validators.required],
    Password: ['', Validators.required],
    ConfirmPassword: ['', Validators.required],
    Date: ['',Validators.required],
    City: ['', Validators.required],
    Street: ['', Validators.required],
    Number: ['', Validators.required],
    TypeOfPerson: ['', Validators.required],
    Picture: ['', Validators.required],
  });

  selectedImage: any;
  constructor( private fb: FormBuilder,public authService: AuthService,public router: Router) { }

  ngOnInit() {
  }

  selected (event: any) {
    //update the ui
    this.registerForm.value.TypeOfPerson = event.target.value;
    
  }

  onFileSelected(event){
    this.selectedImage = event.target.files;
   
  }
  

  register() {
    this.authService.regiter(this.registerForm.value).subscribe((data) => {
      console.log(data);

      this.mssg = data;
    });
  }

}
