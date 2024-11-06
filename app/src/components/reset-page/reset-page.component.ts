import { Component } from '@angular/core';

@Component({
  selector: 'app-reset-page',
  templateUrl: './reset-page.component.html',
  styleUrls: ['./reset-page.component.css']
})
export class ResetPageComponent {
  newPassword: string = '';
  confirmPassword: string = '';
  passwordResetSuccess: boolean = false;
  passwordMismatch: boolean = false;

  onSubmit() {
    // Check if passwords match
    if (this.newPassword === this.confirmPassword) {
      // Simulate password reset
      console.log('Resetting password to:', this.newPassword);
      this.passwordResetSuccess = true;
      this.passwordMismatch = false;
    } else {
      this.passwordMismatch = true;
      this.passwordResetSuccess = false;
    }
  }
}
