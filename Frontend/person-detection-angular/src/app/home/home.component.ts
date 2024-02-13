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
import { BehaviorSubject, Subject } from 'rxjs';
import { SignalRService } from '../signalr.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  template: `
    <div class="container mt-5">
      <h2>Angular Webcam Capture Image from Camera</h2>
      <button class="btn btn-primary" (click)="start()">Start</button>
      <video #received_video [autoplay]="true"></video>
      <div class="layered-image">
        <video
          class="image-base"
          #local_video
          [autoplay]="true"
          [muted]="true"
        ></video>
      </div>
      <img class="layered-image" [src]="receivedImage" alt="" />
      <p>{{ modelPerformance }}</p>
    </div>
  `,
  styles: `
  .layered-image {
  position: relative;
}
.layered-image img {
  width: 640px;
  height: 480px;
}
.image-overlay {
  position: absolute;
  top: 0px;
  left: 0px;
  opacity: .7
}`,
})
export class HomeComponent implements AfterViewInit {
  mediaConstraints = {
    video: { width: 640, height: 480 },
  };

  private _authService: AuthService = inject(AuthService);
  private _signalR: SignalRService = inject(SignalRService);

  @ViewChild('local_video') localVideo: ElementRef | undefined;
  private _localStream: MediaStream | undefined;

  public receivedImage: string = '';
  public receivedOverlay: string = '';

  private _getConnection: signalR.HubConnection | undefined;

  private _mediaRecorder: MediaRecorder | undefined;

  public modelPerformance: string = '';

  ngOnInit() {
    this._authService.identify().add(() => {
      if (this._authService.user.userName == undefined) {
        location.replace('auth/login');
      }
    });
  }

  ngAfterViewInit(): void {
    this._signalR.startConnectionAndStream(
      this.mediaConstraints,
      this.localVideo!
    );

    this._signalR.OnModelPerformance.subscribe((data) => {
      this.modelPerformance = data;
    });

    this._signalR.OnReceivedMask.subscribe((data) => {
      this.receivedImage = data;
    });
  }

  public start() {
    this._signalR.requestData();
  }
}
