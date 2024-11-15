import { Router } from '@angular/router';
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../auth/auth.service';  // Ensure AuthService has registerUser method
import { UserMasterModel } from '../../model/login.model';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {
  @Input() user: UserMasterModel | null = null;  // Changed to UserMasterModel
  @Output() formSubmit = new EventEmitter<UserMasterModel>();

  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService  // Use AuthService for registration
  ) {}

  ngOnInit() {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],  // Ensure this field is needed
      email: ['', [Validators.required, Validators.email]],
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      studentPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      userTypeID: [1, Validators.required],  // Example userTypeID
      userMasterStatus: [1],  // Example userMasterStatus
      mobileNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],  // Added mobileNumber field
    }, {
      validator: this.passwordMatchValidator
    });

    if (this.user) {
      this.registerForm.patchValue(this.user);
    }
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('studentPassword')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    console.log('Submit button clicked');  // Add this to check if the method is triggered
    if (this.registerForm.valid) {
      const userData: UserMasterModel = this.registerForm.value;
      this.authService.registerUser(userData).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          alert('Registration successful!');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Error registering user:', error);
          alert('An error occurred during registration.');
        }
      });
    } else {
      alert('Please fill out the form correctly.');
    }
  }

}
