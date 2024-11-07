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
  getAllStudents(): Observable<StudentDetail[]> {
    return this.http.get<StudentDetail[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Error fetching student details:', error);
        return throwError(() => error);
      })
    );
  }

  // Add or Update Student details
  addOrUpdateStudentDetails(student: StudentDetail): Observable<StudentDetail> {
    if (student.studentID) {
      // If studentID exists, it's an update
      return this.http.post<StudentDetail>(`${this.apiUrl}/AddorStudentStudentDetails`, student).pipe(
        catchError(error => {
          console.error('Error creating/updating student:', error);
          return throwError(() => error);
        })
      );
    } else {
      // If no studentID, it's a new student
      return this.http.post<StudentDetail>(`${this.apiUrl}/AddorStudentStudentDetails`, student).pipe(
        catchError(error => {
          console.error('Error creating student:', error);
          return throwError(() => error);
        })
      );
    }
  }
  
  // Deactivate student by setting status to 99
  deactivateStudent(studentID: number): Observable<StudentDetail> {
    return this.http.post<StudentDetail>(`${this.apiUrl}/Deactivate/${studentID}`, {}).pipe(
      catchError(error => {
        console.error('Error deactivating student:', error);
        return throwError(() => new Error('Error deactivating student'));
      })
    );
  }
}
