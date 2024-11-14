// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Login } from '../model/login.model'; // Assuming you have a Login model

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7075/api/LogIn/Authenticate'; // Your backend login endpoint

  constructor(private http: HttpClient) {}

  // Method to check credentials
  login(email: string, studentPassword: string): Observable<Login> {
    return this.http.post<Login>(this.apiUrl, { userName:email, studentPassword }).pipe(
      catchError(error => {
        console.error('Error during login:', error);
        if (error.error && error.error.errors) {
          console.error('Validation Errors:', error.error.errors); // Log the validation errors
        }
        return throwError(() => new Error('Login failed'));
      })
    );
  }
  
}
