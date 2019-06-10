import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';
import { BuyTicketService } from './buy-ticket.service';

@Component({
  selector: 'app-buying-ticket',
  templateUrl: './app-user-buy-ticket.component.html',
  styleUrls: ['./app-user-buy-ticket.component.css']
})

export class AppUserBuyTicketComponent implements OnInit {

  ticketForm = this.fb.group({
    email: ['', Validators.required],
   
  });

  constructor(public service: BuyTicketService, private route: Router,private fb: FormBuilder) { }

  ticketType : string = '';
  lineType: string = 'Urban';
  message : string = '';
  price : string = '';
  types : string[];
  


  selectedLineType (event: any) {
    //update the ui
    this.lineType = event.target.value;
    this.service.GetTicketPrice(this.ticketType,this.lineType).subscribe((data) => {
      this.price = "Your ticket price is: " + data + " RSD";});
  }
  
  selected (event: any) {
    //update the ui
    this.ticketType = event.target.value;
    this.service.GetTicketPrice(this.ticketType,this.lineType).subscribe((data) => {
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
