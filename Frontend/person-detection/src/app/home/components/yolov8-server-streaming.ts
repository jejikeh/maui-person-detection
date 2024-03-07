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

@Component({
  selector: 'yolov8-server-streaming',
  standalone: true,
  imports: [
    DemosComboboxComponent,
    CamOverlayCaptureComponent,
    HlmButtonDirective,
    NgIf,
  ],
  template: `
    <h1 class="${hlmH1}">YOLOv8 Server Streaming</h1>
    <button
      hlmBtn
      *ngIf="!subscription || subscription?.closed === true"
      class="btn btn-primary mt-4"
      (click)="start()"
    >
      Start
    </button>
    <button
      hlmBtn
      *ngIf="subscription?.closed === false"
      class="btn btn-primary mt-4"
      (click)="stop()"
    >
      Stop
    </button>
    <div class="mt-4">
      <video-capture [overlayBase64]="receivedPhoto" #videoCapture />
    </div>
  `,
})
export class YoloV8ServerStreamingComponent {
  @ViewChild('videoCapture', { static: true })
  videoCapture!: CamOverlayCaptureComponent;

  signalr = inject(SignalRService);
  subject: signalR.Subject<string> | undefined;

  receivedPhoto = '';

  intervalHandle: Observable<number> | undefined;
  subscription: Subscription | undefined;

  constructor() {
    this.signalr.startConnection().subscribe(() => {
      console.log('connected');
      this.subject = this.signalr.createSubject('ProcessYolo8Photo');
    });

    this.signalr.receivePhoto().subscribe((data) => {
      this.receivedPhoto = data;
    });
  }

  start() {
    this.intervalHandle = interval(300);
    this.subscription = this.intervalHandle.subscribe(() => {
      const data = this.videoCapture.captureBase64Image();
      this.subject?.next(data);
    });
  }

  stop() {
    this.subscription?.unsubscribe();
  }
}
