// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Login, UserMasterModel } from '../model/login.model'; // Assuming you have a Login model

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrl = 'https://localhost:7075/api/LogIn/Authenticate'; // Login endpoint
  private registerUrl = 'https://localhost:7075/api/LogIn/Register'; // Register endpoint (adjust if needed)

  constructor(private http: HttpClient) {}

  // Method to check credentials and get JWT token
  login(email: string, userPassword: string): Observable<Login> {
    return this.http.post<Login>(this.loginUrl, { userName: email, userPassword }).pipe(
      catchError(error => {
        console.error('Error during login:', error);
        if (error.error && error.error.errors) {
          console.error('Validation Errors:', error.error.errors); // Log the validation errors
        }
        return throwError(() => new Error('Login failed'));
      })
    );
  }

  // Method to register user
  registerUser(user: UserMasterModel): Observable<any> {
    return this.http.post(this.registerUrl, user).pipe(
      catchError(error => {
        console.error('Error during registration:', error);
        return throwError(() => new Error('Registration failed'));
      })
    );
  }

  // Method to retrieve JWT from localStorage and set as Authorization header
  getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('jwtToken');
    return token ? new HttpHeaders({ Authorization: `Bearer ${token}` }) : new HttpHeaders();
  }
}
