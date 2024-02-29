import { Component, inject } from '@angular/core';
import { ScriptService } from '../script.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-stream',
  standalone: true,
  imports: [],
  template: `
    <h1>{{ CurrentModel }} ML.NET Yolo Segmentation model</h1>
    <button class="btn btn-primary" (click)="switchModel()">
      Switch Model
    </button>
    <div class="layered-image">
      <video
        class="image-base"
        id="video-stream-source"
        controls
        autoplay
      ></video>
      <img
        class="image-overlay"
        id="output-image"
        width="640px"
        height="640px"
      />
    </div>
  `,
  styles: `
  .layered-image {
  position: relative;
}
.layered-image img {
  width: 640px;
  height: 640px;
}
.image-overlay {
  position: absolute;
  top: 0px;
  left: 0px;
}
  `,
})
export class StreamComponent {
  private _scriptService: ScriptService = inject(ScriptService);
  private _http: HttpClient = inject(HttpClient);

  public CurrentModel = '';

  ngAfterViewInit() {
    this._scriptService.load('signalr-sendstream');

    this.switchModel();
  }

  public switchModel() {
    this._http
      .post<any>(environment.backend + '/model/switch', {})
      .subscribe((response) => {
        this.CurrentModel = response;
      });
  }
}
