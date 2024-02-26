import { Component, inject } from '@angular/core';
import { ScriptService } from '../script.service';

@Component({
  selector: 'app-stream',
  standalone: true,
  imports: [],
  template: `
    <video id="video-stream-source" controls autoplay></video>
    <img id="output-image" width="640px" height="480px" />
  `,
  styles: ``,
})
export class StreamComponent {
  private _scriptService: ScriptService = inject(ScriptService);

  ngAfterViewInit() {
    this._scriptService.load('signalr-sendstream');
  }
}
