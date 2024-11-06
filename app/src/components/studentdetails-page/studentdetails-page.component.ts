import { Component, OnInit } from '@angular/core';
import { StudentDetail } from '../../model/student.model';
import { StudentDetailsService } from '../../services/apiservices.service';

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

  constructor(private studentDetailsService: StudentDetailsService) {}

  ngOnInit(): void {
    this.loading = true;
    this.getAllStudentDetails();
  }
  //getAll
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
  //Pop-up open
  openModal(student?: StudentDetail) {
    this.studentToEdit = student ? { ...student } : {} as StudentDetail;
    this.showModal = true;
  }

  // addOrUpdateStudent
  // addOrUpdateStudent(updatedStudent: StudentDetail) {
  //   this.studentDetailsService.addOrUpdateStudentDetails(updatedStudent).subscribe({
  //     next: () => {
  //       this.getAllStudentDetails();
  //       this.closeModal();
  //     },
  //     error: err => {
  //       console.error('Error saving student:', err);
  //       this.error = 'Failed to save student details';
  //     }
  //   });
  // }

  // deleteStudent
  deleteStudent(studentID: number) {
    if (confirm('Are you sure you want to delete this student?')) {
      this.studentDetailsService.deleteStudent(studentID).subscribe({
        next: (resp: any) => {
          if (resp.responseCode === 1) {
            this.getAllStudentDetails(); // Refresh the student list
          } else {
            this.error = resp.message; // Show the error message if the deletion fails
          }
        },
        error: err => {
          console.error('Error deleting student:', err);
          this.error = 'Error deleting student';
        }
      });
    }
  }
  
  // pop-up close 
  closeModal() {
    this.showModal = false;
    this.studentToEdit = null;
  }

  filterStudents() {
    this.filteredStudentDetails = this.studentDetail.filter(student => 
      student.firstName.toLowerCase().includes(this.searchText.toLowerCase()) );
  }
  
}
