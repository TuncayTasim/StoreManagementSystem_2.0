import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  UserLoginDTO,
  UserRegisterDTO,
  UserResponseDTO,
  ForgotPasswordDTO,
  ResetPasswordDTO
} from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;

  // BehaviorSubject holds the current user state and emits to subscribers.
  // We initialise it from localStorage so the user stays logged in on page refresh.
  private currentUserSubject = new BehaviorSubject<UserResponseDTO | null>(
    this.getStoredUser()
  );

  /** Observable stream of the currently logged-in user (or null). */
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  /** Register a new user. The API sends a confirmation email. */
  register(dto: UserRegisterDTO): Observable<UserResponseDTO> {
    return this.http.post<UserResponseDTO>(`${this.apiUrl}/register`, dto);
  }

  /** Log in and store the JWT + user info in localStorage. */
  login(dto: UserLoginDTO): Observable<UserResponseDTO> {
    return this.http.post<UserResponseDTO>(`${this.apiUrl}/login`, dto).pipe(
      tap(user => {
        // Save to localStorage so the session survives page refreshes
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
      })
    );
  }

  /** Confirm a user's email with the token they received. */
  confirmEmail(token: string): Observable<string> {
    return this.http.post(`${this.apiUrl}/confirm?token=${token}`, {}, { responseType: 'text' });
  }

  /** Request a password reset email. */
  forgotPassword(dto: ForgotPasswordDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/forgot-password`, dto, { responseType: 'text' });
  }

  /** Reset password using the token from the email. */
  resetPassword(dto: ResetPasswordDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/reset-password`, dto, { responseType: 'text' });
  }

  /** Change password for logged-in user. */
  changePassword(dto: any): Observable<string> {
    return this.http.post(`${this.apiUrl}/change-password`, dto, { responseType: 'text' });
  }

  /** Log out — clear stored user data. */
  logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  /** Get the JWT token (used by the AuthInterceptor). */
  getToken(): string | null {
    return this.currentUserSubject.value?.token ?? null;
  }

  /** Check if a user is currently logged in. */
  isLoggedIn(): boolean {
    return this.currentUserSubject.value !== null;
  }

  /** Get the current user's role ID. */
  getRoleId(): number | null {
    return this.currentUserSubject.value?.roleId ?? null;
  }

  /** Read user data from localStorage (called once on service init). */
  private getStoredUser(): UserResponseDTO | null {
    const stored = localStorage.getItem('currentUser');
    return stored ? JSON.parse(stored) : null;
  }
}
