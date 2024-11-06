import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from '../components/login-page/login-page.component';
import { RegisterPageComponent } from '../components/register-page/register-page.component';
import { StudentdetailsPageComponent } from '../components/studentdetails-page/studentdetails-page.component';
import { ForgettenPageComponent } from '../components/forgotten-page/forgetten-page.component';
import { ResetPageComponent } from '../components/reset-page/reset-page.component';
import { PageNotFoundComponent } from '../components/page-not-found/page-not-found.component';

const routes: Routes = [
    { path: '', redirectTo: '/login', pathMatch: 'full' },
    { path: 'login', component: LoginPageComponent },
    { path: 'register', component: RegisterPageComponent },
    { path: 'studentdetails', component: StudentdetailsPageComponent },
    { path: 'forgettenpassword', component: ForgettenPageComponent },
    { path: 'resetpassword', component: ResetPageComponent },
    { path: 'page-not-found', component: PageNotFoundComponent },
    { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
