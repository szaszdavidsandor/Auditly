export type UserRole = 'accountant' | 'entrepreneur';

export interface LoginRequest {
  email: string;
  password: string;
  role: UserRole;
  rememberMe: boolean;
}

export interface AuthSession {
  email: string;
  role: UserRole;
}

export type AuthErrorCode = 'invalid-credentials' | 'network' | 'server';

export class AuthError extends Error {
  constructor(readonly code: AuthErrorCode) {
    super(code);
    this.name = 'AuthError';
  }
}

export const AUTH_ERROR_MESSAGES: Record<AuthErrorCode, string> = {
  'invalid-credentials': 'Hibás email cím vagy jelszó.',
  network: 'Nem sikerült kapcsolódni a szerverhez. Ellenőrizd az internetkapcsolatot.',
  server: 'Váratlan szerverhiba történt. Próbáld újra később.',
};

export const ROLE_HOME_ROUTES: Record<UserRole, string> = {
  accountant: '/accountant',
  entrepreneur: '/entrepreneur',
};
