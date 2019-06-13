import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { CheckInTicketService } from './check-in-ticket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-check-in-ticket',
  templateUrl: './check-in-ticket.component.html',
  styleUrls: ['./check-in-ticket.component.css']
})
export class CheckInTicketComponent implements OnInit {

  
  ticketForm = this.fb.group({
    Id: ['', Validators.required],
   
  });

  mssg: string;
  constructor(private service: CheckInTicketService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
  }


  CheckIn()
  {
    this.service.CheckInTcket(this.ticketForm.value.Id).subscribe((data)=>{this.mssg = data});
  }

  Navigate()
  {
    if(localStorage.role == "AppUser")
    {
      this.router.navigate(['app-user-home']);
    }
    else
    {
      this.router.navigate(['home']);
    }
    
  }


}
