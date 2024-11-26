
import { CanActivateFn } from '@angular/router';
import { CanLoad, Route, UrlSegment, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const checkStatus = !!localStorage.getItem('jwtToken');

  if (checkStatus) {
    return true;
  } else {
    router.navigate(['/']);
    return false;
  }
};



export class AuthLoadGuard implements CanLoad {
  private router = inject(Router);

  canLoad(route: Route, segments: UrlSegment[]): boolean {
    const isAuthenticated = !!localStorage.getItem('jwtToken');
    
    if (isAuthenticated) {
      return true;
    } else {
      // Redirect to login page if the user is not authenticated
      this.router.navigate(['/login']);
      return false;
    }
  }
}

