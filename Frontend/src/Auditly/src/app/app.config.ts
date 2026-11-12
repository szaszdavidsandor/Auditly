import { provideHttpClient, withFetch } from '@angular/common/http';
import { ApplicationConfig, isDevMode, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { ApiAuthService } from './features/auth/services/api-auth.service';
import { AuthService } from './features/auth/services/auth.service';
import { MockAuthService } from './features/auth/services/mock-auth.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withFetch()),
    { provide: AuthService, useClass: isDevMode() ? MockAuthService : ApiAuthService },
  ],
};
