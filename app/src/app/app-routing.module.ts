import { NgModule } from '@angular/core';
import { LoginPageComponent } from '../components/login-page/login-page.component';
import { StudentdetailsPageComponent } from '../components/studentdetails-page/studentdetails-page.component';
import { ForgettenPageComponent } from '../components/forgotten-page/forgetten-page.component';
import { ResetPageComponent } from '../components/reset-page/reset-page.component';
import { PageNotFoundComponent } from '../components/page-not-found/page-not-found.component';
import { RegisterPageComponent } from '../components/register-page/register-page.component';
import { Routes, RouterModule } from '@angular/router';
import { CalendarComponent } from '../components/calendar/calendar.component';
import { FullCalendarComponent } from '../components/full-calendar/full-calendar.component';

  const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { 
      path: 'login', 
      component: LoginPageComponent, 
      children: [
        { path: 'resetpassword', component: ResetPageComponent }
      ]
    },
    { 
      path: 'register', 
      component: RegisterPageComponent 
    },
    { 
      path: 'studentdetails', 
      component: StudentdetailsPageComponent,
      children: [
      ]
    },
    { path: 'opencalendar', component: CalendarComponent },
    { path: 'fullcalendar', component: FullCalendarComponent },
    { path: 'forgettenpassword', component: ForgettenPageComponent },
    { path: 'page-not-found', component: PageNotFoundComponent },
    { path: '**', component: PageNotFoundComponent }
  ];
  

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
