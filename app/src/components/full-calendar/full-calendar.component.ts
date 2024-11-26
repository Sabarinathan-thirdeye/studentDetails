import { Component, OnInit } from '@angular/core';
import { CalendarOptions, EventClickArg } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin, { DateClickArg } from '@fullcalendar/interaction';
import { StudentDetailsService } from '../../services/apiservices.service';
import { StudentDetail } from '../../model/student.model';
import { Router } from '@angular/router';


@Component({
  selector: 'app-full-calendar',
  templateUrl: './full-calendar.component.html',
  styleUrls: ['./full-calendar.component.css']
})
export class FullCalendarComponent implements OnInit {
  studentDetail: StudentDetail[] = [];
  filteredStudentDetails: StudentDetail[] = [];
  error: string | null = null;
  loading = false;
  isModalOpen = false;  // Modal visibility state
  selectedStudent: StudentDetail | null = null;

  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin, interactionPlugin],
    dateClick: (arg) => this.handleDateClick(arg),
    eventClick: (arg) => this.handleEventClick(arg), // Add eventClick handler
    weekends: false,
    events: [],// Add event click handler
  };

  constructor(private studentDetailsService: StudentDetailsService, private router: Router) { }

  ngOnInit(): void {
    this.getAllStudentDetails(); // Fetch all student details
    this.loading = true;
  }

  // API call to fetch student details
  getAllStudentDetails() {
    console.log('Fetching all student details...');
    this.studentDetailsService.getAllStudents().subscribe({
      next: (Response: any) => {
        if (Response.ResponseCode === 1) { // If response has responseCode
          this.studentDetail = Response.ResponseData;
          this.filteredStudentDetails = [...this.studentDetail];

          // Map student data to events
          const studentEvents = Response.ResponseData.map((student: any) => ({
            title: student.firstName + ' ' + student.lastName,
            date: student.createdOn.split('T')[0],  // Store only the date part of createdOn
            student: student,  // Store the student data for later use
          }));

          this.calendarOptions.events = studentEvents; // Update events in the calendar

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

  // // Helper function to create events from student data
  // createStudentEvents() {
  //   const studentEvents = this.studentDetail.map((student: any) => ({
  //     title: `${student.firstName} ${student.lastName}`,
  //     date: this.extractDate(student.createdOn), // Ensure date is in YYYY-MM-DD format
  //     extendedProps: {
  //       student: student // Pass the whole student data to the event
  //     }
  //   }));

  //   // Pass the events to FullCalendar
  //   this.calendarOptions.events = studentEvents;
  // }

  // // Helper function to extract date from datetime string
  // extractDate(datetime: string): string {
  //   const date = new Date(datetime);
  //   return date.toISOString().split('T')[0]; // returns only the date part in YYYY-MM-DD format
  // }

  toggleWeekends() {
    this.calendarOptions.weekends = !this.calendarOptions.weekends; // toggle the boolean!
  }

  handleDateClick(arg: DateClickArg) {
    alert('Date clicked! ' + arg.dateStr);
  }

  // Event click handler to show student details
  handleEventClick(arg: EventClickArg) {
    console.log(arg.event.extendedProps['student']); // Check if student data is being passed correctly
    this.selectedStudent = arg.event.extendedProps['student'];
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false; // Close the modal
  }

  navigateTohome() {
    this.router.navigate(['studentdetails'], {
    });
  }
}

