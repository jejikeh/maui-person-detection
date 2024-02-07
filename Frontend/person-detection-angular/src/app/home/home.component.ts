import {
  AfterViewInit,
  Component,
  ElementRef,
  NgZone,
  ViewChild,
  inject,
} from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../environments/environment';
import * as signalR from '@aspnet/signalr';
import * as base64 from 'base64-js';
import { SimpleChannel } from './simple-channel';
import { BehaviorSubject } from 'rxjs';
import { VideoData } from './video-data.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  template: `
    <div class="container mt-5">
      <h2>Angular Webcam Capture Image from Camera</h2>
      <button class="btn btn-primary" (click)="startStream()">Start</button>
      <p class="video-description">Local:</p>
      <video #local_video [autoplay]="true" [muted]="true"></video>
      <p class="video-description">Remote:</p>
      <video #received_video [autoplay]="true"></video>
      <img [src]="receivedImage" alt="Red dot" />
    </div>
  `,
  styles: ``,
})
export class HomeComponent implements AfterViewInit {
  mediaConstraints = {
    video: { width: 640, height: 480 },
  };

  private _authService: AuthService = inject(AuthService);

  @ViewChild('local_video') localVideo: ElementRef | undefined;
  private _localStream: MediaStream | undefined;

  @ViewChild('received_video') receivedVideo: ElementRef | undefined;
  private _receivedStream: MediaStream | undefined;

  public receivedImage: string = '';

  private _connection: signalR.HubConnection | undefined;
  private _getConnection: signalR.HubConnection | undefined;

  private _mediaRecorder: MediaRecorder | undefined;

  constructor(private ngZone: NgZone) {}

  ngOnInit() {
    this._authService.identify().add(() => {
      if (this._authService.user.userName == undefined) {
        location.replace('auth/login');
      }
    });
  }

  ngAfterViewInit(): void {
    this.startStream();
    this.getStream();
  }

  public async startStream() {
    if (this._connection == undefined) {
      this._connection = new signalR.HubConnectionBuilder()
        .withUrl(environment.webrtcBackend)
        .configureLogging(signalR.LogLevel.Error)
        .build();
    }

    this._connection?.start().then(async () => {
      await navigator.mediaDevices
        .getUserMedia(this.mediaConstraints)
        .then((stream) => {
          this._localStream = stream;
          this._mediaRecorder = new MediaRecorder(stream, {
            mimeType: 'video/webm',
          });

          this._mediaRecorder.ondataavailable = async (event) => {
            var canvas = document.createElement('canvas');
            canvas.width = 640;
            canvas.height = 480;
            canvas
              .getContext('2d')
              ?.drawImage(this.localVideo!.nativeElement, 0, 0);

            var base64 = canvas
              .toDataURL('image/png')
              .replace(/^data:image\/(png|jpg);base64,/, '');

            this._connection?.invoke('SendVideoData', {
              index: 0,
              data: base64,
            });
          };

          this._mediaRecorder.start();

          setInterval(() => {
            if (this._mediaRecorder?.state == 'recording') {
              this._mediaRecorder?.requestData();
            }
          }, 1);
        });

      this._localStream?.getTracks().forEach((track) => {
        track.enabled = true;
      });

      this.localVideo!.nativeElement.srcObject = this._localStream;

      this._connection?.onclose(() => {
        this._localStream?.getTracks().forEach((track) => {
          track.enabled = false;
        });
        this._mediaRecorder?.stop();
      });
    });
  }

  public getStream() {
    if (this._getConnection == undefined) {
      this._getConnection = new signalR.HubConnectionBuilder()
        .withUrl(environment.webrtcBackend)
        .build();
    }

    this.receivedVideo!.nativeElement.autoplay = true;
    this.receivedVideo!.nativeElement.muted = true;
    this.receivedVideo!.nativeElement.controls = false;

    this._getConnection?.start().then(async () => {
      this._getConnection?.on('ReceiveVideoData', (r: VideoData) => {
        const ab = base64.toByteArray(r.data);
        this.receivedImage =
          'data:image/png;base64, ' + base64.fromByteArray(ab);
      });
    });
  }
}
