import { Routes } from '@angular/router';
import { roleGuard } from './features/auth/guards/role.guard';

const loadPlaceholder = () =>
  import('./features/auth/pages/placeholder-page/placeholder-page').then((m) => m.PlaceholderPage);

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    title: 'Bejelentkezés | Auditly',
    loadComponent: () =>
      import('./features/auth/pages/login-page/login-page').then((m) => m.LoginPage),
  },

  {
    path: 'forgot-password',
    title: 'Elfelejtett jelszó | Auditly',
    loadComponent: loadPlaceholder,
    data: {
      heading: 'Elfelejtett jelszó',
      description:
        'A jelszó-visszaállítás hamarosan elérhető lesz. Addig kérjük, fordulj a könyvelődhöz vagy a rendszergazdához.',
    },
  },
  {
    path: 'register',
    title: 'Regisztráció | Auditly',
    loadComponent: loadPlaceholder,
    data: {
      heading: 'Regisztráció',
      description:
        'Az önálló regisztráció hamarosan elérhető lesz. Jelenleg a fiókokat a rendszergazda hozza létre.',
    },
  },
  {
    path: 'accountant',
    title: 'Könyvelői felület | Auditly',
    canActivate: [roleGuard('accountant')],
    loadComponent: loadPlaceholder,
    data: {
      heading: 'Könyvelői felület',
      description: 'Itt jelenik meg majd a könyvelői irányítópult.',
      showLogout: true,
    },
  },
  {
    path: 'entrepreneur',
    title: 'Vállalkozói felület | Auditly',
    canActivate: [roleGuard('entrepreneur')],
    loadComponent: loadPlaceholder,
    data: {
      heading: 'Vállalkozói felület',
      description: 'Itt jelenik meg majd a vállalkozói irányítópult.',
      showLogout: true,
    },
  },

  { path: '**', redirectTo: 'login' },
];
