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

  constructor(private studentDetailsService: StudentDetailsService, private router: Router) { }

  ngOnInit(): void {
    this.loading = true;
    this.getAllStudentDetails();
  }

//API
  // Get all student details
  getAllStudentDetails() {
    this.studentDetailsService.getAllStudents().subscribe({
      next: (resp: any) => {
        if (resp.responseCode === 1) {
          this.studentDetail = resp.responseData;
          this.filteredStudentDetails = [...this.studentDetail];
        }
        this.loading = false;
      },
      error: err => {
        this.error = 'Error loading data';
        console.error(err);
        this.loading = false;
      }
    });
  }

  // Add or update student
  addOrUpdateStudent(): void {
    if (!this.studentToEdit) return;
    this.studentDetailsService.addOrUpdateStudentDetails(this.studentToEdit).subscribe({
      next: (response) => {
        alert(`${this.studentToEdit?.studentID ? 'Student updated' : 'Student added'} successfully`);
        this.closeModal();
        this.getAllStudentDetails();
      },
      error: (error) => {
        console.error('Error updating/adding student:', error);
        alert('Failed to update/add student');
      }
    });
  }

  // Delete (deactivate) student
  deleteStudent(studentID: number): void {
    if (confirm('Are you sure you want to delete this student?')) {
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

  // Filter student details
  filterStudents() {
    this.filteredStudentDetails = this.studentDetail.filter(student =>
      student.firstName.toLowerCase().includes(this.searchText.toLowerCase()));
  }

  //Add Student
  navigateToaddStudent() {
    this.router.navigate(['/register']);
  }

  // Open modal to add/edit student
  openModal(student?: StudentDetail) {
    this.studentToEdit = student ? { ...student } : {} as StudentDetail;
    this.showModal = true;
  }

  // Close modal
  closeModal() {
    this.showModal = false;
    this.studentToEdit = null;
  }
}
