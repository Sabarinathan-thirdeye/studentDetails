import { Component, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';  // Import HttpClientModule
import { StudentDetailsService } from '../services/apiservices.service';  // Ensure correct import path

// Components 
import { AppComponent } from './app.component';
import { LoginPageComponent } from '../components/login-page/login-page.component';
import { RegisterPageComponent } from '../components/register-page/register-page.component';
import { ForgettenPageComponent } from '../components/forgotten-page/forgetten-page.component';
import { ResetPageComponent } from '../components/reset-page/reset-page.component';
import { DatePipe } from '@angular/common';  // Import DatePipe
import { CommonModule } from '@angular/common';  // Import CommonModule
import { StudentdetailsPageComponent } from '../components/studentdetails-page/studentdetails-page.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    RegisterPageComponent,
    ForgettenPageComponent,
    ResetPageComponent,
    StudentdetailsPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CommonModule,
    HttpClientModule
  ],
  providers: [
    DatePipe,  // Add the DatePipe in providers
    StudentDetailsService  // Ensure your service is added to providers
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
