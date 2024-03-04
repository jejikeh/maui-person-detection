import {
  Component,
  ElementRef,
  ViewChild,
  effect,
  inject,
} from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { NgIf, NgSwitch, NgSwitchCase } from '@angular/common';
import { LoginCardComponent } from '../auth/components/login-card.components';
import { DemosComboboxComponent } from './components/ui/demos-combobox.component';
import {
  hlmH1,
  hlmH2,
  hlmH3,
  hlmLead,
  hlmP,
} from '@spartan-ng/ui-typography-helm';
import { HlmAspectRatioDirective } from '@spartan-ng/ui-aspectratio-helm';
import { HlmSeparatorDirective } from '@spartan-ng/ui-separator-helm';
import { YoloV5ServerStreamingComponent } from './components/yolov5-server-streaming';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NgIf,
    LoginCardComponent,
    DemosComboboxComponent,
    HlmAspectRatioDirective,
    HlmSeparatorDirective,
    NgSwitchCase,
    NgSwitch,
    YoloV5ServerStreamingComponent,
  ],
  host: {
    class: 'block p-10',
  },
  template: `
    <div *ngIf="auth.currentUser() === null">
      <login-card />
    </div>

    <div class="w-full">
      <h1 class="${hlmH1}">
        Object Detection and Semantic Segmentation Demos in .NET OnnxRuntime
      </h1>
      <p class="${hlmLead} mt-4">
        This site is a collection of demonstrations featuring Object Detection
        and Semantic Segmentation models implemented in .NET using OnnxRuntime.
        Explore the capabilities of YOLOv5, YOLOv8, and Mediapipe for
        comparison, providing real-time insights into their performance and
        functionalities.
      </p>
      <p class="${hlmLead} mt-4">
        You can view the source code for the demos on
        <a
          href="https://github.com/jejikeh/maui-person-detection"
          class="underline"
        >
          GitHub </a
        >.
      </p>

      <div></div>
    </div>
    <div class="mt-10 ">
      <h2 class="${hlmH2}">Overview</h2>
      <div class="mt-4">
        <h3 class="${hlmH3}">Cross-platform Maui Application</h3>
        <div class="overflow-hidden rounded-xl drop-shadow max-w-xl">
          <img alt="Mac Application" src="/assets/mac-application.png" />
        </div>
        <p class="${hlmP}">
          Maui Person Detection demonstrate maui application with onnxruntime
          YOLOv5 Object Detection integration. The supported platforms are
          Android and Mac. However, on the Mac version does not work
          OnnxRuntime, so application uses ASP.Core Web API.
        </p>
      </div>

      <brn-separator hlmSeparator />

      <div class="mt-4">
        <h3 class="${hlmH3}">Angular Application</h3>
        <div class="overflow-hidden rounded-xl drop-shadow max-w-xl">
          <img alt="Mac Application" src="/assets/web-application.png" />
        </div>
        <p class="${hlmP}">
          Utilizing Angular framework for building the web interface with
          Tailwind CSS and Spartan. Uses ASP.Core Web API.
        </p>
      </div>
    </div>
    <div class="mt-10 ">
      <h2 class="${hlmH2}">Run</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4 mt-10">
        <div>
          <p class="${hlmP}">
            Here provided runnable demos in browser. You can interact with
            real-time Object Detection and Semantic Segmentation examples
            powered by YOLOv5, YOLOv8, and Mediapipe. Choose one of the demos
            below to try it out:
          </p>
          <div class="mt-4">
            <demos-combobox #demosCombobox />
          </div>
        </div>
        <div>
          <ng-container
            class="overflow-hidden rounded-xl drop-shadow max-w-xl"
            [ngSwitch]="demosCombobox.currentModel()?.value"
          >
            <ng-container *ngSwitchCase="'yolov5-ss'"
              ><yolov5-server-streaming />
            </ng-container>
          </ng-container>
        </div>
      </div>
    </div>
  `,
})
export class HomeComponent {
  auth = inject(AuthService);

  @ViewChild('demosCombobox', { static: false })
  buttonToggle!: DemosComboboxComponent;

  s = effect(() => {
    console.log(this.buttonToggle.currentModel()?.value);
  });
}
