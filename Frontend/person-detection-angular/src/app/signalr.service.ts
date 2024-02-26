import { ElementRef, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import * as base64 from 'base64-js';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private _connection: signalR.HubConnection;
  private _mediaRecorder: MediaRecorder | undefined;

  private _localStream: MediaStream | undefined;
  private _localVideoStreamElement: ElementRef | undefined;

  private _onMediaData$ = new BehaviorSubject<string>('');

  public OnReceivedMask = new BehaviorSubject<string>('');
  public OnModelPerformance = new BehaviorSubject<string>('');

  public StreamImageData = 'ReceiveVideoData';
  public UploadClientData = 'UploadClientData';
  public ModelPerformanceData = 'SendModelPerformance';

  constructor(localVideoStreamElement: ElementRef) {
    this._localVideoStreamElement = localVideoStreamElement;

    this._connection = new HubConnectionBuilder()
      .withUrl(environment.webrtcBackend)
      .configureLogging(LogLevel.Error)
      .build();
  }

  public startConnectionAndStream(
    mediaConstraints: MediaStreamConstraints
  ): void {
    console.log(this._connection);

    this._connection.start().then(async () => {
      this._localStream = await navigator.mediaDevices.getUserMedia(
        mediaConstraints
      );

      this._mediaRecorder = new MediaRecorder(this._localStream, {
        mimeType: 'video/webm',
      });

      this._onMediaData$.subscribe((data) => {
        this.handleMediaData(this._connection, data);
      });

      this.setupStream();
      this._mediaRecorder.start();

      this._localStream?.getTracks().forEach((track) => {
        track.enabled = true;
      });

      this._localVideoStreamElement!.nativeElement.srcObject =
        this._localStream;

      this._connection?.on(this.ModelPerformanceData, (data: string) =>
        this.OnModelPerformance.next(data)
      );
    });

    this._connection.onclose(() => {
      this._localStream?.getTracks().forEach((track) => {
        track.enabled = false;
      });

      this._mediaRecorder?.stop();
    });
  }

  public requestData() {
    if (
      this._mediaRecorder?.state == 'inactive' ||
      this._mediaRecorder?.state == 'paused'
    ) {
      this._mediaRecorder.start();
    }
    this._mediaRecorder?.requestData();
  }

  public stop() {
    this._mediaRecorder?.stop();
  }

  private handleMediaData(hub: signalR.HubConnection, data: string) {
    if (data.length == 0) {
      return;
    }

    console.log(data);

    hub.stream(this.StreamImageData, data).subscribe({
      next: (request) => {
        this.OnReceivedMask.next('data:image/png;base64, ' + request);
        this._mediaRecorder?.requestData();
      },
      error: function (err: any): void {},
      complete: function (): void {},
    });
  }

  private setupStream(): void {
    if (!this._connection || !this._mediaRecorder) {
      return;
    }

    this._mediaRecorder.ondataavailable = async (event) => {
      this._onMediaData$.next(this.captureBase64Image());
    };
  }

  private captureBase64Image(): string {
    const canvas = document.createElement('canvas');
    canvas.width = 640;
    canvas.height = 480;

    canvas
      .getContext('2d')
      ?.drawImage(this._localVideoStreamElement?.nativeElement, 0, 0);

    return canvas
      .toDataURL('image/png')
      .replace(/^data:image\/(png|jpg);base64,/, '');
  }
}
