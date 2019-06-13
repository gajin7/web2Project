import { Component, OnInit } from '@angular/core';
import { Prices } from './Prices';
import { Discounts } from './Discounts';
import { TicketPriceService } from './ticket-price.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ticket-price',
  templateUrl: './ticket-price.component.html',
  styleUrls: ['./ticket-price.component.css']
})
export class TicketPriceComponent implements OnInit {
   
   prices : string[];
  

  constructor(public service: TicketPriceService,private router: Router) { }

  ngOnInit() {
    
    this.getPricelist();  
  }

  public getPricelist()
  {
    this.service.GetPricelist().subscribe((data) => {
      this.prices = data});
  }

  public goHome()
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

  public buyTicket()
  {
    if(localStorage.role == "AppUser")
    {
      this.router.navigate(['app-user-buy-ticket']);
    }
    else
    {
      this.router.navigate(['buying-ticket']);
    }
    
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
