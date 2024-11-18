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
    mobileNumber: '',
    dateOfBirth: '',
    userPassword: '',
    confirmPassword: '',
    userMasterStatus: 1, // Default value set to 1 (Active)
    gender: '',
    userTypeID: 1,
    userName: ''
  };
  formTitle: string = 'Register User';
  formSubmitted = false;
  errors: string[] = []; // Store error messages

  constructor(private authService: AuthService, private router: Router) {}

  // Handle form submission
  onSubmit(): void {
    this.formSubmitted = true;
    this.errors = []; // Reset errors

    // Validate the form
    if (!this.validateForm()) {
      return; // Stop submission if validation fails
    }

    // If passwords match, proceed to register the user
    if (this.user.userPassword === this.user.confirmPassword) {
      
      this.authService.registerUser(this.user).subscribe(
        (response) => {
          console.log('User registered successfully', response);
          this.router.navigate(['/login']);
        },
        (error) => {
          console.error('Registration failed', error);
        }
      );
    } else {
      this.errors.push('Passwords do not match.');
    }
  }

  // Form validation logic
  validateForm(): boolean {
    let isValid = true;

    // Required fields validation
    if (!this.user.firstName) {
      this.errors.push('First name is required.');
      isValid = false;
    }

    if (!this.user.lastName) {
      this.errors.push('Last name is required.');
      isValid = false;
    }

    if (!this.user.email) {
      this.errors.push('Email is required.');
      isValid = false;
    } else if (!this.isValidEmail(this.user.email)) {
      this.errors.push('Invalid email format.');
      isValid = false;
    }

    if (!this.user.userPassword) {
      this.errors.push('Password is required.');
      isValid = false;
    }

    if (!this.user.confirmPassword) {
      this.errors.push('Confirm password is required.');
      isValid = false;
    }

    if (this.user.userPassword && this.user.confirmPassword && this.user.userPassword !== this.user.confirmPassword) {
      this.errors.push('Passwords do not match.');
      isValid = false;
    }

    return isValid;
  }

  // Email format validation
  isValidEmail(email: string): boolean {
    const emailPattern = /^[\w-]+(\.[\w-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,})$/;
    return emailPattern.test(email);
  }
}
