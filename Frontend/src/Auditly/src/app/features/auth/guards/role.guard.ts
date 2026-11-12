import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserRole } from '../models/auth.models';
import { AuthService } from '../services/auth.service';

export function roleGuard(role: UserRole): CanActivateFn {
  return () => {
    const session = inject(AuthService).session();
    return session?.role === role ? true : inject(Router).createUrlTree(['/login']);
  };
}
