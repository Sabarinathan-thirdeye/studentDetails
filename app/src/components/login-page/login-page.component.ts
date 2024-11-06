import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppModule } from '../../app/app.module';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  email: string = '';
  password: string = '';

  constructor(private router: Router) {}

  onSubmit() {
    if (this.email === 'admin' && this.password === 'admin') {
      alert('login successfully');
      this.router.navigate(['/studentdetails']);
    } else {
      alert('Invalid login credentials');
    }
  }

  navigateToForgotPassword() {
    this.router.navigate(['/forgettenpassword']);
  }
  navigateToRegisterform() {
    this.router.navigate(['/register']);
  }
  navigateToReset() {
    this.router.navigate(['/resetpassword']);
  }
}
