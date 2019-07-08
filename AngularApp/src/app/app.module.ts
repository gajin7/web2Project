import { NgModule }       from '@angular/core';
import { BrowserModule }  from '@angular/platform-browser';
import { ReactiveFormsModule , FormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent }            from './app.component';
import { PageNotFoundComponent }   from './page-not-found/page-not-found.component';

import { AppRoutingModule }        from './app-routing.module';
import { AdminComponent } from './admin/admin.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { BuyingTicketComponent } from './buying-ticket/buying-ticket.component';
import { TicketPriceComponent } from './ticket-price/ticket-price.component';
import { AppUserBuyTicketComponent } from './app-user-buy-ticket/app-user-buy-ticket.component';
import { AppUserHomeComponent } from './app-user-home/app-user-home.component';
import { CheckInTicketComponent } from './check-in-ticket/check-in-ticket.component';
import { ControlTicketComponent } from './control-ticket/control-ticket.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { MyTicketsComponent } from './my-tickets/my-tickets.component';
import { AdminPriceListComponent } from './admin-price-list/admin-price-list.component';
import { AdminScheduleComponent } from './admin-schedule/admin-schedule.component';
import { AdminLinesComponent } from './admin-lines/admin-lines.component';
import { AdminStationComponent } from './admin-station/admin-station.component';
import { EditProfileService } from './edit-profile/edit-profile.service';
import { JwtInterceptor } from './auth/jwt-interceptor';
import { MyTicketsService } from './my-tickets/my-tickets.service';
import { BuyTicketService } from './app-user-buy-ticket/buy-ticket.service';
import { ControlerComponent } from './controler/controler.component';
import { BusMapsComponent } from './bus-maps/bus-maps.component';
import { AgmDirectionModule } from 'agm-direction';
import { AgmCoreModule } from '@agm/core';
import { NgxPayPalModule } from 'ngx-paypal';
import { BusLocationComponent } from './bus-location/bus-location.component';




@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    AdminComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    ScheduleComponent,
    BuyingTicketComponent,
    TicketPriceComponent,
    AppUserBuyTicketComponent,
    AppUserHomeComponent,
    CheckInTicketComponent,
    ControlTicketComponent,
    EditProfileComponent,
    MyTicketsComponent,
    AdminPriceListComponent,
    AdminScheduleComponent,
    AdminLinesComponent,
    AdminStationComponent,
    ControlerComponent,
    BusMapsComponent,
    BusLocationComponent,
  

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    AgmDirectionModule,
    AgmCoreModule.forRoot({apiKey: 'AIzaSyDnihJyw_34z5S1KZXp90pfTGAqhFszNJk'}),
    NgxPayPalModule,
  ],

  providers: [ 
    EditProfileService,{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    MyTicketsService,{provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    BuyTicketService,{provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
    
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule {}
