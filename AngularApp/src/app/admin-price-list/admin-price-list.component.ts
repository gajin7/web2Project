import { Component, OnInit } from '@angular/core';
import { TicketPriceService } from '../ticket-price/ticket-price.service';
import { FormBuilder, Validators } from '@angular/forms';
import { AdminPriceListService } from './admin-price-list.service';

@Component({
  selector: 'app-admin-price-list',
  templateUrl: './admin-price-list.component.html',
  styleUrls: ['./admin-price-list.component.css']
})
export class AdminPriceListComponent implements OnInit {
  prices : string[];
  TicketType : string;
  UserType : string;
  mssgDiscount : string;
  mssgPrice : string;
  priceListVersions : any = [];
  discountVersions : any = [];
  version : string;

  ticketForm = this.fb.group({
    newPrice: ['', Validators.required],
    newDiscount: ['', Validators.required],
   
  });
  constructor(public GetPriceListservice: TicketPriceService,public service :AdminPriceListService ,public fb: FormBuilder) {
    
   }

  ngOnInit() {
    this.getPricelist();
    this.service.GetPriceListVersions().subscribe((data)=>{
      this.priceListVersions = data;
    });
    this.service.GetDiscountsVersions().subscribe((data)=>{
      this.discountVersions = data;
    });
  }

  public getPricelist()
  {
    this.GetPriceListservice.GetPricelist().subscribe((data) => {
      this.prices = data});
  }

  selected (event: any) {
    //update the ui
    this.TicketType = event.target.value;
  }

  selectedUser (event: any) {
    //update the ui
    this.UserType = event.target.value;
  }

  ChangePrice()
  {
    if(this.TicketType == "TimeTicket")
    {
      this.version = this.priceListVersions[0];
    }
    else if(this.TicketType == "DailyTicket")
    {
      this.version = this.priceListVersions[3];
    }
    else if(this.TicketType == "MonthlyTicket")
    {
      this.version = this.priceListVersions[1];
    }
    else if(this.TicketType == "AnnualTicket")
    {
      this.version = this.priceListVersions[2];
    }
    this.service.ChangeTicket(this.TicketType,this.ticketForm.value.newPrice,this.version).subscribe((d)=>{
      this.mssgPrice = d;
      this.GetPriceListservice.GetPricelist().subscribe((data) => {
        this.prices = data});
        this.service.GetPriceListVersions().subscribe((data)=>{
          this.priceListVersions = data;
        });
        this.service.GetDiscountsVersions().subscribe((data)=>{
          this.discountVersions = data;
        });
    });
  }

  ChangeDiscount()
  {
    if(this.UserType == "student")
    {
      this.version = this.discountVersions[2];
    }
    else if(this.UserType == "retiree")
    {
      this.version = this.discountVersions[1];
    }
    this.service.ChangeDiscount(this.UserType,this.ticketForm.value.newDiscount,this.version).subscribe((d)=>{
      this.mssgDiscount = d;
    this.GetPriceListservice.GetPricelist().subscribe((data) => {
        this.prices = data});
        this.service.GetPriceListVersions().subscribe((data)=>{
          this.priceListVersions = data;
        });
      this.service.GetDiscountsVersions().subscribe((data)=>{
      this.discountVersions = data;
        });
    });
  }
  

}
