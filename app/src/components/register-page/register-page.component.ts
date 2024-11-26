import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserMasterModel } from '../../model/login.model';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent {
  user: UserMasterModel = {
    firstName: '',
    lastName: '',
    email: '',
    userPassword: '',
    confirmPassword: '',
    userMasterStatus: 0, // Default value set to 1 (Active)
    userTypeID: 1,
    userName: '',
    countryCode: '' // Default empty, will prepend '+' when submitting
  };

  formTitle: string = 'Register User';
  formSubmitted = false;
  errors: string[] = []; // Store error messages

  constructor(private authService: AuthService, private router: Router) { }

  // Handle form submission
  onSubmit(): void {
    this.formSubmitted = true;
    this.errors = []; // Reset errors

    // Validate the form
    if (!this.validateForm()) {
      return; // Stop submission if validation fails
    }

    // Check if passwords match
    if (this.user.userPassword === this.user.confirmPassword) {
      // Ensure country code starts with '+' if not present
      if (this.user.countryCode && !this.user.countryCode.startsWith('+')) {
        this.user.countryCode = '+' + this.user.countryCode;
      }

      // Call the register API
      this.authService.registerUser(this.user).subscribe(
        (response) => {
          console.log('User registered successfully', response);
          this.router.navigate(['/login']);
        },
        (error) => {
          console.error('Registration failed', error);
          // Capture and display error messages from backend
          this.errors.push(error.error.message || 'Registration failed.');
        }
      );
    } else {
      this.errors.push('Passwords do not match.');
    }
  }

  // Form validation logic
  validateForm(): boolean {
    let isValid = true;

    if (!this.user.email) {
      this.errors.push('Email is required.');
      isValid = false;
    } else if (!this.isValidEmail(this.user.email)) {
      this.errors.push('Invalid email format.');
      isValid = false;
    }

    return isValid;
  }

  // Email format validation
  isValidEmail(email: string): boolean {
    const emailPattern = /^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$/;
    return emailPattern.test(email);
  }

  // Navigate to login page
  navigateToLoginform() {
    this.router.navigate(['/login']);
  }
}
