import { Router } from '@angular/router';
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { StudentDetail } from '../../model/student.model';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {
  constructor(private router: Router) {}

  @Input() student: StudentDetail | null = null;
  @Output() formSubmit = new EventEmitter<StudentDetail>();

  
  studentvalue: StudentDetail = {
    studentID: 0,
    firstName: '',
    lastName: '',
    email: '',
    studentPassword: '',
    confirmPassword: '',
    gender: '',
    mobileNumber: '',
    dateOfBirth: '',
    studentstatus: 0 // Adjust this field as needed based on your StudentDetail model
  };

  ngOnInit() {
    if (this.student) {
      // Prefill form values if editing an existing student
      this.studentvalue = { ...this.student };
    }
  }

  onSubmit() {
    if (this.studentvalue.studentPassword === this.studentvalue.confirmPassword) {
      this.formSubmit.emit(this.studentvalue); // Emit form data to parent component
      console.log('Registration successful:', this.studentvalue);
      alert('Register successful');
      this.router.navigate(['/login']);
    } else {
      alert('Passwords do not match');
    }
  }
}
