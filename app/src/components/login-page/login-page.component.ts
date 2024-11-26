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
  errorMessage: string = '';

  constructor(private router: Router, private authService: AuthService) {}

  
  onSubmit() {
  
    // Call login service when the form is submitted
    this.authService.login(this.email, this.password).subscribe(
      (response: any) => {
        // Store JWT token in localStorage
        localStorage.setItem('jwtToken', response?.ResponseData[0]?.JwtToken);
        alert('Login successful');
        // Navigate to student details page
        this.router.navigate(['/studentdetails']);
      },
      (error) => {
        // Handle specific errors based on backend status codes
        if (error?.status === 404) {
          // Email not found
          alert('Email not found. Please check or register a new account.');
        } else if (error?.status === 412) {
          // Incorrect password
          alert('Incorrect password. Please try again.');
        } else {
          // Generic error handling
          const errorMessage = error?.error?.message || 'Invalid email or password';
          alert(errorMessage);
          console.error('Login error:', error);
        }
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
