import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { StudentDetail } from '../model/student.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StudentDetailsService {
  private apiUrl = 'https://localhost:7075/api/Student';

  constructor(private http: HttpClient) {}

  // Fetch all student details
  getAllStudents(): Observable<StudentDetail[]> {
    const token = localStorage.getItem('JwtToken');
    
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
    // If studentID exists, it's an update (PUT request)
    return this.http.put<StudentDetail>(`${this.apiUrl}/AddorUpdateStudentDetails`, student).pipe(
      catchError(error => {
        console.error('Error updating student:', error);
        return throwError(() => error);
      })
    );
  } else {
    // If no studentID, it's a new student (POST request)
    return this.http.post<StudentDetail>(`${this.apiUrl}/AddorUpdateStudentDetails`, student).pipe(
      catchError(error => {
        console.error('Error adding student:', error);
        return throwError(() => error);
      })
    );
  }
}

// Deactivate student by setting status to 99
deactivateStudent(studentID: number): Observable<void> {
  return this.http.post<void>(`${this.apiUrl}/Deactivate/${studentID}`, {}).pipe(
    catchError(error => {
      console.error('Error deactivating student:', error);
      return throwError(() => new Error('Error deactivating student'));
    })
  );
}

}
