import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { ControlTicketService } from './control-ticket.service';

@Component({
  selector: 'app-control-ticket',
  templateUrl: './control-ticket.component.html',
  styleUrls: ['./control-ticket.component.css']
})
export class ControlTicketComponent implements OnInit {

  ticketForm = this.fb.group({
    Id: ['', Validators.required],
   
  });

  mssg: string;
  constructor(private service: ControlTicketService , private fb: FormBuilder) { }

  ngOnInit() {
  }


  CheckIn()
  {
    this.service.ControlTicket(this.ticketForm.value.Id).subscribe((data)=>{this.mssg = data});
  }

}
