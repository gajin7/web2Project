import { Component, OnInit } from '@angular/core';
import { Prices } from './Prices';
import { Discounts } from './Discounts';
import { TicketPriceService } from './ticket-price.service';

@Component({
  selector: 'app-ticket-price',
  templateUrl: './ticket-price.component.html',
  styleUrls: ['./ticket-price.component.css']
})
export class TicketPriceComponent implements OnInit {
   
   prices : string[];
  

  constructor(public service: TicketPriceService) { }

  ngOnInit() {
    this.service.GetPricelist().subscribe((data) => {
      this.prices = data});

      console.log("test");
      
  }

  

}
