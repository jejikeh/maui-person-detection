import {
  TestBed,
  ComponentFixture,
  fakeAsync,
  tick,
} from '@angular/core/testing';
import { Mediapipe } from './mediapipe';
import { ScriptService } from '../../services/script.service';

describe('MediapipeComponent', () => {
  let component: Mediapipe;
  let fixture: ComponentFixture<Mediapipe>;
  let scriptServiceSpy: jasmine.SpyObj<ScriptService>;

  beforeEach(async () => {
    const scriptServiceSpyObj = jasmine.createSpyObj('ScriptService', [
      'loadScript',
    ]);

    await TestBed.configureTestingModule({
      imports: [Mediapipe],
      providers: [{ provide: ScriptService, useValue: scriptServiceSpyObj }],
    }).compileComponents();

    scriptServiceSpy = TestBed.inject(
      ScriptService
    ) as jasmine.SpyObj<ScriptService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(Mediapipe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadScript method of ScriptService when start method is called', () => {
    component.start();
    expect(scriptServiceSpy.loadScript).toHaveBeenCalledWith('mediapipe');
  });
});
