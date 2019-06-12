import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { EditProfileService } from './edit-profile.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  Form = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Email: ['', Validators.required],
    Date: ['',Validators.required],
    City: ['', Validators.required],
    Street: ['', Validators.required],
    Number: ['', Validators.required],
    TypeOfPerson: ['', Validators.required],
  });


  selectedImage: any;
  constructor( private fb: FormBuilder,public Service: EditProfileService,public router: Router) { }


  ngOnInit() {
    this.Service.GetUserInfo().subscribe((data) => {
      this.Form.setValue({
        FirstName: data.FirstName, 
        LastName: data.LastName,
        Email: data.Email,
        Date: data.Date,
        City: data.City,
        Street : data.Street,
        Number : data.Number,
        TypeOfPerson : "",
      });
      
      });
  }

  EditProfile()
  {
    this.Service.EditProfile(this.Form.value).subscribe((data) =>{

    });
  }

}
