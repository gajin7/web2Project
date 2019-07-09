import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';
import { BuyTicketService } from './buy-ticket.service';
import {
  IPayPalConfig,
  ICreateOrderRequest 
} from 'ngx-paypal';
import { EditProfileService } from '../edit-profile/edit-profile.service';

@Component({
  selector: 'app-buying-ticket',
  templateUrl: './app-user-buy-ticket.component.html',
  styleUrls: ['./app-user-buy-ticket.component.css']
})

export class AppUserBuyTicketComponent implements OnInit {

 

  constructor(public service: BuyTicketService, public profileService: EditProfileService,  private route: Router,private fb: FormBuilder) { }

  public payPalConfig?: IPayPalConfig;
  ticketType : string = '';
  lineType: string = 'Urban';
  message : string = '';
  price : string = '';
  types : string[];
  status: string;
  


  selectedLineType (event: any) {
    //update the ui
    this.lineType = event.target.value;
    this.service.GetTicketPrice(this.ticketType,this.lineType).subscribe((data) => {
      this.price = "Your ticket price is: " + data + " USD";});
  }
  
  selected (event: any) {
    //update the ui
    this.ticketType = event.target.value;
    this.service.GetTicketPrice(this.ticketType,this.lineType).subscribe((data) => {
    this.price = "Your ticket price is: " + data + " USD";});
  }
  
  ngOnInit() {
    this.profileService.GetUserInfo().subscribe((data)=>{
      this.status = data.Status;

      if(this.status != "Approved")
      {
           window.alert("You can't buy ticket, because your account isn't approved by controlor. Current status: " + this.status + " . Please try again later");
           this.route.navigate(['app-user-home']);
      }
    });
  }

  buyTicket(data: any)
  {
    this.service.BuyTicket(this.ticketType, data).subscribe((data) => {
      this.message =  data;
      window.alert("You successfully bought your ticket. Ticket ID: " + data)

      this.route.navigate(['app-user-home']);
    });

      
  }

  SetTIcketForSale()
  {
    this.service.GetTicketForSale(this.ticketType).subscribe((data)=> {
      this.message = data.mssg + data.mail; this.price = data.price  

      if(data.price != "")
      {
        this.initConfig();
        
      }
    });
  }

  private initConfig(): void {
    
   
    /*var diffDays =this.priceWDiscount;
.
    console.log("cena u dinarima: ", diffDays);
    diffDays = diffDays/118;
    var str = diffDays.toFixed(2);
    console.log("cena u evrima: ", str);*/

    this.payPalConfig = {
      currency: 'USD',
      clientId: 'sb',

      createOrderOnClient: (data) => <ICreateOrderRequest> {
          intent: 'CAPTURE',
          purchase_units: [{
              amount: {
                  currency_code: 'USD',
                  value: this.price,
                  breakdown: {
                      item_total: {
                          currency_code: 'USD',
                          value: this.price
                      }
                  }
              },
              items: [{
                  name: 'Enterprise Subscription',
                  quantity: '1',
                  category: 'DIGITAL_GOODS',
                  unit_amount: {
                      currency_code: 'USD',
                      value: this.price,
                  },
              }]
          }]
      },
      advanced: {
          commit: 'true'
      },
      style: {
          label: 'paypal',
          layout: 'horizontal',
          size:  'medium',
    shape: 'pill',
    color:  'blue' 

      },

      onApprove: (data, actions) => {
          console.log('onApprove - transaction was approved, but not authorized', data, actions);
          //actions.order.get().then(details => {
            //  console.log('onApprove - you can get full order details inside onApprove: ', details);
         // });

      },
      onClientAuthorization: (data) => {
          console.log('onClientAuthorization - you should probably inform your server about completed transaction at this point', data);
          this.buyTicket(data);
        
          
          
          
      },
      onCancel: (data, actions) => {
          console.log('OnCancel', data, actions);
         // this.showCancel = true;

      },
      onError: err => {
        window.alert("Something went wrong!");
          console.log('OnError', err);
          //this.showError = true;
      },
      onClick: (data, actions) => {
          console.log('onClick', data, actions);
          //this.resetStatus();
      },
  };
}

}
