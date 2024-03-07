import {
  Component,
  EventEmitter,
  Input,
  ViewChild,
  WritableSignal,
  signal,
} from '@angular/core';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'video-capture',
  standalone: true,
  imports: [],
  template: `
    <div class="relative">
      <video
        #video
        [autoplay]="true"
        [muted]="true"
        class="rounded shadow absolute z-0"
      ></video>
      <img [src]="overlayBase64" class="absolute z-10" />
    </div>
  `,
})
export class CamOverlayCaptureComponent {
  @Input() overlayBase64!: string;

  @ViewChild('video', { static: true }) video!: any;

  mediaRecorder: MediaRecorder | undefined;

  constructor() {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
      navigator.mediaDevices
        .getUserMedia({
          video: {
            width: 640,
            height: 640,
          },
          audio: false,
        })
        .then((stream) => {
          this.video.nativeElement.srcObject = stream;
          this.mediaRecorder = new MediaRecorder(stream, {
            mimeType: 'video/webm',
          });

          this.mediaRecorder.start();
        })
        .catch((err) => {
          console.log('Something went wrong!', err);
        });
    }
  }

  ngOnDestroy() {
    this.mediaRecorder?.stop();
  }

  captureBase64Image(): string {
    const canvas = document.createElement('canvas');
    canvas.width = 640;
    canvas.height = 640;

    canvas.getContext('2d')?.drawImage(this.video?.nativeElement, 0, 0);

    return canvas
      .toDataURL('image/png')
      .replace(/^data:image\/(png|jpg);base64,/, '');
  }
}
