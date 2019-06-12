import { NgModule }       from '@angular/core';
import { BrowserModule }  from '@angular/platform-browser';
import { ReactiveFormsModule , FormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

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
    AdminScheduleComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule {}
