import { Router } from '@angular/router';
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { StudentDetail } from '../../model/student.model';
import { StudentDetailsService } from '../../services/apiservices.service';
@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {
  constructor(private router: Router,private studentDetailsService: StudentDetailsService) {}


  @Input() student: StudentDetail | null = null;
  @Output() formSubmit = new EventEmitter<StudentDetail>();

  
  studentvalue: StudentDetail = {
    studentID: 0,
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    gender: '',
    email: '',
    mobileNumber: '',
    studentPassword: '',
    confirmPassword: '',
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
      // Submit the form data to the backend using the service
      this.studentDetailsService.addOrUpdateStudentDetails(this.studentvalue).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          alert('Register successful');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Error registering student:', error);
          alert('An error occurred during registration.');
        }
      });
    } else {
      alert('Passwords do not match');
    }
  }
}
