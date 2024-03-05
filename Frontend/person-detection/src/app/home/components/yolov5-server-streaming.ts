import { Component, ViewChild, inject } from '@angular/core';
import { DemosComboboxComponent } from './ui/demos-combobox.component';
import { hlmH1 } from '@spartan-ng/ui-typography-helm';
import { VideoCaptureComponent } from './ui/video-capture.component';
import { SignalRService } from '../../services/signal-r.service';

@Component({
  selector: 'yolov5-server-streaming',
  standalone: true,
  imports: [DemosComboboxComponent, VideoCaptureComponent],
  template: `
    <h1 class="${hlmH1}">YOLOv5 Server Streaming</h1>
    <div class="mt-4">
      <video-capture #videoCapture />
    </div>
    <button class="btn btn-primary mt-4" (click)="capture()">Capture</button>
  `,
})
export class YoloV5ServerStreamingComponent {
  @ViewChild('videoCapture', { static: true })
  videoCapture!: VideoCaptureComponent;

  signalr = inject(SignalRService);
  subject: signalR.Subject<string> | undefined;

  constructor() {
    this.signalr.startConnection().subscribe(() => {
      console.log('connected');
      this.subject = this.signalr.createSubject('ProcessYolo5Photo');
    });
  }

  capture() {
    const data = this.videoCapture.captureBase64Image();

    this.subject?.next(data);
  }
}
