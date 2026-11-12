import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { Icon } from '../../../../shared/icon/icon';

@Component({
  selector: 'app-login-button',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <button type="submit" class="btn" [disabled]="loading()" [attr.aria-busy]="loading()">
      @if (loading()) {
        <app-icon class="btn__spinner" name="loader" [size]="15" [strokeWidth]="2.25" />
        <span>{{ loadingLabel() }}</span>
      } @else {
        <span>{{ label() }}</span>
      }
    </button>
  `,
  styles: `
    :host {
      display: block;
    }

    .btn {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 8px;
      width: 100%;
      height: 38px;
      padding: 0 16px;
      border: 0;
      border-radius: var(--radius-control);
      background: var(--gradient-primary);
      color: #fff;
      font: inherit;
      font-size: 12.5px;
      font-weight: 600;
      cursor: pointer;
      box-shadow: 0 6px 18px -8px rgba(52, 124, 242, 0.7);
      transition:
        filter var(--transition-fast),
        box-shadow var(--transition-fast),
        transform var(--transition-fast);
    }

    .btn:hover:not(:disabled) {
      background: var(--gradient-primary-hover);
      box-shadow: 0 8px 22px -8px rgba(52, 124, 242, 0.85);
    }

    .btn:active:not(:disabled) {
      transform: translateY(1px);
    }

    .btn:disabled {
      cursor: progress;
      filter: saturate(0.75) brightness(0.92);
    }

    .btn__spinner {
      animation: spin 0.8s linear infinite;
    }

    @keyframes spin {
      to {
        transform: rotate(360deg);
      }
    }
  `,
})
export class LoginButton {
  readonly loading = input<boolean>(false);
  readonly label = input<string>('Bejelentkezés');
  readonly loadingLabel = input<string>('Bejelentkezés…');
}
