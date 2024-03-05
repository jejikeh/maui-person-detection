import { Component, ViewChild } from '@angular/core';

@Component({
  selector: 'video-capture',
  standalone: true,
  imports: [],
  template: `
    <video
      #video
      [autoplay]="true"
      [muted]="true"
      class="rounded shadow"
    ></video>
  `,
})
export class VideoCaptureComponent {
  @ViewChild('video', { static: true }) video!: any;

  mediaRecorder!: MediaRecorder | undefined;

  constructor() {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
      navigator.mediaDevices
        .getUserMedia({ video: true })
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

  captureBase64Image(): string {
    const canvas = document.createElement('canvas');
    canvas.width = 640;
    canvas.height = 480;

    canvas.getContext('2d')?.drawImage(this.video?.nativeElement, 0, 0);

    return canvas
      .toDataURL('image/png')
      .replace(/^data:image\/(png|jpg);base64,/, '');
  }
}
