import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AuthBackground } from '../../components/auth-background/auth-background';
import { LoginCard } from '../../components/login-card/login-card';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-page',
  imports: [AuthBackground, LoginCard],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {
  protected readonly isDemo = inject(AuthService).isDemo;
}
