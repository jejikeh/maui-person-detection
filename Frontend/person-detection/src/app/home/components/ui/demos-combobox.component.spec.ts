import { TestBed, ComponentFixture } from '@angular/core/testing';
import { DemosComboboxComponent } from './demos-combobox.component';
import { ModelType } from '../../../common/models.types';

describe('DemosComboboxComponent', () => {
  let component: DemosComboboxComponent;
  let fixture: ComponentFixture<DemosComboboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DemosComboboxComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DemosComboboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.model.length).toBe(3);
    expect(component.currentModel()).toBeUndefined();
    expect(component.state()).toBe('closed');
  });

  it('should change state when stateChanged is called', () => {
    component.stateChanged('open');
    expect(component.state()).toBe('open');
    component.stateChanged('closed');
    expect(component.state()).toBe('closed');
  });

  it('should update currentModel when commandSelected is called', () => {
    const model = { label: 'Test Model', value: ModelType.YOLOv5 };
    component.commandSelected(model);
    expect(component.currentModel()).toEqual(model);
    component.commandSelected(model);
    expect(component.currentModel()).toBeUndefined();
  });
});
