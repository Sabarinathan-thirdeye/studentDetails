import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { StudentDetailsService } from '../../services/apiservices.service';
import { StudentDetail } from '../../model/student.model';


@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.css'],
})
export class CalendarComponent implements OnInit {
  selectedDate: Date | null | undefined;
  studentDetail: StudentDetail[] = [];
  filteredStudentDetails: StudentDetail[] = [];
  loading = false;
  error: string | null = null;
  currentPage: number = 1;
  pageSize: number = 5;  // Display 5 students per page
  totalPages: number = 0;  // Total number of pages
  searchText: string = '';
  selectedStudent: StudentDetail | null = null; // Add this property to store the selected student

  constructor(
    private router: Router,
    private studentDetailsService: StudentDetailsService
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.getAllStudentDetails();
  }

  getAllStudentDetails() {
    console.log('Fetching all student details in calendar...');
    this.studentDetailsService.getAllStudents().subscribe({
      next: (response: any) => {
        if (response.ResponseCode === 1) {
          this.studentDetail = response.ResponseData;
          this.calculateTotalPages();  // Recalculate total pages
        } else {
          this.error = 'Failed to fetch student details.';
        }
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching student details:', err);
        this.error = 'Error fetching student details.';
        this.loading = false;
      }
    });
  }

  onDateSelect(date: Date) {
    this.selectedDate = date;
    const selectedDateString = date.toISOString().split('T')[0];

    this.filteredStudentDetails = this.studentDetail.filter((student) => {
      if (student.createdOn) {
        const createdOnDateString = new Date(student.createdOn).toISOString().split('T')[0];
        return createdOnDateString === selectedDateString;
      }
      return false;
    });

    if (this.filteredStudentDetails.length === 0) {
      this.error = `No students registered on ${selectedDateString}`;
    } else {
      this.error = null;
    }

    this.calculateTotalPages();  // Recalculate total pages after filtering
  }

  // Add this method to handle displaying student details
  showStudentDetails(student: StudentDetail) {
    console.log(student)
    this.selectedStudent = student; // Store the clicked student in selectedStudent
  }
  // Method to close the modal
  closeModal() {
    this.selectedStudent = null; // Reset selectedStudent to close the modal
  }


  filterStudents() {
    if (this.searchText && this.searchText.trim() !== '') {
      this.filteredStudentDetails = this.studentDetail.filter(student =>
        student.firstName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        student.lastName.toLowerCase().includes(this.searchText.toLowerCase())
      );
    } else {
      this.filteredStudentDetails = [...this.studentDetail];
    }
    this.calculateTotalPages();  // Recalculate total pages based on the filter
  }

  calculateTotalPages() {
    this.totalPages = Math.ceil(this.filteredStudentDetails.length / this.pageSize);
  }

  getPaginatedData(): StudentDetail[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.filteredStudentDetails.slice(startIndex, endIndex);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
  navigateTodashboard(){
    this.router.navigate(['/studentdetails']);
  }
  logOut() {
    console.log('Logging out...');
    this.router.navigate(['/login']);
  }
}
