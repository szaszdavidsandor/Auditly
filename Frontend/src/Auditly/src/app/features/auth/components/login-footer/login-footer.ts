import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login-footer',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <p class="footer">
      Még nincs fiókod?
      <a class="footer__link" routerLink="/register">Regisztráció</a>
    </p>
  `,
  styles: `
    .footer {
      margin: 0;
      text-align: center;
      font-size: 10.5px;
      color: var(--color-text-faint);
    }

    .footer__link {
      color: var(--color-primary-text);
      font-weight: 500;
      text-decoration: none;
      border-radius: 3px;
      transition: color var(--transition-fast);
    }

    .footer__link:hover {
      color: var(--color-text);
      text-decoration: underline;
      text-underline-offset: 2px;
    }
  `,
})
export class LoginFooter {}
