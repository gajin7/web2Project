import { Component, OnInit } from '@angular/core';
import { Prices } from './Prices';
import { Discounts } from './Discounts';

@Component({
  selector: 'app-ticket-price',
  templateUrl: './ticket-price.component.html',
  styleUrls: ['./ticket-price.component.css']
})
export class TicketPriceComponent implements OnInit {
   
   prices : Prices[];
   discounts : Discounts[];

  constructor() { }

  ngOnInit() {
  }

}
