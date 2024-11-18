import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { StudentDetailsService } from '../../services/apiservices.service';

@Component({
  selector: 'app-addstudent',
  templateUrl: './add-student.component.html',
  styleUrls: ['./add-student.component.css']
})
export class AddStudentComponent implements OnInit {
  addStudentForm!: FormGroup; // Use non-null assertion

  constructor(
    private fb: FormBuilder,
    private studentDetailsService: StudentDetailsService,
    private router: Router
  ) {}

  // Implement ngOnInit to avoid the error
  ngOnInit(): void {
    this.addStudentForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      gender: ['', Validators.required],
      mobileNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],  // 10-digit number pattern
      dateOfBirth: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.addStudentForm.invalid) {
      return;  // If form is invalid, do not proceed
    }

    const formData = this.addStudentForm.value;

    // Call the service to add the student
    this.studentDetailsService.addOrUpdateStudentDetails(formData).pipe(
      catchError(error => {
        console.error('Error while adding student:', error);
        return throwError(() => new Error('Error while adding student'));
      })
    ).subscribe({
      next: (response) => {
        alert('Student added successfully!');
        this.router.navigate(['studentdetails']);  // Navigate to student list or another page after success
      },
      error: (err) => {
        alert('Failed to add student');
        console.error('Error:', err);
      }
    });
  }
}
