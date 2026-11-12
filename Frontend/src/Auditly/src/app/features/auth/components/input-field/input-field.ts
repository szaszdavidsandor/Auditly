import { ChangeDetectionStrategy, Component, ElementRef, input, model, output, viewChild } from '@angular/core';
import { Icon, IconName } from '../../../../shared/icon/icon';

@Component({
  selector: 'app-input-field',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './input-field.html',
  styleUrl: './input-field.css',
})
export class InputField {
  readonly fieldId = input.required<string>();
  readonly label = input.required<string>();
  readonly icon = input.required<IconName>();
  readonly type = input<'text' | 'email' | 'password'>('text');
  readonly placeholder = input<string>('');
  readonly autocomplete = input<string>('off');
  readonly inputMode = input<string | null>(null);
  readonly error = input<string | null>(null);
  readonly disabled = input<boolean>(false);

  readonly value = model<string>('');
  readonly blurred = output<void>();

  private readonly inputRef = viewChild.required<ElementRef<HTMLInputElement>>('inputEl');

  protected onInput(event: Event): void {
    this.value.set((event.target as HTMLInputElement).value);
  }

  focus(): void {
    this.inputRef().nativeElement.focus();
  }
}
