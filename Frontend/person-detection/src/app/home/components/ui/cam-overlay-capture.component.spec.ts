import { TestBed, ComponentFixture } from '@angular/core/testing';
import { CamOverlayCaptureComponent } from './cam-overlay-capture.component';

describe('CamOverlayCaptureComponent', () => {
  let component: CamOverlayCaptureComponent;
  let fixture: ComponentFixture<CamOverlayCaptureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CamOverlayCaptureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });
});
