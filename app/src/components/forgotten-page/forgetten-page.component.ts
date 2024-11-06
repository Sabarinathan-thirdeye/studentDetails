import { Component } from '@angular/core';

@Component({
  selector: 'app-forgetten-page',
  templateUrl: './forgetten-page.component.html',
  styleUrls: ['./forgetten-page.component.css']
})
export class ForgettenPageComponent {
  email: string = '';
  isEmailSent: boolean = false;

  onSubmit() {
    if (this.email) {
      // Simulating email sending
      console.log('Sending password reset link to:', this.email);
      this.isEmailSent = true;
    } else {
      alert('Please enter a valid email address.');
    }
  }
}
