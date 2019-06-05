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



@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    AdminComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent

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
