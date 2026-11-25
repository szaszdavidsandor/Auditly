import { ChangeDetectionStrategy, Component, input, model } from '@angular/core';
import { Icon } from '../../../../shared/icon/icon';

@Component({
  selector: 'app-remember-me',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <label class="remember">
      <input
        type="checkbox"
        class="remember__input sr-only"
        [id]="fieldId()"
        [checked]="checked()"
        [disabled]="disabled()"
        (change)="checked.set($any($event.target).checked)"
      />
      <span class="remember__box" aria-hidden="true">
        <app-icon name="check" [size]="11" [strokeWidth]="3" />
      </span>
      <span class="remember__label"><ng-content>Emlékezz rám</ng-content></span>
    </label>
  `,
  styleUrl: './remember-me.css',
})
export class RememberMe {
  readonly fieldId = input<string>('remember-me');
  readonly checked = model<boolean>(true);
  readonly disabled = input<boolean>(false);
}
