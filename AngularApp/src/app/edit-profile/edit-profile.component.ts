import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { EditProfileService } from './edit-profile.service';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

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

  changePasswordForm = this.fb.group({
    OldPassword :['', Validators.required],
    NewPassword :['', Validators.required],
    ConfirmPassword :['', Validators.required]
  });


  selectedImage: any;
  mssg : string;
  mssgEdit : string;
  status : string;
  imageSource : any;
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
        TypeOfPerson : data.Type,
      });
      this.status = data.Status;
      this.imageSource = 'data:image/jpeg;base64,' + data.Image;

      });
     
  }

  onFileSelected(event){
    this.selectedImage = event.target.files;
   
  }

  ChangeImage()
  {
    if (this.selectedImage != undefined){
      this.Service.EditImage(this.selectedImage, this.Form.value.Email).subscribe((data) =>{
        this.Service.GetUserInfo().subscribe((data) => {
          this.status = data.Status;
          this.imageSource = 'data:image/jpeg;base64,' + data.Image;
          this.Form.setValue({
            FirstName: data.FirstName, 
            LastName: data.LastName,
            Email: data.Email,
            Date: data.Date,
            City: data.City,
            Street : data.Street,
            Number : data.Number,
            TypeOfPerson : data.Type,
          });
           // window.alert(data);
          });
      //this.mssg += data;
   });
  }
  }

  EditProfile()
  {
    this.Service.EditProfile(this.Form.value).subscribe((data) =>{
      this.mssgEdit = data;
      window.alert(data);
      
    this.Service.GetUserInfo().subscribe((data) => {
      this.status = data.Status;
      this.imageSource = 'data:image/jpeg;base64,' + data.Image;
      this.Form.setValue({
        FirstName: data.FirstName, 
        LastName: data.LastName,
        Email: data.Email,
        Date: data.Date,
        City: data.City,
        Street : data.Street,
        Number : data.Number,
        TypeOfPerson : data.Type,
      });
     
      });
      
    });
  }

  ChangePassword()
  {
    this.Service.ChangePassword(this.changePasswordForm.value).subscribe((data) =>{
        this.mssg = data;
        window.alert(data);
      }); 
  }

}
