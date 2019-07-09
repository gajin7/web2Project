import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { ControlTicketService } from './control-ticket.service';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-control-ticket',
  templateUrl: './control-ticket.component.html',
  styleUrls: ['./control-ticket.component.css']
})
export class ControlTicketComponent implements OnInit {

  ticketForm = this.fb.group({
    Id: ['', Validators.required],
   
  });
  image: any;
  imageSrc: any;
  users : any = [];
  mssg: string;
  constructor(private service: ControlTicketService , private logServie : AuthService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.GetUsersToApprove();
  }

  Logout()
  {
    this.logServie.logout();
    this.router.navigate(['login']);

  }


  CheckIn()
  {
    this.service.ControlTicket(this.ticketForm.value.Id).subscribe((data)=>{this.mssg = data});
  }

  GetUsersToApprove()
  {
   this.service.GetUserToApprove().subscribe((data)=>{
     
    this.users = data;
    this.users.forEach(element => {
      element.ImageSrc = 'data:image/jpeg;base64,' + element.Image;
      console.log(element.ImageSrc);
    });
    
    });
  }

  Approve(Email : any)
  {
   this.service.AproveUser(Email).subscribe((data)=> {
    this.GetUsersToApprove();
   });
  }

  Decline(Email : any)
  {
    this.service.DeclineUser(Email).subscribe((data)=> {
      this.GetUsersToApprove();
    });
  }


}
