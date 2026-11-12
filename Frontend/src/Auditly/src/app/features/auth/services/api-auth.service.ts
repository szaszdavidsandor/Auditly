import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AuthError, AuthSession, LoginRequest } from '../models/auth.models';
import { API_BASE_URL, AuthService } from './auth.service';

@Injectable()
export class ApiAuthService extends AuthService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = inject(API_BASE_URL);

  readonly isDemo = false;

  async login(request: LoginRequest): Promise<AuthSession> {
    try {
      const session = await firstValueFrom(
        this.http.post<AuthSession>(`${this.baseUrl}/auth/login`, request, {
          withCredentials: true,
        }),
      );
      this.sessionState.set({ email: session.email, role: session.role });
      return session;
    } catch (error) {
      throw this.toAuthError(error);
    }
  }

  async logout(): Promise<void> {
    try {
      await firstValueFrom(
        this.http.post<void>(`${this.baseUrl}/auth/logout`, null, { withCredentials: true }),
      );
    } finally {
      this.sessionState.set(null);
    }
  }

  private toAuthError(error: unknown): AuthError {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) return new AuthError('network');
      if (error.status === 400 || error.status === 401 || error.status === 403) {
        return new AuthError('invalid-credentials');
      }
    }
    return new AuthError('server');
  }
}
