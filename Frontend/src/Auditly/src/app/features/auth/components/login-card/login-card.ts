import { ChangeDetectionStrategy, Component, computed, inject, signal, viewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Icon } from '../../../../shared/icon/icon';
import {
  AUTH_ERROR_MESSAGES,
  AuthError,
  ROLE_HOME_ROUTES,
  UserRole,
} from '../../models/auth.models';
import { AuthService } from '../../services/auth.service';
import { validateEmail, validatePassword } from '../../validators/login.validators';
import { InputField } from '../input-field/input-field';
import { LoginButton } from '../login-button/login-button';
import { LoginFooter } from '../login-footer/login-footer';
import { PasswordField } from '../password-field/password-field';
import { RememberMe } from '../remember-me/remember-me';
import { RoleSelector } from '../role-selector/role-selector';

@Component({
  selector: 'app-login-card',
  imports: [
    RouterLink,
    Icon,
    RoleSelector,
    InputField,
    PasswordField,
    RememberMe,
    LoginButton,
    LoginFooter,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login-card.html',
  styleUrl: './login-card.css',
})
export class LoginCard {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  private readonly emailField = viewChild.required(InputField);
  private readonly passwordField = viewChild.required(PasswordField);

  protected readonly emailPlaceholder = 'kovacs.peter@auditly.hu';

  protected readonly role = signal<UserRole>('accountant');
  protected readonly email = signal('');
  protected readonly password = signal('');
  protected readonly rememberMe = signal(true);

  protected readonly loading = signal(false);
  protected readonly formError = signal<string | null>(null);

  private readonly submitAttempted = signal(false);
  protected readonly emailTouched = signal(false);
  protected readonly passwordTouched = signal(false);

  protected readonly emailError = computed(() =>
    this.emailTouched() || this.submitAttempted() ? validateEmail(this.email()) : null,
  );
  protected readonly passwordError = computed(() =>
    this.passwordTouched() || this.submitAttempted() ? validatePassword(this.password()) : null,
  );

  protected async onSubmit(event: Event): Promise<void> {
    event.preventDefault();
    if (this.loading()) return;

    this.submitAttempted.set(true);
    this.formError.set(null);

    if (validateEmail(this.email())) {
      this.emailField().focus();
      return;
    }
    if (validatePassword(this.password())) {
      this.passwordField().focus();
      return;
    }

    this.loading.set(true);
    try {
      const session = await this.auth.login({
        email: this.email().trim(),
        password: this.password(),
        role: this.role(),
        rememberMe: this.rememberMe(),
      });
      this.password.set('');
      await this.router.navigateByUrl(ROLE_HOME_ROUTES[session.role]);
    } catch (error) {
      const code = error instanceof AuthError ? error.code : 'server';
      this.formError.set(AUTH_ERROR_MESSAGES[code]);
    } finally {
      this.loading.set(false);
    }
  }
}
