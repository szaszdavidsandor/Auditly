import {
  ChangeDetectionStrategy,
  Component,
  input,
  model,
  output,
  signal,
  viewChild,
} from '@angular/core';
import { Icon } from '../../../../shared/icon/icon';
import { InputField } from '../input-field/input-field';

@Component({
  selector: 'app-password-field',
  imports: [InputField, Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './password-field.html',
  styleUrl: './password-field.css',
})
export class PasswordField {
  readonly fieldId = input.required<string>();
  readonly label = input<string>('Jelszó');
  readonly placeholder = input<string>('');
  readonly autocomplete = input<string>('current-password');
  readonly error = input<string | null>(null);
  readonly disabled = input<boolean>(false);

  readonly value = model<string>('');
  readonly blurred = output<void>();

  protected readonly visible = signal(false);
  private readonly field = viewChild.required(InputField);

  protected toggleVisibility(): void {
    this.visible.update((v) => !v);
  }

  focus(): void {
    this.field().focus();
  }
}
