import { Component, inject } from '@angular/core';
import { ScriptService } from '../script.service';

@Component({
  selector: 'app-stream',
  standalone: true,
  imports: [],
  template: `
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
        height="480px"
      />
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
export class StreamComponent {
  private _scriptService: ScriptService = inject(ScriptService);

  ngAfterViewInit() {
    this._scriptService.load('signalr-sendstream');
  }
}
