import { ChangeDetectionStrategy, Component, input, model } from '@angular/core';
import { Icon, IconName } from '../../../../shared/icon/icon';
import { UserRole } from '../../models/auth.models';

interface RoleOption {
  value: UserRole;
  label: string;
  icon: IconName;
}

@Component({
  selector: 'app-role-selector',
  imports: [Icon],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './role-selector.html',
  styleUrl: './role-selector.css',
})
export class RoleSelector {
  readonly role = model<UserRole>('accountant');
  readonly disabled = input<boolean>(false);
  readonly name = input<string>('role');
  readonly legend = input<string>('Válassz szerepkört');

  protected readonly options: readonly RoleOption[] = [
    { value: 'accountant', label: 'Könyvelő', icon: 'accountant' },
    { value: 'entrepreneur', label: 'Vállalkozó', icon: 'briefcase' },
  ];
}
