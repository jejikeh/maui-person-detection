import { HttpClient } from '@angular/common/http';
import { Component, Input, inject } from '@angular/core';
import { provideIcons } from '@ng-icons/core';
import { lucideDownload, lucideDelete } from '@ng-icons/lucide';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HlmIconComponent } from '@spartan-ng/ui-icon-helm';
import { BrnMenuTriggerDirective } from '@spartan-ng/ui-menu-brain';
import {
  HlmMenuComponent,
  HlmMenuGroupComponent,
  HlmMenuItemCheckboxDirective,
  HlmMenuItemCheckComponent,
  HlmMenuItemDirective,
  HlmMenuItemIconDirective,
  HlmMenuItemRadioComponent,
  HlmMenuItemRadioDirective,
  HlmMenuItemSubIndicatorComponent,
  HlmMenuLabelComponent,
  HlmMenuSeparatorComponent,
  HlmMenuShortcutComponent,
  HlmSubMenuComponent,
} from '@spartan-ng/ui-menu-helm';
import { ApiRoutesService } from '../../services/api-routes.service';

@Component({
  selector: 'dropdown-photo-menu',
  standalone: true,
  imports: [
    BrnMenuTriggerDirective,

    HlmMenuComponent,
    HlmSubMenuComponent,
    HlmMenuItemDirective,
    HlmMenuItemSubIndicatorComponent,
    HlmMenuLabelComponent,
    HlmMenuShortcutComponent,
    HlmMenuSeparatorComponent,
    HlmMenuItemIconDirective,
    HlmMenuItemCheckComponent,
    HlmMenuItemRadioComponent,
    HlmMenuGroupComponent,
    HlmMenuItemRadioDirective,
    HlmMenuItemCheckboxDirective,

    HlmButtonDirective,
    HlmIconComponent,
  ],
  providers: [provideIcons({ lucideDownload, lucideDelete })],
  template: `
    <div class="grid h-full w-full place-items-center p-4">
      <div class="mt-4">
        <img [src]="photo" alt="photo" class="rounded shadow" />
      </div>
      <div class="mt-4">
        <button
          hlmBtn
          variant="outline"
          align="center"
          [brnMenuTriggerFor]="menu"
        >
          Open
        </button>
      </div>
    </div>
    <ng-template #menu>
      <hlm-menu class="w-56">
        <hlm-menu-label>Photo Actions</hlm-menu-label>

        <hlm-menu-group>
          <button hlmMenuItem (triggered)="download()">
            <hlm-icon name="lucideDownload" hlmMenuIcon />
            Download
          </button>
          <button hlmMenuItem (triggered)="delete()">
            <hlm-icon name="lucideDelete" hlmMenuIcon />
            Delete
          </button>
        </hlm-menu-group>
      </hlm-menu>
    </ng-template>
  `,
})
export class DropdownPhotoMenuComponent {
  @Input() photo = '';
  @Input() index = 0;

  api = inject(ApiRoutesService);
  http = inject(HttpClient);

  download() {
    const a = document.createElement('a');

    const receivedFile = this.photo.replace('data:image/jpeg;base64,', '');
    const byteCharacters = atob(receivedFile);
    a.href = URL.createObjectURL(
      new Blob([byteCharacters], { type: 'image/png' })
    );
    a.download = `${this.index}.png`;
    document.body.appendChild(a);
    a.click();
  }

  delete() {
    this.photo = '';

    this.http
      .delete(this.api.DeletePhoto(this.index), { withCredentials: true })
      .subscribe({
        next: (data) => {
          console.log(data);
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {},
      });
  }
}
