import { Component, OnInit } from '@angular/core';
import { StudentDetail } from '../../model/student.model';
import { StudentDetailsService } from '../../services/apiservices.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-studentdetails-page',
  templateUrl: './studentdetails-page.component.html',
  styleUrls: ['./studentdetails-page.component.css']
})
export class StudentdetailsPageComponent implements OnInit {
  studentDetail: StudentDetail[] = [];
  filteredStudentDetails: StudentDetail[] = [];
  searchText = '';
  loading = false;
  error: string | null = null;
  showModal = false;
  studentToEdit: StudentDetail | null = null;
  emailError: string | null = null;  // Variable to store email error message


  constructor(private studentDetailsService: StudentDetailsService, private router: Router) { }

  ngOnInit(): void {
    this.loading = true;
    this.getAllStudentDetails();
  }

  //API
  getAllStudentDetails() {
    console.log('Fetching all student details...');
    this.studentDetailsService.getAllStudents().subscribe({
      next: (Response: any) => {
        if (Response.ResponseCode === 1) { // If response has responseCode
          this.studentDetail = Response.ResponseData;
          this.filteredStudentDetails = [...this.studentDetail];
        }
        this.loading = false;
      },
      error: err => {
        this.error = 'Error loading data';
        console.error('Error fetching student details:', err);
        this.loading = false;
      }
    });
  }


  // Add or update student
  addOrUpdateStudent(): void {
    if (!this.studentToEdit) return;
    // Set lastName to null if empty
    if (!this.studentToEdit.lastName) {
      this.studentToEdit.lastName = '';
    }

    console.log('Saving student:', this.studentToEdit);  // Debug log to verify student details

    this.studentDetailsService.addOrUpdateStudentDetails(this.studentToEdit).subscribe({
      next: (response) => {
        // Check if studentID exists to determine if it's an update or add
        alert(`${this.studentToEdit?.studentID ? 'Student updated' : 'Student added'} successfully`);
        this.closeModal();  // Close the modal after saving the student
        this.getAllStudentDetails();  // Refresh the student details list
      },
      error: (error) => {
        console.error('Error updating/adding student:', error);

        // Check if the error is due to email already being used
        if (error.error?.Message && error.error.Message.includes('A student with this email already exists.')) {
          this.emailError = 'A student with this email already exists.';  // Set the email error message
        }
        else {
          this.emailError = 'Failed to update/add student';  // Generic error message
        }

        alert(this.emailError);  // Show the error message

      }
    });
  }



  // Delete (deactivate) student
  deleteStudent(studentID: number): void {
    if (confirm('Are you sure you want to deactivate this student?')) {
      this.studentDetailsService.deactivateStudent(studentID).subscribe({
        next: (response) => {
          alert('Student deactivated successfully');
          this.getAllStudentDetails(); // Refresh the student list after deactivation
        },
        error: (err) => {
          console.error('Error deactivating student:', err);
          alert('Failed to deactivate student');
        }
      });
    }
  }

  logOut() {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/']);
  }


  // Filter student details
  filterStudents() {
    this.filteredStudentDetails = this.studentDetail.filter(student =>
      student.firstName.toLowerCase().includes(this.searchText.toLowerCase()))
  }

  //Add Student
  navigateToaddStudent() {
    this.router.navigate(['/addstudent']);
  }

  // Open modal to add/edit student
  openModal(student?: StudentDetail) {
    this.studentToEdit = student ? { ...student } : {} as StudentDetail;
    this.showModal = true;
    this.emailError = null;  // Clear email error when opening the modal

  }


  // Close modal
  closeModal() {
    this.showModal = false;
    this.studentToEdit = null;
    this.emailError = null;  // Clear email error when opening the modal

  }

}
