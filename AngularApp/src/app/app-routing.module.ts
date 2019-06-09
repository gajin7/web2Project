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
  },
  { 
    path: 'register', 
    component: RegisterComponent, 
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
