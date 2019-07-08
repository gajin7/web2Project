import { Component, OnInit } from '@angular/core';
import { BuyingTicketService } from './buying-ticket.service';
import { Router } from '@angular/router';
import { Validators, FormBuilder } from '@angular/forms';
import {
  IPayPalConfig,
  ICreateOrderRequest 
} from 'ngx-paypal';


@Component({
  selector: 'app-buying-ticket',
  templateUrl: './buying-ticket.component.html',
  styleUrls: ['./buying-ticket.component.css']
})
export class BuyingTicketComponent implements OnInit {

  public payPalConfig?: IPayPalConfig;

  ticketForm = this.fb.group({
    email: ['', Validators.required],
   
  });
  showCancel: boolean;
  showError: boolean;



  constructor(public service: BuyingTicketService, private route: Router,private fb: FormBuilder) { }

  lineType: string = 'Urban';
  message : string = '';
  price : string = '';
  types : string[];
  


  selected (event: any) {
    //update the ui
    this.lineType = event.target.value;
    this.service.GetTicketPrice("TimeTicket",this.lineType).subscribe((data) => {
      this.price = "Your ticket price is: " + data + " USD";});
  }
  
  
  ngOnInit() {
    
  }

  SetTimeTicketForSale()
  {
    this.service.SetTimeTicketForSale(this.lineType,this.ticketForm.value.email).subscribe((data) => {
      this.message = data.mssg + data.mail; this.price = data.price  

      if(data.price != "")
      {
        this.initConfig();
      }
     
    });
  }

  buyTimeTicket(data: any)
  {
    this.service.BuyTimeTicket(data, this.ticketForm.value.email).subscribe((data) => {
      window.alert("You successfully bought your ticket. Ticket ID: " + data)

      this.route.navigate(['home']);
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
          this.buyTimeTicket(data);
          
          
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

