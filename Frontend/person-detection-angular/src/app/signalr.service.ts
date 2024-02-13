import { ElementRef, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import * as base64 from 'base64-js';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private _connection: signalR.HubConnection | undefined;
  private _mediaRecorder: MediaRecorder | undefined;

  private _localStream: MediaStream | undefined;
  private _localVideoStreamElement: ElementRef | undefined;

  private _onMediaData = new BehaviorSubject<string>('');

  public OnReceivedMask = new BehaviorSubject<string>('');
  public OnModelPerformance = new BehaviorSubject<string>('');

  public StreamImageData = 'ReceiveVideoData';
  public ModelPerformanceData = 'SendModelPerformance';

  public startConnectionAndStream(
    mediaConstraints: MediaStreamConstraints,
    localVideoStreamElement: ElementRef
  ): void {
    this._connection = new HubConnectionBuilder()
      .withUrl(environment.webrtcBackend)
      .configureLogging(LogLevel.Error)
      .build();

    this._localVideoStreamElement = localVideoStreamElement;

    this._connection.start().then(async () => {
      this._localStream = await navigator.mediaDevices.getUserMedia(
        mediaConstraints
      );

      this._mediaRecorder = new MediaRecorder(this._localStream, {
        mimeType: 'video/webm',
      });

      this._onMediaData.subscribe(this.handleMediaData);
      this.setupStream();
      this._mediaRecorder.start();

      this._localStream?.getTracks().forEach((track) => {
        track.enabled = true;
      });

      this._localVideoStreamElement!.nativeElement.srcObject =
        this._localStream;

      this._connection?.on(
        this.ModelPerformanceData,
        this.handleModelPerformance
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
    this._mediaRecorder?.requestData();
  }

  private handleModelPerformance(data: string) {
    this.OnModelPerformance.next(data);
  }

  private handleMediaData(data: string) {
    if (data.length == 0) {
      return;
    }

    this._connection?.stream(this.StreamImageData, data).subscribe({
      next: (request) => {
        this.OnReceivedMask.next('data:image/png;base64, ' + request);
        this._mediaRecorder?.requestData();
      },
      error: function (err: any): void {
        throw new Error('Function not implemented.');
      },
      complete: function (): void {
        throw new Error('Function not implemented.');
      },
    });
  }

  private setupStream(): void {
    if (!this._connection || !this._mediaRecorder) {
      return;
    }

    this._mediaRecorder.ondataavailable = async (event) => {
      this._onMediaData.next(this.captureBase64Image());
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
