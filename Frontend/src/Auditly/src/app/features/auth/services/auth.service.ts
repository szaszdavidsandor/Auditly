import { Injectable, InjectionToken, computed, signal } from '@angular/core';
import { AuthSession, LoginRequest } from '../models/auth.models';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL', {
  factory: () => '/api',
});

@Injectable()
export abstract class AuthService {
  protected readonly sessionState = signal<AuthSession | null>(null);

  readonly session = this.sessionState.asReadonly();
  readonly isAuthenticated = computed(() => this.sessionState() !== null);

  abstract readonly isDemo: boolean;

  abstract login(request: LoginRequest): Promise<AuthSession>;

  abstract logout(): Promise<void>;
}
