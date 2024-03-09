import {
  Component,
  EventEmitter,
  ViewChild,
  inject,
  signal,
} from '@angular/core';
import { DemosComboboxComponent } from './ui/demos-combobox.component';
import { hlmH1 } from '@spartan-ng/ui-typography-helm';
import { CamOverlayCaptureComponent } from './ui/cam-overlay-capture.component';
import { SignalRService } from '../../services/signal-r.service';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import {
  Observable,
  Subject,
  Subscriber,
  Subscription,
  interval,
  timer,
} from 'rxjs';
import { NgIf } from '@angular/common';
import { ScriptService } from '../../services/script.service';

@Component({
  selector: 'mediapipe',
  standalone: true,
  imports: [
    DemosComboboxComponent,
    CamOverlayCaptureComponent,
    HlmButtonDirective,
    NgIf,
  ],
  template: `
    <h1 class="${hlmH1}">Mediapipe</h1>
    <button hlmBtn class="btn btn-primary mt-4" (click)="start()">Start</button>
    <div class="mt-4">
      <video class="input_video" hidden="true"></video>
      <canvas class="output_canvas" width="640px" height="640px"></canvas>
    </div>
  `,
})
export class Mediapipe {
  scripts = inject(ScriptService);

  start() {
    this.scripts.loadScript('mediapipe');
  }
}
