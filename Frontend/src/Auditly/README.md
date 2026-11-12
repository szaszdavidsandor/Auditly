# Auditly

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 22.2.0.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

## Bejelentkezés (login-page) – fejlesztői megjegyzések

- Oldal: `src/app/features/auth/pages/login-page`, komponensek: `src/app/features/auth/components`
  (`LoginCard`, `RoleSelector`, `InputField`, `PasswordField`, `RememberMe`, `LoginButton`, `LoginFooter`, `AuthBackground`).
- Színek / design tokenek: `src/styles/tokens.css` (CSS-változók).
- Útvonalak: `/login`, `/forgot-password`, `/register` (placeholder), `/accountant`, `/entrepreneur` (szerepkör-guarddal védett placeholder).

### Demó (mock) hitelesítés

`ng serve` (dev mód) alatt a `MockAuthService` fut – **ez nem valódi hitelesítés**, a felületen
„Demó mód” jelzés látható. Production buildben az `ApiAuthService` (`POST /api/auth/login`) fut.
Csere helye: `src/app/app.config.ts`.

Mock szabályok:
- bármely formailag helyes email, legalább 8 karakteres jelszó → demó belépés a választott szerepkör felületére,
- rövidebb jelszó → „Hibás email cím vagy jelszó.”,
- `@offline.test` végű email → hálózati hiba, `@error.test` végű email → szerverhiba.

A jelszó sehol nem tárolódik; az „Emlékezz rám” csak jelzőként megy a backendnek (HttpOnly munkamenet-süti élettartama).
