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

  ticketForm = this.fb.group({
    newPrice: ['', Validators.required],
    newDiscount: ['', Validators.required],
   
  });
  constructor(public GetPriceListservice: TicketPriceService,public service :AdminPriceListService ,public fb: FormBuilder) {
    
   }

  ngOnInit() {
    this.getPricelist();
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
    this.service.ChangeTicket(this.TicketType,this.ticketForm.value.newPrice).subscribe((d)=>{
      this.mssgPrice = d;
      this.GetPriceListservice.GetPricelist().subscribe((data) => {
        this.prices = data});
    });
  }

  ChangeDiscount()
  {
    this.service.ChangeDiscount(this.UserType,this.ticketForm.value.newDiscount).subscribe((d)=>{
      this.mssgDiscount = d;
      this.GetPriceListservice.GetPricelist().subscribe((data) => {
        this.prices = data});
    });
  }

}
