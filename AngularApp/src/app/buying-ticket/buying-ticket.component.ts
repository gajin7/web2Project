import { Component, OnInit } from '@angular/core';
import { BuyingTicketService } from './buying-ticket.service';
import { Router } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-buying-ticket',
  templateUrl: './buying-ticket.component.html',
  styleUrls: ['./buying-ticket.component.css']
})
export class BuyingTicketComponent implements OnInit {

  ticketForm = this.fb.group({
    email: ['', Validators.required],
   
  });



  constructor(public service: BuyingTicketService, private route: Router,private fb: FormBuilder) { }

  lineType: string = 'Urban';
  message : string = '';
  price : string = '';
  types : string[];
  


  selected (event: any) {
    //update the ui
    this.lineType = event.target.value;
    this.service.GetTicketPrice("TimeTicket",this.lineType).subscribe((data) => {
      this.price = "Your ticket price is: " + data + " RSD";});
  }
  
  
  ngOnInit() {
  }

  buyTimeTicket()
  {
    this.service.BuyTimeTicket(this.ticketForm.value.email).subscribe((data) => {
      this.message = "You successfully bought your ticket. Ticket ID is" + data + ". We also sent it to email: " + this.ticketForm.value.email;});

      
  }

}
