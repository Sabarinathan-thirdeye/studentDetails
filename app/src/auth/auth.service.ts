// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Login, UserMasterModel } from '../model/login.model'; 
import { environment } from '../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginUrlPage = environment.loginUrlPage;

  constructor(private http: HttpClient) {}

  // Method to check credentials and get JWT token
  login(email: string, userPassword: string): Observable<Login> {
    return this.http.post<Login>(`${this.loginUrlPage}/Authenticate`, { userName: email, userPassword }).pipe(
      catchError(error => {
        console.error('Error during login:', error);
        if (error.error && error.error.errors) {
          console.error('Validation Errors:', error.error.errors); // Log the validation errors
        }
        return throwError(() => new Error('Login failed'));
      })
    );
  }

  // auth.service.ts
  registerUser(user: UserMasterModel): Observable<any> {
    return this.http.post<any>(`${this.loginUrlPage}/RegisterUserDetail`, user).pipe(
      catchError(error => {
        console.error('Error during registration:', error);
        return throwError(() => new Error('Registration failed'));
      }),
      map(response => {
        return response;
      })
    );
  }
  

}
