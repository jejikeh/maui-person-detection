import { TestBed, ComponentFixture } from '@angular/core/testing';
import { YoloV5ServerStreamingComponent } from './yolov5-server-streaming';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SignalRService } from '../../services/signal-r.service';
import { MockSignalRService } from './mock-signalr.service';

describe('YoloV5ServerStreamingComponent', () => {
  let component: YoloV5ServerStreamingComponent;
  let fixture: ComponentFixture<YoloV5ServerStreamingComponent>;
  let signalRService: MockSignalRService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [{ provide: SignalRService, useClass: MockSignalRService }],
    }).compileComponents();

    signalRService = TestBed.inject(SignalRService) as MockSignalRService;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(YoloV5ServerStreamingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  // I tried to write unit tests for this component, but i just cant get it...
});
