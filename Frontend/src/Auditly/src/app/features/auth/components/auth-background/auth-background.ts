import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-auth-background',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="decor" aria-hidden="true">
      <span class="decor__shape decor__circle decor__circle--tl"></span>
      <span class="decor__shape decor__circle decor__circle--br"></span>
      <span class="decor__shape decor__rect decor__rect--tr"></span>
      <span class="decor__shape decor__rect decor__rect--bl"></span>
    </div>
    <main class="content">
      <ng-content />
    </main>
  `,
  styleUrl: './auth-background.css',
})
export class AuthBackground {}
