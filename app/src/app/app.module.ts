import { Component, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';  // Import HttpClientModule
import { StudentDetailsService } from '../services/apiservices.service';  // Ensure correct import path
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from '../services/HttpInterceptor.service';
import { CalendarModule } from 'primeng/calendar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // Required for PrimeNG animations

// Components 
import { AppComponent } from './app.component';
import { LoginPageComponent } from '../components/login-page/login-page.component';
import { ForgettenPageComponent } from '../components/forgotten-page/forgetten-page.component';
import { ResetPageComponent } from '../components/reset-page/reset-page.component';
import { DatePipe } from '@angular/common';  // Import DatePipe
import { CommonModule } from '@angular/common';  // Import CommonModule
import { StudentdetailsPageComponent } from '../components/studentdetails-page/studentdetails-page.component';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { RegisterPageComponent } from '../components/register-page/register-page.component';
import { CalendarComponent } from '../components/calendar/calendar.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { FullCalendarComponent } from '../components/full-calendar/full-calendar.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    ForgettenPageComponent,
    ResetPageComponent,
    StudentdetailsPageComponent,
    NavbarComponent,
    RegisterPageComponent,
    CalendarComponent,
    FullCalendarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    CalendarModule,
    BrowserAnimationsModule,
    FullCalendarModule // register FullCalendar with your app

  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    DatePipe,  // Add the DatePipe in providers
    StudentDetailsService,  // Ensure your service is added to providers
    
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
