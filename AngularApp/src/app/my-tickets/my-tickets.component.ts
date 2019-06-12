import { Component, OnInit } from '@angular/core';
import { MyTicketsService } from './my-tickets.service';

@Component({
  selector: 'app-my-tickets',
  templateUrl: './my-tickets.component.html',
  styleUrls: ['./my-tickets.component.css']
})
export class MyTicketsComponent implements OnInit {

  constructor(private service : MyTicketsService) { }

  ngOnInit() {
    this.service.GetTickets().subscribe((data)=>{
      
    });
  }



}
