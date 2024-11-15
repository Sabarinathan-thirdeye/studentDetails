import { Router } from '@angular/router';
import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StudentDetail } from '../../model/student.model';
import { StudentDetailsService } from '../../services/apiservices.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {
  @Input() student: StudentDetail | null = null;
  @Output() formSubmit = new EventEmitter<StudentDetail>();

  registerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private studentDetailsService: StudentDetailsService
  ) {}

  ngOnInit() {
    this.registerForm = this.fb.group({
      studentID: [0],  // Default ID, adjust as necessary
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      gender: ['', Validators.required],
      mobileNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      dateOfBirth: ['', Validators.required],
      studentPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      studentstatus: [0] // Adjust based on your requirements
    }, {
      validator: this.passwordMatchValidator
    });

    if (this.student) {
      this.registerForm.patchValue(this.student);
    }
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('studentPassword')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const studentData: StudentDetail = this.registerForm.value;
      this.studentDetailsService.addOrUpdateStudentDetails(studentData).subscribe({
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
      alert('Please fill out the form correctly.');
    }
  }
}
