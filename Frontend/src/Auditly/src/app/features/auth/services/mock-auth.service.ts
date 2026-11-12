import { Injectable } from '@angular/core';
import { AuthError, AuthSession, LoginRequest } from '../models/auth.models';
import { AuthService } from './auth.service';

const MOCK_LATENCY_MS = 900;
const MOCK_MIN_PASSWORD_LENGTH = 8;

@Injectable()
export class MockAuthService extends AuthService {
  readonly isDemo = true;

  async login(request: LoginRequest): Promise<AuthSession> {
    await new Promise((resolve) => setTimeout(resolve, MOCK_LATENCY_MS));

    const email = request.email.trim().toLowerCase();
    if (email.endsWith('@offline.test')) throw new AuthError('network');
    if (email.endsWith('@error.test')) throw new AuthError('server');
    if (request.password.length < MOCK_MIN_PASSWORD_LENGTH) {
      throw new AuthError('invalid-credentials');
    }

    const session: AuthSession = { email, role: request.role };
    this.sessionState.set(session);
    return session;
  }

  async logout(): Promise<void> {
    this.sessionState.set(null);
  }
}
