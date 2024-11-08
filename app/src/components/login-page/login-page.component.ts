// login-page.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = ''; // Variable to hold the error message if login fails

  constructor(private router: Router, private authService: AuthService) {}

  
  onSubmit() {
      // Call login service when the form is submitted
      this.authService.login(this.email, this.password).subscribe(
        (response) => {
          console.log('Login Response:', response);  // Log response to inspect its structure
          alert('Login successful');
          this.router.navigate(['/studentdetails']);
        },
        (error) => {
          this.errorMessage = 'Invalid login credentials';
          console.error('Login error:', error);
        }
      );
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
