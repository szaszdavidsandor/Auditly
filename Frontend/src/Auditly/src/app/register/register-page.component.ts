import { DOCUMENT } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthBackground } from '../features/auth/components/auth-background/auth-background';
import { InputField } from '../features/auth/components/input-field/input-field';
import { LoginButton } from '../features/auth/components/login-button/login-button';
import { PasswordField } from '../features/auth/components/password-field/password-field';
import { RememberMe } from '../features/auth/components/remember-me/remember-me';
import { RoleSelector } from '../features/auth/components/role-selector/role-selector';
import { RegisterRequest, UserRole } from '../features/auth/models/auth.models';
import { validateEmail, validatePassword } from '../features/auth/validators/login.validators';
import { Icon } from '../shared/icon/icon';

type Field = 'name' | 'email' | 'password' | 'confirm' | 'terms';

const FIELD_ORDER: readonly Field[] = ['name', 'email', 'password', 'confirm', 'terms'];

@Component({
  selector: 'app-register-page',
  imports: [
    RouterLink,
    Icon,
    AuthBackground,
    RoleSelector,
    InputField,
    PasswordField,
    RememberMe,
    LoginButton,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './register-page.component.html',
  styleUrls: [
    '../features/auth/components/login-card/login-card.css',
    './register-page.component.css',
  ],
})
export class RegisterPageComponent {
  private readonly document = inject(DOCUMENT);

  protected readonly role = signal<UserRole>('accountant');
  protected readonly name = signal('');
  protected readonly email = signal('');
  protected readonly password = signal('');
  protected readonly passwordConfirm = signal('');
  protected readonly termsAccepted = signal(false);
  protected readonly notice = signal<string | null>(null);

  private readonly submitted = signal(false);
  private readonly touched = signal(new Set<Field>());

  private readonly errors = computed<Record<Field, string | null>>(() => ({
    name: this.name().trim() ? null : 'Add meg a neved vagy a cégneved.',
    email: validateEmail(this.email()),
    password: validatePassword(this.password()),
    confirm: !this.passwordConfirm()
      ? 'Erősítsd meg a jelszavad.'
      : this.passwordConfirm() !== this.password()
        ? 'A két jelszó nem egyezik.'
        : null,
    terms: this.termsAccepted() ? null : 'A regisztrációhoz fogadd el az ÁSZF-et.',
  }));

  readonly request = computed<RegisterRequest>(() => ({
    role: this.role(),
    name: this.name().trim(),
    email: this.email().trim(),
    password: this.password(),
    termsAccepted: this.termsAccepted(),
  }));

  protected error(field: Field): string | null {
    return this.submitted() || this.touched().has(field) ? this.errors()[field] : null;
  }

  protected touch(field: Field): void {
    this.touched.update((fields) => new Set(fields).add(field));
  }

  protected onSubmit(event: Event): void {
    event.preventDefault();
    this.submitted.set(true);
    this.notice.set(null);

    const invalid = FIELD_ORDER.find((field) => this.errors()[field]);
    if (invalid) {
      this.document.getElementById(`register-${invalid}`)?.focus();
      return;
    }

    this.notice.set('Az online regisztráció még nem érhető el. A fiók a backend bekötése után hozható létre.');
  }
}
