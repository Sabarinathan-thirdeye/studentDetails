import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { StudentDetail } from '../model/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentDetailsService {
  private apiUrl = 'https://localhost:7075/api/Student';
  private login = 'https://localhost:7075/api/LogIn/';

  constructor(private http: HttpClient) { }

  
  // Fetch all student details
  getAllStudents(): Observable<StudentDetail[]> {
    
    const token = localStorage.getItem('jwtToken');
    // Initialize headers with content-type
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    // Add Authorization header if token exists
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }
    
    return this.http.get<StudentDetail[]>(this.apiUrl, { headers }).pipe(
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
      return this.http.post<StudentDetail>(``, student).pipe(
        catchError(error => {
          console.error('Error creating/updating student:', error);
          return throwError(() => error);
        })
      );
    } else {
      // If no studentID, it's a new student
      return this.http.post<StudentDetail>(`${this.login}/AddorStudentStudentDetails`, student).pipe(
        catchError(error => {
          console.error('Error creating student:', error);
          return throwError(() => error);
        })
      );
    }
  }

  // Deactivate student by setting status to 99
  deactivateStudent(studentID: number): Observable<any> {
    const token = localStorage.getItem('jwtToken'); // Retrieve the token
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }
  
    return this.http.post<any>(`${this.apiUrl}/Deactivate/${studentID}`, {}, { headers }).pipe(
      catchError(error => {
        console.error('Error deactivating student:', error);
        return throwError(() => error);
      })
    );
  }
  
}
