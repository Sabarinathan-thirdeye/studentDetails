import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { StudentDetail } from '../model/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentDetailsService {

  private apiUrl = 'https://localhost:44356/api/Student';

  constructor(private http: HttpClient) {}

  // Fetch all student details
  getAllStudents(): Observable<StudentDetail> {
    return this.http.get<any>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching student details:', error);
        return throwError(() => error);
      })
    );
  }

  // Add or Update student details
  // addOrUpdateStudentDetails(student: StudentDetail): Observable<StudentDetail> {
  //   if (student.studentID) {
  //     // If studentID exists, update the student
  //     return this.http.put(`${this.apiUrl}/addOrUpdateStudentDetails`, student).pipe(
  //       catchError(error => {
  //         console.error('Error updating student:', error);
  //         return throwError(() => error);
  //       })
  //     );
  //   } else {
  //     // If no studentID, create a new student
  //     return this.http.post(`${this.apiUrl}/addOrUpdateStudentDetails`, student).pipe(
  //       catchError(error => {
  //         console.error('Error creating student:', error);
  //         return throwError(() => error);
  //       })
  //     );
  //   }
  // }

  // Delete student
  deleteStudent(studentID: number): Observable<StudentDetail> {
    return this.http.delete<any>(`${this.apiUrl}/Deactivate/${studentID}`);
  }
}
