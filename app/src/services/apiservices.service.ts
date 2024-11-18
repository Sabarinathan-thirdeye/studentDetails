import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { StudentDetail } from '../model/student.model';

@Injectable({
  providedIn: 'root'
})
export class StudentDetailsService {
  private apiUrl = 'https://localhost:7075/api/Student'; // Your base API URL
  private login = 'https://localhost:7075/api/LogIn/';

  constructor(private http: HttpClient) { }

  // Fetch all student details
  getAllStudents(): Observable<StudentDetail[]> {
    const token = localStorage.getItem('jwtToken');
    
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
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

  // Add or Update Student details (same API for both)
  addOrUpdateStudentDetails(student: StudentDetail): Observable<StudentDetail> {
    const token = localStorage.getItem('jwtToken');
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    
    // Add Authorization header if token exists
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }

    // Use POST for both adding and updating
    return this.http.post<StudentDetail>(`${this.apiUrl}/AddOrUpdateStudentDetails`, student, { headers }).pipe(
      catchError(error => {
        console.error('Error adding/updating student:', error);
        return throwError(() => error);
      })
    );
  }

  // Deactivate student by setting status to 99
deactivateStudent(studentID: number): Observable<any> {
  const token = localStorage.getItem('jwtToken');
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
