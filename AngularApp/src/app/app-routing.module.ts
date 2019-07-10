import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AdminComponent } from './admin/admin.component';

import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { BuyingTicketComponent } from './buying-ticket/buying-ticket.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { TicketPriceComponent } from './ticket-price/ticket-price.component';
import { AppUserBuyTicketComponent } from './app-user-buy-ticket/app-user-buy-ticket.component';
import { AppUserHomeComponent } from './app-user-home/app-user-home.component';
import { CheckInTicketComponent } from './check-in-ticket/check-in-ticket.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { MyTicketsComponent } from './my-tickets/my-tickets.component';
import { ControlTicketComponent } from './control-ticket/control-ticket.component';
import { AdminPriceListComponent } from './admin-price-list/admin-price-list.component';
import { AdminScheduleComponent } from './admin-schedule/admin-schedule.component';
import { AdminLinesComponent } from './admin-lines/admin-lines.component';
import { AdminStationComponent } from './admin-station/admin-station.component';
import { ControlerComponent } from './controler/controler.component';
import { UserGuard } from './auth/user.guard';
import { ControlorGuard } from './auth/controlor.guard';
import { BusMapsComponent } from './bus-maps/bus-maps.component';
import { BusLocationComponent } from './bus-location/bus-location.component';
import { LoginGuard } from './auth/login.guard';

const appRoutes: Routes = [
  { 
    path: '', 
    component: HomeComponent, 
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'login', 
    component: LoginComponent, 
    canActivate: [LoginGuard]
  },
  { 
    path: 'register', 
    component: RegisterComponent, 
    canActivate: [LoginGuard]
  },
  { 
    path: 'home', 
    component: HomeComponent, 
  },
  {
    path: 'buying-ticket',
    component: BuyingTicketComponent
  },
  {
    path: 'schedule',
    component: ScheduleComponent
  },
  {
    path: 'ticket-price',
    component: TicketPriceComponent
  },
  {
    path: 'app-user-buy-ticket',
    component: AppUserBuyTicketComponent,
    canActivate: [UserGuard]
  },
  {
    path: 'app-user-home',
    component: AppUserHomeComponent,
    canActivate: [UserGuard]
  },
  {
    path: 'check-in-ticket',
    component: CheckInTicketComponent
  },
  {
    path: 'edit-profile',
    component: EditProfileComponent,
    canActivate: [UserGuard]
  },
  {
    path: 'control-ticket',
    component: ControlTicketComponent,
    canActivate: [ControlorGuard]
  },
  {
    path: 'my-tickets',
    component: MyTicketsComponent,
    canActivate: [UserGuard]
  },
  {
    path: 'admin-price-list',
    component: AdminPriceListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin-schedule',
    component: AdminScheduleComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin-lines',
    component: AdminLinesComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin-station',
    component: AdminStationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'controler',
    component: ControlerComponent,
    canActivate: [ControlorGuard]
  },
  {
    path: 'bus-maps',
    component: BusMapsComponent,
  },
  {
    path: 'bus-location',
    component: BusLocationComponent,
  },
  {
    path: '**',
    component: PageNotFoundComponent
  },
 
  
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      appRoutes,
    )
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
