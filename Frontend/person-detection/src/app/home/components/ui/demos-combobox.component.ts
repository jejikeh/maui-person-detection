import { Component, signal } from '@angular/core';
import { BrnCommandImports } from '@spartan-ng/ui-command-brain';
import { HlmCommandImports } from '@spartan-ng/ui-command-helm';
import { HlmIconComponent } from '@spartan-ng/ui-icon-helm';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import {
  BrnPopoverComponent,
  BrnPopoverContentDirective,
  BrnPopoverTriggerDirective,
} from '@spartan-ng/ui-popover-brain';
import { HlmPopoverContentDirective } from '@spartan-ng/ui-popover-helm';
import { NgForOf } from '@angular/common';
import { provideIcons } from '@ng-icons/core';
import {
  lucideChevronsUpDown,
  lucideCheck,
  lucideSearch,
} from '@ng-icons/lucide';

type Framework = { label: string; value: string };

@Component({
  selector: 'demos-combobox',
  standalone: true,
  imports: [
    BrnCommandImports,
    HlmCommandImports,
    HlmIconComponent,
    HlmButtonDirective,
    BrnPopoverComponent,
    BrnPopoverTriggerDirective,
    HlmPopoverContentDirective,
    BrnPopoverContentDirective,
    NgForOf,
  ],
  providers: [
    provideIcons({ lucideChevronsUpDown, lucideSearch, lucideCheck }),
  ],
  template: `
    <brn-popover
      [state]="state()"
      (stateChanged)="stateChanged($event)"
      sideOffset="5"
      closeDelay="100"
    >
      <button
        class="w-[200px] justify-between"
        id="edit-profile"
        variant="outline"
        brnPopoverTrigger
        hlmBtn
      >
        {{ currentModel() ? currentModel()?.label : 'Select demo...' }}
        <hlm-icon size="sm" name="lucideChevronsUpDown" />
      </button>
      <brn-cmd
        *brnPopoverContent="let ctx"
        hlmPopoverContent
        hlm
        class="p-0 w-[200px]"
      >
        <hlm-cmd-input-wrapper>
          <hlm-icon name="lucideSearch" />
          <input placeholder="Search framework..." brnCmdInput hlm />
        </hlm-cmd-input-wrapper>
        <div *brnCmdEmpty hlmCmdEmpty>No results found.</div>
        <brn-cmd-list hlm>
          <brn-cmd-group hlm>
            <button
              *ngFor="let framework of model"
              brnCmdItem
              [value]="framework.value"
              (selected)="commandSelected(framework)"
              hlm
            >
              <hlm-icon
                [class.opacity-0]="currentModel()?.value !== framework.value"
                name="lucideCheck"
                hlmCmdIcon
              />
              {{ framework.label }}
            </button>
          </brn-cmd-group>
        </brn-cmd-list>
      </brn-cmd>
    </brn-popover>
  `,
})
export class DemosComboboxComponent {
  public model = [
    {
      label: 'YOLOv5 Server Streaming',
      value: 'yolov5-ss',
    },
    {
      label: 'YOLOv8 Server Streaming',
      value: 'yolov8-ss',
    },
    {
      label: 'YOLOv5 Client Streaming',
      value: 'yolov5-cs',
    },
    {
      label: 'YOLOv8 Client Streaming',
      value: 'yolov8-cs',
    },
    {
      label: 'Mediapipe',
      value: 'mediapipe',
    },
  ];
  public currentModel = signal<Framework | undefined>(undefined);
  public state = signal<'closed' | 'open'>('closed');

  stateChanged(state: 'open' | 'closed') {
    this.state.set(state);
  }

  commandSelected(model: Framework) {
    this.state.set('closed');
    if (this.currentModel()?.value === model.value) {
      this.currentModel.set(undefined);
    } else {
      this.currentModel.set(model);
    }
  }
}
