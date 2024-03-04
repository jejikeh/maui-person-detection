import { Component } from '@angular/core';
import { DemosComboboxComponent } from './ui/demos-combobox.component';
import { hlmH1 } from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'yolov5-server-streaming',
  standalone: true,
  imports: [DemosComboboxComponent],
  template: `<h1 class="${hlmH1}">YOLOv5 Server Streaming</h1>`,
})
export class YoloV5ServerStreamingComponent {}
