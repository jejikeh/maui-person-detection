import {
  AfterViewInit,
  Component,
  ElementRef,
  Inject,
  NgZone,
  Renderer2,
  ViewChild,
  inject,
} from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { SignalRService } from '../signalr.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { DOCUMENT } from '@angular/common';
import { ScriptService } from '../script.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  template: `
    <div>
      <h1>{{ CurrentModel }} ML.NET Yolo Segmentation model</h1>
      <nav>
        <ul>
          <li>
            <p>Response Time: {{ modelPerformance }} ms</p>
          </li>
          <li>
            <p>Average Time: {{ AverageModelPerformance }} ms</p>
          </li>
        </ul>
      </nav>
      <div>
        <button class="btn btn-primary" (click)="switchModel()">
          Switch Model
        </button>
        <button class="btn btn-primary" (click)="start()">Start</button>
        <button class="btn btn-primary" (click)="stop()">Stop</button>
      </div>
      <div class="layered-image">
        <video
          class="image-base"
          #local_video
          [autoplay]="true"
          [muted]="true"
        ></video>
        <img class="image-overlay" [src]="receivedOverlay" alt="" />
      </div>
    </div>
    <div>
      <h1>MediaPipe JS Segmentation Model</h1>
      <div class="container">
        <video class="input_video" hidden="true"></video>
        <canvas class="output_canvas" width="640px" height="480px"></canvas>
      </div>
      <button (click)="startMediapipe()">Start</button>
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
}
  `,
})
export class HomeComponent implements AfterViewInit {
  mediaConstraints = {
    video: { width: 640, height: 480 },
  };

  private _http: HttpClient = inject(HttpClient);
  private _authService: AuthService = inject(AuthService);
  private _signalR: SignalRService | undefined;
  private _scriptService: ScriptService = inject(ScriptService);

  private _sumPerformance: number = 0;
  private _numReceivedPerformanceTimes: number = 0;

  @ViewChild('local_video') localVideo: ElementRef | undefined;

  public receivedImage: string = '';
  public receivedOverlay: string = '';

  public modelPerformance: string = '';
  public AverageModelPerformance: string = '';

  public CurrentModel = 'UnQuantized';

  ngOnInit() {
    this._authService.identify().add(() => {
      if (this._authService.user.userName == undefined) {
        location.replace('auth/login');
      }
    });
  }

  ngAfterViewInit(): void {
    this._signalR = new SignalRService(this.localVideo!);

    this._signalR?.startConnectionAndStream(this.mediaConstraints);

    this._signalR?.OnModelPerformance.subscribe((data) => {
      this.modelPerformance = data;
      this._sumPerformance = this._sumPerformance + Number.parseInt(data);

      if (Number.isNaN(this._sumPerformance)) {
        this._sumPerformance = Number.parseInt(data);
      }

      this._numReceivedPerformanceTimes += 1;
      this.AverageModelPerformance = (
        this._sumPerformance / this._numReceivedPerformanceTimes
      ).toFixed(1);
    });

    this._signalR?.OnReceivedMask.subscribe((data) => {
      this.receivedOverlay = data;
    });
  }

  public start() {
    this._signalR?.requestData();
  }

  public stop() {
    this._signalR?.stop();
  }

  public switchModel() {
    this._http
      .post<any>(environment.backend + '/model/switch', {})
      .subscribe((response) => {
        this.CurrentModel = response;
      });
  }

  public startMediapipe() {
    this._scriptService.load('mediapipe');
  }
}
