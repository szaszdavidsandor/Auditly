import { ChangeDetectionStrategy, Component, inject, input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Icon } from '../../../../shared/icon/icon';
import { AuthBackground } from '../../components/auth-background/auth-background';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-placeholder-page',
  imports: [AuthBackground, RouterLink, Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './placeholder-page.html',
  styleUrl: './placeholder-page.css',
})
export class PlaceholderPage {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly heading = input<string>('Hamarosan');
  readonly description = input<string>('');
  readonly showLogout = input<boolean>(false);

  protected readonly session = this.auth.session;

  protected async logout(): Promise<void> {
    await this.auth.logout();
    await this.router.navigateByUrl('/login');
  }
}
