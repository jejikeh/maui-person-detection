import { TestBed, ComponentFixture } from '@angular/core/testing';
import { YoloV8ServerStreamingComponent } from './yolov8-server-streaming';
import { SignalRService } from '../../services/signal-r.service';
import { CamOverlayCaptureComponent } from './ui/cam-overlay-capture.component';
import { of, Subject } from 'rxjs';

describe('YoloV8ServerStreamingComponent', () => {
  let component: YoloV8ServerStreamingComponent;
  let fixture: ComponentFixture<YoloV8ServerStreamingComponent>;
  let mockSignalRService: Partial<SignalRService>;

  beforeEach(async () => {
    mockSignalRService = {
      startConnection: jasmine
        .createSpy('startConnection')
        .and.returnValue(of()),
      createSubject: jasmine
        .createSpy('createSubject')
        .and.returnValue(new Subject()),
      receivePhoto: () => of('mockPhotoData'),
    };

    await TestBed.configureTestingModule({
      imports: [CamOverlayCaptureComponent],
      providers: [{ provide: SignalRService, useValue: mockSignalRService }],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(YoloV8ServerStreamingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });
});
